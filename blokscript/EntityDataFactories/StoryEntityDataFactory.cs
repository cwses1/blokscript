using System;
using System.Collections;

using BlokScript.Entities;
using BlokScript.Serializers;
using BlokScript.Utils;

namespace BlokScript.EntityDataFactories
{
	public class StoryEntityDataFactory
	{
		public static object CreateData (StoryEntity Story)
		{
			Hashtable StoryHash = new Hashtable();

			if (Story.StoryId != null)
				StoryHash["id"] = Int32.Parse(Story.StoryId);

			if (Story.ParentId != null)
				StoryHash["parent_id"] = Int32.Parse(Story.ParentId);

			StoryHash["name"] = Story.Name;
			StoryHash["slug"] = Story.Slug;
			StoryHash["is_folder"] = Story.IsFolder;

			if (Story.HasContent)
				StoryHash["content"] = Story.Content;

			Hashtable StoryWrapperHash = new Hashtable();
			StoryWrapperHash["story"] = StoryHash;
			return JsonSerializer.SerializeToNative(StoryWrapperHash);
		}
	}
}
