namespace NFA_To_Regex.Exceptions
{
    internal class EmptyAutomateExceptions : Exception
    {
        private const string DefaultMessage = "The automate is empty";

        public EmptyAutomateExceptions() : base(DefaultMessage) { }

        public EmptyAutomateExceptions(string message) : base(message)
        {
        }

    }
}