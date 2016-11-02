using Stump.Core.Attributes;
using Stump.Core.Mathematics;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Mounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Items.Player.Custom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts
{
    public class MountManager : DataManager<MountManager>, ISaveable
    {
        [Variable]
        public static int MountStorageValidityDays = 40;

        public static TimeSpan MountStorageValidity => TimeSpan.FromDays(MountStorageValidityDays);

        private Dictionary<int, MountTemplate> m_mountTemplates;
        private Dictionary<int, MountRecord> m_mounts;

        [Initialization(InitializationPass.Sixth)]
        public override void Initialize()
        {
            m_mountTemplates = Database.Query<MountTemplate>(MountTemplateRelator.FetchQuery).ToDictionary(entry => entry.Id);
            var mountBonus = Database.Query<MountBonus>(MountBonusRelator.FetchQuery).ToDictionary(entry => entry.Id);

            foreach (var mountTemplate in m_mountTemplates)
            {
                mountTemplate.Value.Bonuses.AddRange(mountBonus.Where(x => x.Value.MountTemplateId == mountTemplate.Key)
                    .Select(x => x.Value));
            }

            Database.Execute(string.Format(MountRecordRelator.DeleteStoredSince, (DateTime.Now - MountStorageValidity).ToString("yyyy-MM-dd HH:mm:ss.fff")));
            m_mounts = Database.Query<MountRecord>(MountRecordRelator.FetchQuery).ToDictionary(x => x.Id);

            World.Instance.RegisterSaveableInstance(this);
        }

        public MountTemplate[] GetTemplates() => m_mountTemplates.Values.ToArray();

        public MountTemplate GetTemplate(int id)
        {
            MountTemplate result;
            return !m_mountTemplates.TryGetValue(id, out result) ? null : result;
        }

        public MountTemplate GetTemplateByScrollId(int scrollId) => m_mountTemplates.FirstOrDefault(x => x.Value.ScrollId == scrollId).Value;

        public void AddMount(MountRecord record)
        {
            if (!m_mounts.ContainsKey(record.Id))
                m_mounts.Add(record.Id, record);
        }

        public void RemoveMount(MountRecord record)
        {
            m_mounts.Remove(record.Id);
        }

        public void SaveMount(MountRecord record)
        {
            if (record.IsNew)
                Database.Insert(record);
            else
                Database.Update(record);

            record.IsDirty = false;
            record.IsNew = false;
        }

        public void DeleteMount(MountRecord record)
        {
            RemoveMount(record);
            Database.Delete(record);
        }

        public MountRecord GetMount(int mountId)
        {
            MountRecord record;
            if (!m_mounts.TryGetValue(mountId, out record))
                return null;

            return record;
        }

        public List<MountRecord> GetMounts(int ownerId) => m_mounts.Where(x => x.Value.OwnerId == ownerId).Select(x => x.Value).ToList();

        private static short GetBonusByLevel(int finalBonus, int level) => (short)Math.Floor(finalBonus * level / 100d);

        public List<EffectInteger> GetMountEffects(Mount mount) => mount.Template.Bonuses.Select(x => new EffectInteger((EffectsEnum)x.EffectId, GetBonusByLevel(x.Amount, mount.Level))).ToList();

        public Mount CreateMount(Character owner, MountTemplate template)
        {
            var rand = new CryptoRandom();
            return CreateMount(owner, template, rand.Next(2) == 1);
        }

        public Mount CreateMount(Character owner, MountTemplate template, bool sex)
        {
            var record = new MountRecord
            {
                IsNew = true,
                TemplateId = template.Id,
                OwnerId = owner.Id,
                OwnerName = owner.Name,
                Name = template.Name,
                Sex = sex,
            };
            record.AssignIdentifier();

            AddMount(record);

            return new Mount(owner, record);
        }

        public BasePlayerItem StoreMount(Character character, Mount mount)
        {
            // null effect bypass initialization
            var item = ItemManager.Instance.CreatePlayerItem(character, mount.ScrollItem, 1, new List<EffectBase> { new EffectBase(-1, new EffectBase()) }) as MountCertificate;

            if (item == null)
                throw new Exception($"Item {mount.ScrollItem} type isn't MountCertificate");

            item.InitializeEffects(mount);
            return character.Inventory.AddItem(item);
        }

        public void Save()
        {
            foreach (var mount in m_mounts)
                SaveMount(mount.Value);
        }
    }
}