using System;
using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;

namespace BlokScript.FileReaders
{
	public class SpaceFileReader
	{
		public static SpaceEntity Read (string FilePath)
		{
			return SpaceParser.Parse(StringFileReader.Read(FilePath));
		}
	}
}
