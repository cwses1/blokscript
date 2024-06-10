using System.Collections.Generic;
using BlokScript.Entities;

namespace BlokScript.Parsers
{
	public class SpacesParser
	{
		public static SpaceEntity[] Parse (string JsonString)
		{
			return ParseDynamic(JsonParser.ParseAsDynamic(JsonString));
		}

		public static SpaceEntity[] ParseDynamic (dynamic SpacesJsonArray)
		{
			List<SpaceEntity> SpaceEntityList = new List<SpaceEntity>();

			foreach (dynamic SpaceJsonObject in SpacesJsonArray)
			{
				SpaceEntityList.Add(SpaceParser.ParseDynamic(SpaceJsonObject));
			}

			return SpaceEntityList.ToArray();
		}
	}
}
