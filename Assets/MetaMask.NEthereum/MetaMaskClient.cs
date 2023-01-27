using System.Threading.Tasks;
using MetaMask.Models;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MetaMask.NEthereum
{
    public class MetaMaskClient : ClientBase
    {
        private MetaMaskWallet _metaMask;

        public MetaMaskClient(MetaMaskWallet metaMask)
        {
            this._metaMask = metaMask;
        }

        protected override async Task<RpcResponseMessage> SendAsync(RpcRequestMessage rpcRequestMessage, string route = null)
        {
            var response = await _metaMask.Request(new MetaMaskEthereumRequest()
            {
                Id = rpcRequestMessage.Id.ToString(),
                Method = rpcRequestMessage.Method,
                Parameters = rpcRequestMessage.RawParameters
            });

            var convertedResponse = JsonConvert.DeserializeObject<JToken>(response.ToString());

            return new RpcResponseMessage(rpcRequestMessage.Id, convertedResponse);
        }
    }
}