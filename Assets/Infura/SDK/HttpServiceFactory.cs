using System;

namespace Infura.SDK
{
    public static class HttpServiceFactory
    {
        private static Func<string, string, IHttpService> _instanceCreator;

        public static IHttpService NewHttpService(string baseURL, string apiKey)
        {
            if (_instanceCreator == null)
                throw new Exception("No IHttpService creator set! Use HttpServiceFactory.SetCreator first");

            return _instanceCreator(baseURL, apiKey);
        }

        public static void SetCreator(Func<string, string, IHttpService> creator)
        {
            if (_instanceCreator != null)
                throw new Exception("IHttpService creator already set!");

            _instanceCreator = creator;
        }
    }
}