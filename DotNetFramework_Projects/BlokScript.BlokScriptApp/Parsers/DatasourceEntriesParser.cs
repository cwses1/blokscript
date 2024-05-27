using System.Collections.Generic;
using BlokScript.Parsers;
using BlokScript.Entities;

namespace BlokScript.Parsers
{
	public class DatasourceEntriesParser
	{
		public static DatasourceEntryEntity[] Parse (string ResponseString)
		{
			return ParseDynamic(JsonParser.ParseAsDynamic(ResponseString).datasource_entries);
		}

		public static DatasourceEntryEntity[] ParseDynamic (dynamic datasource_entries)
		{
			List<DatasourceEntryEntity> DatasourceEntryEntityList = new List<DatasourceEntryEntity>();

			foreach (dynamic datasource_entry in datasource_entries)
			{
				DatasourceEntryEntityList.Add(DatasourceEntryParser.ParseDynamic(datasource_entry));
			}

			return DatasourceEntryEntityList.ToArray();
		}
	}
}
