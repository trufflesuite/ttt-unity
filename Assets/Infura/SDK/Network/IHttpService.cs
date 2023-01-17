using System.Threading.Tasks;

namespace Infura.SDK.Network
{
    public interface IHttpService
    {
        Task<string> Get(string uri);

        Task<string> Post(string uri, string @params);
    }
}