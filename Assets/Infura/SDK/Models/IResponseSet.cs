namespace Infura.SDK.Models
{
    /// <summary>
    /// An interface that represents a Model that contains an array of type T.
    /// </summary>
    /// <typeparam name="T">The type of response array in this model</typeparam>
    public interface IResponseSet<T>
    {
        /// <summary>
        /// The array of type T this response contains
        /// </summary>
        T[] Data { get; }
    }
}