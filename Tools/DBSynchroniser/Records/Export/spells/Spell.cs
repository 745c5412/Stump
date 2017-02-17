 


// Generated on 02/14/2017 17:01:41
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Spells")]
    [D2OClass("Spell", "com.ankamagames.dofus.datacenter.spells")]
    public class SpellRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Spells";
        public int id;
        [I18NField]
        public uint nameId;
        [I18NField]
        public uint descriptionId;
        public uint typeId;
        public uint order;
        public String scriptParams;
        public String scriptParamsCritical;
        public int scriptId;
        public int scriptIdCritical;
        public int iconId;
        public List<uint> spellLevels;
        public List<int> variants;
        public Boolean useParamCache = true;
        public Boolean verbose_cast;
        public uint obtentionLevel;
        public Boolean useSpellLevelScaling;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        [D2OIgnore]
        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        [D2OIgnore]
        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        [D2OIgnore]
        [NullString]
        public String ScriptParams
        {
            get { return scriptParams; }
            set { scriptParams = value; }
        }

        [D2OIgnore]
        [NullString]
        public String ScriptParamsCritical
        {
            get { return scriptParamsCritical; }
            set { scriptParamsCritical = value; }
        }

        [D2OIgnore]
        public int ScriptId
        {
            get { return scriptId; }
            set { scriptId = value; }
        }

        [D2OIgnore]
        public int ScriptIdCritical
        {
            get { return scriptIdCritical; }
            set { scriptIdCritical = value; }
        }

        [D2OIgnore]
        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> SpellLevels
        {
            get { return spellLevels; }
            set
            {
                spellLevels = value;
                m_spellLevelsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_spellLevelsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] SpellLevelsBin
        {
            get { return m_spellLevelsBin; }
            set
            {
                m_spellLevelsBin = value;
                spellLevels = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> Variants
        {
            get { return variants; }
            set
            {
                variants = value;
                m_variantsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_variantsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] VariantsBin
        {
            get { return m_variantsBin; }
            set
            {
                m_variantsBin = value;
                variants = value == null ? null : value.ToObject<List<int>>();
            }
        }

        [D2OIgnore]
        public Boolean UseParamCache
        {
            get { return useParamCache; }
            set { useParamCache = value; }
        }

        [D2OIgnore]
        public Boolean Verbose_cast
        {
            get { return verbose_cast; }
            set { verbose_cast = value; }
        }

        [D2OIgnore]
        public uint ObtentionLevel
        {
            get { return obtentionLevel; }
            set { obtentionLevel = value; }
        }

        [D2OIgnore]
        public Boolean UseSpellLevelScaling
        {
            get { return useSpellLevelScaling; }
            set { useSpellLevelScaling = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Spell)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            DescriptionId = castedObj.descriptionId;
            TypeId = castedObj.typeId;
            Order = castedObj.order;
            ScriptParams = castedObj.scriptParams;
            ScriptParamsCritical = castedObj.scriptParamsCritical;
            ScriptId = castedObj.scriptId;
            ScriptIdCritical = castedObj.scriptIdCritical;
            IconId = castedObj.iconId;
            SpellLevels = castedObj.spellLevels;
            Variants = castedObj.variants;
            UseParamCache = castedObj.useParamCache;
            Verbose_cast = castedObj.verbose_cast;
            ObtentionLevel = castedObj.obtentionLevel;
            UseSpellLevelScaling = castedObj.useSpellLevelScaling;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Spell)parent : new Spell();
            obj.id = Id;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            obj.typeId = TypeId;
            obj.order = Order;
            obj.scriptParams = ScriptParams;
            obj.scriptParamsCritical = ScriptParamsCritical;
            obj.scriptId = ScriptId;
            obj.scriptIdCritical = ScriptIdCritical;
            obj.iconId = IconId;
            obj.spellLevels = SpellLevels;
            obj.variants = Variants;
            obj.useParamCache = UseParamCache;
            obj.verbose_cast = Verbose_cast;
            obj.obtentionLevel = ObtentionLevel;
            obj.useSpellLevelScaling = UseSpellLevelScaling;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_spellLevelsBin = spellLevels == null ? null : spellLevels.ToBinary();
            m_variantsBin = variants == null ? null : variants.ToBinary();
        
        }
    }
}