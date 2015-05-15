// <copyright>LogoUI Co.</copyright>
// <author>LogoUI Team</author>
// Partial Copyright (c) LogoUI 2012 LTD
// Autor: David Kossoglyad, LogoUI Team
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.


using System;

namespace LogoFX.UI.Data.Async
{
	/// <summary>
	/// Represents the container for marshalling a Task execution result.
	/// </summary>
	public class AsyncResultContainer
	{
		#region Fields

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncResultContainer"/> class.
		/// </summary>
		public AsyncResultContainer() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncResultContainer"/> class.
		/// </summary>
		/// <param name="error">The error description.</param>
		/// <param name="exception">The exception.</param>
		public AsyncResultContainer(string error, Exception exception)
		{
			Error = new Error(error, exception);
		}

		#endregion

		#region Public Properties


		/// <summary>
		/// Gets or sets the error.
		/// </summary>
		/// <value>
		/// The error.
		/// </value>
		public Error Error { get; set; }

		#endregion

	}

	public class AsyncResultContainer<TResult> : AsyncResultContainer
	{

		public AsyncResultContainer() 
		{
		}

		public AsyncResultContainer(TResult result)
		{
			Result = result;
		}

		public AsyncResultContainer(TResult result, string error, Exception exception) : base(error, exception)
		{
			Result = result;
		}

		public TResult Result { get; set; }
	}
}