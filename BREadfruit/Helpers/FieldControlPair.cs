using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREadfruit.Helpers
{

	/// <summary>
	/// Small DTO that holds information about an assigment, for either input or output
	/// indicating what is the column name (or parameter for SPs, etc) and what is the
	/// control that has to show or provide the data.
	/// </summary>
	public class FieldControlPair
	{
		private readonly string _dataFieldName;

		/// <summary>
		/// Gets the name of the Data field (like a "column").
		/// </summary>
		public string DataFieldName
		{
			get { return _dataFieldName; }
		}

		private readonly string _controlName;
		/// <summary>
		/// Gets the name of the control that has to show or provide
		/// the actual data value.
		/// </summary>
		public string ControlName
		{
			get { return _controlName; }
		}


		// ---------------------------------------------------------------------------------

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dataFieldName"></param>
		/// <param name="controlName"></param>
		public FieldControlPair ( string dataFieldName, string controlName)
		{
			if ( String.IsNullOrWhiteSpace ( dataFieldName ) )
				throw new ArgumentException ( "Data field cannot be null or empty", "dataFieldName" );
			if ( String.IsNullOrWhiteSpace ( controlName ) )
				throw new ArgumentException ( "Name of control cannot be null or empty", "dataFieldName" );

			this._dataFieldName = dataFieldName;
			this._controlName = controlName;
		}


		// ---------------------------------------------------------------------------------

		/// <summary>
		/// Returns a string represenation of the assignment pair where
		/// the "column" is always on the left hand side, and the name of the 
		/// control that has to show or provide the actual data value
		/// is on the right hand side.
		/// </summary>
		/// <returns></returns>
		public override string ToString ()
		{
			return string.Format ( "{0}:{1}", this._dataFieldName, this._controlName );
		}


		// ---------------------------------------------------------------------------------


	}
}
