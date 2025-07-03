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
			OutputBuilder.AppendLine($"SpaceId: '{Space.SpaceId}'");
			OutputBuilder.AppendLine($"Name: '{Space.Name}'");
			OutputBuilder.AppendLine($"DataLocation: '{Space.DataLocation}'");
			OutputBuilder.AppendLine($"FilePath: '{Space.FilePath}'");
			OutputBuilder.AppendLine($"ServerPath: '{Space.ServerPath}'");
			OutputBuilder.AppendLine("-----");
			return OutputBuilder.ToString();
		}

		public static string FormatJson (SpaceEntity Space)
		{
			return JsonFormatter.FormatIndented(Space.Data);
		}

		public static string FormatHumanFriendly (SpaceEntity Space)
		{
			return $"'{Space.Name} ({Space.SpaceId})'";
		}
	}
}
