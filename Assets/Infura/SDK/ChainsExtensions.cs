using System;

namespace Infura.SDK
{
    public static class ChainsExtensions
    {
        public static string AsName(this Chains chain)
        {
            return nameof(chain);
        }

        public static string RpcUrl(this Chains chain)
        {
            return chain switch
            {
                Chains.Mainnet => "https://mainnet.infura.io",
                Chains.Goerli => "https://goerli.infura.io",
                Chains.Polygon => "https://polygon-mainnet.infura.io",
                Chains.Mumbai => "https://polygon-mumbai.infura.io",
                _ => throw new ArgumentException("Invalid chain: " + chain)
            };
        }
    }
}