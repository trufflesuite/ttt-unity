using System.Threading.Tasks;
using Infura.SDK.Organization;

namespace Infura.SDK.Models
{
    /// <summary>
    /// An interface that represents a model that can gather additional Organization information.
    /// </summary>
    public interface IOrgLinkable
    {
        /// <summary>
        /// Attempt to gather additional Organization information given the Organization API. 
        /// </summary>
        /// <param name="client">The Organization API to use to gather additional information</param>
        Task<bool> TryLinkOrganization(OrgApiClient client);
    }
}