using System;
namespace MineSwepper
{
    public static class Constants
    {
        public const string BOOM = "💥";
        public const string BOMB = "💣";
        public const string FLAG = "🚩";
        public const string SQUARE = "⬜️";
        public const string SELECTED_SQUARE = "🔳";

        public const int EASY_BOMB_CHANCE = 5;
        public const int MIDD_BOMB_CHANCE = 6;
        public const int HARD_BOMB_CHANCE = 8;

        // прадумаць памеры!!!
        public static readonly Pair<uint, uint> EASY_SIZE_MAP
        = new(7, 7);
        public static readonly Pair<uint, uint> MIDD_SIZE_MAP
        = new(15, 15);
        public static readonly Pair<uint, uint> HARD_SIZE_MAP
        = new(16, 30);

        public const int EASY_MODE = 1;
        public const int MIDD_MODE = 2;
        public const int HARD_MODE = 3;

    }
    enum ErrorCodes
    {
        WrongInput = 1,
        WrongData,
        NullValue,
        NegativeValue,
    }
}

