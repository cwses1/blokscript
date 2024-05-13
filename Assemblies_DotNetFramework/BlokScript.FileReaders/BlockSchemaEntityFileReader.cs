using System;
using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;

namespace BlokScript.FileReaders
{
	public class BlockSchemaEntityFileReader
	{
		public static BlockSchemaEntity Read (string FilePath)
		{
			return BlockSchemaEntityParser.Parse(JsonFileReader.Read(FilePath));
		}
	}
}
