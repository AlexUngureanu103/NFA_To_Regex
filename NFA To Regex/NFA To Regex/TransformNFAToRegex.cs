﻿using NFA_To_Regex.Exceptions;
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
            //NFAAutomate.Transitions[17].ToState = NFAAutomate.Transitions[17].FromState;
            //NFAAutomate.Transitions[17].Symbol = "c";
            HandleTransitionsWithMultipleSymbols();
            HandleTransitionLoopsItSelf();
            HandleTransitionLoopsItSelf();
            HandleQ0ToQ1ToQ0();
            NFAAutomate.PrintAutomate();
            RemoveState("A");
            NFAAutomate.PrintAutomate();
            RemoveState("B");
            HandleTransitionsWithMultipleSymbols();
            NFAAutomate.PrintAutomate();
            RemoveState("C");
            HandleTransitionsWithMultipleSymbols();
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
                    NFAAutomate.Transitions[index].Symbol = '(' + NFAAutomate.Transitions[index].Symbol + ")*";
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
            // combine the possible outputs ??
            if (loopTransition != null)
            {
                // move the loops to every other states
                //IF the there are circular transitions 
                //eg: qi , d ->q && q, a ->qi
            }

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
                            transition.Symbol += string.Empty + '(';
                            transition.Symbol += toTransition.Symbol;
                            transition.Symbol += string.Empty + ')';
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
                            transition.Symbol += string.Empty + '(';
                            transition.Symbol += fromTransition.Symbol;
                            transition.Symbol += string.Empty + ')';
                        }
                        else
                        {
                            transition.Symbol += fromTransition.Symbol;
                        }
                    }
                    NFAAutomate.Transitions.Add(transition);
                }
            }
            //remove leftovers
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
        }
    }
}
