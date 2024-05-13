using System.Collections.Generic;

using BlokScript.Parsers;
using BlokScript.Entities;
using BlokScript.Common;
using BlokScript.Formatters;

namespace BlokScript.ResponseReaders
{
	public class StoriesResponseReader
	{
		public static StoryEntity[] ReadResponseString (string ResponseString)
		{
			List<StoryEntity> EntityList = new List<StoryEntity>();

			foreach (dynamic CurrentEntityJson in JsonParser.ParseAsDynamic(ResponseString).stories)
			{
				StoryEntity CurrentEntity = StoryEntityParser.Parse(CurrentEntityJson);
				EntityList.Add(CurrentEntity);
			}

			return EntityList.ToArray();
		}
	}
}
