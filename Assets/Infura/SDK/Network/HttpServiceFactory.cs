using System;

namespace Infura.SDK.Network
{
    public static class HttpServiceFactory
    {
        private static Func<string, string, string, IHttpService> _instanceCreator;

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