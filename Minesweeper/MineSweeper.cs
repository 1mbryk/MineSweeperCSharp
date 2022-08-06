using System;
using System.Collections.Generic;
namespace MineSweeper
{
    class MineSweeper : GameFieldParametrs
    {
        public void GameMenu()
        {
            Console.Clear();
            Console.WriteLine("MINESWEEPER");
            Console.WriteLine("MENU");
            Console.WriteLine("1. Start Game.");
            Console.WriteLine("2. Rules.");
            Console.WriteLine("0. Exit.");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    StartGame();
                    break;
                case 2:
                    //
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Wrong Input");
                    break;
            }


        }
        private void StartGame()
        {
            Console.Clear();
            Console.WriteLine("Please choose dificult: ");
            Console.WriteLine("1. Easy\n" +
                              "2. Middle\n" +
                              "3. Hard");
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
                    width = Constants.HARD_SIZE_MAP.second;
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
            int choice = 0;
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
            PrintAll();

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

    }
}