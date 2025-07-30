namespace weather_api_sysprog_1.Core.Entities
{
    public class Alert
    {
        public string Headline { get; set; } = string.Empty;
        public string MsgType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Urgency { get; set; } = string.Empty;
        public string Areas { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Certainty { get; set; } = string.Empty;
        public string Event { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public DateTime Effective { get; set; }
        public DateTime Expires { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Instruction { get; set; } = string.Empty;
    }
}
