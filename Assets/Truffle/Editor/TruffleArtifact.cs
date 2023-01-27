using System;
using Nethereum.Generators.Model;
using Newtonsoft.Json;

namespace Truffle.Editor
{
    [Serializable]
    public struct TruffleArtifact
    {
        [JsonProperty("contractName")]
        public string ContractName;
            
        [JsonProperty("abi")]
        public object[] ABI;

        [JsonProperty("bytecode")]
        public string ByteCode;

        [JsonIgnore] public ContractABI ContractABI;

        [JsonIgnore] public string JsonPath;

    }
}