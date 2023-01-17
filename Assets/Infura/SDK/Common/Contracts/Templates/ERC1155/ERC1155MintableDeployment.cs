namespace Infura.SDK.Common.Contracts.Templates.ERC1155
{
    public partial class ERC1155MintableDeployment : ERC1155MintableDeploymentBase
    {
        public ERC1155MintableDeployment() : base(BYTECODE) { }
        public ERC1155MintableDeployment(string byteCode) : base(byteCode) { }
    }
}