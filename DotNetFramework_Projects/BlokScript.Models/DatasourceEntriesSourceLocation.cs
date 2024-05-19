using BlokScript.Entities;

namespace BlokScript.Models
{
	public class DatasourceEntriesSourceLocation
	{
		public bool FromLocalCache;

		public bool FromFile;
		public FileSpec FileSpec;

		//public bool FromFiles;

		public bool ToUrl;
		public UrlSpec UrlSpec;

		public bool FromDatasource;
		public DatasourceEntity Datasource;
	}
}
