namespace Data
{
    public class LoggerEvent
    {
        public string Message { get; set; }

        public string Level { get; set; }


        public LoggerEvent(string message, string level)
        {
            Message = message;
            Level = level;
        }
    }
}
