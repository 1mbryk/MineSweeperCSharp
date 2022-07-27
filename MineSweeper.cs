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
            is_defused = false;
            bomb_counter = 0;
            upper_layer = Constants.SQUARE;

        }
        public bool is_bomb;
        public bool is_open;
        public bool is_defused;
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
                    value == Constants.BOMB ||
                    value == Constants.FLAG ||
                    value == Constants.SQUARE ||
                    value == Constants.QUESTION ||
                    value == Constants.FALSE_DEFUSE ||
                    value == Constants.DEFUSED_BOMB ||
                    value == "  " ||
                    value == bomb_counter.ToString() + " ")
                    upper_layer = value;
                else
                    throw new Exception("Wrong Data func: Seter of UpperLayer");

            }
        }
        public void OpenSquare()
        {
            is_open = true;
            if (bomb_counter != 0)
                upper_layer = bomb_counter.ToString() + " ";
            else if (!is_bomb)
                upper_layer = "  ";
            else
                upper_layer = Constants.BOOM;

        }
    }

    class MineSwepper
    {
        public GameField[,]? game_field;
        private int bomb_amount;
        private int space;

        // coordinates
        int x = -1; // width
        int y = -1; // hight
        uint hight;
        uint width;

        public void StartGame()
        {
            Console.WriteLine("Please choose dificult: ");
            Console.WriteLine("1. Easy\n" + "2. Middle\n" + "3. Hard");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case Constants.EASY_MODE:
                    hight = Constants.EASY_SIZE_MAP.first;
                    width = Constants.EASY_SIZE_MAP.second;
                    bomb_amount = Constants.EASY_BOMB_AMOUNT;
                    space = Constants.EASY_SPACE;
                    break;
                case Constants.MIDD_MODE:
                    hight = Constants.MIDD_SIZE_MAP.first;
                    width = Constants.MIDD_SIZE_MAP.second;
                    bomb_amount = Constants.MIDD_BOMB_AMOUNT;
                    space = Constants.MIDD_SPACE;
                    break;
                case Constants.HARD_MODE:
                    hight = Constants.HARD_SIZE_MAP.first;
                    hight = Constants.HARD_SIZE_MAP.second;
                    bomb_amount = Constants.HARD_BOMB_AMOUNT;
                    space = Constants.HARD_SPACE;
                    break;
                default:
                    throw new Exception("Wrong Input func StartGame");
            }
            InitGameField();
            Game();
        }

        private void InitGameField()
        {
            game_field = new GameField[hight, width];
            for (int i = 0; i < hight; ++i)
            {
                for (int j = 0; j < width; ++j)
                    game_field[i, j] = new();
            }
            Print();
            SelectSquare();

            Random rand = new();
            int bomb_amount_copy = bomb_amount;
            {
                int i = 0;
                int j = 0;
                while (bomb_amount_copy != 0)
                {
                    i = (int)(rand.Next() % hight);
                    j = (int)(rand.Next() % width);
                    if (!game_field[i, j].is_bomb &&
                        (i != y ||
                         j != x))
                    {
                        game_field[i, j].is_bomb = true;
                        --bomb_amount_copy;
                    }

                }
            }

            int bomb_counter = 0;
            for (int i = 0; i < hight; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    bomb_counter = 0;
                    for (int k = -1; k < 2; ++k)
                    {
                        for (int l = -1; l < 2; ++l)
                        {
                            if (i + k < 0 ||
                               j + l < 0 ||
                               i + k >= hight ||
                               j + l >= width ||
                               game_field[i, j].is_bomb)
                                continue;

                            if (game_field[i + k, j + l].is_bomb)
                                ++bomb_counter;

                        }
                    }
                    game_field[i, j].BombCounter = bomb_counter;

                }
            }
            SquareOptions();
        }

        private void Print()
        {
            if (game_field == null)
                throw new Exception("Null Value func: Print");
            Console.Clear();
            Console.WriteLine($"{Constants.BOMB}:{bomb_amount}" +
                              $"\t\t Current Position: ({x}:{y})");

            for (int i = 0; i < hight; ++i)
            {
                // if (i % space == 0)
                //     Console.Write("\n");
                for (int j = 0; j < width; ++j)
                {
                    switch (game_field[i, j].UpperLayer)
                    {
                        case "1 ":
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case "2 ":
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case "3 ":
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case "4 ":
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            break;
                        case "5 ":
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        default:
                            Console.ResetColor();
                            break;
                    }

                    // if (j % space == 0)
                    //     Console.Write(" ");

                    if (i == y && j == x &&
                        !game_field[i, j].is_open &&
                        !game_field[i, j].is_marked)
                        Console.Write(Constants.SELECTED_SQUARE);
                    else if (i == y && j == x)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.Write(game_field[i, j].UpperLayer);
                    }
                    else
                        Console.Write(game_field[i, j].UpperLayer);

                    Console.ResetColor();
                }
                Console.Write('\n');
            }
        }
        private void Game()
        {
            bool win_flag = false;
            while (!IsBombOpen())
            {
                Print();
                SelectSquare();
                SquareOptions();
                if (IsAllDefused())
                {
                    win_flag = true;
                    break;
                }
            }
            FinalOutput();
            if (win_flag)
                Console.WriteLine("CONRATULATIONS!!!!! YOU WIN!!!!");
            else
                Console.WriteLine("YOU LOOSE :(");
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
                x = Convert.ToInt32(Console.ReadLine());
                y = Convert.ToInt32(Console.ReadLine());
                if (x < 0 || y < 0)
                    throw new Exception("Negative Value func: SelectSquare");

                is_open = game_field[y, x].is_open;
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
            game_field[y, x].is_marked = false;
            if (game_field[y, x].is_defused)
            {
                game_field[y, x].is_defused = false;
                ++bomb_amount;
            }
            game_field[y, x].OpenSquare();
            if (game_field[y, x].UpperLayer == "  ")
            {
                OpenEmptySquare(x, y);
            }

        }
        private void OpenEmptySquare(int x, int y)
        {
            if (game_field == null)
                throw new Exception("Null Value func: OpenEmptySquare");
            for (int k = -1; k < 2; ++k)
            {
                for (int l = -1; l < 2; ++l)
                {
                    if (y + k < 0 ||
                        x + l < 0 ||
                        y + k >= hight ||
                        x + l >= width ||
                    (k == 0 && l == 0))
                        continue;

                    if (!game_field[y + k, x + l].is_bomb &&
                        !game_field[y + k, x + l].is_open)
                    {
                        game_field[y + k, x + l].OpenSquare();
                        if (game_field[y + k, x + l].UpperLayer == "  ")
                            OpenEmptySquare(x + l, y + k);
                    }

                }
            }
        }
        private void DefuseSquare()
        {
            if (game_field == null)
                throw new Exception("Null Value func: DefuseSquare");
            game_field[y, x].UpperLayer = Constants.FLAG;
            game_field[y, x].is_marked = true;
            if (game_field[y, x].is_bomb)
                game_field[y, x].is_defused = true;
            --bomb_amount;
        }
        private void SetQuestion()
        {
            if (game_field == null)
                throw new Exception("Null Value func: SetQuestions");

            if (game_field[y, x].is_defused)
            {
                game_field[y, x].is_defused = false;
                ++bomb_amount;
            }
            game_field[y, x].UpperLayer = Constants.QUESTION;
            game_field[y, x].is_marked = true;
        }

        public void FinalOutput()
        {
            if (game_field == null)
                throw new Exception("Null Value func: FinalOutput");
            for (int i = 0; i < hight; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if (game_field[i, j].is_bomb && !game_field[i, j].is_open)
                    {
                        if (game_field[i, j].is_defused)
                            game_field[i, j].UpperLayer = Constants.DEFUSED_BOMB;
                        else
                            game_field[i, j].UpperLayer = Constants.BOMB;
                    }
                    else if (game_field[i, j].is_defused)
                        game_field[i, j].UpperLayer = Constants.FALSE_DEFUSE;
                    else if (!game_field[i, j].is_open)
                        game_field[i, j].OpenSquare();
                }
            }
            x = -1;
            y = -1;
            Print();
        }

        private bool IsAllDefused()
        {
            if (game_field == null)
                throw new Exception("Null Value func: IsAllDefused");
            for (int i = 0; i < hight; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if ((!game_field[i, j].is_defused && game_field[i, j].is_bomb) ||
                        (game_field[i, j].is_defused && !game_field[i, j].is_bomb))
                        return false;
                }

            }
            return true;
        }
        private bool IsBombOpen()
        {
            if (game_field == null)
                throw new Exception("Null Value func: IsBombOpen");

            for (int i = 0; i < hight; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if (game_field[i, j].is_bomb == true &&
                        game_field[i, j].is_open == true)
                        return true;
                }
            }
            return false;
        }

    }
}