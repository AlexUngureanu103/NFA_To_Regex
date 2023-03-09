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
            string regex = string.Empty;
            NFAAutomate.Transitions[17].ToState = NFAAutomate.Transitions[17].FromState;
            //NFAAutomate.Transitions[17].Symbol = "c";
            HandleTransitionsWithMultipleSymbols();
            HandleTransitionLoopsItSelf();
            HandleTransitionLoopsItSelf();
            NFAAutomate.PrintAutomate();
            return regex;
        }

        private void HandleTransitionsWithMultipleSymbols()
        {
            for (int index1 = 0; index1 < NFAAutomate.Transitions.Count; index1++)
            {
                for (int index2 = index1 + 1; index2 < NFAAutomate.Transitions.Count; index2++)
                {
                    if (CompareTransitions(NFAAutomate.Transitions[index1], NFAAutomate.Transitions[index2]))
                    {
                        Transition transition = new Transition(NFAAutomate.Transitions[index1].FromState, NFAAutomate.Transitions[index1].Symbol + "+" + NFAAutomate.Transitions[index2].Symbol, NFAAutomate.Transitions[index1].ToState);
                        NFAAutomate.Transitions.Remove(NFAAutomate.Transitions[index2]);
                        NFAAutomate.Transitions.Remove(NFAAutomate.Transitions[index1]);
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
                if (NFAAutomate.Transitions[index].FromState == NFAAutomate.Transitions[index].ToState && !NFAAutomate.Transitions[index].Symbol.Contains("*"))
                {
                    NFAAutomate.Transitions[index].Symbol += '*';
                }
            }
        }
    }
}
