using System.Text;
using BlokScript.Common;
using BlokScript.Entities;

namespace BlokScript.Formatters
{
	public class DatasourceEntryFormatter
	{
		public static string FormatJson (DatasourceEntryEntity DatasourceEntry)
		{
			return JsonFormatter.FormatIndented(DatasourceEntry.Data);
		}
		
		public static string FormatHumanFriendly (DatasourceEntryEntity DatasourceEntry)
		{
			return $"'{DatasourceEntry.Name} ({DatasourceEntry.Value})'";
		}
	}
}
