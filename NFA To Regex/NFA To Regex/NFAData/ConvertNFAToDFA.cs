//namespace NFA_To_Regex.NFAData
//{
//    internal class ConvertNFAToDFA
//    {
//        public string FindNextStates(string initial, List<Transition> transitionsLambda, char alphabetCharacter)
//        {
//            Stack<string> states = new Stack<string>();
//            states.Push(initial);
//            string firstState = "";
//            if (alphabetCharacter != 'λ')
//            {
//                initial = "";
//            }
//            while (states.Any())
//            {
//                firstState = states.Pop();
//                transitionsLambda.ForEach(transition =>
//                {
//                    string currState = transition.FromState;
//                    char character = transition.Symbol;
//                    string nextState = transition.ToState;
//                    string aux = "";
//                    for (ushort i = 0; i < firstState.Length; i++)
//                    {
//                        while (i < firstState.Length && firstState[i] != ',')
//                        {
//                            aux += firstState[i];
//                            i++;
//                        }
//                        if (currState == aux && character == alphabetCharacter)
//                        {
//                            if (initial.IndexOf(nextState) == -1)
//                            {
//                                if (alphabetCharacter == 'λ')
//                                {
//                                    states.Push(nextState);
//                                }
//                                if (!string.IsNullOrEmpty(initial))
//                                {
//                                    initial += ',';
//                                }
//                                initial += nextState;
//                            }
//                        }
//                        aux = "";
//                    }
//                });
//            }

//            return initial;
//        }

//        public NFA FromAFNLambdaToAFD(NFA automatLambda)
//        {
//            //using Transition = Tuple<char, char, char>;
//            List<Transition> transitionsAFD = new List<Transition>();
//            List<string> statesBeforeLambda = new List<string>();
//            List<string> AFDStates = new List<string>();
//            List<string> finalStates = new List<string>();
//            string initial, current, aux, aux2;

//            var transitionsLambdaAFN = automatLambda.Transitions;
//            var states = automatLambda.States;
//            var alphabetCharacters = automatLambda.Alphabet;
//            var finalStatesAFN = automatLambda.FinalStates;

//            bool ok;
//            ushort stateIndex = 0;
//            char stateSymbol = 'A';

//            initial = automatLambda.StartState;
//            statesBeforeLambda.Add(initial);
//            Queue<string> unexploredStates = new Queue<string>();
//            initial = FindNextStates(initial, transitionsLambdaAFN, automatLambda.Lambda);
//            AFDStates.Add(stateSymbol.ToString());
//            stateSymbol++;

//            unexploredStates.Enqueue(initial);

//            while (unexploredStates.Count > 0)
//            {
//                initial = unexploredStates.Dequeue();

//                foreach (var alphabetChar in alphabetCharacters)
//                {
//                    current = initial;
//                    current = FindNextStates(current, transitionsLambdaAFN, alphabetChar);
//                    if (!string.IsNullOrEmpty(current))
//                    {
//                        ok = true;
//                        for (ushort i = 0; i < statesBeforeLambda.Count; i++)
//                        {
//                            if (i != stateIndex && current == statesBeforeLambda[i])
//                            {
//                                transitionsAFD.Add(new Transition(AFDStates[stateIndex], alphabetChar, AFDStates[i]));
//                                ok = false;
//                                break;
//                            }
//                        }
//                        if (ok)
//                        {
//                            statesBeforeLambda.Add(current);
//                            current = FindNextStates(current, transitionsLambdaAFN, 'λ');
//                            unexploredStates.Enqueue(current);

//                            aux = "";
//                            aux2 = "";
//                            for (ushort i = 0; i < current.Length; i++)
//                            {
//                                while (i < current.Length && current[i] != ',')
//                                {
//                                    aux += current[i];
//                                    i++;
//                                }

//                                for (ushort ij = 0; ij < finalStatesAFN[0].Length; ij++)
//                                {
//                                    while (ij < finalStatesAFN[0].Length && !char.IsDigit(finalStatesAFN[0][ij]))
//                                    {
//                                        aux2 += finalStatesAFN[0][ij];
//                                        ij++;
//                                    }
//                                    while (ij < finalStatesAFN[0].Length && char.IsDigit(finalStatesAFN[0][ij]))
//                                    {
//                                        aux2 += finalStatesAFN[0][ij];
//                                        ij++;
//                                    }
//                                    if (aux == aux2)
//                                    {
//                                        break;
//                                    }
//                                    aux2 = "";
//                                    if (ij < finalStatesAFN[0].Length)
//                                    {
//                                        aux2 += finalStatesAFN[0][ij];
//                                    }
//                                }
//                                if (aux == aux2)
//                                {
//                                    finalStates.Add(stateSymbol.ToString());
//                                    break;
//                                }
//                                aux = "";
//                            }

//                            AFDStates.Add(stateSymbol.ToString());
//                            stateSymbol++;
//                            transitionsAFD.Add(new Transition(AFDStates[stateIndex], alphabetChar, AFDStates[AFDStates.Count - 1]));
//                        }
//                    }
//                }
//                stateIndex++;
//            }

//            NFA DFA = new NFA();
//            List<char> AFDalphabet = new List<char>();
//            foreach (var character in alphabetCharacters)
//            {
//                AFDalphabet.Add(character);
//            }
//            DFA.Alphabet = AFDalphabet;
//            DFA.States = AFDStates;
//            DFA.Transitions = transitionsAFD;
//            DFA.StartState = AFDStates[0];
//            DFA.FinalStates = finalStates;

//            return DFA;
//        }

//    }
//}
