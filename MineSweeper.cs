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
        private int bomb_counter;

        public int BombCounter
        {
            set
            {
                if (value >= 0 && value < 8)
                    bomb_counter = value;
                else
                    throw new Exception("Wrong Data");
            }
            get
            {
                return bomb_counter;
            }
        }
        private string upper_layer;
        public string UpperLayer
        {
            get
            {
                return upper_layer;
            }
            set
            {
                if (value == Constants.BOOM ||
                   value == Constants.FLAG ||
                   value == Constants.SQUARE ||
                   value == Constants.QUESTION ||
                   value == " " ||
                   value == bomb_counter.ToString())
                    upper_layer = value;
                else
                    throw new Exception("Wrong Data");

            }
        }
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
                    throw new Exception("Wrong Input");
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
            for (int i = 0; i < size.first; ++i)  // перарабіць генерацыю бомбаў
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
            int bomb_counter = 0;
            for (int i = 0; i < size.first; ++i)
            {
                for (int j = 0; j < size.first; ++j)
                {
                    for (int k = -1; k < 2; ++k)
                    {
                        for (int l = -1; l < 2; ++l)
                        {
                            if (i + k < 0 ||
                               j + l < 0 ||
                               i + k >= size.first ||
                               j + l >= size.second ||
                               game_field[i, j].is_bomb)
                                continue;

                            if (game_field[i + k, j + l].is_bomb)
                                ++bomb_counter;

                        }
                    }
                    game_field[i, j].BombCounter = bomb_counter;

                }
            }

        }

        private void Print()
        {
            if (game_field == null)
                throw new Exception("Null Value");
            Console.Clear();
            Console.WriteLine("{0}:{1}", Constants.BOMB, amount_of_bombs);
            for (int i = 0; i < size.first; ++i)
            {
                for (int j = 0; j < size.second; ++j)
                {
                    switch (game_field[i, j].UpperLayer)
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
                        Console.Write(game_field[i, j].UpperLayer);
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
                throw new Exception("Null Value");

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

            if (game_field == null)
                throw new Exception("Null Value");
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
                    throw new Exception("Negative Value");

                is_open = game_field[chosen_square.first, chosen_square.second].is_open;
            } while (is_open);
        }
        private void SquareOptions()
        {
            int choice = 0;
            bool complete_flag = false;
            while (!complete_flag)
            {
                Console.WriteLine("Please choose next options: ");
                Console.WriteLine("1. Open. ");
                Console.WriteLine("2. Defuse. ");
                Console.WriteLine("3. Set question.");
                Console.WriteLine("0. Change choice.");
                Console.Write("Your input: ");
                choice = Convert.ToInt32(Console.ReadLine());


            }
        }
    }
}


