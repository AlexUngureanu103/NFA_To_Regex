using NFA_To_Regex.NFAData;

namespace NFA_To_Regex
{
    internal class NFAToRegex
    {
        NFA nfa;
        public string GetRegex(NFA myNfa)
        {
            nfa = myNfa;
            ReduceOrTransitions();
            nfa.PrintAutomate();
            AddFinalState();
            ReduceStates();
            nfa.PrintAutomate();

            //create regex formula from the states left
            string regexFormula = string.Empty;         
            foreach(Transition trans in nfa.Transitions) 
            {
                if(trans.FromState == nfa.StartState && trans.ToState == nfa.FinalStates[0])
                {
                    regexFormula = trans.Symbol;                    
                    break;
                }
            }

            return regexFormula;
        }

        public void ReduceOrTransitions()
        {
            for (int i = 0; i < nfa.Transitions.Count - 1; i++)
            {
                for (int j = i + 1; j < nfa.Transitions.Count; j++)
                {
                    if (nfa.Transitions[i].FromState == nfa.Transitions[j].FromState && nfa.Transitions[i].ToState == nfa.Transitions[j].ToState)
                    {
                        if (nfa.Transitions[i].Symbol != nfa.Transitions[j].Symbol)
                        {
                            nfa.Transitions[i].Symbol = '(' + nfa.Transitions[i].Symbol + '+' + nfa.Transitions[j].Symbol + ')';
                        }
                        nfa.Transitions.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        private void AddFinalState()
        {
            string finState = "Fin";
            nfa.FinalStates.ForEach(fState => 
            {
                nfa.Transitions.Add(new Transition(fState, nfa.Lambda.ToString(), finState));
            });
            nfa.FinalStates.Clear();
            nfa.FinalStates.Add(finState);

            string initState = "Init";
            nfa.Transitions.Add(new Transition(initState, nfa.Lambda.ToString(), nfa.StartState));
            nfa.StartState = initState;

            nfa.States.Add(initState);
            nfa.States.Add(finState);
        }

        private void ReduceStates()
        {
            List<string> states = new List<string>(nfa.States);
            states.Remove(nfa.StartState);
            states.Remove(nfa.FinalStates[0]);
            foreach (string state in states)
            {
                RemoveState(state);
                Console.WriteLine("Reduce " + state);
                nfa.PrintAutomate();
                ReduceOrTransitions();
            }
        }

        public void RemoveState(string state)
        {
            List<Transition> indexTransToState = new List<Transition>();
            List<Transition> indexTransStateTo = new List<Transition>();
            int indexStarTrans = -1;

            //find states related with the state i want to erase
            for (int i = 0; i < nfa.Transitions.Count; i++)
            {
                if(nfa.Transitions[i].ToState == state && nfa.Transitions[i].FromState == state)
                {
                    indexStarTrans = i;
                }
                else if (nfa.Transitions[i].ToState == state)
                {
                    indexTransToState.Add(nfa.Transitions[i]);
                }
                else if (nfa.Transitions[i].FromState == state)
                {
                    indexTransStateTo.Add(nfa.Transitions[i]);
                }
            }

            //find out if exist an star transition and remove it
            string starTrans = string.Empty;
            if (indexStarTrans != -1)
            {
                if (nfa.Transitions[indexStarTrans].Symbol.Length != 1 )
                {
                    starTrans = '(' + nfa.Transitions[indexStarTrans].Symbol + ")*";
                }
                else
                {
                    starTrans = nfa.Transitions[indexStarTrans].Symbol + '*';
                }
                nfa.Transitions.RemoveAt(indexStarTrans);
            }

            //create new transitions and delete the old ones
            string fromSymbol, toSymbol;
            indexTransToState.ForEach(fromS =>
            {
                fromSymbol = fromS.Symbol;
                if (fromSymbol[0] == nfa.Lambda)
                {
                    fromSymbol = "";
                }
                indexTransStateTo.ForEach(toS =>
                {
                    toSymbol = toS.Symbol;
                    if (toSymbol[0] == nfa.Lambda && toSymbol!= fromS.Symbol)
                    {
                        toSymbol = "";
                    }

                    Transition transition = new Transition();
                    transition.FromState = fromS.FromState;
                    transition.ToState = toS.ToState;
                    transition.Symbol = fromSymbol + starTrans + toSymbol;
                    nfa.Transitions.Add(transition);

                    if (indexTransToState[indexTransToState.Count - 1] == fromS)
                    {
                        nfa.Transitions.Remove(toS);
                    }
                });
                nfa.Transitions.Remove(fromS);
            });

            indexTransToState.Clear();
            indexTransStateTo.Clear();
            nfa.States.Remove(state);
        }
    }
}
