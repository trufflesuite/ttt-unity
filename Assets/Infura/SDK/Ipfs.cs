using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ipfs;
using Ipfs.CoreApi;
using Ipfs.Http;

namespace Infura.SDK
{
    public class Ipfs
    {
        private IpfsClient Client { get; }

        public string ProjectId { get; }
        
        public string ApiKeySecret { get; }

        public Ipfs(string projectId, string apiKeySecret)
        {
            ProjectId = projectId;
            ApiKeySecret = apiKeySecret;

            Client = new IpfsClient("https://ipfs.infura.io:5001");
            
            ForceAuthHeaders($"{projectId}:{apiKeySecret}");
        }

        // Found from this Github issue:
        // https://github.com/richardschneider/net-ipfs-http-client/issues/67#issuecomment-897248835
        private void ForceAuthHeaders(string authHeader)
        {
            var httpClientInfo = typeof(IpfsClient).GetField("api", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (httpClientInfo == null) throw new Exception("Could not find api field for Auth Header setting");
            
            var apiObj = httpClientInfo.GetValue(null);
            if (apiObj != null) throw new Exception("Could not get value of api field for Auth Header setting");
            
            MethodInfo createMethod = typeof(IpfsClient).GetMethod("Api", BindingFlags.NonPublic | BindingFlags.Instance);
            if (createMethod == null) throw new Exception("Could not find Api method for Auth Header setting");
            
            var o = createMethod.Invoke(Client, Array.Empty<object>());
            var httpClient = o as HttpClient;

            var byteArray = Encoding.ASCII.GetBytes(authHeader);
            if (httpClient == null) throw new Exception("Could not get HttpClient for Auth Header setting");
            
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task<string> UploadContent(string source, AddFileOptions options = null, CancellationToken? token = null)
        {
            token ??= CancellationToken.None;

            return
                $"ipfs://{(await this.Client.FileSystem.AddTextAsync(source, options, (CancellationToken) token)).Id}";
        }

        public async Task<string> UploadFile(string source, string name = "", AddFileOptions options = null, CancellationToken? token = null)
        {
            token ??= CancellationToken.None;

            if (Utils.IsUri(source))
            {
                return $"ipfs://{(await this.Client.FileSystem.AddAsync(Utils.UrlSource(source), name, options, (CancellationToken) token)).Id}";
            }

            if (!System.IO.File.Exists(source))
            {
                throw new FileNotFoundException("Could not find file " + source);
            }

            return
                $"ipfs://{(await this.Client.FileSystem.AddFileAsync(source, options, (CancellationToken) token)).Id}";
        }

        public async Task<string> UploadArray(string[] sources, bool isErc1155 = false, AddFileOptions options = null,
            CancellationToken? token = null)
        {
            token ??= CancellationToken.None;

            var dag = await Client.Object.NewDirectoryAsync((CancellationToken) token);

            var files = sources.Select(s => Client.FileSystem.AddTextAsync(s, cancel: (CancellationToken) token));

            var links = (await Task.WhenAll(files)).Select(f => f.ToLink());

            var folder = dag.AddLinks(links);

            var directory = await Client.Object.PutAsync(folder, (CancellationToken) token);

            return $"ipfs://{directory.Id}";
        }

        public Task<IEnumerable<Cid>> UnpinFile(string hash)
        {
            return Client.Pin.RemoveAsync(Cid.Decode(hash));
        }

        public Task Close()
        {
            return Client.ShutdownAsync();
        }
    }
}