using System;
using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;

namespace BlokScript.Parsers
{
	public class BlockParser
	{
		public static BlockSchemaEntity ParseDynamic (dynamic BlockJson)
		{
			return BlockSchemaEntityParser.ParseDynamic(BlockJson);
		}
	}
}
