
namespace MineSweeper
{
    class Program
    {
        public static void Main()
        {
            try
            {
                MineSwepper game = new();
                game.StartGame();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return;
            }

        }

    }
}