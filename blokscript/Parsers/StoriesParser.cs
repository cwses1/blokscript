using System;
using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using BlokScript.FileReaders;

namespace BlokScript.Parsers
{
	public class StoriesParser
	{
		public static StoryEntity[] Parse (string StoriesJson)
		{
			return ParseDynamic(JsonParser.Parse(StoriesJson));
		}


		public static StoryEntity[] ParseDynamic (dynamic StoriesJson)
		{
			List<StoryEntity> StoryList = new List<StoryEntity>();

			foreach (dynamic JsonStory in StoriesJson)
			{
				StoryList.Add(StoryParser.Parse(JsonStory.story));
			}

			return StoryList.ToArray();
		}
	}
}
