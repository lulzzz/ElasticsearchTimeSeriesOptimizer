using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoneCo.ElasticsearchTimeSeriesOptimizer.Controllers;
using StoneCo.ElasticsearchTimeSeriesOptimizer.ServiceLayer.Contracts;
using StoneCo.Infrastructure;

namespace StoneCo.ElasticsearchTimeSeriesOptimizerTests
{
    /// <summary>
    /// Shrink controller test.
    /// </summary>
    [TestClass]
    public class ShrinkControllerTest
    {

        #region Private properties

        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        private IShrinkController Controller { get; set; }

        #endregion

        #region TestInitialize

        /// <summary>
        /// Test initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.Controller = new ShrinkController();
        }

        #endregion

        #region Shrink

        /// <summary>
        /// Operation must fail with ArgumentNullException present in Operation.Error.
        /// </summary>
        [TestMethod]
        public void Shrink_when_request_is_null()
        {
            BaseServiceResponse<ShrinkResponseBody> response = Controller.Shrink(null);

            Assert.IsTrue(response.Operation.Failed);
            Assert.IsNull(response.Body);
            Assert.AreEqual(nameof(ArgumentNullException),response.Operation.Error.ClassName);
        }

		/// <summary>
		/// Operation must fail with ArgumentNullException present in Operation.Error.
		/// </summary>
		[TestMethod]
        public void Shrink_when_index_name_is_null()
        {
            ShrinkRequestBody request = new ShrinkRequestBody();

            BaseServiceResponse<ShrinkResponseBody> response = Controller.Shrink(request);

			Assert.IsTrue(response.Operation.Failed);
			Assert.IsNull(response.Body);
			Assert.AreEqual(nameof(ArgumentNullException), response.Operation.Error.ClassName);
        }

		#endregion

		#region ShrinkByDate

		/// <summary>
		/// Operation must fail with ArgumentNullException present in Operation.Error.
		/// </summary>
		[TestMethod]
        public void ShrinkByDate_when_request_is_null()
        {
            BaseServiceResponse<ShrinkByDateResponseBody> response = Controller.ShrinkByDate(null);

			Assert.IsTrue(response.Operation.Failed);
			Assert.IsNull(response.Body);
			Assert.AreEqual(nameof(ArgumentNullException), response.Operation.Error.ClassName);
        }

        /// <summary>
        /// Operation must fail with ArgumentNullException present in Operation.Error.
        /// </summary>
        [TestMethod]
        public void ShrinkByDate_when_index_prefix_is_null()
		{
            ShrinkByDateRequestBody request = new ShrinkByDateRequestBody
            {
                IndexPrefix = null,
                StartDate = DateTime.UtcNow.AddDays(-2).ToString("yyyy-MM-dd"),
                EndDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd")
            };

			BaseServiceResponse<ShrinkByDateResponseBody> response = Controller.ShrinkByDate(request);

			Assert.IsTrue(response.Operation.Failed);
			Assert.IsNull(response.Body);
			Assert.AreEqual(nameof(ArgumentNullException), response.Operation.Error.ClassName);
		}

		/// <summary>
		/// Operation must fail with ArgumentNullException present in Operation.Error.
		/// </summary>
		[TestMethod]
        public void ShrinkByDate_when_start_date_is_null()
        {
            ShrinkByDateRequestBody request = new ShrinkByDateRequestBody
            {
                IndexPrefix = "index-prefix",
                StartDate = null,
                EndDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd")
            };

			BaseServiceResponse<ShrinkByDateResponseBody> response = Controller.ShrinkByDate(request);

			Assert.IsTrue(response.Operation.Failed);
			Assert.IsNull(response.Body);
			Assert.AreEqual(nameof(ArgumentNullException), response.Operation.Error.ClassName);
        }

		/// <summary>
		/// Operation must fail with ArgumentNullException present in Operation.Error.
		/// </summary>
		[TestMethod]
		public void ShrinkByDate_when_end_date_is_null()
		{
			ShrinkByDateRequestBody request = new ShrinkByDateRequestBody
			{
				IndexPrefix = "index-prefix",
				StartDate = DateTime.UtcNow.AddDays(-2).ToString("yyyy-MM-dd"),
				EndDate = null
			};

			BaseServiceResponse<ShrinkByDateResponseBody> response = Controller.ShrinkByDate(request);

			Assert.IsTrue(response.Operation.Failed);
			Assert.IsNull(response.Body);
			Assert.AreEqual(nameof(ArgumentNullException), response.Operation.Error.ClassName);
		}

		/// <summary>
		/// Operation must fail with ArgumentOutOfRangeException present in Operation.Error.
		/// </summary>
		[TestMethod]
		public void ShrinkByDate_when_start_date_is_current_date()
		{
			ShrinkByDateRequestBody request = new ShrinkByDateRequestBody
			{
				IndexPrefix = "index-prefix",
				StartDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
				EndDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd")
			};

			BaseServiceResponse<ShrinkByDateResponseBody> response = Controller.ShrinkByDate(request);

			Assert.IsTrue(response.Operation.Failed);
			Assert.IsNull(response.Body);
            Assert.AreEqual(nameof(ArgumentOutOfRangeException), response.Operation.Error.ClassName);
		}

		/// <summary>
		/// Operation must fail with ArgumentOutOfRangeException present in Operation.Error.
		/// </summary>
		[TestMethod]
		public void ShrinkByDate_when_start_date_is_in_the_future()
		{
			ShrinkByDateRequestBody request = new ShrinkByDateRequestBody
			{
				IndexPrefix = "index-prefix",
				StartDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd"),
				EndDate = DateTime.UtcNow.AddDays(-2).ToString("yyyy-MM-dd")
			};

			BaseServiceResponse<ShrinkByDateResponseBody> response = Controller.ShrinkByDate(request);

			Assert.IsTrue(response.Operation.Failed);
			Assert.IsNull(response.Body);
			Assert.AreEqual(nameof(ArgumentOutOfRangeException), response.Operation.Error.ClassName);
		}

		/// <summary>
		/// Operation must fail with ArgumentOutOfRangeException present in Operation.Error.
		/// </summary>
		[TestMethod]
		public void ShrinkByDate_when_end_date_is_the_current_date()
		{
			ShrinkByDateRequestBody request = new ShrinkByDateRequestBody
			{
				IndexPrefix = "index-prefix",
				StartDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd"),
				EndDate = DateTime.UtcNow.ToString("yyyy-MM-dd")
			};

			BaseServiceResponse<ShrinkByDateResponseBody> response = Controller.ShrinkByDate(request);

			Assert.IsTrue(response.Operation.Failed);
			Assert.IsNull(response.Body);
			Assert.AreEqual(nameof(ArgumentOutOfRangeException), response.Operation.Error.ClassName);
		}

		/// <summary>
		/// Operation must fail with ArgumentOutOfRangeException present in Operation.Error.
		/// </summary>
		[TestMethod]
		public void ShrinkByDate_when_end_date_is_in_the_future()
		{
			ShrinkByDateRequestBody request = new ShrinkByDateRequestBody
			{
				IndexPrefix = "index-prefix",
				StartDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd"),
				EndDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd")
			};

			BaseServiceResponse<ShrinkByDateResponseBody> response = Controller.ShrinkByDate(request);

			Assert.IsTrue(response.Operation.Failed);
			Assert.IsNull(response.Body);
			Assert.AreEqual(nameof(ArgumentOutOfRangeException), response.Operation.Error.ClassName);
		}

		/// <summary>
		/// Operation must fail with ArgumentOutOfRangeException present in Operation.Error.
		/// </summary>
		[TestMethod]
		public void ShrinkByDate_when_start_date_is_higher_than_end_date()
		{
			ShrinkByDateRequestBody request = new ShrinkByDateRequestBody
			{
				IndexPrefix = "index-prefix",
				StartDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd"),
				EndDate = DateTime.UtcNow.AddDays(-2).ToString("yyyy-MM-dd")
			};

			BaseServiceResponse<ShrinkByDateResponseBody> response = Controller.ShrinkByDate(request);

			Assert.IsTrue(response.Operation.Failed);
			Assert.IsNull(response.Body);
			Assert.AreEqual(nameof(ArgumentOutOfRangeException), response.Operation.Error.ClassName);
		}

        #endregion

    }
}
