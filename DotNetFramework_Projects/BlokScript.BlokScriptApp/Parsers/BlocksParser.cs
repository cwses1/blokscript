using System;
using System.Collections.Generic;

using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;

namespace BlokScript.Parsers
{
	public class BlocksParser
	{
		public static BlockSchemaEntity[] Parse (string JsonString)
		{
			return ParseDynamic(JsonParser.Parse(JsonString));
		}

		public static BlockSchemaEntity[] ParseDynamic (dynamic BlocksJson)
		{
			List<BlockSchemaEntity> BlockList = new List<BlockSchemaEntity>();

			foreach (dynamic BlockJson in BlocksJson)
			{
				BlockList.Add(BlockParser.ParseDynamic(BlockJson));
			}

			return BlockList.ToArray();
		}
	}
}
