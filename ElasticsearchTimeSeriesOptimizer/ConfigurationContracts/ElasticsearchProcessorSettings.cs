using System;
namespace StoneCo.ElasticsearchTimeSeriesOptimizer.ConfigurationContracts
{
    /// <summary>
    /// Elasticsearch processor settings.
    /// </summary>
    public class ElasticsearchProcessorSettings
    {
		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		/// <value>The address.</value>
		public Uri Address { get; set; }

		/// <summary>
		/// Gets or sets the elasticsearch time out in seconds.
		/// </summary>
		/// <value>The elasticsearch time out in seconds.</value>
		public int TimeOutInSeconds { get; set; }        
    }
}
