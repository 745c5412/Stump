﻿using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.DofusProtocol.Enums;
using Stump.ORM;
using Stump.ORM.Relator;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database.Mounts
{
    public class MountTemplateRelator : OneToManyRelator<MountTemplate, MountBonus>
    {
        public static string FetchQuery = "SELECT * FROM mounts_templates LEFT JOIN mounts_bonus ON mounts_templates.Id = mounts_bonus.MountTemplateId";
    }

    [TableName("mounts_templates")]
    [D2OClass("Mount", "com.ankamagames.dofus.datacenter.mounts")]
    public sealed class MountTemplate : IAssignedByD2O, IAutoGeneratedRecord, IOneToManyRecord1<MountBonus>
    {
        public const int DEFAULT_SCROLL_ITEM = (int) ItemIdEnum.CERTIFICAT_DE_MONTURE_7806;

        private ActorLook m_entityLook;
        private string m_name;
        private string m_lookAsString;
        private ItemTemplate m_scrollItem;


        [PrimaryKey("Id", false)]
        public int Id
        {
            get;
            set;
        }

        int IJoined.JoinedId => Id;

        public uint NameId
        {
            get;
            set;
        }

        [Ignore]
        public string Name
        {
            get { return m_name ?? (m_name = TextManager.Instance.GetText(NameId)); }
        }

        public string LookAsString
        {
            get
            {
                if (EntityLook == null)
                    return string.Empty;

                if (string.IsNullOrEmpty(m_lookAsString))
                    m_lookAsString = EntityLook.ToString();

                return m_lookAsString;
            }
            set
            {
                m_lookAsString = value;

                if (!string.IsNullOrEmpty(value) && value != "null")
                    m_entityLook = ActorLook.Parse(m_lookAsString);
                else
                    m_entityLook = null;
            }
        }

        [Ignore]
        public ActorLook EntityLook
        {
            get { return m_entityLook; }
            set
            {
                m_entityLook = value;

                if (value != null)
                    m_lookAsString = value.ToString();
            }
        }

        public uint ScrollId
        {
            get;
            set;
        }

        public ItemTemplate ScrollItem => m_scrollItem ?? (m_scrollItem = ItemManager.Instance.TryGetTemplate(ScrollId == 0 ? DEFAULT_SCROLL_ITEM : (int) ScrollId));

        public int PodsBase
        {
            get;
            set;
        }

        public int PodsPerLevel
        {
            get;
            set;
        }

        public int EnergyBase
        {
            get;
            set;
        }

        public int EnergyPerLevel
        {
            get;
            set;
        }

        public int MaturityBase
        {
            get;
            set;
        }

        public int FecondationTime
        {
            get;
            set;
        }

        public sbyte LearnCoefficient
        {
            get;
            set;
        }

        [Ignore]
        public List<MountBonus> Bonuses
        {
            get;
            set;
        } = new List<MountBonus>();

        List<MountBonus> IOneToManyRecord1<MountBonus>.ManyProperty1 => Bonuses;

        public int FamilyId
        {
            get;
            set;
        }

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var mount = (Mount)d2oObject;
            Id = (int)mount.id;
            NameId = mount.nameId;
            LookAsString = mount.look;
            FamilyId = (int)mount.FamilyId;
        }

        #endregion
    }
}
