using System.Collections.Generic;

using BlokScript.Parsers;
using BlokScript.Entities;
using BlokScript.Common;
using BlokScript.Formatters;

namespace BlokScript.ResponseReaders
{
	public class DatasourceEntriesResponseReader
	{
		public static DatasourceEntryEntity[] ReadResponseString (string ResponseString, string DatasourceId)
		{
			List<DatasourceEntryEntity> EntityList = new List<DatasourceEntryEntity>();

			foreach (dynamic CurrentComponentJson in JsonParser.ParseAsDynamic(ResponseString).datasource_entries)
			{
				DatasourceEntryEntity CurrentEntity = new DatasourceEntryEntity();
				CurrentEntity.DatasourceEntryId = CurrentComponentJson.id.ToString();
				CurrentEntity.Name = CurrentComponentJson.name;
				CurrentEntity.Value = CurrentComponentJson.value;
				CurrentEntity.DatasourceId = DatasourceId;
				CurrentEntity.Data = CurrentComponentJson;
				EntityList.Add(CurrentEntity);
			}

			return EntityList.ToArray();
		}
	}
}
