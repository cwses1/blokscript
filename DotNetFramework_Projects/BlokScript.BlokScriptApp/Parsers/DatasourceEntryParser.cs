using BlokScript.Entities;

namespace BlokScript.Parsers
{
	public class DatasourceEntryParser
	{
		public static DatasourceEntryEntity ParseDynamic (dynamic datasource_entry)
		{
			DatasourceEntryEntity CurrentEntity = new DatasourceEntryEntity();

			if (datasource_entry.id != null)
				CurrentEntity.DatasourceEntryId = datasource_entry.id.ToString();

			CurrentEntity.Name = datasource_entry.name;
			CurrentEntity.Value = datasource_entry.value;

			if (datasource_entry.datasource_id != null)
				CurrentEntity.DatasourceId = datasource_entry.datasource_id.ToString();

			CurrentEntity.Data = datasource_entry;
			return CurrentEntity;
		}
	}
}
