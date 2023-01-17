using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Infura.SDK.Common.Contracts.Templates.ERC1155
{
    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
        [Parameter("uint256", "id", 2)]
        public virtual BigInteger Id { get; set; }
    }

    public partial class BalanceOfBatchFunction : BalanceOfBatchFunctionBase { }

    [Function("balanceOfBatch", "uint256[]")]
    public class BalanceOfBatchFunctionBase : FunctionMessage
    {
        [Parameter("address[]", "accounts", 1)]
        public virtual List<string> Accounts { get; set; }
        [Parameter("uint256[]", "ids", 2)]
        public virtual List<BigInteger> Ids { get; set; }
    }
    
    public partial class IsApprovedForAllFunction : IsApprovedForAllFunctionBase { }

    [Function("isApprovedForAll", "bool")]
    public class IsApprovedForAllFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
        [Parameter("address", "operator", 2)]
        public virtual string Operator { get; set; }
    }
    
    public partial class RoyaltyInfoFunction : RoyaltyInfoFunctionBase { }

    [Function("royaltyInfo", typeof(RoyaltyInfoOutputDTO))]
    public class RoyaltyInfoFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("uint256", "_salePrice", 2)]
        public virtual BigInteger SalePrice { get; set; }
    }

    public partial class SafeBatchTransferFromFunction : SafeBatchTransferFromFunctionBase { }

    [Function("safeBatchTransferFrom")]
    public class SafeBatchTransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256[]", "ids", 3)]
        public virtual List<BigInteger> Ids { get; set; }
        [Parameter("uint256[]", "amounts", 4)]
        public virtual List<BigInteger> Amounts { get; set; }
        [Parameter("bytes", "data", 5)]
        public virtual byte[] Data { get; set; }
    }

    public partial class SafeTransferFromFunction : SafeTransferFromFunctionBase { }

    [Function("safeTransferFrom")]
    public class SafeTransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "id", 3)]
        public virtual BigInteger Id { get; set; }
        [Parameter("uint256", "amount", 4)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("bytes", "data", 5)]
        public virtual byte[] Data { get; set; }
    }

    public partial class SetApprovalForAllFunction : SetApprovalForAllFunctionBase { }

    [Function("setApprovalForAll")]
    public class SetApprovalForAllFunctionBase : FunctionMessage
    {
        [Parameter("address", "operator", 1)]
        public virtual string Operator { get; set; }
        [Parameter("bool", "approved", 2)]
        public virtual bool Approved { get; set; }
    }
    
    public partial class MintFunction : MintFunctionBase { }

    [Function("mint")]
    public class MintFunctionBase : FunctionMessage
    {
        [Parameter("address", "to_", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256", "id_", 2)]
        public virtual BigInteger Id { get; set; }
        [Parameter("uint256", "quantity_", 3)]
        public virtual BigInteger Quantity { get; set; }
    }

    public partial class MintBatchFunction : MintBatchFunctionBase { }

    [Function("mintBatch")]
    public class MintBatchFunctionBase : FunctionMessage
    {
        [Parameter("address", "to_", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256[]", "ids_", 2)]
        public virtual List<BigInteger> Ids { get; set; }
        [Parameter("uint256[]", "quantities_", 3)]
        public virtual List<BigInteger> Quantities { get; set; }
    }

    public partial class AddIdsFunction : AddIdsFunctionBase { }

    [Function("addIds")]
    public class AddIdsFunctionBase : FunctionMessage
    {
        [Parameter("uint256[]", "ids_", 1)]
        public virtual List<BigInteger> Ids { get; set; }
    }

    public partial class SetURIFunction : SetURIFunctionBase { }

    [Function("setURI")]
    public class SetURIFunctionBase : FunctionMessage
    {
        [Parameter("string", "newUri_", 1)]
        public virtual string Newuri { get; set; }
    }

    public partial class SetRoyaltiesFunction : SetRoyaltiesFunctionBase { }

    [Function("setRoyalties")]
    public class SetRoyaltiesFunctionBase : FunctionMessage
    {
        [Parameter("address", "receiver_", 1)]
        public virtual string Receiver { get; set; }
        [Parameter("uint96", "feeNumerator_", 2)]
        public virtual BigInteger Feenumerator { get; set; }
    }
    
    public partial class UriFunction : UriFunctionBase { }

    [Function("uri", "string")]
    public class UriFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId_", 1)]
        public virtual BigInteger Tokenid { get; set; }
    }
}