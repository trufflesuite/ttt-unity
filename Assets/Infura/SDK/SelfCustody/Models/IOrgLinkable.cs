using System.Threading.Tasks;
using Infura.SDK.Organization;

namespace Infura.SDK.SelfCustody.Models
{
    public interface IOrgLinkable
    {
        Task TryLinkOrganization(OrgApiClient client);
    }
}