
namespace MineSweeper
{
    class Program
    {
        public static void Main()
        {
            try
            {
                MineSwepper game = new();
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