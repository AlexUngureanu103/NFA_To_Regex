
using NFA_To_Regex.NFAData;

namespace NFA_To_Regex
{
    class Program
    {
        /// <summary>
        /// `Main` is the entry point of the program.
        /// </summary>
        /// <param name="args"> There are currently 11 available examples, with possible more to come. To change them , use another index from args</param>
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    //0 "Resources\Exemple2.xml"
                    //1 "Resources\Exemple3.xml"
                    //2 "Resources\Exemple4.xml"
                    //3 "Resources\Exemple5.xml"
                    //4 "Resources\Exemple6.xml"
                    //5 "Resources\NFA.xml"
                    //6 "Resources\Exemple7 - curs.xml"
                    //7 "Resources\Exemple8 - curs.xml"
                    //8 "Resources\NFA_.xml"
                    //9 "Resources\NFA1.xml"
                    //10 "Resources\NFA2.xml"

                    NFA nfa = new NFA();
                    nfa.LoadFile(args[7]);

                    TransformNFAToRegex transformNFAToRegex = new TransformNFAToRegex();
                    Console.WriteLine();
                    Console.WriteLine($"The regular expression for the given NFA is :\n{transformNFAToRegex.TransformNFAinToRegex(nfa)}");
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
}