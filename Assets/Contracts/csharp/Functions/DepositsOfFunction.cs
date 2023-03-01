using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Truffle.Data;

namespace Truffle.Functions
{
    public partial class DepositsOfFunction : DepositsOfFunctionBase { }

    [Function("depositsOf", "uint256")]
    public class DepositsOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "payee", 1)]
        public virtual string Payee { get; set; }
    }
}
