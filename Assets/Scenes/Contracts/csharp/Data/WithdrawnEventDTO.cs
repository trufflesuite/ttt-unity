using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Truffle.Data
{
    public partial class WithdrawnEventDTO : WithdrawnEventDTOBase { }

    [Event("Withdrawn")]
    public class WithdrawnEventDTOBase : IEventDTO
    {
        [Parameter("address", "payee", 1, true )]
        public virtual string Payee { get; set; }
        [Parameter("uint256", "weiAmount", 2, false )]
        public virtual BigInteger WeiAmount { get; set; }
    }
}
