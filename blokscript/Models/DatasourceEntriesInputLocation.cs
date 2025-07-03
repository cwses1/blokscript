using BlokScript.Entities;

namespace BlokScript.Models
{
	public class DatasourceEntriesInputLocation
	{
		public bool FromFile;
		public string FilePath;

		public bool FromDatasource;
		public DatasourceEntity Datasource;
	}
}
