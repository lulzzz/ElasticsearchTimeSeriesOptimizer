namespace StoneCo.Infrastructure
{
    /// <summary>
    /// The BaseServiceResponse Operation class.
    /// </summary>
    public class Operation
    {   
        /// <summary>
        /// The error object. Can be null.
        /// </summary>
        public OperationError Error { get; set; }

        /// <summary>
        /// Flag to determine if an error occurred.
        /// </summary>
        public bool Failed { get; set; }
    }
}