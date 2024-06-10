using BlokScript.Entities;

namespace BlokScript.Models
{
	public class BlocksOutputLocation
	{
		public bool ToFile;
		public string FilePath;

		public bool ToSpace;
		public SpaceEntity Space;
	}
}
