using System;
using System.Collections;

using BlokScript.Entities;
using BlokScript.Serializers;

namespace BlokScript.EntityDataFactories
{
	public class DatasourceEntityDataFactory
	{
		public static object CreateData (DatasourceEntity Datasource)
		{
			Hashtable DatasourceHash = new Hashtable();
			if (Datasource.DatasourceId != null)
				DatasourceHash["id"] = Int32.Parse(Datasource.DatasourceId);
			DatasourceHash["name"] = Datasource.Name;
			DatasourceHash["slug"] = Datasource.Slug;

			Hashtable DatasourceWrapperHash = new Hashtable();
			DatasourceWrapperHash["datasource"] = DatasourceHash;
			return JsonSerializer.SerializeToNative(DatasourceWrapperHash);
		}
	}
}
