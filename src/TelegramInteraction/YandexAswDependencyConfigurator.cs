using Amazon.Runtime;
using Amazon.S3;

using BusinessLogic;

using SimpleInjector;

namespace TelegramInteraction;

public static class YandexAswDependencyConfigurator
{
    public static void ConfigureYandexAws(this Container container, IApplicationSettings settings
    )
    {
        var accessKey = settings.GetString("AwsAccessKey");
        var secretKey = settings.GetString("AwsSecretKey");
        var amazonS3Client = new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey), new AmazonS3Config
            {
                ServiceURL = "https://s3.yandexcloud.net",
            });

        container.RegisterInstance<IAmazonS3>(amazonS3Client);
    }
}