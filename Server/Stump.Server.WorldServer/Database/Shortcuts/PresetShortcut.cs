﻿using Stump.DofusProtocol.Types;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer.Database.Characters;

namespace Stump.Server.WorldServer.Database.Shortcuts
{
    public class PresetShortcutRelator
    {
        public static string FetchQuery = "SELECT * FROM characters_shortcuts_items_presets";

        /// <summary>
        /// Use string.Format
        /// </summary>
        public static string FetchByOwner = "SELECT * FROM characters_shortcuts_items_presets WHERE OwnerId={0}";
    }

    [TableName("characters_shortcuts_items_presets")]
    public class PresetShortcut : ItemShortcut
    {
        public PresetShortcut(CharacterRecord owner, int slot, int itemTemplateId, int itemGuid, int presetId)
            : base(owner, slot, itemTemplateId, itemGuid)
        {
            PresetId = presetId;
        }

        private int m_presetId;

        public int PresetId
        {
            get { return m_presetId; }
            set
            {
                m_presetId = value;
                IsDirty = true;
            }
        }

        public override DofusProtocol.Types.Shortcut GetNetworkShortcut()
        {
            return new ShortcutObjectPreset(Slot, (sbyte)PresetId);
        }
    }
}
