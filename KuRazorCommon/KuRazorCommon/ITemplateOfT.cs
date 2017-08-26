namespace KuRazorCommon
{
    public interface ITemplate<T> : ITemplate
    {
        #region Properties
        /// <summary>
        /// Gets the or sets the model.
        /// </summary>
        T Model { get; set; }
        #endregion
    }
}