using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infura.SDK.Common
{
    /// <summary>
    /// Utils and extension methods
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Determine whether the specified string is a valid URI
        /// </summary>
        /// <param name="uri">The string to check</param>
        /// <returns>Returns true if the given string is a uri, otherwise false</returns>
        public static bool IsUri(string uri)
        {
            return Regex.IsMatch(uri, @"^(ipfs|http|https):\/\/");
        }

        /// <summary>
        /// Turn a URL string into a Stream.
        /// </summary>
        /// <param name="url">The URL to download data from</param>
        /// <returns>A read stream using a WebClient that connects to the given URL</returns>
        public static Stream UrlSource(string url)
        {
            WebClient client = new WebClient();
            return client.OpenRead(url);
        }
        
        /// <summary>
        /// Convert an IObservable to a Task. The list will include all items from the observable that will ever
        /// exist in the observable. The task will complete when the observable completes.
        /// </summary>
        /// <param name="observable">The observable to convert to a list</param>
        /// <typeparam name="T">The type of the observable and resulting list</typeparam>
        /// <returns>An async task that returns a list. The task will complete when the observable completes.</returns>
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