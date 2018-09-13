using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace Agema.Common
{
	// ReSharper disable once InconsistentNaming
	/// <summary>
	/// Helper methods for the EPPlus Package for Excel files
	/// </summary>
	public static class EPPlusHelper
	{
		/// <summary>
		/// Finds the header cell by value.
		/// </summary>
		/// <param name="worksheet">The worksheet.</param>
		/// <param name="value">The value.</param>
		/// <param name="comparisonType">Type of the comparison.</param>
		/// <param name="cellAddress">The cell address, default is the first row: 1:1 </param>
		/// <returns></returns>
		public static IEnumerable<ExcelRangeBase> FindHeaderCellsByValue(this ExcelWorksheet worksheet, 
			string value,
			StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase,
			string cellAddress = "1:1") {
			var cells = new List<ExcelRangeBase>();

			foreach (var cell in worksheet.Cells[cellAddress]) {
				if (cell.Value != null && cell.Value.ToString().Equals(value,comparisonType)) {
					cells.Add(cell);
				}
			}

			return cells;
		}

		/// <summary>
		/// Finds the header cell by value.  If more than one cells are found, an error is returned.
		/// </summary>
		/// <param name="worksheet">The worksheet.</param>
		/// <param name="value">The value.</param>
		/// <param name="comparisonType">Type of the comparison.</param>
		/// <param name="cellAddress">The cell address.</param>
		/// <returns></returns>
		public static ExcelRangeBase FindHeaderCellByValue(this ExcelWorksheet worksheet,
			string value,
			StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase,
			string cellAddress = "1:1") {

			var cells = FindHeaderCellsByValue(worksheet, value, comparisonType, cellAddress).ToList();

			if (cells.Count.Equals(0))
				return null;

			if (cells.Count.Equals(1))
				return cells.First();

			throw new ApplicationException($"Multiple cells found with value: {value}, but expecting 1. Or use FindHeaderCellsByValue if support for multiple cells is needed.");
		}


	}
}
