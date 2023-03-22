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

        private void RemoveState(string state)
        {
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

            foreach (var toTransition in toTransitionToChange)
            {
                foreach (var fromTransition in fromTransitionsToChange)
                {
                    NFAAutomate.Transitions.Add(FormNewTransition(fromTransition, toTransition, hasLoops, loopTransition));
                }
            }
            /*
             (((b((a*+(cb)*))*c)+(b((a*+(cb)*))*c)a*)+(b((a*+(cb)*))*b)(c((a*+(cb)*))*b)*(((c((a*+(cb)*))*c)+(c((a*+(cb)*))*c)a*)))
             */

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

        private string UpdateTransitionSymbolWhenRemovingAState(Transition transition, bool hasLoops)
        {
            string newSymbol = string.Empty;
            if (!(transition.Symbol != string.Empty + this.NFAAutomate.Lambda))
            {
                return newSymbol;
            }
            if (transition.Symbol.Length > 1 && hasLoops)
            {
                newSymbol += string.Empty + '(';
                newSymbol += transition.Symbol;
                newSymbol += string.Empty + ')';
            }
            else
            {
                newSymbol += transition.Symbol;
            }

            return newSymbol;
        }

        private Transition FormNewTransition(Transition fromTransition, Transition toTransition, bool hasLoops, Transition loopTransition)
        {
            Transition transition = new();
            transition.FromState = toTransition.FromState;
            transition.ToState = fromTransition.ToState;

            transition.Symbol += UpdateTransitionSymbolWhenRemovingAState(toTransition, hasLoops);
            if (loopTransition != null)
            {
                transition.Symbol += loopTransition.Symbol;
            }
            transition.Symbol += UpdateTransitionSymbolWhenRemovingAState(fromTransition, hasLoops);
            if (string.IsNullOrEmpty(transition.Symbol))
            {
                transition.Symbol = NFAAutomate.Lambda + string.Empty;
            }
            return transition;
        }
    }
}
