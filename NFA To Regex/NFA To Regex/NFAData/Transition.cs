namespace NFA_To_Regex.NFAData
{
    public class Transition
    {
        public string FromState { get; set; }

        public string ToState { get; set; }

        public char Symbol { get; set; }

        public Transition()
        {

        }

        public Transition(string fromState, char symbol, string toState)
        {
            if (string.IsNullOrEmpty(fromState))
                throw new ArgumentNullException(nameof(fromState));
            if (string.IsNullOrEmpty(toState))
                throw new ArgumentNullException(nameof(toState));
            if (symbol == '\0')
                throw new ArgumentNullException(nameof(symbol));

            FromState = fromState;
            ToState = toState;
            Symbol = symbol;
        }
    }
}
