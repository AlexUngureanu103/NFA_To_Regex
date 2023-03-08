namespace NFA_To_Regex
{
    internal class NFA
    {
        public char Lambda = 'λ';

        public List<string> States { get; set; }

        public List<char> Alphabet { get; set; }

        public List<Transition> Transitions { get; set; }

        public string StartState { get; set; }

        public List<string> FinalStates { get; set; }
    }
}
