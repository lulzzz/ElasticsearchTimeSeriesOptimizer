using Elasticsearch.Net;
using StoneCo.ElasticsearchTimeSeriesOptimizer.ServiceLayer.Contracts;

namespace StoneCo.ElasticsearchTimeSeriesOptimizer.Processors
{
    public interface IElasticsearchProcessor
    {
        ElasticsearchResponse<VoidResponse> Shrink(string indexName);
    }
}