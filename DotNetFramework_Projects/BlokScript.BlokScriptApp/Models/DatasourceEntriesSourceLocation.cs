using BlokScript.Entities;

namespace BlokScript.Models
{
	public class DatasourceEntriesSourceLocation
	{
		public bool FromLocalCache;

		public bool FromFile;
		public string FilePath;

		//public bool FromFiles;

		public bool FromUrl;
		public UrlSpec UrlSpec;

		public bool FromDatasource;
		public DatasourceEntity Datasource;
	}
}
