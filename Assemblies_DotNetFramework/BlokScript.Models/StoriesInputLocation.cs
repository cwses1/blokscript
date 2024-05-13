using BlokScript.Entities;

namespace BlokScript.Models
{
	public class StoriesInputLocation
	{
		public bool FromLocalCache;

		public bool FromFile;
		public string FilePath;

		public bool FromFiles;

		public bool FromSpace;
		public SpaceEntity Space;
	}
}
