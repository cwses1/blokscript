using BlokScript.Entities;

namespace BlokScript.Models
{
	public class StoriesInputLocation
	{
		public bool FromFile;
		public string FilePath;

		public bool FromSpace;
		public SpaceEntity Space;
	}
}
