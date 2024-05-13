using BlokScript.Common;

namespace BlokScript.Entities
{
	public class StoryEntity
	{
		public string StoryId;
		public string Url;
		public string Name;
		public string SpaceId;
		public object Data;
		public BlokScriptEntityDataLocation DataLocation;
		public string FilePath;
		public string ServerPath;
		public bool HasContent;
		public string[] Tags;
	}
}
