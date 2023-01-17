namespace Infura.SDK.Common.Contracts.Templates.ERC721
{
    public partial class ERC721UserMintableDeployment : ERC721UserMintableDeploymentBase
    {
        public ERC721UserMintableDeployment() : base(BYTECODE) { }
        public ERC721UserMintableDeployment(string byteCode) : base(byteCode) { }
    }
}