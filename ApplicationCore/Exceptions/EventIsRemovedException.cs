namespace ApplicationCore.Exceptions{
    public class EventIsRemovedException : Exception
    {
        public EventIsRemovedException() : base() { }
        public EventIsRemovedException(string message) : base(message) { }
        public EventIsRemovedException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
