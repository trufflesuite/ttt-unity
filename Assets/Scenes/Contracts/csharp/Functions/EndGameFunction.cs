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
    public partial class EndGameFunction : EndGameFunctionBase { }

    [Function("endGame")]
    public class EndGameFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "game_id", 1)]
        public virtual BigInteger GameId { get; set; }
        [Parameter("uint256", "winner", 2)]
        public virtual BigInteger Winner { get; set; }
    }
}
