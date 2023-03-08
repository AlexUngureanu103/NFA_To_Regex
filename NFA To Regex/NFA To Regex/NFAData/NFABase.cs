namespace NFA_To_Regex.NFAData
{
    public class NFABase
    {
        public char Lambda = 'λ';

        public List<string> States { get; set; }

        public List<char> Alphabet { get; set; }

        public List<Transition> Transitions { get; set; }

        public string StartState { get; set; }

        public List<string> FinalStates { get; set; }
    }
}
