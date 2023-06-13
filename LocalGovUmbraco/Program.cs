namespace LocalGovUmbraco
{
  public class Program
  {
    public static void Main(string[] args)
      => Host.CreateDefaultBuilder(args)
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
