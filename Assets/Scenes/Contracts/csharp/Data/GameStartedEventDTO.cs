using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Truffle.Data
{
    public partial class GameStartedEventDTO : GameStartedEventDTOBase { }

    [Event("GameStarted")]
    public class GameStartedEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "game_id", 1, false )]
        public virtual BigInteger GameId { get; set; }
    }
}
