using System;
using System.Collections.Generic;
using System.Text;

namespace Stoneco.Infrastructure
{
    /// <summary>
    /// Represents an error that can be safely serialized.
    /// Implements the basic properties from 'Exception' class.
    /// </summary>
    public sealed class OperationError
    {
        #region Constructors

        /// <summary>
        /// Base constructor.
        /// </summary>
        public OperationError() { }

        /// <summary>
        /// Constructor with exception object to load.
        /// </summary>
        /// <param name="exception">Exception object to load.</param>
        public OperationError(Exception exception)
        {
            this.LoadExceptionData(exception, null);
        }

        /// <summary>
        /// Constructor that receives an exception object to load and a list with the exceptions already loaded.
        /// </summary>
        /// <param name="exception">Exception object to load.</param>
        /// <param name="handledExceptionList">List of expcetions already handled.</param>
        private OperationError(Exception exception, HashSet<Exception> handledExceptionList)
        {
            this.LoadExceptionData(exception, handledExceptionList);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Class name of the exception.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Fully qualified name of the exception class, including the namespace.
        /// </summary>
        public string FullClassName { get; set; }

        /// <summary>
        /// 'ServiceError' object that caused the current error.
        /// </summary>
        public OperationError InnerError { get; set; }

        /// <summary>
        /// Message that describes the current error.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Name of the application or the object that caused the error.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// String representation of the immediate frames on the call stack.
        /// </summary>
        public string StackTrace { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a new object that is a deep copy of the current instance.
        /// </summary>
        /// <typeparam name="T">Type to cast the returning object.</typeparam>
        /// <returns>A new object that is a deep copy of this instance.</returns>
        public T CreateDeepCopy<T>()
        {
            Dictionary<OperationError, OperationError> handledErrorList = new Dictionary<OperationError,OperationError>();
            object obj = this.CreateDeepCopy(handledErrorList);
            return (T)obj;
        }

        /// <summary>
        /// Loads data from a given exception.
        /// </summary>
        /// <param name="exception">Exception to load.</param>
        public void LoadException(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            this.LoadExceptionData(exception, null);
        }

        /// <summary>
        /// Override 'ToString'.
        /// </summary>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            // Create list of handled operation errors.
            HashSet<OperationError> handledErrorList = new HashSet<OperationError>();

            // Add exception information.
            this.AppendErrorString(builder, handledErrorList);

            // Clear list of handled errors.
            handledErrorList.Clear();

            // Add stack trace information.
            this.AppendStackTraceString(builder, handledErrorList);

            return builder.ToString();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Adds error information to a given string builder.
        /// </summary>
        /// <param name="builder">String builder to append data.</param>
        /// <param name="handledErrorList">List of errors already handled.</param>
        /// <returns>Whether data was written.</returns>
        private bool AppendErrorString(StringBuilder builder, HashSet<OperationError> handledErrorList)
        {
            // Return if this error is already handled.
            if (handledErrorList.Contains(this))
                return false;

            // Add current error to list of handled errors.
            handledErrorList.Add(this);

            // Add class name.
            builder.Append(this.FullClassName);

            // Add message.
            if (string.IsNullOrEmpty(this.Message) == false)
            {
                builder.Append(": ");
                builder.Append(this.Message);
            }

            // Add inner error.
            if (this.InnerError != null)
            {
                if (handledErrorList.Contains(this.InnerError) == false)
                    builder.Append(" ---> ");
                this.InnerError.AppendErrorString(builder, handledErrorList);
            }

            return true;
        }

        /// <summary>
        /// Adds stack trace information to a given string builder.
        /// </summary>
        /// <param name="builder">String builder to append data.</param>
        /// <param name="handledErrorList">List of errors already handled.</param>
        /// <returns>Whether data was written.</returns>
        private bool AppendStackTraceString(StringBuilder builder, HashSet<OperationError> handledErrorList)
        {
            // Return if this error is already handled.
            if (handledErrorList.Contains(this))
                return false;

            // Add current error to list of handled errors.
            handledErrorList.Add(this);

            // Add inner stack trace.
            if (this.InnerError != null)
            {
                if (this.InnerError.AppendStackTraceString(builder, handledErrorList))
                {
                    builder.Append(System.Environment.NewLine);
                    builder.Append("   --- End of inner exception stack trace ---");
                }
            }

            // Add stack trace.
            if (string.IsNullOrEmpty(this.StackTrace) == false)
            {
                builder.Append(System.Environment.NewLine);
                builder.Append(this.StackTrace);
            }

            return true;
        }

        /// <summary>
        /// Creates a new object that is a deep copy of the current instance.
        /// </summary>
        /// <param name="handledErrorList">List of errors already handled.</param>
        /// <returns>A new object that is a deep copy of this instance.</returns>
        private OperationError CreateDeepCopy(IDictionary<OperationError, OperationError> handledErrorList)
        {
            // Return self copy if already in list.
            OperationError thisCopy;
            if (handledErrorList.TryGetValue(this, out thisCopy))
                return thisCopy;

            // Create a copy of this.
            OperationError copy = new OperationError();

            // Add copy to the list.
            handledErrorList.Add(this, copy);

            // Copy attributes.
            copy.ClassName = this.ClassName;
            copy.FullClassName = this.FullClassName;
            copy.Message = this.Message;
            copy.Source = this.Source;
            copy.StackTrace = this.StackTrace;

            // Copy inner error.
            if (this.InnerError != null)
                copy.InnerError = this.InnerError.CreateDeepCopy(handledErrorList);

            return copy;
        }

        /// <summary>
        /// Loads data from a given exception.
        /// </summary>
        /// <param name="exception">Exception to load.</param>
        /// <param name="handledExceptionList">List of expcetions already handled.</param>
        private void LoadExceptionData(Exception exception, HashSet<Exception> handledExceptionList)
        {
            if (exception == null)
                return;

            Type exceptionType = exception.GetType();

            this.ClassName = exceptionType.Name;
            this.FullClassName = exceptionType.FullName;
            this.Message = exception.Message;
            this.Source = exception.Source;
            this.StackTrace = exception.StackTrace;

            // Treat 'OperationErrorException' as a special case.
            OperationErrorException serviceException = exception as OperationErrorException;
            if (serviceException != null)
            {
                // Copy inner error if needed.
                if (serviceException.InnerError != null)
                    this.InnerError = serviceException.InnerError.CreateDeepCopy<OperationError>();

                return;
            }

            // Create inner error if needed.
            if (exception.InnerException == null)
            {
                this.InnerError = null;
            }
            else
            {
                // Get inner exception.
                Exception inner = exception.InnerException;

                // Create list of handled exceptions if needed.
                if (handledExceptionList == null)
                    handledExceptionList = new HashSet<Exception>();

                // Add current exception to list of handled exceptions.
                handledExceptionList.Add(exception);

                // Create inner error if inner exception is not handled yet.
                if (handledExceptionList.Contains(inner) == false)
                    this.InnerError = new OperationError(inner, handledExceptionList);
            }
        }

        #endregion
    }
}
