using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Truffle.Functions
{
    public partial class PullPaymentDeployment : PullPaymentDeploymentBase
    {
        public PullPaymentDeployment() : base(BYTECODE) { }
        public PullPaymentDeployment(string byteCode) : base(byteCode) { }
    }

    public class PullPaymentDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "0x";
        public PullPaymentDeploymentBase() : base(BYTECODE) { }
        public PullPaymentDeploymentBase(string byteCode) : base(byteCode) { }

    }
}
