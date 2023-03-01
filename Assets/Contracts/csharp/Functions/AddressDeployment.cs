using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Truffle.Functions
{
    public partial class AddressDeployment : AddressDeploymentBase
    {
        public AddressDeployment() : base(BYTECODE) { }
        public AddressDeployment(string byteCode) : base(byteCode) { }
    }

    public class AddressDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "0x60566050600b82828239805160001a6073146043577f4e487b7100000000000000000000000000000000000000000000000000000000600052600060045260246000fd5b30600052607381538281f3fe73000000000000000000000000000000000000000030146080604052600080fdfea264697066735822122056388b4bd61a0a82b1b5642110ef2e3e47829ce58d55205c0109152159a2901a64736f6c63430008120033";
        public AddressDeploymentBase() : base(BYTECODE) { }
        public AddressDeploymentBase(string byteCode) : base(byteCode) { }

    }
}
