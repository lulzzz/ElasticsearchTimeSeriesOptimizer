using System;
using StoneCo.ElasticsearchTimeSeriesOptimizer.ServiceLayer.Contracts;
using StoneCo.Infrastructure;

namespace StoneCo.ElasticsearchTimeSeriesOptimizer.Controllers
{
    /// <summary>
    /// Shrink controller Interface.
    /// </summary>
    public interface IShrinkController
    {

        /// <summary>
        /// Shrink the specified request.
        /// </summary>
        /// <returns>The shrink.</returns>
        /// <param name="request">Request.</param>
        BaseServiceResponse<ShrinkResponseBody> Shrink(ShrinkRequestBody request);

        /// <summary>
        /// Shrinks the by date.
        /// </summary>
        /// <returns>The by date.</returns>
        /// <param name="request">Request.</param>
        BaseServiceResponse<ShrinkByDateResponseBody> ShrinkByDate(ShrinkByDateRequestBody request);

    }
}
