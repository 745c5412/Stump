﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;
using NetworkGuildEmblem = Stump.DofusProtocol.Types.GuildEmblem;
using TaxCollectorSpawn = Stump.Server.WorldServer.Database.World.WorldMapTaxCollectorRecord;

namespace Stump.Server.WorldServer.Game.Guilds
{
    public class GuildManager : DataManager<GuildManager>, ISaveable
    {
        private UniqueIdProvider m_idProvider;
        private Dictionary<int, Guild> m_guilds;
        private Dictionary<int, EmblemRecord> m_emblems;
        private Dictionary<int, GuildMember> m_guildsMembers;
        private readonly Stack<Guild> m_guildsToDelete = new Stack<Guild>();

        private readonly object m_lock = new object();

        [Initialization(InitializationPass.Sixth)]
        public override void Initialize()
        {            
            m_emblems = Database.Query<EmblemRecord>(EmblemRelator.FetchQuery).ToDictionary(x => x.Id);
            m_guilds = Database.Query<GuildRecord>(GuildRelator.FetchQuery).ToList().Select(x => new Guild(x, FindGuildMembers(x.Id))).ToDictionary(x => x.Id);
            m_guildsMembers = m_guilds.Values.SelectMany(x => x.Members).ToDictionary(x => x.Id);
            m_idProvider = m_guilds.Any() ? new UniqueIdProvider(m_guilds.Select(x => x.Value.Id).Max()) : new UniqueIdProvider(1);

            World.Instance.RegisterSaveableInstance(this);
        }

        public bool DoesNameExist(string name)
        {
            return m_guilds.Any(x => String.Equals(x.Value.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool DoesEmblemExist(NetworkGuildEmblem emblem)
        {
            return m_guilds.Any(x => x.Value.Emblem.DoesEmblemMatch(emblem));
        }

        public bool DoesEmblemExist(GuildEmblem emblem)
        {
            return m_guilds.Any(x => x.Value.Emblem.DoesEmblemMatch(emblem));
        }

        public GuildMember[] FindGuildMembers(int guildId)
        {
            return Database.Fetch<GuildMemberRecord, CharacterRecord, GuildMemberRecord>(new GuildMemberRelator().Map,
                string.Format(GuildMemberRelator.FetchByGuildId, guildId)).Select(x => new GuildMember(x)).ToArray();
        }

        public Guild TryGetGuild(int id)
        {
            lock (m_lock)
            {
                Guild guild;
                return m_guilds.TryGetValue(id, out guild) ? guild : null;
            }
        }

        public Guild TryGetGuild(string name)
        {
            lock (m_lock)
            {
                return m_guilds.FirstOrDefault(x => String.Equals(x.Value.Name, name, StringComparison.CurrentCultureIgnoreCase)).Value;
            }
        }

        public EmblemRecord TryGetEmblem(int id)
        {
            EmblemRecord record;
            return m_emblems.TryGetValue(id, out record) ? record : null;
        }

        public GuildMember TryGetGuildMember(int characterId)
        {
            lock (m_lock)
            {
                GuildMember guildMember;
                return m_guildsMembers.TryGetValue(characterId, out guildMember) ? guildMember : null;
            }
        }

        public Guild CreateGuild(string name)
        {
            lock (m_lock)
            {
                var guild = new Guild(m_idProvider.Pop(), name);
                m_guilds.Add(guild.Id, guild);

                return guild;
            }
        }

        public GuildCreationResultEnum CreateGuild(Character character, string name, NetworkGuildEmblem emblem)
        {
            var guildalogemme = character.Inventory.TryGetItem(ItemManager.Instance.TryGetTemplate(ItemIdEnum.Guildalogem));
            if (guildalogemme == null)
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_REQUIREMENT_UNMET;

            if (!Regex.IsMatch(name, "^[A-Z][a-z]{2,9}(?:-[A-Z][a-z]{2,9}|[a-z]{1,10})$", RegexOptions.Compiled))
            {
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_NAME_INVALID;
            }

            if (DoesNameExist(name))
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_NAME_ALREADY_EXISTS;

            if (DoesEmblemExist(emblem))
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_EMBLEM_ALREADY_EXISTS;

            character.Inventory.RemoveItem(guildalogemme, 1);
            character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, 1, guildalogemme.Template.Id);

            var guild = CreateGuild(name);
            if (guild == null)
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_CANCEL;

            guild.Emblem.ChangeEmblem(emblem);

            GuildMember member;
            if (!guild.TryAddMember(character, out member))
            {
                DeleteGuild(guild);
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_CANCEL;
            }

            character.GuildMember = member;
            character.RefreshActor();

            return GuildCreationResultEnum.GUILD_CREATE_OK;
        }

        public bool DeleteGuild(Guild guild)
        {
            lock (m_lock)
            {
                guild.RemoveTaxCollectors();

                m_guilds.Remove(guild.Id);
                m_guildsToDelete.Push(guild);

                return true;
            }
        }

        public void RegisterGuildMember(GuildMember member)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(
                () => Database.Insert(member.Record));

            lock (m_lock)
            {
                m_guildsMembers.Add(member.Id, member);
            }
        }

        public bool DeleteGuildMember(GuildMember member)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(
                () => Database.Delete(member.Record));

            lock (m_lock)
            {
                m_guildsMembers.Remove(member.Id);
                return true;
            }
        }

        public void Save()
        {
            lock (m_lock)
            {
                foreach (var guild in m_guilds.Values.Where(guild => guild.IsDirty))
                {
                    guild.Save(Database);
                }

                while (m_guildsToDelete.Count > 0)
                {
                    var guild = m_guildsToDelete.Pop();

                    Database.Delete(guild.Record);
                }
            }
        }
    }
}