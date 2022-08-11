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
        private int prev_x;
        private int prev_y;
        protected uint hight;
        protected uint width;


        protected void PrintAll()
        {
            if (game_field == null)
                throw new Exception("Null Value func: Print");
            Console.Clear();
            Console.WriteLine($"{Constants.BOMB}:{bomb_amount}" +
                              $"\t\t Current Position: ({x}:{y})");

            for (int i = 0; i < hight; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    SetColor(j, i);
                    if (i == y && j == x
                        && !game_field[i, j].is_open)
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

        protected void PrintOne()
        {
            if (game_field == null)
                throw new Exception("Null Value func: PrintOne");

            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"{Constants.BOMB}:{bomb_amount}" +
                              $"\t\t Current Position: ({x}:{y})");

            Console.SetCursorPosition(2 * prev_x, prev_y + 1);
            //  
            SetColor(prev_x, prev_y);
            Console.Write(game_field[prev_y, prev_x].UpperLayer);
            Console.SetCursorPosition(2 * x, y + 1);
            if (game_field[y, x].is_open || game_field[y, x].is_marked)
            {
                SetColor(x, y);
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.Write(game_field[y, x].UpperLayer);
            }
            else
                Console.Write(Constants.SELECTED_SQUARE);
            Console.ResetColor();

            prev_x = x;
            prev_y = y;
            Console.SetCursorPosition(0, (int)(hight + 2));
        }

        private void SetColor(int x, int y)
        {
            if (game_field == null)
                throw new Exception("Null Value func: SetColor");

            switch (game_field[y, x].UpperLayer)
            {
                case "\x1b[1m1 \x1b[0m":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "\x1b[1m2 \x1b[0m":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "\x1b[1m3 \x1b[0m":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "\x1b[1m4 \x1b[0m":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "\x1b[1m5 \x1b[0m":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                default:
                    Console.ResetColor();
                    break;
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
            PrintAll();
        }
        protected void SelectSquare()
        {

            if (game_field == null)
                throw new Exception("Null Value func: SelectSquare");
            System.ConsoleKeyInfo key_info;
            PrintAll();
            Console.WriteLine("Choose square.");
            Console.WriteLine("With WASD or arrows");
            do
            {
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
                PrintOne();

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