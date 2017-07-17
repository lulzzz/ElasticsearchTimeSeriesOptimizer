using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Stoneco.ElasticsearchTimeSeriesOptimizer.Domain;
using Stoneco.Infrastructure;

namespace Stoneco.ElasticsearchTimeSeriesOptimizer.Controllers
{

    /// <summary>
    /// Shrink controller.
    /// </summary>
    [Route("api/v1")]
    public class ShrinkController
    {

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

                // StartDate must be a valid date.
                if (DateTime.TryParseExact(request.StartDate, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime parsedStartDate) == false){
                    throw new ArgumentException(string.Format("The argument {0} must be a valid date.", nameof(request.StartDate)));
                }

                // EndDate must be a valid date.
                if (DateTime.TryParseExact(request.EndDate, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime parsedEndDate) == false)
				{
                    throw new ArgumentException(string.Format("The argument {0} must be a valid date.", nameof(request.EndDate)));
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
