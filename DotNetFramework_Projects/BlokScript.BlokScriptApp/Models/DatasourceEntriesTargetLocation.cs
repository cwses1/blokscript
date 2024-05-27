using BlokScript.Entities;

namespace BlokScript.Models
{
	public class DatasourceEntriesTargetLocation
	{
		public bool ToConsole;
		public bool ToLocalCache;
		public bool ToFile;
		public FileSpec FileSpec;
		//public bool ToFiles;

		public bool ToUrl;
		public UrlSpec UrlSpec;

		public bool ToDatasource;
		public DatasourceEntity Datasource;
	}
}
