namespace NFA_To_Regex.Exceptions
{
    internal class EmptyAutomateException : Exception
    {
        private const string DefaultMessage = "The automate is empty";

        public EmptyAutomateException() : base(DefaultMessage) { }

        public EmptyAutomateException(string message) : base(message)
        {
        }

    }
}