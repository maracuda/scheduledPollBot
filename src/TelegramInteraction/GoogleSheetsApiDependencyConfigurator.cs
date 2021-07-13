using System;
using System.IO;
using System.Threading;

using BusinessLogic;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;

using SimpleInjector;

using Telegram.Bot;

namespace TelegramInteraction
{
    public static class GoogleSheetsApiDependencyConfigurator
    {
        public static void ConfigureTelegramClient(this Container container, IApplicationSettings settings)
        {
            // https://elements.heroku.com/buildpacks/buyersight/heroku-google-application-credentials-buildpack
            
            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromFile().Secrets,
                    new []{SheetsService.Scope.Spreadsheets},
                    "konturSportBot",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            
            var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
        }
    }
}