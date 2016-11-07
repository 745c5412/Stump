using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using System;

namespace Stump.Server.WorldServer.Game.Effects.Instances
{
    [Serializable]
    public class EffectString : EffectBase
    {
        public EffectString()
        {
        }

        public EffectString(EffectString copy)
            : this(copy.Id, copy.Text, copy)
        {
        }

        public EffectString(short id, string value, EffectBase effect)
            : base(id, effect)
        {
            Text = value;
        }

        public EffectString(EffectsEnum id, string value)
            : this((short)id, value, new EffectBase())
        {
        }

        public EffectString(EffectInstanceString effect)
            : base(effect)
        {
            Text = effect.text;
        }

        public override int ProtocoleId
        {
            get { return 74; }
        }

        public override byte SerializationIdenfitier
        {
            get
            {
                return 10;
            }
        }

        public string Text
        {
            get;
            set;
        }

        public override object[] GetValues()
        {
            return new object[] { Text };
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectString(Id, Text);
        }

        public override EffectInstance GetEffectInstance()
        {
            return new EffectInstanceString()
            {
                effectId = (uint)Id,
                targetId = (int)Targets,
                delay = Delay,
                duration = Duration,
                group = Group,
                random = Random,
                modificator = Modificator,
                trigger = Trigger,
                hidden = Hidden,
                zoneMinSize = ZoneMinSize,
                zoneSize = ZoneSize,
                zoneShape = (uint)ZoneShape,
                text = Text
            };
        }

        public override EffectBase GenerateEffect(EffectGenerationContext context, EffectGenerationType type = EffectGenerationType.Normal)
        {
            return new EffectString(this);
        }

        protected override void InternalSerialize(ref System.IO.BinaryWriter writer)
        {
            base.InternalSerialize(ref writer);

            writer.Write(Text);
        }

        protected override void InternalDeserialize(ref System.IO.BinaryReader reader)
        {
            base.InternalDeserialize(ref reader);

            Text = reader.ReadString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectString))
                return false;
            return base.Equals(obj) && Text == (obj as EffectString).Text;
        }

        public static bool operator ==(EffectString a, EffectString b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object)a == null) || ((object)b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectString a, EffectString b)
        {
            return !(a == b);
        }

        public bool Equals(EffectString other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other.Text, Text);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (Text != null ? Text.GetHashCode() : 0);
            }
        }
    }
}