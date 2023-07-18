namespace INT.Application.Setup
{
    public class AppOptions
    {
        public AppEnvironment Environment { get; set; }
    }

    public class AppEnvironment
    {
        public string Value01 { get; set; }
        public bool EnableHSTS { get; set; }
    }
}
