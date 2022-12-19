using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Infura.SDK;
using Infura.Unity.Utils;
using UnityEngine.Networking;

namespace Infura.Unity.Network
{
    public class UnityHttpService : Singleton<UnityHttpService>
    {
        public class UnityHttpServiceRequest
        {
            public TaskCompletionSource<string> requestTask;
            public string url;
            public string @params;
            public bool isPost;
            public string authKey;
            public string authValue;
        }

        public class UnityHttpServiceProvider : IHttpService
        {
            private UnityHttpService service;
            private string baseUrl;
            private string authValue;
            private string authKey;

            public UnityHttpServiceProvider(string baseUrl, string authKey, string authValue, UnityHttpService service)
            {
                this.service = service;
                this.baseUrl = baseUrl;
                this.authValue = authValue;
                this.authKey = authKey;
            }

            public Task<string> Get(string uri)
            {
                var fullUrl = baseUrl.EndsWith("/") || uri.StartsWith("/") ? $"{baseUrl}{uri}" : $"{baseUrl}/{uri}";
                var request = new UnityHttpServiceRequest()
                {
                    url = fullUrl,
                    requestTask = new TaskCompletionSource<string>(),
                    authKey = authKey,
                    authValue = authValue
                };
                
                service.requests.Enqueue(request);

                return request.requestTask.Task;
            }

            public Task<string> Post(string uri, string @params)
            {
                var fullUrl = baseUrl.EndsWith("/") || uri.StartsWith("/") ? $"{baseUrl}{uri}" : $"{baseUrl}/{uri}";
                var request = new UnityHttpServiceRequest()
                {
                    url = fullUrl,
                    requestTask = new TaskCompletionSource<string>(),
                    isPost = true,
                    @params = @params,
                    authKey = authKey,
                    authValue = authValue
                };
                
                service.requests.Enqueue(request);

                return request.requestTask.Task;
            }
        }

        private Queue<UnityHttpServiceRequest> requests = new();
        private bool isCheckingQueue;

        private void Awake()
        {
            HttpServiceFactory.SetCreator(CreateHttpService);
        }

        private IHttpService CreateHttpService(string baseUrl, string authHeaderValue, string authHeaderName = "Authorization")
        {
            return new UnityHttpServiceProvider(baseUrl, authHeaderName, authHeaderValue, this);
        }

        private void Update()
        {
            if (!isCheckingQueue)
            {
                isCheckingQueue = true;
                StartCoroutine(ProcessQueue());
            }
        }

        private IEnumerator ProcessQueue()
        {
            while (requests.Count > 0)
            {
                yield return ProcessRequest(requests.Dequeue());
            }

            isCheckingQueue = false;
        }

        private IEnumerator ProcessRequest(UnityHttpServiceRequest request)
        {
            using (UnityWebRequest uwr = request.isPost
                       ? new UnityWebRequest(request.url, "POST")
                       : UnityWebRequest.Get(request.url))
            {
                uwr.SetRequestHeader(request.authKey, request.authValue);
                uwr.SetRequestHeader("X-Infura-User-Agent", "infura/sdk-csharp 1.0.0");

                if (request.isPost)
                {
                    byte[] jsonToSend = Encoding.UTF8.GetBytes(request.@params);
                    uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
                    uwr.downloadHandler = new DownloadHandlerBuffer();
                    uwr.uploadHandler.contentType = "application/json";
                    uwr.SetRequestHeader("Content-Type", "application/json");
                }
                
                yield return uwr.SendWebRequest();

                switch (uwr.result)
                {
                    case UnityWebRequest.Result.ConnectionError: 
                    case UnityWebRequest.Result.DataProcessingError:
                    case UnityWebRequest.Result.ProtocolError:
                        request.requestTask.SetException(new IOException(uwr.error));
                        break;
                    case UnityWebRequest.Result.Success:
                        request.requestTask.SetResult(uwr.downloadHandler.text);
                        break;
                }
            }
        }
    }
}