using System.Collections.Generic;

using BlokScript.Entities;

namespace BlokScript.EntityCloners
{
	public class StoryCloner
	{
		public static StoryEntity Clone (StoryEntity SourceStory)
		{
			StoryEntity ClonedStory = new StoryEntity();
			ClonedStory.StoryId = SourceStory.StoryId;
			ClonedStory.Url = SourceStory.Url;
			ClonedStory.Name = SourceStory.Name;
			ClonedStory.SpaceId = SourceStory.SpaceId;
			ClonedStory.Slug = SourceStory.Slug;
			ClonedStory.ParentId = SourceStory.ParentId;
			ClonedStory.IsFolder = SourceStory.IsFolder;
			ClonedStory.Content = SourceStory.Content;
			ClonedStory.Data = SourceStory.Data;
			ClonedStory.DataLocation = SourceStory.DataLocation;
			ClonedStory.FilePath = SourceStory.FilePath;
			ClonedStory.ServerPath = SourceStory.ServerPath;
			ClonedStory.HasContent = SourceStory.HasContent;
			ClonedStory.Tags = CloneTags(SourceStory.Tags);
			return ClonedStory;
		}

		public static string[] CloneTags (string[] SourceTags)
		{
			List<string> TagList = new List<string>();
			TagList.AddRange(SourceTags);
			return TagList.ToArray();
		}
	}
}
