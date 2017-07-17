using System;

namespace Stoneco.Infrastructure
{
    /// <summary>
    /// Represents a base service response.
    /// </summary>
    public class BaseServiceResponse<T>
    {
        /// <summary>
        /// Body of the response with data.
        /// </summary>
        public T Body { get; set; }

        /// <summary>
        /// Information about the operation's result.
        /// </summary>
        /// <returns></returns>
        public Operation Operation { get; set; }

        /// <summary>
        /// Base constructor.
        /// </summary>
        public BaseServiceResponse()
        {
            this.Operation = new Operation();            
        }

        /// <summary>
        /// Constructor with exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public BaseServiceResponse(Exception ex)
        {
            this.Operation = new Operation()
            {
                Failed = true,
                Error = new OperationError(ex)
            };
        }
    }
}