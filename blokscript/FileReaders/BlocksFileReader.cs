using System;
using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;

namespace BlokScript.FileReaders
{
	public class BlocksFileReader
	{
		public static BlockSchemaEntity[] Read (string FilePath)
		{
			return BlocksParser.Parse(StringFileReader.Read(FilePath));
		}
	}
}
