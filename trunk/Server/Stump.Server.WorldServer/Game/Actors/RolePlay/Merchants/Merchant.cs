using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Merchants;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants
{
    public class Merchant : NamedActor
    {
        public const short BAG_SKIN = 237;

        private readonly WorldMapMerchantRecord m_record;
        private readonly List<MerchantShopDialog> m_openedDialogs = new List<MerchantShopDialog>();
        private bool m_isRecordDirty;


        public Merchant(Character character)
        {
            var look = character.Look.Copy();
            look.subentities = new List<SubEntity>(look.subentities)
                {
                    new SubEntity((int) SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_MERCHANT_BAG, 0,
                                  new EntityLook(BAG_SKIN, new short[0], new int[0], new short[0], new SubEntity[0]))
                };

            m_record = new WorldMapMerchantRecord()
                {
                    CharacterId = character.Id,
                    AccountId = character.Account.Id,
                    Name = character.Name,
                    Map = character.Map,
                    Cell = character.Cell.Id,
                    Direction = (int) character.Direction,
                    EntityLook = look,
                    IsActive = true,
                    MerchantSince = DateTime.Now,
                };

            Bag = new MerchantBag(this, character.MerchantBag);
            Position = character.Position.Clone();
        }

        public Merchant(WorldMapMerchantRecord record)
        {
            m_record = record;
            Bag = new MerchantBag(this);

            if (record.Map == null)
                throw new Exception("Merchant's map not found");

            Position = new ObjectPosition(
                record.Map,
                record.Map.Cells[m_record.Cell],
                (DirectionsEnum)m_record.Direction);
        }

        public WorldMapMerchantRecord Record
        {
            get
            {
                return m_record;
            }
        }

        public ReadOnlyCollection<MerchantShopDialog> OpenDialogs
        {
            get { return m_openedDialogs.AsReadOnly(); }
        }

        public override int Id
        {
            get { return m_record.CharacterId; }
            protected set { m_record.CharacterId = value; }
        }

        public override ObjectPosition Position
        {
            get;
            protected set;
        }

        public MerchantBag Bag
        {
            get;
            protected set;
        }

        public override EntityLook Look
        {
            get { return m_record.EntityLook; }
            set { m_record.EntityLook = value; }
        }

        public override string Name
        {
            get { return m_record.Name; }
        }

        public uint KamasEarned
        {
            get { return m_record.KamasEarned; }
            set { m_record.KamasEarned = value; }
        }

        public bool IsRecordDirty
        {
            get { return m_isRecordDirty || Bag.IsDirty; }
            set { m_isRecordDirty = value; }
        }

        protected override void OnDisposed()
        {
            m_record.IsActive = false;

            foreach (var dialog in OpenDialogs.ToArray())
            {
                dialog.Close();
            }

            base.OnDisposed();
        }

        public override bool CanBeSee(Maps.WorldObject byObj)
        {
            return base.CanBeSee(byObj) && !IsBagEmpty();
        }

        public bool IsBagEmpty()
        {
            return Bag.Count == 0;
        }

        public void LoadRecord()
        {
            Bag.LoadRecord();
        }

        public void Save()
        {
            if (Bag.IsDirty)
                Bag.Save();

            WorldServer.Instance.DBAccessor.Database.Update(m_record);
        }

        public bool IsMerchantOwner(WorldAccount account)
        {
            return account.Id == m_record.AccountId;
        }

        public void OnDialogOpened(MerchantShopDialog dialog)
        {
            m_openedDialogs.Add(dialog);
        }

        public void OnDialogClosed(MerchantShopDialog dialog)
        {
            m_openedDialogs.Remove(dialog);
        }

        #region Network

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayMerchantInformations(Id, Look, GetEntityDispositionInformations(), Name, 0);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Id);
        }
    }
}