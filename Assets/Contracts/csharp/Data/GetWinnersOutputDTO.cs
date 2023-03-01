using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Truffle.Data
{
    public partial class GetWinnersOutputDTO : GetWinnersOutputDTOBase { }

    [FunctionOutput]
    public class GetWinnersOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address[]", "", 1)]
        public virtual List<string> ReturnValue1 { get; set; }
    }
}
