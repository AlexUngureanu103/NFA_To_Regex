﻿using NFA_To_Regex.Exceptions;
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
            Alphabet = new List<char>();
            Transitions = new List<Transition>();
            FinalStates = new List<string>();
        }

        public void TransformNFAToDFA()
        {
            if (States == null)
            {
                throw new EmptyAutomateExceptions();
            }
        }

        public void LoadNFAFromFile()
        {

            string[] lines = File.ReadAllLines(filePath);

            List<string> states = new List<string>();
            foreach (string state in lines[0].Split(' '))
            {
                states.Add(state);
            }
            List<char> alphabet = new List<char>();
            foreach (string symbol in lines[1].Split(' '))
            {
                alphabet.Add(symbol[0]);
            }
            string initialState = lines[2].Split(' ')[0];

            List<string> finalStates = new List<string>();
            foreach (string finState in lines[3].Split(' '))
            {
                finalStates.Add(finState);
            }
            List<Transition> transitions = new List<Transition>();
            for (int i = 4; i < lines.Length; i++)
            {
                Transition transition = new Transition();
                string[] line = lines[i].Split(' ');
                transition.FromState = line[0];
                transition.Symbol = line[1];
                transition.ToState = line[2];
                transitions.Add(transition);
            }
            this.Alphabet = alphabet;
            this.StartState = initialState;
            this.States = states;
            this.Transitions = transitions;
            this.FinalStates = finalStates;

            //SaveFile();
        }

        //public void SaveFile()
        //{
        //    NFABase nFABase = new NFABase();
        //    nFABase.Alphabet = this.Alphabet;
        //    nFABase.StartState = this.StartState;
        //    nFABase.States = this.States;
        //    nFABase.Transitions = this.Transitions;
        //    nFABase.FinalStates = this.FinalStates;
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(NFABase));
        //    using (StreamWriter writer = new StreamWriter(filePath))
        //    {
        //        xmlSerializer.Serialize(writer, nFABase);
        //    }
        //}

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
