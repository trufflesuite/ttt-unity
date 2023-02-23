using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Truffle.Data
{
    public partial class GameWonEventDTO : GameWonEventDTOBase { }

    [Event("GameWon")]
    public class GameWonEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "game_id", 1, false )]
        public virtual BigInteger GameId { get; set; }
        [Parameter("address", "winner", 2, false )]
        public virtual string Winner { get; set; }
        [Parameter("uint256", "amount", 3, false )]
        public virtual BigInteger Amount { get; set; }
    }
}
