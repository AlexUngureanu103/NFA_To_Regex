
namespace NFA_To_Regex
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    DisplayError(ex);
                }
                Pause();
            }
        }

        private static void DisplayError(Exception ex)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex);
            Console.ForegroundColor = oldColor;
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine();
        }

    }