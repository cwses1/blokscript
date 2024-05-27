using System.Collections.Generic;

using BlokScript.Parsers;
using BlokScript.Entities;
using BlokScript.Common;
using BlokScript.Formatters;

namespace BlokScript.ResponseReaders
{
	public class SpacesResponseReader
	{
		public static SpaceEntity[] ReadResponseString (string ResponseString)
		{
			List<SpaceEntity> SpaceEntityList = new List<SpaceEntity>();

			foreach (dynamic SpaceData in JsonParser.ParseAsDynamic(ResponseString).spaces)
			{
				SpaceEntity Space = new SpaceEntity();
				Space.SpaceId = SpaceData.id.ToString();
				Space.Name = SpaceData.name;
				Space.Data = SpaceData;
				SpaceEntityList.Add(Space);
			}

			return SpaceEntityList.ToArray();
		}
	}
}
