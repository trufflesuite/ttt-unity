using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infura.SDK.Common;
using Ipfs;
using Ipfs.CoreApi;
using Ipfs.Http;

namespace Infura.SDK.Network
{
    /// <summary>
    /// A class that provides access to the Infura IPFS API.
    /// </summary>
    public class Ipfs : DisposableBase
    {
        /// <summary>
        /// The backing IPFS Client used to make requests to the Infura IPFS API.
        /// </summary>
        private IpfsClient Client { get; }

        /// <summary>
        /// The project id used to authenticate requests to the Infura IPFS API.
        /// </summary>
        public string ProjectId { get; }
        
        /// <summary>
        /// The project secret used to authenticate requests to the Infura IPFS API.
        /// </summary>
        public string ApiKeySecret { get; }

        /// <summary>
        /// Create a new instance of the Infura IPFS API.
        /// </summary>
        /// <param name="projectId">The project Id to use for authentication</param>
        /// <param name="apiKeySecret">The project secret to use for authentication</param>
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

        /// <summary>
        /// Upload text to IPFS with optional AddFileOptions and CancellationToken. 
        /// </summary>
        /// <param name="source">The text to upload to IPFS</param>
        /// <param name="options">Optional AddFileOptions</param>
        /// <param name="token">Optional CancellationToken to cancel the upload task</param>
        /// <returns>The IPFS URL of the uploaded text</returns>
        public async Task<string> UploadContent(string source, AddFileOptions options = null, CancellationToken? token = null)
        {
            token ??= CancellationToken.None;

            return
                $"ipfs://{(await this.Client.FileSystem.AddTextAsync(source, options, (CancellationToken) token)).Id}";
        }

        /// <summary>
        /// Upload a file to IPFS with the file located at the given source. The source can either be a file path on the
        /// system or a URL.
        /// </summary>
        /// <param name="source">The file path or url of the file to upload to IPFS</param>
        /// <param name="name">The name of the file at the given url. This parameter is ignored if source is a local file</param>
        /// <param name="options">Optional AddFileOptions</param>
        /// <param name="token">Optional CancellationToken to cancel the upload task</param>
        /// <returns>The IPFS URL of the uploaded file</returns>
        /// <exception cref="FileNotFoundException">If the file does not exist on the local filesystem and is not a URL</exception>
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

        /// <summary>
        /// Upload a new directory to IPFS. This will create a new directory and fill the directory with the
        /// files in the source enumerable.
        /// </summary>
        /// <param name="sources">An enumerable of text, where each text entry is a file in the directory</param>
        /// <param name="options">Optional AddFileOptions</param>
        /// <param name="token">Optional CancellationToken to cancel the upload task</param>
        /// <returns>The IPFS URL of the new directory</returns>
        public async Task<string> UploadArray(IEnumerable<string> sources, AddFileOptions options = null,
            CancellationToken? token = null)
        {
            token ??= CancellationToken.None;

            var dag = await Client.Object.NewDirectoryAsync((CancellationToken) token);

            var files = sources.Select(s => Client.FileSystem.AddTextAsync(s, options: options, cancel: (CancellationToken) token));

            var links = (await Task.WhenAll(files)).Select(f => f.ToLink());

            var folder = dag.AddLinks(links);

            var directory = await Client.Object.PutAsync(folder, (CancellationToken) token);

            return $"ipfs://{directory.Id}";
        }

        /// <summary>
        /// Unpin a hash from the IPFS node.
        /// </summary>
        /// <param name="hash">The hash to unpin</param>
        /// <returns>All Cids that were unpined as a result of this operation</returns>
        public Task<IEnumerable<Cid>> UnpinFile(string hash)
        {
            return Client.Pin.RemoveAsync(Cid.Decode(hash));
        }

        /// <summary>
        /// Close the connection to the Infura IPFS API.
        /// </summary>
        /// <returns>The task that will close the connection</returns>
        public Task Close()
        {
            return Client.ShutdownAsync();
        }

        protected override async void DisposeManaged()
        {
            await Close();
        }
    }
}