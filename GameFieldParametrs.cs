namespace MineSweeper
{
    public class GameFieldParametrs
    {
        protected GameField[,]? game_field;
        protected int bomb_amount;
        protected int space;

        // coordinates
        protected int x = 0; // width
        protected int y = 0; // hight
        protected uint hight;
        protected uint width;


        protected void Print()
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

        protected void FinalOutput()
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
        protected void SelectSquare()
        {

            if (game_field == null)
                throw new Exception("Null Value func: SelectSquare");
            System.ConsoleKeyInfo key_info;
            do
            {
                Print();
                Console.WriteLine("Choose square.");
                Console.WriteLine("With WASD or arrows");
                key_info = Console.ReadKey(true);
                if (key_info.Key == ConsoleKey.W ||
                   key_info.Key == ConsoleKey.UpArrow)
                {
                    if (y != 0)
                        --y;
                }
                else if (key_info.Key == ConsoleKey.A ||
                   key_info.Key == ConsoleKey.LeftArrow)
                {
                    if (x != 0)
                        --x;
                }
                else if (key_info.Key == ConsoleKey.S ||
                   key_info.Key == ConsoleKey.DownArrow)
                {
                    if (y != hight - 1)
                        ++y;

                }
                else if (key_info.Key == ConsoleKey.D ||
                   key_info.Key == ConsoleKey.RightArrow)
                {
                    if (x != width - 1)
                        ++x;
                }
                else if (key_info.Key == ConsoleKey.Enter)
                {
                    if (game_field[y, x].is_open)
                    {
                        Console.WriteLine("This square is already open.\nPlease select other square");
                    }
                    else
                        break;
                }

            } while (true);
        }

        protected bool IsAllDefused()
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
        protected bool IsBombOpen()
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