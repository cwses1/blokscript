using System;
using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;

namespace BlokScript.Parsers
{
	public class BlockSchemaEntityParser
	{
		public static BlockSchemaEntity Parse (dynamic BlockJson)
		{
			BlockSchemaEntity Block = new BlockSchemaEntity();
			Block.BlockId = BlockJson.id.ToString();
			Block.ComponentName = BlockJson.name;
			Block.SpaceId = null;
			Block.DataLocation = BlokScriptEntityDataLocation.FilePath;
			Block.Data = BlockJson;
			return Block;
		}
	}
}
