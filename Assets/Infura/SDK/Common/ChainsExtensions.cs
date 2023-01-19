using System;

namespace Infura.SDK.Common
{
    /// <summary>
    /// Extension methods for the Chains enum
    /// </summary>
    public static class ChainsExtensions
    {
        /// <summary>
        /// Returns the chain name for a given chain
        /// </summary>
        /// <param name="chain">The chain enum value to get the name for</param>
        /// <returns>The chain name as a string</returns>
        public static string Name(this Chains chain)
        {
            return nameof(chain);
        }

        /// <summary>
        /// Gets the base rpc url for a given chain
        /// </summary>
        /// <param name="chain">The chain to get the base rpc url for</param>
        /// <returns>The base rpc url for the given chain as a string</returns>
        /// <exception cref="ArgumentException">If the given chain has no base RPC url</exception>
        public static string RpcUrl(this Chains chain)
        {
            return chain switch
            {
                Chains.Ethereum => "https://mainnet.infura.io",
                Chains.Goerli => "https://goerli.infura.io",
                Chains.Polygon => "https://polygon-mainnet.infura.io",
                Chains.Mumbai => "https://polygon-mumbai.infura.io",
                _ => throw new ArgumentException("Invalid chain: " + chain)
            };
        }
    }
}