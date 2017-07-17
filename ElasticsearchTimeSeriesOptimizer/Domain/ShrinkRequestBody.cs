namespace Stoneco.ElasticsearchTimeSeriesOptimizer.Domain
{

    /// <summary>
    /// Shrink request body.
    /// </summary>
    public class ShrinkRequestBody
    {
        /// <summary>
        /// Gets or sets the name of the index name.
        /// </summary>
        /// <value>The name of the index name.</value>
        public string IndexName { get; set; }
    }
}
