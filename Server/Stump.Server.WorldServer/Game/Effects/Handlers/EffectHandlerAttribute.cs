using Stump.DofusProtocol.Enums;
using System;

namespace Stump.Server.WorldServer.Game.Effects.Handlers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class EffectHandlerAttribute : Attribute
    {
        public EffectHandlerAttribute(EffectsEnum effect)
        {
            Effect = effect;
        }

        public EffectsEnum Effect
        {
            get;
            private set;
        }
    }
}