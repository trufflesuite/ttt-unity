using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Infura.SDK.Common.Contracts.Templates.ERC721
{
    public partial class ApproveFunction : ApproveFunctionBase { }

    [Function("approve")]
    public class ApproveFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 2)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
    }

    public partial class GetApprovedFunction : GetApprovedFunctionBase { }

    [Function("getApproved", "address")]
    public class GetApprovedFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
    }
    
    public partial class IsApprovedForAllFunction : IsApprovedForAllFunctionBase { }

    [Function("isApprovedForAll", "bool")]
    public class IsApprovedForAllFunctionBase : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
        [Parameter("address", "operator", 2)]
        public virtual string Operator { get; set; }
    }
    
    public partial class OwnerOfFunction : OwnerOfFunctionBase { }

    [Function("ownerOf", "address")]
    public class OwnerOfFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual BigInteger TokenId { get; set; }
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

    public partial class SafeTransferFromFunction : SafeTransferFromFunctionBase { }

    [Function("safeTransferFrom")]
    public class SafeTransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 3)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class SafeTransferFrom1Function : SafeTransferFrom1FunctionBase { }

    [Function("safeTransferFrom")]
    public class SafeTransferFrom1FunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 3)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("bytes", "_data", 4)]
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
    
    public partial class TransferFromFunction : TransferFromFunctionBase { }

    [Function("transferFrom")]
    public class TransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "tokenId", 3)]
        public virtual BigInteger TokenId { get; set; }
    }

    public partial class ReserveFunction : ReserveFunctionBase { }

    [Function("reserve")]
    public class ReserveFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "quantity_", 1)]
        public virtual BigInteger Quantity { get; set; }
    }

    public partial class MintFunction : MintFunctionBase { }

    [Function("mint")]
    public class MintFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "quantity_", 1)]
        public virtual BigInteger Quantity { get; set; }
    }

    

    public partial class IsSaleActiveFunction : IsSaleActiveFunctionBase { }

    [Function("isSaleActive", "bool")]
    public class IsSaleActiveFunctionBase : FunctionMessage
    {

    }

    public partial class MaxSupplyFunction : MaxSupplyFunctionBase { }

    [Function("maxSupply", "uint256")]
    public class MaxSupplyFunctionBase : FunctionMessage
    {

    }

    public partial class MaxTokenRequestFunction : MaxTokenRequestFunctionBase { }

    [Function("maxTokenRequest", "uint8")]
    public class MaxTokenRequestFunctionBase : FunctionMessage
    {

    }

    public partial class PriceFunction : PriceFunctionBase { }

    [Function("price", "uint256")]
    public class PriceFunctionBase : FunctionMessage
    {

    }

    public partial class RevealFunction : RevealFunctionBase { }

    [Function("reveal")]
    public class RevealFunctionBase : FunctionMessage
    {
        [Parameter("string", "baseURI_", 1)]
        public virtual string Baseuri { get; set; }
    }

    public partial class SetBaseURIFunction : SetBaseURIFunctionBase { }

    [Function("setBaseURI")]
    public class SetBaseURIFunctionBase : FunctionMessage
    {
        [Parameter("string", "baseURI_", 1)]
        public virtual string Baseuri { get; set; }
    }

    public partial class SetMaxTokenRequestFunction : SetMaxTokenRequestFunctionBase { }

    [Function("setMaxTokenRequest")]
    public class SetMaxTokenRequestFunctionBase : FunctionMessage
    {
        [Parameter("uint8", "maxTokenRequest_", 1)]
        public virtual byte Maxtokenrequest { get; set; }
    }

    public partial class SetPriceFunction : SetPriceFunctionBase { }

    [Function("setPrice")]
    public class SetPriceFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "price_", 1)]
        public virtual BigInteger Price { get; set; }
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

    public partial class ToggleSaleFunction : ToggleSaleFunctionBase { }

    [Function("toggleSale")]
    public class ToggleSaleFunctionBase : FunctionMessage
    {

    }

    public partial class TotalSupplyFunction : TotalSupplyFunctionBase { }

    [Function("totalSupply", "uint256")]
    public class TotalSupplyFunctionBase : FunctionMessage
    {

    }

    public partial class WithdrawFunction : WithdrawFunctionBase { }

    [Function("withdraw")]
    public class WithdrawFunctionBase : FunctionMessage
    {

    }
    
    public partial class MintWithTokenURIFunction : MintWithTokenURIFunctionBase { }

    [Function("mintWithTokenURI", "bool")]
    public class MintWithTokenURIFunctionBase : FunctionMessage
    {
        [Parameter("address", "to_", 1)]
        public virtual string To { get; set; }
        [Parameter("string", "tokenURI_", 2)]
        public virtual string Tokenuri { get; set; }
    }
}