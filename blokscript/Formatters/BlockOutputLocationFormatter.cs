using System.Text;
using BlokScript.Models;
using BlokScript.Entities;

namespace BlokScript.Formatters
{
	public class BlockOutputLocationFormatter
	{
		public static string FormatEcho (BlockOutputLocation TargetLocation)
		{
			StringBuilder OutputBuilder = new StringBuilder();

			OutputBuilder.AppendLine("-----");
			OutputBuilder.AppendLine($"BlockOutputLocation");
			OutputBuilder.AppendLine("-----");
			OutputBuilder.AppendLine($"ToFile: {TargetLocation.ToFile}");
			OutputBuilder.AppendLine($"FilePath: {TargetLocation.FilePath}");
			OutputBuilder.AppendLine($"ToSpace: {TargetLocation.ToSpace}");
			OutputBuilder.AppendLine($"ToSpace: {TargetLocation.ToSpace}");
			OutputBuilder.Append($"SpaceId: {TargetLocation.Space.SpaceId}");
			return OutputBuilder.ToString();
		}

		public static string FormatLog (BlockOutputLocation Location)
		{
			return FormatEcho(Location);
		}


		public static string FormatDebug (BlockOutputLocation Location)
		{
			return FormatEcho(Location);
		}

		public static string FormatJson (BlockOutputLocation Location)
		{
			return JsonFormatter.FormatIndented(Location);
		}
	}
}
