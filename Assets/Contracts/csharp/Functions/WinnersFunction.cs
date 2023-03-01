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
    public partial class WinnersFunction : WinnersFunctionBase { }

    [Function("winners", "address")]
    public class WinnersFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }
}
