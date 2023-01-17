namespace Infura.SDK.Common.Contracts.Templates.ERC721
{
    public partial class ERC721MintableDeployment : ERC721MintableDeploymentBase
    {
        public ERC721MintableDeployment() : base(BYTECODE) { }
        public ERC721MintableDeployment(string byteCode) : base(byteCode) { }
    }
}