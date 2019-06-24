using System.Net;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Unickq.Sitecore.BeAutomation.Utils
{
    public static class CommonUtils
    {
//        private const string Layout = @"| ${date:format=HH\:mm\:ss} | ${logger} | ${pad:padding=5:inner=${level:uppercase=true}} | ${message}";
        private const string Layout =
            @"| ${date:format=HH\:mm\:ss} | ${pad:padding=5:inner=${level:uppercase=true}} | ${pad:padding=13:inner=${logger}} | ${message}";

        private static readonly Logger Logger = LogManager.GetLogger("Sc:Util");

        public static string ToOneLine(this string[] strings)
        {
            var sb = new StringBuilder();
            sb.Append("'");
            for (var i = 0; i < strings.Length; i++)
            {
                sb.Append(strings[i]);
                if (i != strings.Length - 1) sb.Append(", ");
            }

            sb.Append("'");
            return sb.ToString();
        }

        public static void ValidateUrlIsRelatedToSitecore(string url)
        {
            return;

            Logger.Debug($"Validating {url} is related to SiteCore");
            url = string.Concat(url, "/sitecore");
            var wReq = (HttpWebRequest) WebRequest.Create(url);
            try
            {
                var wResp = (HttpWebResponse) wReq.GetResponse();
                if (wResp.StatusCode == HttpStatusCode.OK)
                    if (!new WebClient().DownloadString(url).Contains("<body class=\"sc\">"))
                    {
                        Logger.Fatal($"{url} is not related to SiteCore");
                        throw new NotASiteCoreException($"{url} is not related to SiteCore");
                    }
            }
            catch (WebException we)
            {
                Logger.Error($"{we.Message}");
//                Stream receiveStream = we.Response.GetResponseStream();
//                var readStream = new StreamReader(receiveStream);
//                string data = readStream.ReadToEnd();
//                readStream.Close();
//                Console.WriteLine(data);
                throw;
            }
        }

        public static void SetLogger(LogLevel level)
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);
            consoleTarget.Layout = Layout;
            config.LoggingRules.Add(new LoggingRule("*", level, consoleTarget));
            LogManager.Configuration = config;
        }
    }
}
