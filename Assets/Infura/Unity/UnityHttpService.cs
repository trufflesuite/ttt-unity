using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Infura.SDK;
using UnityEngine.Networking;

namespace Infura.Unity
{
    public class UnityHttpService : Singleton<UnityHttpService>
    {
        public class UnityHttpServiceRequest
        {
            public TaskCompletionSource<string> requestTask;
            public string url;
            public string @params;
            public bool isPost;
            public string apiKey;
        }

        public class UnityHttpServiceProvider : IHttpService
        {
            private UnityHttpService service;
            private string baseUrl;
            private string apiKey;

            public UnityHttpServiceProvider(string baseUrl, string apiKey, UnityHttpService service)
            {
                this.service = service;
                this.baseUrl = baseUrl;
                this.apiKey = apiKey;
            }

            public Task<string> Get(string uri)
            {
                var fullUrl = baseUrl.EndsWith("/") ? $"{baseUrl}{uri}" : $"{baseUrl}/{uri}";
                var request = new UnityHttpServiceRequest()
                {
                    url = fullUrl,
                    requestTask = new TaskCompletionSource<string>(),
                    apiKey = apiKey
                };
                
                service.requests.Enqueue(request);

                return request.requestTask.Task;
            }

            public Task<string> Post(string uri, string @params)
            {
                var fullUrl = baseUrl.EndsWith("/") ? $"{baseUrl}{uri}" : $"{baseUrl}/{uri}";
                var request = new UnityHttpServiceRequest()
                {
                    url = fullUrl,
                    requestTask = new TaskCompletionSource<string>(),
                    isPost = true,
                    @params = @params,
                    apiKey = apiKey
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

        private IHttpService CreateHttpService(string baseUrl, string apiKey)
        {
            return new UnityHttpServiceProvider(baseUrl, apiKey, this);
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
                       ? UnityWebRequest.Post(request.url, request.@params)
                       : UnityWebRequest.Get(request.url))
            {
                uwr.SetRequestHeader("Authorization", $"Basic {request.apiKey}");
                uwr.SetRequestHeader("X-Infura-User-Agent", "infura/sdk-csharp 1.0.0");
                
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