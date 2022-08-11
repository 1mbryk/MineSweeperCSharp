using System;
using System.Collections.Generic;
namespace MineSweeper
{
    class MineSweeper : GameFieldParametrs
    {
        public void GameMenu()
        {
            uint choice;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("MINESWEEPER");
                Console.WriteLine("MENU");
                Console.WriteLine("1. Start Game.");
                Console.WriteLine("2. Rules.");
                Console.WriteLine("0. Exit.");
                choice = Input<uint>();
                switch ((Menu)choice)
                {
                    case Menu.Start:
                        StartGame();
                        break;
                    case Menu.Rules:
                        //
                        break;
                    case Menu.Exit:
                        return;
                    default:
                        Console.WriteLine("Wrong Input. Please try again.");
                        continue;
                }
                break;
            }


        }
        private void StartGame()
        {
            uint choice;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Please choose dificult: ");
                Console.WriteLine("1. Easy\n" +
                                  "2. Middle\n" +
                                  "3. Hard");
                choice = Input<uint>();
                switch ((Dificult)choice)
                {
                    case Dificult.Easy:
                        hight = Constants.EASY_SIZE_MAP.first;
                        width = Constants.EASY_SIZE_MAP.second;
                        bomb_amount = Constants.EASY_BOMB_AMOUNT;
                        break;
                    case Dificult.Midd:
                        hight = Constants.MIDD_SIZE_MAP.first;
                        width = Constants.MIDD_SIZE_MAP.second;
                        bomb_amount = Constants.MIDD_BOMB_AMOUNT;
                        break;
                    case Dificult.Hard:
                        hight = Constants.HARD_SIZE_MAP.first;
                        width = Constants.HARD_SIZE_MAP.second;
                        bomb_amount = Constants.HARD_BOMB_AMOUNT;
                        break;
                    default:
                        Console.WriteLine("Wrong input. Please try again.");
                        System.Threading.Thread.Sleep(500);
                        continue;
                }
                break;
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
            PrintAll();
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
        private void Game()
        {
            bool win_flag = false;
            while (!IsBombOpen())
            {
                PrintAll();
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

        private void SquareOptions()
        {
            uint choice = 0;
            bool complete_flag = false;
            while (!complete_flag)
            {
                PrintAll();
                Console.SetCursorPosition(0, (int)(hight + 1));
                Console.WriteLine("Please choose next options: ");
                Console.WriteLine("1. Open.");
                Console.WriteLine("2. Defuse. ");
                Console.WriteLine("3. Set question.");
                Console.WriteLine("0. Change choice.");
                Console.Write("Your input: ");
                choice = Input<uint>();
                switch ((Options)choice)
                {
                    case Options.Open:
                        OpenSquare();
                        complete_flag = true;
                        break;
                    case Options.Defuse:
                        DefuseSquare();
                        complete_flag = true;
                        break;
                    case Options.Question:
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
            CancelDefusing();
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
            if (CancelDefusing())
            {
                game_field[y, x].UpperLayer = Constants.SQUARE;
                return;
            }
            game_field[y, x].UpperLayer = Constants.FLAG;
            game_field[y, x].is_defused = true;
            --bomb_amount;
        }
        private void SetQuestion()
        {
            if (game_field == null)
                throw new Exception("Null Value func: SetQuestions");
            CancelDefusing();
            game_field[y, x].UpperLayer = Constants.QUESTION;
            game_field[y, x].is_marked = true;
        }
        private bool CancelDefusing()
        {
            if (game_field == null)
                throw new Exception("Null Value func: CanselDefusing");
            if (game_field[y, x].is_defused)
            {
                game_field[y, x].is_defused = false;
                ++bomb_amount;
                return true;
            }
            return false;
        }
        private T Input<T>()
        {
            T value;
            string? input_value;
            int currentLineCursor = Console.CursorTop;
            while (true)
            {
                try
                {
                    input_value = Console.ReadLine();
                    if (input_value != null)
                        value = (T)Convert.ChangeType(input_value, typeof(T));
                    else
                        throw new Exception();
                }
                catch
                {
                    Console.WriteLine("Incorrect input. Please Try again.");
                    System.Threading.Thread.Sleep(500);
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 2);
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(new string(' ', Console.WindowWidth)); // "clear" current line 
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(new string(' ', Console.WindowWidth)); // and next line
                    Console.SetCursorPosition(0, currentLineCursor);
                    continue;
                }
                break;

            }
            return value;
        }

    }

}