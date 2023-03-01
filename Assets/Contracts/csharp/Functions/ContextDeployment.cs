using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Truffle.Functions
{
    public partial class ContextDeployment : ContextDeploymentBase
    {
        public ContextDeployment() : base(BYTECODE) { }
        public ContextDeployment(string byteCode) : base(byteCode) { }
    }

    public class ContextDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "0x";
        public ContextDeploymentBase() : base(BYTECODE) { }
        public ContextDeploymentBase(string byteCode) : base(byteCode) { }

    }
}
