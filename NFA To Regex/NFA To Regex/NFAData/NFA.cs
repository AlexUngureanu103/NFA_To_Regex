using NFA_To_Regex.Exceptions;
using NFA_To_Regex.Presentation;
using System.Xml.Serialization;

namespace NFA_To_Regex.NFAData
{
    internal class NFA : NFABase
    {
        private string filePath = @"..\..\..\Resources\NFA.xml";
        public NFA()
        {
            States = new List<string>();
            Alphabet = new List<string>();
            Transitions = new List<Transition>();
            FinalStates = new List<string>();
        }

        public void LoadFile()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(NFABase));
            using (StreamReader reader = new StreamReader(filePath))
            {
                NFABase nfaBase = (NFABase)xmlSerializer.Deserialize(reader);

                this.Alphabet = nfaBase.Alphabet;
                this.StartState = nfaBase.StartState;
                this.States = nfaBase.States;
                foreach (Transition transition in nfaBase.Transitions)
                {
                    if (transition.Symbol == "#")
                    {
                        transition.Symbol = nfaBase.Lambda.ToString();
                    }
                }
                this.Transitions = nfaBase.Transitions;
                this.FinalStates = nfaBase.FinalStates;
            }
        }

        public void PrintAutomate()
        {
            DisplayBase displayBase = new DisplayBase();
            Console.WriteLine();
            //gamma = letter gamma
            char gamma = 'D';
            displayBase.DisplayLine("Finite Automate : ", ConsoleColor.Gray);
            displayBase.DisplayLine($"Format : M=({{States}}, {{Alphabet}}, {gamma}, StartState, {{FinalStates}})",ConsoleColor.Gray);
            displayBase.DisplayLine($"M =({{ {string.Join(", ",this.States)}}} ,{{{string.Join(", ", this.Alphabet)}}} ,{gamma} ,{this.StartState} ,{{{string.Join(", ", this.FinalStates)}}}) cu D:", ConsoleColor.White);
            foreach (Transition transition in this.Transitions)
            {
                displayBase.DisplayLine($"{gamma}({transition.FromState} ,{transition.Symbol} ,{transition.ToState})", ConsoleColor.White);
            }

        }
    }
}
