
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
                NFAToRegex convert = new NFAToRegex();               
                Console.WriteLine(convert.GetRegex(nfa));
                //nfa.PrintAutomate();
                //ConvertNFAToDFA converter = new ConvertNFAToDFA();
                //NFA nfa1 = converter.FromAFNLambdaToAFD(nfa);
                //nfa1.PrintAutomate();
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