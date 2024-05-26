using System.Collections.Generic;

using BlokScript.Parsers;
using BlokScript.Entities;
using BlokScript.Common;
using BlokScript.Formatters;

namespace BlokScript.ResponseReaders
{
	public class ComponentsResponseReader
	{
		public static BlockSchemaEntity[] ReadResponseString (string ResponseString)
		{
			List<BlockSchemaEntity> BlockSchemaEntityList = new List<BlockSchemaEntity>();

			foreach (dynamic CurrentComponentJson in JsonParser.ParseAsDynamic(ResponseString).components)
			{
				BlockSchemaEntity CurrentEntity = new BlockSchemaEntity();
				CurrentEntity.BlockId = CurrentComponentJson.id.ToString();
				CurrentEntity.ComponentName = CurrentComponentJson.name;
				CurrentEntity.Data = CurrentComponentJson;
				BlockSchemaEntityList.Add(CurrentEntity);
			}

			return BlockSchemaEntityList.ToArray();
		}
	}
}
