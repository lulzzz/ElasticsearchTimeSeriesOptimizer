using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Elasticsearch.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StoneCo.ElasticsearchTimeSeriesOptimizer.ConfigurationContracts;
using StoneCo.ElasticsearchTimeSeriesOptimizer.ServiceLayer.Contracts;

namespace StoneCo.ElasticsearchTimeSeriesOptimizer.Processors
{
    /// <summary>
    /// Elasticsearch processor.
    /// </summary>
    public class ElasticsearchProcessor : IElasticsearchProcessor
    {

        #region Private properties

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        private IElasticLowLevelClient Client { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with IElasticLowLevelClient.
        /// </summary>
        /// <param name="client">Client.</param>
        public ElasticsearchProcessor(IElasticLowLevelClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Constructor with ElasticsearchProcessorSettings.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public ElasticsearchProcessor(ElasticsearchProcessorSettings settings)
        {

            ConnectionConfiguration esSettings = new ConnectionConfiguration(settings.Address)
                .RequestTimeout(TimeSpan.FromSeconds(settings.TimeOutInSeconds));

            Client = new ElasticLowLevelClient(esSettings);
        }

		#endregion

		#region Private methods

		/// <summary>
		/// Returns the node id on cluter with highest free space on disk.
		/// </summary>
		/// <returns>The node with higher free space on disk.</returns>
		private string GetNodeIdWithHigherFreeSpaceOnDisk()
        {
            ElasticsearchResponse<byte[]> response = Client.NodesStatsForAll<byte[]>("fs");
            JObject nodes = (JObject) JsonConvert.DeserializeObject(Encoding.UTF8.GetString(response.Body));

            IDictionary<string, long> nodesFreeSpace = new Dictionary<string, long>();
            foreach (JToken node in nodes["nodes"].Children())
            {
                // If is not a data node, skip.
                if (node.First()["roles"].Values().Contains("data") == false)
                {
                    continue;
                }

				// Getting the node id.
				string nodeId = node.First.Parent.Path.Split('.').Last();

                // Sum all data partitions free space
                long freeInBytes = 0;
                foreach(JToken data in node.First["fs"]["data"])
                {
                    freeInBytes += data["free_in_bytes"].Value<long>();
                }

                nodesFreeSpace.Add(nodeId, freeInBytes);
            }

            // Return the id of the node with max free space.
            return nodesFreeSpace.First((KeyValuePair<string, long> arg) => arg.Value == nodesFreeSpace.Values.Max()).Key;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Shrink the specified indexName.
        /// </summary>
        /// <returns>The shrink.</returns>
        /// <param name="indexName">Index name.</param>
        public ElasticsearchResponse<VoidResponse> Shrink(string indexName)
        {
            #region Validation

            if(string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentNullException(nameof(indexName));
            }

            #endregion

            // Gets the id of the data node with highest free space on disk.
            string nodeId = this.GetNodeIdWithHigherFreeSpaceOnDisk();



            throw new NotImplementedException();
        }

        #endregion

    }
}
