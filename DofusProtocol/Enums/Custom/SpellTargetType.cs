using System;

namespace Stump.DofusProtocol.Enums
{
    [Flags]
    public enum SpellTargetType
    {
        NONE = 0,
        SELF = 0x1,
        ALLY_1 = 0x2,
        ALLY_2 = 0x4,
        ALLY_SUMMONS = 0x8,
        ALLY_STATIC_SUMMONS = 0x10,
        ALLY_BOMBS = 0x20, // not sure about that
        ALLY_SUMMONER = 0x40,
        ALLY_TURRETS = 0x80,
        ALLY_TELEFRAG = 0x100,
        ALLY_ALL = 0x2 | 0x4 | 0x8 | 0x10 | 0x20 | 0x40 | 0x80 | 0x100,
        ENEMY_1 = 0x200,
        ENEMY_2 = 0x400,
        ENEMY_SUMMONS = 0x800,
        ENEMY_STATIC_SUMMONS = 0x1000,
        ENEMY_BOMBS = 0x2000,
        ENEMY_SUMMONER = 0x4000,
        ENEMY_TURRETS = 0x8000,
        ENEMY_TELEFRAG = 0x10000,
        ENEMY_ALL = 0x200 | 0x400 | 0x800 | 0x1000 | 0x2000 | 0x4000 | 0x8000 | 0x10000,
        ALL = 0x7FFF,
        ALL_SUMMONS = 0x8 | 0x10 | 0x800 | 0x1000,
        ONLY_SELF = 0x20000,
    }
}