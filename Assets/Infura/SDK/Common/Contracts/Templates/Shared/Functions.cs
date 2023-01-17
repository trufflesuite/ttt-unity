using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Infura.SDK.Common.Contracts.Templates.Shared
{
    public partial class NameFunction : NameFunctionBase { }

    [Function("name", "string")]
    public class NameFunctionBase : FunctionMessage
    {

    }
    
    public partial class OwnerFunction : OwnerFunctionBase { }

    [Function("owner", "address")]
    public class OwnerFunctionBase : FunctionMessage
    {

    }
    
    public partial class SymbolFunction : SymbolFunctionBase { }

    [Function("symbol", "string")]
    public class SymbolFunctionBase : FunctionMessage
    {

    }
    
    public partial class SupportsInterfaceFunction : SupportsInterfaceFunctionBase { }

    [Function("supportsInterface", "bool")]
    public class SupportsInterfaceFunctionBase : FunctionMessage
    {
        [Parameter("bytes4", "interfaceId_", 1)]
        public virtual byte[] Interfaceid { get; set; }
    }
    
    public partial class ContractURIFunction : ContractURIFunctionBase { }

    [Function("contractURI", "string")]
    public class ContractURIFunctionBase : FunctionMessage
    {

    }
    
    public partial class TransferOwnershipFunction : TransferOwnershipFunctionBase { }

    [Function("transferOwnership")]
    public class TransferOwnershipFunctionBase : FunctionMessage
    {
        [Parameter("address", "newOwner", 1)]
        public virtual string NewOwner { get; set; }
    }
    
    public partial class SetContractURIFunction : SetContractURIFunctionBase { }

    [Function("setContractURI")]
    public class SetContractURIFunctionBase : FunctionMessage
    {
        [Parameter("string", "contractURI_", 1)]
        public virtual string Contracturi { get; set; }
    }
}