using BlokScript.Common;

namespace BlokScript.Entities
{
	public class StoryEntity
	{
		public string StoryId;
		public string Url;
		public string Name;
		public string SpaceId;
		public string Slug;
		public string ParentId;
		public bool IsFolder;
		public object Content;
		public string[] Tags;

		//
		// THE JSON DOCUMENT THAT REPRESENTS THIS STORY.
		// THIS MUST MATCH WITH THE FIELDS ABOVE.
		//
		public object Data;

		//
		// ADDITIONAL FIELDS THAT DESCRIBE WHERE THIS STORY.
		//
		public BlokScriptEntityDataLocation DataLocation;
		public string FilePath;
		public string ServerPath;
		public bool HasContent;
	}
}
