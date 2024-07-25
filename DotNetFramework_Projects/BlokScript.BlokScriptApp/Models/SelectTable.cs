using System.Collections.Generic;

namespace BlokScript.Models
{
	public class SelectTable
	{
		public SelectTable ()
		{
			_Rows = new List<SelectField[]>();
		}

		public void AddRow (SelectField[] Fields)
		{
			_Rows.Add(Fields);
		}

		public SelectField[][] Rows
		{
			get
			{
				return _Rows.ToArray();
			}
		}

		private List<SelectField[]> _Rows;
		public SelectColumn[] Columns;
	}
}
