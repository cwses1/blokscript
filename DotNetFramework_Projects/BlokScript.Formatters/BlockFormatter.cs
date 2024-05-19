using System.Text;
using BlokScript.Entities;

namespace BlokScript.Formatters
{
	public class BlockFormatter
	{
		public static string FormatJson (BlockSchemaEntity Block)
		{
			return JsonFormatter.FormatIndented(Block.Data);
		}
	}
}
