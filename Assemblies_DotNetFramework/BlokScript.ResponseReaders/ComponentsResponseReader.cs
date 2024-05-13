using System.Collections.Generic;

using BlokScript.Parsers;
using BlokScript.Entities;
using BlokScript.Common;
using BlokScript.Formatters;

namespace BlokScript.ResponseReaders
{
	public class ComponentsResponseReader
	{
		public static BlockSchemaEntity[] ReadResponseString (string ResponseString, string SpaceId)
		{
			List<BlockSchemaEntity> BlockSchemaEntityList = new List<BlockSchemaEntity>();

			foreach (dynamic CurrentComponentJson in JsonParser.ParseAsDynamic(ResponseString).components)
			{
				BlockSchemaEntity CurrentEntity = new BlockSchemaEntity();
				CurrentEntity.BlockId = CurrentComponentJson.id.ToString();
				CurrentEntity.ComponentName = CurrentComponentJson.name;
				CurrentEntity.SpaceId = SpaceId;
				CurrentEntity.Data = CurrentComponentJson;
				CurrentEntity.DataLocation = BlokScriptEntityDataLocation.Server;
				BlockSchemaEntityList.Add(CurrentEntity);
			}

			return BlockSchemaEntityList.ToArray();
		}
	}
}
