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
            is_marked = false;
            bomb_counter = 0;
            upper_layer = Constants.SQUARE;

        }
        //перабіць
        public bool is_bomb;
        public bool is_open;

        public bool is_marked;
        private int bomb_counter;

        public int BombCounter
        {
            set
            {
                if (value >= 0 && value < 8)
                    bomb_counter = value;
                else
                    throw new Exception("Wrong Data func: Seter of BombCounter");
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
                   value == "  " ||
                   value == bomb_counter.ToString() + " ")
                    upper_layer = value;
                else
                    throw new Exception("Wrong Data func: Seter of UpperLayer");

            }
        }
        public void OpenBombCounter()
        {
            upper_layer = bomb_counter.ToString() + " ";
        }
    }

    class MineSwepper
    {
        public GameField[,]? game_field;
        private Pair<uint, uint> size;
        private int bomb_amount;
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
                    bomb_amount = Constants.EASY_BOMB_AMOUNT;
                    break;
                case Constants.MIDD_MODE:
                    size = Constants.MIDD_SIZE_MAP;
                    bomb_amount = Constants.MIDD_BOMB_AMOUNT;
                    break;
                case Constants.HARD_MODE:
                    size = Constants.HARD_SIZE_MAP;
                    bomb_amount = Constants.HARD_BOMB_AMOUNT;
                    break;
                default:
                    throw new Exception("Wrong Input func StartGame");
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
            int bomb_amount_copy = bomb_amount;
            // int is_bomb;
            // while (bomb_amount_copy != 0)
            // {
            //     for (int i = 0; i < size.first && bomb_amount_copy != 0; ++i) 
            //     {
            //         for (int j = 0; j < size.first; ++j)
            //         {
            //             if (bomb_amount_copy == 0)
            //                 break;
            //             is_bomb = rand.Next() % Constants.BOMB_CHANCE;
            //             if (is_bomb == 0 &&
            //                 !game_field[i, j].is_bomb &&
            //                 (i != chosen_square.first ||
            //                 j != chosen_square.second))
            // {
            //     game_field[i, j].is_bomb = true;
            //     --bomb_amount_copy;
            // }

            //         }
            //     }
            // }
            {
                int i = 0;
                int j = 0;
                while (bomb_amount_copy != 0)
                {
                    i = (int)(rand.Next() % size.first);
                    j = (int)(rand.Next() % size.second);
                    if (!game_field[i, j].is_bomb &&
                               (i != chosen_square.first ||
                               j != chosen_square.second))
                    {
                        game_field[i, j].is_bomb = true;
                        --bomb_amount_copy;
                    }

                }
            }

            int bomb_counter = 0;
            for (int i = 0; i < size.first; ++i)
            {
                for (int j = 0; j < size.first; ++j)
                {
                    bomb_counter = 0;
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
                throw new Exception("Null Value func: Print");
            Console.Clear();
            Console.WriteLine("{0}:{1} \t\t Current Position: ({2}:{3})",
             Constants.BOMB, bomb_amount, chosen_square.first, chosen_square.second);
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
                    if (i == chosen_square.first && j == chosen_square.second &&
                        !game_field[i, j].is_open &&
                        !game_field[i, j].is_marked)
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
            // while (!IsBombOpen())
            while (true)
            {
                Print();
                SelectSquare();
                SquareOptions();
            }
        }
        private bool IsBombOpen()
        {
            if (game_field == null)
                throw new Exception("Null Value func: IsBombOpen");

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
                throw new Exception("Null Value func: SelectSquare");
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
            Print();
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
                switch (choice)
                {
                    case 1:
                        OpenSquare();
                        complete_flag = true;
                        break;
                    case 2:
                        DefuseSquare();
                        complete_flag = true;
                        break;
                    case 3:
                        SetQuestion();
                        complete_flag = true;
                        break;
                    case 0:
                        SelectSquare();
                        break;
                    default:
                        break;
                }
            }
        }
        private void OpenSquare()
        {
            if (game_field == null)
                throw new Exception("Null Value func: OpenSquare");
            int i = chosen_square.first;
            int j = chosen_square.second;
            if (game_field[i, j].is_bomb)
            {
                game_field[i, j].UpperLayer = Constants.BOOM; // temporary solution
                // Final Output
            }
            else if (game_field[i, j].BombCounter != 0)
            {
                game_field[i, j].OpenBombCounter();
            }
            else
            {
                game_field[i, j].UpperLayer = "  "; // temporary solution 
            }
            game_field[i, j].is_open = true;
        }
        private void DefuseSquare()
        {
            if (game_field == null)
                throw new Exception("Null Value func: OpenSquare");
            int i = chosen_square.first;
            int j = chosen_square.second;
            game_field[i, j].UpperLayer = Constants.FLAG;
            game_field[i, j].is_marked = true;
            --bomb_amount;
        }
        private void SetQuestion()
        {
            if (game_field == null)
                throw new Exception("Null Value func: OpenSquare");
            int i = chosen_square.first;
            int j = chosen_square.second;
            game_field[i, j].UpperLayer = Constants.QUESTION;
            game_field[i, j].is_marked = true;
        }

    }
}


