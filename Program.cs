using Microsoft.Extensions.FileProviders;

namespace CsvFileUpload
{
    class Program
    {
        private static string GetConfigValue(IConfiguration config, string key)
        {
            string value = config[key] ?? throw new ArgumentNullException(key);
            return value;
        }

        static void Main(string[] args)
        {
            // Load environment variables from env.txt
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("env.txt")
                .Build();

            // Access environment variables
            string awsAccessKeyId = GetConfigValue(config, "AWS_ACCESS_KEY_ID");
            string awsSecretAccessKey = GetConfigValue(config, "AWS_SECRET_ACCESS_KEY");
            string awsRegion = GetConfigValue(config, "AWS_REGION");
            string s3BucketName = GetConfigValue(config, "S3_BUCKET_NAME");

            var host = new WebHostBuilder()
                .UseKestrel()
                .ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddControllers();
                    services.AddSingleton(new FileUploadService(awsAccessKeyId, awsSecretAccessKey, awsRegion, s3BucketName));
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(
                            Path.Combine(Directory.GetCurrentDirectory())),
                        RequestPath = ""
                    });
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                })
                .Build();

            host.Run();
        }
    }
}