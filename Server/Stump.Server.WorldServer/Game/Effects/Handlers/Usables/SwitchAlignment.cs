using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [EffectHandler(EffectsEnum.Effect_SwitchAlignment)]
    public class SwitchAlignment : UsableEffectHandler
    {
        public SwitchAlignment(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        public override bool Apply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectInteger;

            if (integerEffect == null)
                return false;

            UsedItems = 1;
            Target.ChangeAlignementSide((AlignmentSideEnum)integerEffect.Value);

            return true;
        }
    }
}