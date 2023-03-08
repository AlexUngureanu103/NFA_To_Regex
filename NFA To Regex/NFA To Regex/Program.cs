
using NFA_To_Regex.NFAData;

namespace NFA_To_Regex
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                //try
                //{
                NFA nfa = new NFA();
                nfa.LoadFile();
                ConvertNFAToDFA converter = new ConvertNFAToDFA();
                nfa = converter.FromAFNLambdaToAFD(nfa);
                nfa.PrintAutomate();
                //}
                //catch (Exception ex)
                //{
                //    DisplayError(ex);
                //}
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
}