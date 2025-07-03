using System.Text;
using BlokScript.Common;
using BlokScript.Entities;

namespace BlokScript.Formatters
{
	public class DatasourceFormatter
	{
		public static string FormatJson (DatasourceEntity Datasource)
		{
			return JsonFormatter.FormatIndented(Datasource.Data);
		}

		public static string FormatHumanFriendly (DatasourceEntity Datasource)
		{
			return $"'{Datasource.Name} ({Datasource.Slug})'";
		}
		
	}
}
