namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Denotes model that wraps foreign object
    /// </summary>
    /// <typeparam name="T">type of object wrapped</typeparam>
    public interface IObjectModel<out T>:IModel
    {
        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        T Object { get; }
    }

    /// <summary>
    /// Denotes model that wraps any foreign object
    /// </summary>
    public interface IObjectModel : IObjectModel<object>
    {
    }
}