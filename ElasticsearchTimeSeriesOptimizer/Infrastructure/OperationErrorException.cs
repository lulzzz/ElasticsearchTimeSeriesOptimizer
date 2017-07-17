using System;
using System.Collections.Generic;
using System.Text;

namespace Stoneco.Infrastructure
{
    /// <summary>
    /// Exception to be thrown for an operation error.
    /// </summary>
    public class OperationErrorException : Exception
    {
        #region Constructors

        /// <summary>
        /// Base constructor.
        /// </summary>
        public OperationErrorException() : this((OperationError)null) { }

        /// <summary>
        /// Constructor with exception message.
        /// </summary>
        /// <param name="message">Message for the exception.</param>
        public OperationErrorException(string message) : this(message, null) { }

        /// <summary>
        /// Constructor with inner operation error.
        /// </summary>
        /// <param name="operationError">Inner operation error.</param>
        public OperationErrorException(OperationError operationError) : this("An operation error occurred.", operationError) { }

        /// <summary>
        /// Constructor with exception message and inner operation error.
        /// </summary>
        /// <param name="message">Message for the exception.</param>
        /// <param name="operationError">Inner operation error.</param>
        public OperationErrorException(string message, OperationError operationError) : base(message)
        {
            if (operationError != null)
                this.InnerError = operationError.CreateDeepCopy<OperationError>();
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Operation error that caused this exception.
        /// </summary>
        public OperationError InnerError { get; private set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Override 'ToString'.
        /// </summary>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            // Add class name.
            builder.Append(this.GetType().FullName);

            // Add message.
            if (string.IsNullOrEmpty(this.Message) == false)
            {
                builder.Append(": ");
                builder.Append(this.Message);
            }

            // Add inner error.
            if (this.InnerError != null)
            {
                // Create list of handled service errors.
                HashSet<OperationError> handledErrorList = new HashSet<OperationError>();

                // Add inner error information.
                builder.Append(" ---> ");
                AppendErrorString(builder, this.InnerError, handledErrorList);

                // Clear list of handled errors.
                handledErrorList.Clear();

                // Add inner error stack trace.
                AppendStackTraceString(builder, this.InnerError, handledErrorList);
                builder.Append(System.Environment.NewLine);
                builder.Append("   --- End of inner exception stack trace ---");
            }

            // Add stack trace information.
            if (string.IsNullOrEmpty(this.StackTrace) == false)
            {
                builder.Append(System.Environment.NewLine);
                builder.Append(this.StackTrace);
            }

            return builder.ToString();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Adds error information to a given string builder.
        /// </summary>
        /// <param name="builder">String builder to append data.</param>
        /// <param name="error">'ServiceError' object to get string.</param>
        /// <param name="handledErrorList">List of errors already handled.</param>
        /// <returns>Whether data was written.</returns>
        static private bool AppendErrorString(StringBuilder builder, OperationError error, HashSet<OperationError> handledErrorList)
        {
            // Return if this error is already handled.
            if (handledErrorList.Contains(error))
                return false;

            // Add current error to list of handled errors.
            handledErrorList.Add(error);

            // Add class name.
            builder.Append(error.FullClassName);

            // Add message.
            if (string.IsNullOrEmpty(error.Message) == false)
            {
                builder.Append(": ");
                builder.Append(error.Message);
            }

            // Add inner error.
            if (error.InnerError != null)
            {
                if (handledErrorList.Contains(error.InnerError) == false)
                    builder.Append(" ---> ");
                AppendErrorString(builder, error.InnerError, handledErrorList);
            }

            return true;
        }

        /// <summary>
        /// Adds stack trace information to a given string builder.
        /// </summary>
        /// <param name="builder">String builder to append data.</param>
        /// <param name="error">'ServiceError' object to get string.</param>
        /// <param name="handledErrorList">List of errors already handled.</param>
        /// <returns>Whether data was written.</returns>
        static private bool AppendStackTraceString(StringBuilder builder, OperationError error, HashSet<OperationError> handledErrorList)
        {
            // Return if this error is already handled.
            if (handledErrorList.Contains(error))
                return false;

            // Add current error to list of handled errors.
            handledErrorList.Add(error);

            // Add inner stack trace.
            if (error.InnerError != null)
            {
                if (AppendStackTraceString(builder, error.InnerError, handledErrorList))
                {
                    builder.Append(System.Environment.NewLine);
                    builder.Append("   --- End of inner exception stack trace ---");
                }
            }

            // Add stack trace.
            if (string.IsNullOrEmpty(error.StackTrace) == false)
            {
                builder.Append(System.Environment.NewLine);
                builder.Append(error.StackTrace);
            }

            return true;
        }

        #endregion
    }
}
