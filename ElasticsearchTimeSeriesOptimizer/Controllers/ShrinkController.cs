using System;
using System.Globalization;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StoneCo.ElasticsearchTimeSeriesOptimizer.ConfigurationContracts;
using StoneCo.ElasticsearchTimeSeriesOptimizer.Processors;
using StoneCo.ElasticsearchTimeSeriesOptimizer.ServiceLayer.Contracts;
using StoneCo.Infrastructure;

namespace StoneCo.ElasticsearchTimeSeriesOptimizer.Controllers
{

    /// <summary>
    /// Shrink controller.
    /// </summary>
    [Route("api/v1")]
    public class ShrinkController : IShrinkController
    {

		#region Private properties

		/// <summary>
		/// Gets or sets the processor.
		/// </summary>
		/// <value>The processor.</value>
		private IElasticsearchProcessor Processor { get; set; }

		#endregion

		#region Constructors

        /*
		/// <summary>
		/// Base constructor of 
		/// <see cref="T:StoneCo.ElasticsearchTimeSeriesOptimizer.Controllers.ShrinkController"/> class.
		/// </summary>
		public ShrinkController()
        {

        }
        */

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:StoneCo.ElasticsearchTimeSeriesOptimizer.Controllers.ShrinkController"/> class with settings.
        /// </summary>
        /// <param name="settings">Settings.</param>
        public ShrinkController(IOptions<ElasticsearchProcessorSettings> settings)
        {
            Processor = new ElasticsearchProcessor(settings.Value);
        }

        /*
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:StoneCo.ElasticsearchTimeSeriesOptimizer.Controllers.ShrinkController"/> class a processor.
        /// </summary>
        /// <param name="processor">Processor.</param>
        public ShrinkController(IElasticsearchProcessor processor)
        {
            Processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }
        */
        #endregion

        /// <summary>
        /// Performs a shrink action on a single index.
        /// </summary>
        /// <returns>The method response.</returns>
        /// <param name="request">The method request.</param>
        [Route("shrink")]
        [HttpPost]
        public BaseServiceResponse<ShrinkResponseBody> Shrink([FromBody]ShrinkRequestBody request)
        {
            BaseServiceResponse<ShrinkResponseBody> response = new BaseServiceResponse<ShrinkResponseBody>();
            try
            {

                #region Validations

                // Request's body can't be null.
                if(request == null){
                    throw new ArgumentNullException(nameof(request));
                }

                // Index prefix must be informed.
                if(string.IsNullOrWhiteSpace((request.IndexName))){
                    throw new ArgumentNullException(nameof(request.IndexName));
                }

                #endregion

                // Performs the shrink operation on index.
                ElasticsearchResponse<VoidResponse> operationResponse = Processor.Shrink(request.IndexName);

            }
            catch(Exception ex){
                response = new BaseServiceResponse<ShrinkResponseBody>(ex);
            }

            return response;
        }

		/// <summary>
		/// Performs a shrink action on all indexes in the date range.
		/// </summary>
		/// <returns>The method response.</returns>
		/// <param name="request">The method request.</param>
		[Route("shrink-by-date")]
		[HttpPost]
		public BaseServiceResponse<ShrinkByDateResponseBody> ShrinkByDate([FromBody]ShrinkByDateRequestBody request)
		{
			BaseServiceResponse<ShrinkByDateResponseBody> response = new BaseServiceResponse<ShrinkByDateResponseBody>();

			try
			{

				#region Validations

                // Request's body can't be null.
				if (request == null)
				{
					throw new ArgumentNullException(nameof(request));
				}

                // Index prefix must be informed.
                if (string.IsNullOrWhiteSpace((request.IndexPrefix)))
				{
					throw new ArgumentNullException(nameof(request.IndexPrefix));
				}

                // StartDate can't be null or empty or whitespaced.
                if (string.IsNullOrWhiteSpace((request.StartDate)))
				{
                    throw new ArgumentNullException(nameof(request.StartDate));
				}

				// EndDate can't be null or empty or whitespaced.
                if (string.IsNullOrWhiteSpace((request.EndDate)))
				{
                    throw new ArgumentNullException(nameof(request.EndDate));
				}

                // StartDate must be a valid date.
                if (DateTime.TryParseExact(request.StartDate, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime parsedStartDate) == false){
                    throw new ArgumentException(string.Format("The argument {0} must be a valid date.", nameof(request.StartDate)));
                }

                // EndDate must be a valid date.
                if (DateTime.TryParseExact(request.EndDate, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime parsedEndDate) == false)
				{
                    throw new ArgumentException(string.Format("The argument {0} must be a valid date.", nameof(request.EndDate)));
				}

                // StartDate must be at least one day in the past
                if(parsedStartDate >= DateTime.UtcNow.Date)
                {
                    throw new ArgumentOutOfRangeException(nameof(request.StartDate), request.StartDate, string.Format("The {0} must be at least one day in the past.", nameof(request.StartDate)));
                }

				// EndDate must be at least onde day in the past
				if (parsedEndDate >= DateTime.UtcNow.Date)
				{
                    throw new ArgumentOutOfRangeException(nameof(request.EndDate), request.EndDate, string.Format("The {0} must be at least one day in the past.", nameof(request.EndDate)));
				}

                // EndDate must be higher than StartDate.
                if(parsedEndDate < parsedStartDate){
                    throw new ArgumentOutOfRangeException(nameof(request.EndDate),request.EndDate,string.Format("The {0} must be higher than {1}.", nameof(request.EndDate), nameof(request.StartDate)));
                }

				#endregion

			}
			catch (Exception ex)
			{
				response = new BaseServiceResponse<ShrinkByDateResponseBody>(ex);
			}

			return response;
		}

    }
}
