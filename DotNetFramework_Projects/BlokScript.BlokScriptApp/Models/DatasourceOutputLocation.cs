using BlokScript.Entities;

namespace BlokScript.Models
{
	public class DatasourceOutputLocation
	{
		public bool ToFile;
		public string FilePath;

		public bool ToSpace;
		public SpaceEntity Space;
	}
}
