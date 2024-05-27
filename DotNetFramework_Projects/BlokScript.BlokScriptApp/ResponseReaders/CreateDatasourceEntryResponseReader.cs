using System.Collections.Generic;

using BlokScript.Parsers;
using BlokScript.Entities;
using BlokScript.Common;
using BlokScript.Formatters;

namespace BlokScript.ResponseReaders
{
	public class CreateDatasourceEntryResponseReader
	{
		public static DatasourceEntryEntity ReadResponseString (string ResponseString)
		{
			dynamic datasource_entry = JsonParser.ParseAsDynamic(ResponseString).datasource_entry;

			DatasourceEntryEntity CurrentEntity = new DatasourceEntryEntity();
			CurrentEntity.DatasourceEntryId = datasource_entry.id.ToString();
			CurrentEntity.Name = datasource_entry.name;
			CurrentEntity.Value = datasource_entry.value;
			CurrentEntity.Data = datasource_entry;
			return CurrentEntity;
		}
	}
}
