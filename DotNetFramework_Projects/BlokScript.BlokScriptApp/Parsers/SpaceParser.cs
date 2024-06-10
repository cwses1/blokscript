using BlokScript.Entities;

namespace BlokScript.Parsers
{
	public class SpaceParser
	{
		public static SpaceEntity Parse (string ResponseString)
		{
			return ParseDynamic(JsonParser.ParseAsDynamic(ResponseString).space);
		}

		public static SpaceEntity ParseDynamic (dynamic SpaceData)
		{
			SpaceEntity Space = new SpaceEntity();
			Space.SpaceId = SpaceData.id.ToString();
			Space.Name = SpaceData.name;
			Space.Data = SpaceData;
			return Space;
		}
	}
}
