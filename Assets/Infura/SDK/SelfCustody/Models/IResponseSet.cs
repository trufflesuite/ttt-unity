namespace Infura.SDK.SelfCustody.Models
{
    public interface IResponseSet<T>
    {
        T[] Data { get; }
    }
}