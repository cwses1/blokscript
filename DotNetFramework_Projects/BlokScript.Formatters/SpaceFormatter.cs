using System.Text;
using BlokScript.Entities;

namespace BlokScript.Formatters
{
	public class SpaceFormatter
	{
		public static string FormatConsole (SpaceEntity Space)
		{
			StringBuilder OutputBuilder = new StringBuilder();
			OutputBuilder.AppendLine("-----");
			OutputBuilder.AppendLine($"SPACE");
			OutputBuilder.AppendLine($"SpaceId: {Space.SpaceId}");
			OutputBuilder.AppendLine($"DataLocation: {Space.DataLocation}");
			OutputBuilder.AppendLine($"FilePath: {Space.FilePath}");
			OutputBuilder.AppendLine($"ServerPath: {Space.ServerPath}");
			OutputBuilder.AppendLine($"Data:");
			OutputBuilder.AppendLine(FormatJson(Space));
			OutputBuilder.AppendLine("-----");
			return OutputBuilder.ToString();
		}

		public static string FormatJson (SpaceEntity Space)
		{
			return JsonFormatter.FormatIndented(Space.Data);
		}
	}
}
