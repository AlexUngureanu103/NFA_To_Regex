﻿
using NFA_To_Regex.NFAData;

namespace NFA_To_Regex
{
    class Program
    {
        /// <summary>
        /// `Main` is the entry point of the program.
        /// </summary>
        /// <param name="args"> There are currently 6 available examples, with possible more to come. To change them , use another index from args</param>
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    NFA nfa = new NFA();
                    nfa.LoadFile(args[4]);
                    //nfa.PrintAutomate();
                    //ConvertNFAToDFA converter = new ConvertNFAToDFA();
                    //NFA nfa1 = converter.FromAFNLambdaToAFD(nfa);
                    //nfa1.PrintAutomate();
                    TransformNFAToRegex transformNFAToRegex = new TransformNFAToRegex();
                    transformNFAToRegex.TransformNFAinToRegex(nfa);

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