using System;
using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BlokScript.Parsers
{
	public class StoryParser
	{
		public static StoryEntity Parse (dynamic EntityJson)
		{
			StoryEntity CurrentEntity = new StoryEntity();
			CurrentEntity.StoryId = EntityJson.id.ToString();
			CurrentEntity.Url ="/" + EntityJson.full_slug;
			CurrentEntity.Name = EntityJson.name;
			CurrentEntity.Data = EntityJson;

			List<string> Tags = new List<string>();

			foreach (object TagString in EntityJson.tag_list)
			{
				Tags.Add((string)((JValue)TagString).Value);
			}

			CurrentEntity.Tags = Tags.ToArray();
			return CurrentEntity;
		}
	}
}
