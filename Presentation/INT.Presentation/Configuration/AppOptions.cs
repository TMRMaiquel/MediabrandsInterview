namespace INT.Presentation.Configuration
{
    public class AppOptions
  {
    public AppEnvironment Environment { get; set; }
  }

  public class AppEnvironment
  {
    public bool EnableHSTS { get; set; }
  }
}
