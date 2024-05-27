using System.Text;
using BlokScript.Common;
using BlokScript.Entities;

namespace BlokScript.Formatters
{
	public class StoryFormatter
	{
		public static string FormatJson (StoryEntity Story)
		{
			return JsonFormatter.FormatIndented(Story.Data);
		}

		public static string FormatEcho (StoryEntity Story)
		{
			StringBuilder OutputBuilder = new StringBuilder();
			OutputBuilder.AppendLine("-----");
			OutputBuilder.AppendLine($"StoryEntity");
			OutputBuilder.AppendLine("-----");
			OutputBuilder.AppendLine($"StoryId: {Story.StoryId}");
			OutputBuilder.AppendLine($"Url: {Story.Url}");
			OutputBuilder.AppendLine($"Name: {Story.Name}");
			OutputBuilder.AppendLine($"SpaceId: {Story.SpaceId}");
			OutputBuilder.AppendLine($"Data: {Story.Data}");
			OutputBuilder.AppendLine($"DataLocation: {Story.DataLocation}");
			OutputBuilder.AppendLine($"FilePath: {Story.FilePath}");
			OutputBuilder.AppendLine($"ServerPath: {Story.ServerPath}");
			return OutputBuilder.ToString();
		}

		public static string FormatHumanFriendly (StoryEntity Story)
		{
			return $"'{Story.Name} ({Story.Url})'";
		}
	}
}
