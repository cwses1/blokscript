using System.Text;
using BlokScript.Entities;

namespace BlokScript.Formatters
{
	public class BlockFormatter
	{
		public static string FormatConsole (BlockSchemaEntity Block)
		{
			StringBuilder OutputBuilder = new StringBuilder();
			OutputBuilder.AppendLine("-----");
			OutputBuilder.AppendLine($"BLOCK");
			OutputBuilder.AppendLine($"SpaceId: {Block.SpaceId}");
			OutputBuilder.AppendLine($"Name: {Block.ComponentName}");
			OutputBuilder.AppendLine($"DataLocation: {Block.DataLocation}");
			OutputBuilder.AppendLine($"FilePath: {Block.FilePath}");
			OutputBuilder.AppendLine($"ServerPath: {Block.ServerPath}");
			OutputBuilder.AppendLine("-----");
			return OutputBuilder.ToString();
		}

		public static string FormatJson (BlockSchemaEntity Block)
		{
			return JsonFormatter.FormatIndented(Block.Data);
		}

		public static string FormatHumanFriendly (BlockSchemaEntity Block)
		{
			return $"'{Block.ComponentName} ({Block.BlockId})'";
		}

		
	}
}
