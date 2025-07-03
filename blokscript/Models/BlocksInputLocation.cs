using BlokScript.Entities;

namespace BlokScript.Models
{
	public class BlocksInputLocation
	{
		public bool FromFile;
		public string FilePath;

		public bool FromSpace;
		public SpaceEntity Space;
	}
}
