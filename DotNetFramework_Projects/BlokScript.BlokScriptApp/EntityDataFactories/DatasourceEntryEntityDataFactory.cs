using System;
using System.Collections;

using BlokScript.Entities;
using BlokScript.Serializers;

namespace BlokScript.EntityDataFactories
{
	public class DatasourceEntryEntityDataFactory
	{
		public static object CreateData (DatasourceEntryEntity Entry)
		{
			Hashtable DatasourceEntryHash = new Hashtable();
			if (Entry.DatasourceEntryId != null)
				DatasourceEntryHash["id"] = Int32.Parse(Entry.DatasourceEntryId);
			DatasourceEntryHash["name"] = Entry.Name;
			DatasourceEntryHash["value"] = Entry.Value;
			DatasourceEntryHash["datasource_id"] = Int32.Parse(Entry.DatasourceId);

			Hashtable DatasourceEntryWrapperHash = new Hashtable();
			DatasourceEntryWrapperHash["datasource_entry"] = DatasourceEntryHash;

			return JsonSerializer.SerializeToNative(DatasourceEntryWrapperHash);
		}
	}
}
