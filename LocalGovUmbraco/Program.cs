namespace LocalGovUmbraco
{
  public class Program
  {
    public static void Main(string[] args)
      => Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(config => config.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true))
        .ConfigureUmbracoDefaults()
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.UseStaticWebAssets();
          webBuilder.UseStartup<Startup>();
        })
        .Build()
        .Run();
  }
}
