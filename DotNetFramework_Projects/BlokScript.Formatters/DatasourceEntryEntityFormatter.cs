using System.Text;
using BlokScript.Common;
using BlokScript.Entities;

namespace BlokScript.Formatters
{
	public class DatasourceEntryEntityFormatter
	{
		public static string FormatJson (DatasourceEntryEntity DatasourceEntry)
		{
			return JsonFormatter.FormatIndented(DatasourceEntry.Data);
		}
	}
}
