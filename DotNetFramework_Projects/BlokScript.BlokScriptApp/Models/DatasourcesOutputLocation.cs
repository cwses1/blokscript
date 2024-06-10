using BlokScript.Entities;

namespace BlokScript.Models
{
	public class DatasourcesOutputLocation
	{
		public bool ToFile;
		public string FilePath;

		public bool ToSpace;
		public SpaceEntity Space;
	}
}
