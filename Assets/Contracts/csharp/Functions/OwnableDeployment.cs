using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Truffle.Functions
{
    public partial class OwnableDeployment : OwnableDeploymentBase
    {
        public OwnableDeployment() : base(BYTECODE) { }
        public OwnableDeployment(string byteCode) : base(byteCode) { }
    }

    public class OwnableDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "0x";
        public OwnableDeploymentBase() : base(BYTECODE) { }
        public OwnableDeploymentBase(string byteCode) : base(byteCode) { }

    }
}
