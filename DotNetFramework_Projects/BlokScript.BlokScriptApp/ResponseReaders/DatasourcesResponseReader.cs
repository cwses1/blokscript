using System.Collections.Generic;

using BlokScript.Parsers;
using BlokScript.Entities;
using BlokScript.Common;
using BlokScript.Formatters;

namespace BlokScript.ResponseReaders
{
	public class DatasourcesResponseReader
	{
		public static DatasourceEntity[] ReadResponseString (string ResponseString, string SpaceId)
		{
			List<DatasourceEntity> EntityList = new List<DatasourceEntity>();

			foreach (dynamic CurrentComponentJson in JsonParser.ParseAsDynamic(ResponseString).datasources)
			{
				DatasourceEntity CurrentEntity = new DatasourceEntity();
				CurrentEntity.DatasourceId = CurrentComponentJson.id.ToString();
				CurrentEntity.Name = CurrentComponentJson.name;
				CurrentEntity.Slug = CurrentComponentJson.slug;
				CurrentEntity.SpaceId = SpaceId;
				CurrentEntity.Data = CurrentComponentJson;
				EntityList.Add(CurrentEntity);
			}

			return EntityList.ToArray();
		}
	}
}
