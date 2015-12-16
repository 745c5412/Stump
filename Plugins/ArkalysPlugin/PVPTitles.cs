using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ArkalysPlugin
{
    public static class PVPTitles
    {
        private static Dictionary<int, short> m_angelTitles = new Dictionary<int, short>
        {
            { 1, 222 },
            { 2, 224 },
            { 3, 226 },
            { 4, 228 },
            { 5, 230 },
            { 6, 232 },
            { 7, 234 },
            { 8, 236 },
            { 9, 238 },
            { 10, 240 }
        };

        private static Dictionary<int, short> m_evilTitles = new Dictionary<int, short>
        {
            { 1, 223 },
            { 2, 225 },
            { 3, 227 },
            { 4, 229 },
            { 5, 231 },
            { 6, 233 },
            { 7, 235 },
            { 8, 237 },
            { 9, 239 },
            { 10, 241 }
        };

        [Initialization(typeof(World), Silent = true)]
        public static void Initialize()
        {
            World.Instance.CharacterJoined += OnCharacterJoined;
            World.Instance.CharacterLeft += OnCharacterLeft;
        }

        private static void OnCharacterJoined(Character character)
        {
            ResetTitles(character);

            character.GradeChanged += OnGradeChanged;
            character.AligmenentSideChanged += OnAlignementSideChanged;
            character.PvPToggled += OnPvPToggled;
        }

        private static void OnCharacterLeft(Character character)
        {
            character.GradeChanged -= OnGradeChanged;
            character.AligmenentSideChanged -= OnAlignementSideChanged;
            character.PvPToggled -= OnPvPToggled;
        }

        private static void OnPvPToggled(Character character, bool enabled)
        {
            ResetTitles(character);
        }

        private static void OnGradeChanged(Character character, sbyte currentGrade, int difference)
        {
            ResetTitles(character);
        }

        private static void OnAlignementSideChanged(Character character, AlignmentSideEnum side)
        {
            ResetTitles(character);
        }

        private static void ResetTitles(Character character)
        {
            foreach (var title in m_angelTitles)
            {
                character.RemoveTitle(title.Value);
            }

            foreach (var title in m_evilTitles)
            {
                character.RemoveTitle(title.Value);
            }

            if (!character.PvPEnabled)
                return;

            if (character.AlignmentSide != AlignmentSideEnum.ALIGNMENT_ANGEL && character.AlignmentSide != AlignmentSideEnum.ALIGNMENT_EVIL)
                return;

            var titles = character.AlignmentSide == AlignmentSideEnum.ALIGNMENT_ANGEL ? m_angelTitles : m_evilTitles;

            character.AddTitle(titles.First(x => x.Key == character.AlignmentGrade).Value);
        }
    }
}
