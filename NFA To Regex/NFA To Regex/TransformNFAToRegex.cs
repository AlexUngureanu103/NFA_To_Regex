using NFA_To_Regex.Exceptions;
using NFA_To_Regex.NFAData;
using System;

namespace NFA_To_Regex
{
    internal class TransformNFAToRegex
    {
        NFA NFAAutomate;

        public string TransformNFAinToRegex(NFA nfaAutomate)
        {
            this.NFAAutomate = nfaAutomate;

            if (nfaAutomate == null)
            {
                throw new EmptyAutomateException();
            }
            NFAAutomate.PrintAutomate();
            ReduceTheAutomate();

            NFAAutomate.PrintAutomate();
            return NFAAutomate.Transitions[0].Symbol;
        }

        private string GetNonInitFinalState()
        {
            foreach (var state in NFAAutomate.States)
            {
                if (state == NFAAutomate.StartState || NFAAutomate.FinalStates.Contains(state))
                {
                    continue;
                }
                Console.WriteLine($"State to remove: {state}");
                return state;
            }
            return string.Empty;
        }

        private string RemoveLambdaSymbols(string regex)
        {
            return regex.Replace(NFAAutomate.Lambda + string.Empty, "");
        }
        private void ReduceTheAutomate()
        {
            while (NFAAutomate.Transitions.Count > 1)
            {
                HandleTransitionLoopsItSelf();
                HandleTransitionsWithMultipleSymbols();
                HandleTransitionLoopsItSelf();
                RemoveState(GetNonInitFinalState());
                NFAAutomate.PrintAutomate();
            }
        }

        private void HandleTransitionsWithMultipleSymbols()
        {
            for (int index1 = 0; index1 < NFAAutomate.Transitions.Count; index1++)
            {
                for (int index2 = index1 + 1; index2 < NFAAutomate.Transitions.Count; index2++)
                {
                    if (CompareTransitions(NFAAutomate.Transitions[index1], NFAAutomate.Transitions[index2]))
                    {
                        int counter = 0;
                        #region Create new symbol
                        string newSymbol = string.Empty;
                        if (NFAAutomate.Transitions[index1].Symbol != NFAAutomate.Lambda + string.Empty)
                        {
                            newSymbol += NFAAutomate.Transitions[index1].Symbol;
                            counter++;
                        }
                        if (counter == 0)
                        {
                            newSymbol = NFAAutomate.Transitions[index2].Symbol;
                        }
                        else if (NFAAutomate.Transitions[index2].Symbol != NFAAutomate.Lambda + string.Empty)
                        {
                            newSymbol += '+' + NFAAutomate.Transitions[index2].Symbol;
                        }
                        if (string.IsNullOrEmpty(newSymbol))
                            newSymbol = NFAAutomate.Lambda + string.Empty;
                        if (newSymbol.Length > 1)
                        {
                            newSymbol = '(' + newSymbol + ')';
                        }
                        #endregion

                        Transition transition = new Transition(NFAAutomate.Transitions[index1].FromState, newSymbol, NFAAutomate.Transitions[index1].ToState);
                        NFAAutomate.Transitions.Remove(NFAAutomate.Transitions[index2]);
                        NFAAutomate.Transitions.Remove(NFAAutomate.Transitions[index1]);
                        if (index1 > 0)
                            index1--;
                        index2--;
                        NFAAutomate.Transitions.Add(transition);
                    }
                }
            }
        }

        private bool CompareTransitions(Transition t1, Transition t2)
        {
            if (t1.ToState != t2.ToState || t1.FromState != t2.FromState)
            {
                return false;
            }
            if (t1.Symbol == t2.Symbol)
            {
                return false;
            }
            return true;
        }

        private void HandleTransitionLoopsItSelf()
        {
            for (int index = 0; index < NFAAutomate.Transitions.Count; index++)
            {
                if (NFAAutomate.Transitions[index].FromState == NFAAutomate.Transitions[index].ToState && NFAAutomate.Transitions[index].Symbol.Last() != '*')
                {
                    if (NFAAutomate.Transitions[index].Symbol.Length > 1)
                        NFAAutomate.Transitions[index].Symbol = '(' + NFAAutomate.Transitions[index].Symbol + ")*";
                    else
                    {
                        NFAAutomate.Transitions[index].Symbol += '*';
                    }
                }
            }
        }

        private void HandleQ0ToQ1ToQ0()
        {
            for (int index = 0; index < NFAAutomate.Transitions.Count; index++)
            {
                for (int index2 = 0; index2 < NFAAutomate.Transitions.Count; index2++)
                {
                    if (NFAAutomate.Transitions[index] != NFAAutomate.Transitions[index2] && NFAAutomate.Transitions[index].ToState == NFAAutomate.Transitions[index].FromState)
                    {
                        //
                    }
                }
            }
        }

        private void RemoveState(string state)
        {
            #region initializations for the changing transitions
            bool hasLoops = false;
            Transition loopTransition = null;
            Stack<Transition> fromTransitionsToChange = new();
            Stack<Transition> toTransitionToChange = new();
            NFAAutomate.Transitions.ForEach(x =>
            {
                if (x.FromState == state && x.FromState != x.ToState)
                {
                    fromTransitionsToChange.Push(x);
                }
                else if (x.ToState == state && x.FromState != x.ToState)
                {
                    toTransitionToChange.Push(x);
                }
                if (x.FromState == x.ToState && x.FromState == state)
                {
                    loopTransition = x;
                    hasLoops = true;
                }
            });
            #endregion

            foreach (var toTransition in toTransitionToChange)
            {
                foreach (var fromTransition in fromTransitionsToChange)
                {
                    Transition transition = new();
                    transition.FromState = toTransition.FromState;
                    transition.ToState = fromTransition.ToState;

                    if (toTransition.Symbol != string.Empty + this.NFAAutomate.Lambda)
                    {
                        if (hasLoops)
                        {
                            if (toTransition.Symbol.Length > 1)
                            {
                                transition.Symbol += string.Empty + '(';
                                transition.Symbol += toTransition.Symbol;
                                transition.Symbol += string.Empty + ')';
                            }
                            else
                            {
                                transition.Symbol += toTransition.Symbol;
                            }
                        }
                        else
                        {
                            transition.Symbol = toTransition.Symbol;
                        }
                    }
                    if (loopTransition != null)
                    {
                        transition.Symbol += loopTransition.Symbol;
                    }
                    if (fromTransition.Symbol != string.Empty + this.NFAAutomate.Lambda)
                    {
                        if (hasLoops)
                        {
                            if (fromTransition.Symbol.Length > 1)
                            {
                                transition.Symbol += string.Empty + '(';
                                transition.Symbol += fromTransition.Symbol;
                                transition.Symbol += string.Empty + ')';
                            }
                            else
                            {
                                transition.Symbol += fromTransition.Symbol;
                            }
                        }
                        else
                        {
                            transition.Symbol += fromTransition.Symbol;
                        }
                    }
                    if (string.IsNullOrEmpty(transition.Symbol))
                    {
                        transition.Symbol = NFAAutomate.Lambda + string.Empty;
                    }
                    NFAAutomate.Transitions.Add(transition);
                }
            }

            #region remove leftovers
            NFAAutomate.States.Remove(state);
            while (fromTransitionsToChange.Count > 0)
            {
                NFAAutomate.Transitions.Remove(fromTransitionsToChange.First());
                fromTransitionsToChange.Pop();
            }
            while (toTransitionToChange.Count > 0)
            {
                NFAAutomate.Transitions.Remove(toTransitionToChange.First());
                toTransitionToChange.Pop();
            }
            if (loopTransition != null)
            {
                NFAAutomate.Transitions.Remove(loopTransition);
            }
            #endregion
        }
    }
}
