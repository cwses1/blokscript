using BlokScript.Common;

namespace BlokScript.Entities
{
	public class DatasourceEntryEntity
	{
		public string DatasourceEntryId;
		public string Name;
		public string Value;
		public string DatasourceId;
		public object Data;

		public BlokScriptEntityDataLocation DataLocation;
		public string FilePath;
		public string ServerPath;
	}
}
