using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Infura.SDK.Common.Contracts.Templates.Shared
{
    public partial class OwnershipTransferredEventDTO : OwnershipTransferredEventDTOBase { }

    [Event("OwnershipTransferred")]
    public class OwnershipTransferredEventDTOBase : IEventDTO
    {
        [Parameter("address", "previousOwner", 1, true )]
        public virtual string PreviousOwner { get; set; }
        [Parameter("address", "newOwner", 2, true )]
        public virtual string NewOwner { get; set; }
    }
    
    public partial class NameOutputDTO : NameOutputDTOBase { }

    [FunctionOutput]
    public class NameOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class OwnerOutputDTO : OwnerOutputDTOBase { }

    [FunctionOutput]
    public class OwnerOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }
    
    public partial class SymbolOutputDTO : SymbolOutputDTOBase { }

    [FunctionOutput]
    public class SymbolOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }
    
    public partial class ContractURIOutputDTO : ContractURIOutputDTOBase { }

    [FunctionOutput]
    public class ContractURIOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }
    
    public partial class SupportsInterfaceOutputDTO : SupportsInterfaceOutputDTOBase { }

    [FunctionOutput]
    public class SupportsInterfaceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }
    
    public partial class AssertionFailedEventDTO : AssertionFailedEventDTOBase { }

    [Event("AssertionFailed")]
    public class AssertionFailedEventDTOBase : IEventDTO
    {
        [Parameter("string", "message", 1, false )]
        public virtual string Message { get; set; }
    }

    public partial class AssertionFailedDataEventDTO : AssertionFailedDataEventDTOBase { }

    [Event("AssertionFailedData")]
    public class AssertionFailedDataEventDTOBase : IEventDTO
    {
        [Parameter("int256", "eventId", 1, false )]
        public virtual BigInteger EventId { get; set; }
        [Parameter("bytes", "encodingData", 2, false )]
        public virtual byte[] EncodingData { get; set; }
    }

    public partial class ContractDeployedEventDTO : ContractDeployedEventDTOBase { }

    [Event("ContractDeployed")]
    public class ContractDeployedEventDTOBase : IEventDTO
    {
        [Parameter("address", "contractAddress_", 1, false )]
        public virtual string Contractaddress { get; set; }
    }
}