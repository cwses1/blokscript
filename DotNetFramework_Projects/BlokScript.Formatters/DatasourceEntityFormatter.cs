using System.Text;
using BlokScript.Common;
using BlokScript.Entities;

namespace BlokScript.Formatters
{
	public class DatasourceEntityFormatter
	{
		public static string FormatJson (DatasourceEntity Datasource)
		{
			return JsonFormatter.FormatIndented(Datasource.Data);
		}
	}
}
