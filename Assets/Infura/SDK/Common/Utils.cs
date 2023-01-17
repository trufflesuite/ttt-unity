using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Infura.SDK.Common
{
    public static class Utils
    {
        public static bool IsUri(string uri)
        {
            return Regex.IsMatch(uri, @"^(ipfs|http|https):\/\/");
        }

        public static Stream UrlSource(string url)
        {
            WebClient client = new WebClient();
            return client.OpenRead(url);
        }
    }
}