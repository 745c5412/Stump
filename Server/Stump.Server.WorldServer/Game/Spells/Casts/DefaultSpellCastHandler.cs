using Stump.Core.Threading;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Spells.Casts
{
    [DefaultSpellCastHandler]
    public class DefaultSpellCastHandler : SpellCastHandler
    {
        protected bool m_initialized;

        public DefaultSpellCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public SpellEffectHandler[] Handlers
        {
            get;
            protected set;
        }

        public override bool SilentCast
        {
            get { return m_initialized && Handlers.Any(entry => entry.RequireSilentCast()); }
        }

        public override bool Initialize()
        {
            var random = new AsyncRandom();

            var effects = Critical && SpellLevel.CriticalEffects.Any() ? SpellLevel.CriticalEffects : SpellLevel.Effects;
            var handlers = new List<SpellEffectHandler>();

            var groups = effects.GroupBy(x => x.Group);
            double totalRandSum = effects.Sum(entry => entry.Random);
            var randGroup = random.NextDouble();
            var stopRandGroup = false;

            foreach (var groupEffects in groups)
            {
                double randSum = groupEffects.Sum(entry => entry.Random);

                if (randSum > 0)
                {
                    if (stopRandGroup)
                        continue;

                    if (randGroup > randSum / totalRandSum)
                    {
                        // group ignored
                        randGroup -= randSum / totalRandSum;
                        continue;
                    }

                    // random group found, there can be only one
                    stopRandGroup = true;
                }

                var randEffect = random.NextDouble();
                var stopRandEffect = false;

                foreach (var effect in groupEffects)
                {
                    if (groups.Count() <= 1)
                    {
                        if (effect.Random > 0)
                        {
                            if (stopRandEffect)
                                continue;

                            if (randEffect > effect.Random / randSum)
                            {
                                // effect ignored
                                randEffect -= effect.Random / randSum;
                                continue;
                            }

                            // random effect found, there can be only one
                            stopRandEffect = true;
                        }
                    }

                    var handler = EffectManager.Instance.GetSpellEffectHandler(effect, Caster, Spell, TargetedCell, Critical);

                    if (MarkTrigger != null)
                        handler.MarkTrigger = MarkTrigger;

                    if (!handler.CanApply())
                        return false;

                    handlers.Add(handler);
                }
            }

            Handlers = handlers.ToArray();
            m_initialized = true;

            return true;
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            foreach (var handler in Handlers)
            {
                if (handler.Dice.Delay > 0)
                {
                    var affectedActors = handler.GetAffectedActors().ToArray();
                    handler.SetAffectedActors(affectedActors);

                    var id = Caster.PopNextBuffId();
                    var buff = new DelayBuff(id, Caster, Caster, handler.Dice, Spell, false, false, BuffTrigger)
                    {
                        Duration = (short)handler.Dice.Delay,
                        Token = handler
                    };

                    Caster.AddAndApplyBuff(buff);
                }
                else
                    handler.Apply();
            }
        }

        public void BuffTrigger(DelayBuff buff, object token)
        {
            if (Fight.State == FightState.Ended)
                return;

            if (token is SpellEffectHandler)
                ((SpellEffectHandler)token).Apply();
        }

        public override IEnumerable<SpellEffectHandler> GetEffectHandlers()
        {
            return Handlers;
        }
    }
}