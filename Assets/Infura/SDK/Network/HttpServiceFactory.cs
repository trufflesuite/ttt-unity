using System;

namespace Infura.SDK.Network
{
    /// <summary>
    /// A factory that creates a new instance of the <see cref="IHttpService"/> interface. A creator function
    /// must first be provided using the <see cref="SetCreator"/> method before the factory can be used.
    /// </summary>
    public static class HttpServiceFactory
    {
        private static Func<string, string, string, IHttpService> _instanceCreator;

        /// <summary>
        /// Create a new HTTP service instance that uses the given base url and authentication token. The token will
        /// be used for all requests made by the service.
        /// </summary>
        /// <param name="baseURL">The base URL to use for all requests</param>
        /// <param name="authValue">The authentication token to use for all requests</param>
        /// <param name="authKey">The authentication header to place the auth token in for all requests</param>
        /// <returns>A new IHttpService instance</returns>
        /// <exception cref="Exception">If no Creator function was set or if the Creator function throws an Exception during creation of the http service</exception>
        public static IHttpService NewHttpService(string baseURL, string authValue, string authKey = "Authorization")
        {
            if (_instanceCreator == null)
                throw new Exception("No IHttpService creator set! Use HttpServiceFactory.SetCreator first");

            if (authKey == "Authorization" && !authValue.StartsWith("Basic"))
                authValue = $"Basic {authValue}";

            return _instanceCreator(baseURL, authValue, authKey);
        }

        public static void SetCreator(Func<string, string, string, IHttpService> creator)
        {
            if (_instanceCreator != null)
                throw new Exception("IHttpService creator already set!");

            _instanceCreator = creator;
        }
    }
}