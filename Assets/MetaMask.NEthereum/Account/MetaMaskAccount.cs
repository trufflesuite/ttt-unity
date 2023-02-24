using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Accounts;
using Nethereum.RPC.NonceServices;
using Nethereum.RPC.TransactionManagers;

namespace MetaMask.NEthereum
{
    public class MetaMaskAccount : IAccount
    {
        private readonly MetaMaskWallet _wallet;
        private readonly IClient _client;

        public string Address
        {
            get
            {
                return _wallet.SelectedAddress;
            }
        }

        public ITransactionManager TransactionManager { get; }
        public INonceService NonceService { get; set; }

        public IClient Client
        {
            get
            {
                return _client;
            }
        }

        public MetaMaskAccount(MetaMaskWallet wallet, IClient client)
        {
            _wallet = wallet;
            _client = client;
            TransactionManager = new MetaMaskTransactionManager(this);
            NonceService = new InMemoryNonceService(_wallet.SelectedAddress, client);
        }
    }
}