using System;
using System.Collections.Generic;
namespace MineSweeper
{
    class GameField
    {
        public GameField()
        {
            is_bomb = false;
            is_open = false;
            bomb_counter = 0;
            upper_layer = Constants.SQUARE;

        }
        //перабіць
        public bool is_bomb;
        public bool is_open;
        public int bomb_counter;
        //{
        //    set
        //    {
        //        if (value >= 0 && value < 8)
        //            bomb_counter = value;
        //        else
        //            Environment.Exit((int)ErrorCodes.WrongData);
        //    }
        //    get
        //    {
        //        return bomb_counter;
        //    }
        //}
        public string? upper_layer;
        //{
        //    get
        //    {
        //        return upper_layer;
        //    }
        //    set
        //    {
        //        if (value == Constants.BOOM ||
        //           value == Constants.FLAG ||
        //           value == Constants.SQUARE ||
        //           value == " " ||
        //           value == bomb_counter.ToString())
        //            upper_layer = value;
        //        else
        //            Environment.Exit((int)ErrorCodes.WrongData);

        //    }
        //} 
    }

    class MineSwepper
    {
        public GameField[,]? game_field;
        private Pair<uint, uint> size;
        private int bomb_chance;
        private int amount_of_bombs;
        private Pair<int, int> chosen_square = new(-1, -1);

        public void StartGame()
        {
            Console.WriteLine("Please choose dificult: ");
            Console.WriteLine("1. Easy\n" + "2. Middle\n" + "3. Hard");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case Constants.EASY_MODE:
                    size = Constants.EASY_SIZE_MAP;
                    bomb_chance = Constants.EASY_BOMB_CHANCE;
                    break;
                case Constants.MIDD_MODE:
                    size = Constants.MIDD_SIZE_MAP;
                    bomb_chance = Constants.MIDD_BOMB_CHANCE;
                    break;
                case Constants.HARD_MODE:
                    size = Constants.HARD_SIZE_MAP;
                    bomb_chance = Constants.HARD_BOMB_CHANCE;
                    break;
                default:

                    Environment.Exit((int)ErrorCodes.WrongInput);
                    return;
            }
            InitGameField();
            Game();
        }

        private void InitGameField()
        {
            game_field = new GameField[size.first, size.second];
            for (int i = 0; i < size.first; ++i)
            {
                for (int j = 0; j < size.second; ++j)
                    game_field[i, j] = new();
            }
            Print();
            SelectSquare();

            Random rand = new();
            int is_bomb;
            for (int i = 0; i < size.first; ++i)
            {
                for (int j = 0; j < size.first; ++j)
                {
                    is_bomb = rand.Next() % bomb_chance;
                    if (is_bomb == 0 &&
                        (i != chosen_square.first ||
                        j != chosen_square.second))
                    {
                        game_field[i, j].is_bomb = true;
                        ++amount_of_bombs;
                    }

                }
            }
        }

        private void Print()
        {
            if (game_field == null)
                Exit.WithErrorCode((int)ErrorCodes.NullValue);
            Console.Clear();
            Console.WriteLine("{0}:{1}", Constants.BOMB, amount_of_bombs);
            for (int i = 0; i < size.first; ++i)
            {
                for (int j = 0; j < size.second; ++j)
                {
                    switch (game_field[i, j].upper_layer)
                    {
                        case "1":
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case "2":
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case "3":
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case "4":
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            break;
                        case "5":
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        default:
                            Console.ResetColor();
                            break;

                    }
                    if (i == chosen_square.first && j == chosen_square.second)
                        Console.Write(Constants.SELECTED_SQUARE);
                    else
                        Console.Write(game_field[i, j].upper_layer);
                    Console.ResetColor();
                }
                Console.Write('\n');
            }
        }
        private void Game()
        {
            while (!IsBombOpen())
            {
                Print();
                SelectSquare();
            }
        }
        private bool IsBombOpen()
        {
            if (game_field == null)
                Exit.WithErrorCode((int)ErrorCodes.NullValue);
            // IsNull();
            for (int i = 0; i < size.first; ++i)
            {
                for (int j = 0; j < size.second; ++j)
                {
                    if (game_field[i, j].is_bomb == true &&
                        game_field[i, j].is_open == true)
                        return true;
                }
            }
            return false;
        }
        private void SelectSquare()
        {
            // IsNull();
            if (game_field == null)
                Exit.WithErrorCode((int)ErrorCodes.NullValue);
            bool is_open = false;
            do
            {
                if (is_open)
                {
                    Console.WriteLine("This square is already open.");
                }
                Console.WriteLine("Choose square: (x;y)");
                chosen_square.first = Convert.ToInt32(Console.ReadLine());
                chosen_square.second = Convert.ToInt32(Console.ReadLine());
                if (chosen_square.first < 0 || chosen_square.second < 0)
                    Exit.WithErrorCode((int)ErrorCodes.NegativeValue);

                is_open = game_field[chosen_square.first, chosen_square.second].is_open;
            } while (is_open);
        }

    }
}


