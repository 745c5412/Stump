using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System.Collections.Generic;
using System.Linq;

namespace ArkalysPlugin
{
    public static class PVPTitles
    {
        static Dictionary<int, short> m_angelTitles = new Dictionary<int, short>
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

        static Dictionary<int, short> m_evilTitles = new Dictionary<int, short>
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

        static Dictionary<int, short> m_mercenaryTitles = new Dictionary<int, short>
        {
            { 1, 311 },
            { 2, 312 },
            { 3, 313 },
            { 4, 314 },
            { 5, 315 },
            { 6, 316 },
            { 7, 317 },
            { 8, 318 },
            { 9, 319 },
            { 10, 320 }
        };

        [Initialization(typeof(World), Silent = true)]
        public static void Initialize()
        {
            World.Instance.CharacterJoined += OnCharacterJoined;
            World.Instance.CharacterLeft += OnCharacterLeft;
        }

        static void OnCharacterJoined(Character character)
        {
            ResetTitles(character);

            character.GradeChanged += OnGradeChanged;
            character.AligmenentSideChanged += OnAlignementSideChanged;
            character.PvPToggled += OnPvPToggled;
        }

        static void OnCharacterLeft(Character character)
        {
            character.GradeChanged -= OnGradeChanged;
            character.AligmenentSideChanged -= OnAlignementSideChanged;
            character.PvPToggled -= OnPvPToggled;
        }

        static void OnPvPToggled(Character character, bool enabled)
        {
            ResetTitles(character);
        }

        static void OnGradeChanged(Character character, sbyte currentGrade, int difference)
        {
            ResetTitles(character);
        }

        static void OnAlignementSideChanged(Character character, AlignmentSideEnum side)
        {
            ResetTitles(character);
        }

        static void ResetTitles(Character character)
        {
            foreach (var title in m_angelTitles)
            {
                character.RemoveTitle(title.Value);
            }

            foreach (var title in m_evilTitles)
            {
                character.RemoveTitle(title.Value);
            }


            foreach (var title in m_mercenaryTitles)
            {
                character.RemoveTitle(title.Value);
            }

            if (!character.PvPEnabled)
                return;

            switch (character.AlignmentSide)
            {
                case AlignmentSideEnum.ALIGNMENT_ANGEL:
                    character.AddTitle(m_angelTitles.First(x => x.Key == character.AlignmentGrade).Value);
                    break;
                case AlignmentSideEnum.ALIGNMENT_EVIL:
                    character.AddTitle(m_evilTitles.First(x => x.Key == character.AlignmentGrade).Value);
                    break;
                case AlignmentSideEnum.ALIGNMENT_MERCENARY:
                    character.AddTitle(m_mercenaryTitles.First(x => x.Key == character.AlignmentGrade).Value);
                    break;
            }
        }
    }
}
