using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFA_To_Regex
{
    internal class Transition
    {
        public string FromState { get; set; }

        public string ToState { get; set; }

        public char Symbol { get; set; }

        public Transition()
        {

        }

        public Transition(string fromState, string toState, char symbol)
        {
            FromState = fromState;
            ToState = toState;
            Symbol = symbol;
        }
    }
}
