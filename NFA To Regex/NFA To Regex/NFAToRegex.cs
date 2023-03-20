using NFA_To_Regex.NFAData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            RemoveState("q1");
            nfa.PrintAutomate();

            string regexFormula = '(' + nfa.Transitions[0].Symbol + ")*";
            regexFormula += nfa.Transitions[1].Symbol;
            regexFormula += '(' + nfa.Transitions[2].Symbol + ")*";

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
                        nfa.Transitions[i].Symbol = '(' + nfa.Transitions[i].Symbol + '+' + nfa.Transitions[j].Symbol + ')';
                        nfa.Transitions.RemoveAt(j);
                        j--;
                    }
                }
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
                starTrans = nfa.Transitions[indexStarTrans].Symbol + '*';
                nfa.Transitions.RemoveAt(indexStarTrans);
            }

            //create new transitions and delete the old ones
            indexTransToState.ForEach(fromS =>
            {
                indexTransStateTo.ForEach(toS =>
                {
                    Transition transition = new Transition();
                    transition.FromState = fromS.FromState;
                    transition.ToState = toS.ToState;
                    transition.Symbol = fromS.Symbol + starTrans + toS.Symbol;
                    nfa.Transitions.Add(transition);

                    if (indexTransToState.Count == 1)
                    {
                        nfa.Transitions.Remove(toS);
                    }
                });
                nfa.Transitions.Remove(fromS);
            });

            indexTransStateTo.ForEach(toS =>
            {
            });
        }
    }
}
