namespace NFA_To_Regex.NFAData
{
    public class NFABase
    {
        public char Lambda = '#';

        public List<string> States { get; set; }

        public List<string> Alphabet { get; set; }

        public List<Transition> Transitions { get; set; }

        public string StartState { get; set; }

        public List<string> FinalStates { get; set; }
    }
}
