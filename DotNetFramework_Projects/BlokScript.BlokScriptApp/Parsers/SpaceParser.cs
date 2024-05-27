using BlokScript.Entities;

namespace BlokScript.Parsers
{
	public class SpaceParser
	{
		public static SpaceEntity Parse (string ResponseString)
		{
			dynamic SpaceData = JsonParser.ParseAsDynamic(ResponseString).space;

			SpaceEntity Space = new SpaceEntity();
			Space.SpaceId = SpaceData.id.ToString();
			Space.Name = SpaceData.name;
			Space.Data = SpaceData;
			return Space;
		}
	}
}
