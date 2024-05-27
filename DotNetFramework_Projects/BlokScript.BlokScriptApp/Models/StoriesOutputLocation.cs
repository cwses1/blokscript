using BlokScript.Entities;

namespace BlokScript.Models
{
	public class StoriesOutputLocation
	{
		public bool ToConsole;
		public bool ToLocalCache;
		public bool ToFile;
		public string FilePath;
		public bool ToFiles;
		public bool ToSpace;
		public SpaceEntity Space;
	}
}
