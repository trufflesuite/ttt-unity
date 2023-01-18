using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        
        public static async Task<List<T>> AsList<T>(this IObservable<T> observable)
        {
            List<T> nfts = new List<T>();
            TaskCompletionSource<bool> wait = new TaskCompletionSource<bool>();
            
            observable.Subscribe(ni => nfts.Add(ni), () => wait.SetResult(true));
            await wait.Task;
            
            return nfts;
        }
    }
}