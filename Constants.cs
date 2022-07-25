using System;
namespace MineSweeper
{
    public static class Constants
    {
        public const string BOOM = "💥";
        public const string BOMB = "💣";
        public const string FLAG = "🚩";
        public const string SQUARE = "⬜️";
        public const string SELECTED_SQUARE = "🔳";
        public const string QUESTION = "❓";

        public const int BOMB_CHANCE = 8;

        public const int EASY_BOMB_AMOUNT = 10;
        public const int MIDD_BOMB_AMOUNT = 32;
        public const int HARD_BOMB_AMOUNT = 60;

        public static readonly Pair<uint, uint> EASY_SIZE_MAP
        = new(9, 9);
        public static readonly Pair<uint, uint> MIDD_SIZE_MAP
        = new(16, 16);
        public static readonly Pair<uint, uint> HARD_SIZE_MAP
        = new(16, 30);

        public const int EASY_MODE = 1;
        public const int MIDD_MODE = 2;
        public const int HARD_MODE = 3;

    }

}

