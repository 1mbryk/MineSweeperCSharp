
namespace MineSweeper
{
    class Program
    {
        public static void Main()
        {
            try
            {
                MineSweeper game = new();
                game.GameMenu();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return;
            }

        }

    }
}