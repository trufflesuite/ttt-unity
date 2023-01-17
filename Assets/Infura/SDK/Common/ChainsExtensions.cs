using System;

namespace Infura.SDK.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class ChainsExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chain"></param>
        /// <returns></returns>
        public static string AsName(this Chains chain)
        {
            return nameof(chain);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chain"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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