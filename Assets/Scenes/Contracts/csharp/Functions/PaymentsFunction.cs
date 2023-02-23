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
    public partial class PaymentsFunction : PaymentsFunctionBase { }

    [Function("payments", "uint256")]
    public class PaymentsFunctionBase : FunctionMessage
    {
        [Parameter("address", "dest", 1)]
        public virtual string Dest { get; set; }
    }
}
