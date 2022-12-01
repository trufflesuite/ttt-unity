using System.Threading.Tasks;

namespace Infura.SDK
{
    public interface IHttpService
    {
        Task<string> Get(string uri);

        Task<string> Post(string uri, string @params);
    }
}