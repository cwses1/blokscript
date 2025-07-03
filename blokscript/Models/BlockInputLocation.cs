using BlokScript.Entities;

namespace BlokScript.Models
{
	public class BlockInputLocation
	{
		public bool FromFile;
		public string FilePath;

		public bool FromSpace;
		public SpaceEntity Space;
	}
}
