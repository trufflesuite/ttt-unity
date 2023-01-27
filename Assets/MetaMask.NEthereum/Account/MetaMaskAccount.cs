using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Accounts;
using Nethereum.RPC.NonceServices;
using Nethereum.RPC.TransactionManagers;

namespace MetaMask.NEthereum
{
    public class MetaMaskAccount : IAccount
    {
        private readonly MetaMaskWallet _wallet;

        public string Address
        {
            get
            {
                return _wallet.SelectedAddress;
            }
        }

        public ITransactionManager TransactionManager { get; }
        public INonceService NonceService { get; set; }
        
        public MetaMaskAccount(MetaMaskWallet wallet, IClient client)
        {
            _wallet = wallet;
            TransactionManager = new TransactionManager(client);
            NonceService = new InMemoryNonceService(_wallet.SelectedAddress, client);
        }
    }
}