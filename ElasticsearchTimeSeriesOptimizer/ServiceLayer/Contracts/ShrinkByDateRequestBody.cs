﻿namespace StoneCo.ElasticsearchTimeSeriesOptimizer.ServiceLayer.Contracts
{
    /// <summary>
    /// Shrink by date request body.
    /// </summary>
    public class ShrinkByDateRequestBody
    {
	/// <summary>
	/// Gets or sets the name of the index.
	/// </summary>
	/// <value>The name of the index.</value>
	public string IndexPrefix { get; set; }

	/// <summary>
	/// Gets or sets the start date.
	/// </summary>
	/// <value>The start date.</value>
	public string StartDate { get; set; }

	/// <summary>
	/// Gets or sets the end date.
	/// </summary>
	/// <value>The end date.</value>
	public string EndDate { get; set; }        
    }
}