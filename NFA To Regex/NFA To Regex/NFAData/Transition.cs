namespace NFA_To_Regex.NFAData
{
    public class Transition
    {
        public string FromState { get; set; }

        public string ToState { get; set; }

        public string Symbol { get; set; }

        public Transition()
        {

        }

        public Transition(string fromState, string symbol, string toState)
        {
            if (string.IsNullOrEmpty(fromState))
                throw new ArgumentNullException(nameof(fromState));
            if (string.IsNullOrEmpty(toState))
                throw new ArgumentNullException(nameof(toState));
            if (string.IsNullOrEmpty(symbol))
                throw new ArgumentNullException(nameof(symbol));
            if (symbol.Length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(symbol));
            }

            FromState = fromState;
            ToState = toState;
            Symbol = symbol;
        }
    }
}
