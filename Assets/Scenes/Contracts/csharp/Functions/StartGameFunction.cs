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
    public partial class StartGameFunction : StartGameFunctionBase { }

    [Function("startGame")]
    public class StartGameFunctionBase : FunctionMessage
    {
        [Parameter("address", "payout_x", 1)]
        public virtual string PayoutX { get; set; }
        [Parameter("address", "payout_o", 2)]
        public virtual string PayoutO { get; set; }
    }
}
