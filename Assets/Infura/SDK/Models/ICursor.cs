namespace Infura.SDK.Models
{
    /// <summary>
    /// An interface that represents a response that is paginated
    /// </summary>
    public interface ICursor
    {
        /// <summary>
        /// The current cursor ID for this query. This is used to get the next page of results.
        /// </summary>
        string Cursor { get; }
    }
}