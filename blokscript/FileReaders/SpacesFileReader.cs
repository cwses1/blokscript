using System;
using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;

namespace BlokScript.FileReaders
{
	public class SpacesFileReader
	{
		public static SpaceEntity[] Read (string FilePath)
		{
			return SpacesParser.Parse(StringFileReader.Read(FilePath));
		}
	}
}
