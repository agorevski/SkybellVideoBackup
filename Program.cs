using CommandLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BackupVideos
{
    internal class Program
    {
        private const string CLIENT_ID = "5eb04d99b579f3f00303e40841a8283c46aefc15a2237727";
        private const string CLIENT_SECRET = "1c4dbc6faa6b2e477744c05d5d1ab1e2049bc0bac30ea325";
        private const string SKYBELL_APP_ID = "4adbb8c2-bd46-466b-a211-6e4b6b13013e"; // This is a random guid

        private static void Main(string[] args)
        {
            var options = new Options();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opts => options = opts)
                .WithNotParsed(errs =>
                {
                    throw new ArgumentException("Errors when parsing command line!");
                });

            var curTime = DateTime.Now;
            var saveDir = options.OutputDir;

            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }

            var client = new HttpClient();


            var loginUrl = "https://cloud.myskybell.com/api/v3/login";


            var dict = new Dictionary<string, string>()
            {
                { "username", options.Username },
                { "password", options.Password },
                { "protocol", "gcm" },
                { "token", SKYBELL_APP_ID },
                { "grant_type", "password" },
                { "client_id", CLIENT_ID },
                { "client_secret", CLIENT_SECRET }
            };
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("en-US"));
            client.DefaultRequestHeaders.Add("x-skybell-app-id", SKYBELL_APP_ID);
            client.DefaultRequestHeaders.Add("x-skybell-client-id", CLIENT_ID);

            var stringContent = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/json");

            var loginRequest = client.PostAsync(loginUrl, stringContent);
            loginRequest.Wait();
            var loginResponse = loginRequest.Result.Content.ReadAsStringAsync();
            loginResponse.Wait();
            var loginData = JsonConvert.DeserializeObject<LoginResponse>(loginResponse.Result);

            var authorization_header = $"Bearer {loginData.access_token}";
            client.DefaultRequestHeaders.Add("Authorization", authorization_header);

            var subscriptionUrl = "https://cloud.myskybell.com/api/v3/subscriptions?include=device,owner";
            var subscriptionInfoRequest = client.GetAsync(subscriptionUrl);
            subscriptionInfoRequest.Wait();
            var subscriptionInfoResponse = subscriptionInfoRequest.Result.Content.ReadAsStringAsync();
            subscriptionInfoResponse.Wait();
            var subscriptionInfo = JsonConvert.DeserializeObject<List<SubscriptionInfo>>(subscriptionInfoResponse.Result);

            var subscription_id = subscriptionInfo[0].id;
            var user_id = subscriptionInfo[0].user;


            var activitiesUrl = $"https://cloud.myskybell.com/v4/users/{user_id}/activities";
            var activitiesRequest = client.GetAsync(activitiesUrl);
            activitiesRequest.Wait();
            var activitiesResponse = activitiesRequest.Result.Content.ReadAsStringAsync();
            activitiesResponse.Wait();
            var activities = JsonConvert.DeserializeObject<Activities>(activitiesResponse.Result);

            var datum = activities.data;

            var folderNames = datum.Select(e => ConvertDateToString(e.ttlStartDate)).Distinct();
            foreach(var folder in folderNames)
            {
                var folderPath = Path.Combine(options.OutputDir, folder);
                if(!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }

            var ids = datum.Select(e => e.id).Distinct();

            Parallel.ForEach(datum, new ParallelOptions() { MaxDegreeOfParallelism = 5 }, vid =>
            {
                var videoUrl = $"https://cloud.myskybell.com/api/v3/subscriptions/{subscription_id}/activities/{vid.id}/video";
                var videoRequest = client.GetAsync(videoUrl);
                videoRequest.Wait();
                var videoResponse = videoRequest.Result.Content.ReadAsStringAsync();
                videoResponse.Wait();
                var video = JsonConvert.DeserializeObject<Video>(videoResponse.Result);
                var uri = new Uri(video.url);
                var folder = ConvertDateToString(vid.ttlStartDate);
                var filename = uri.Segments.Last();
                var fullPath = Path.Combine(options.OutputDir, folder, filename);
                var flag = false;
                while (!flag)
                {
                    try
                    {
                        if (!File.Exists(fullPath))
                        {
                            using (WebClient webClient = new WebClient())
                            {
                                Console.WriteLine($"Downloading {folder} / {uri.Segments.Last()}..");
                                webClient.DownloadFile(uri, fullPath);
                            }
                        }
                        flag = true;
                    }
                    catch
                    {
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                    }
                }
            });
        }

        public static string ConvertDateToString(DateTime curDay)
        {
            var year = curDay.Year.ToString("00");
            var month = curDay.Month.ToString("00");
            var day = curDay.Day.ToString("00");
            return $"{year}{month}{day}";
        }

    }
}