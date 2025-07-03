using System;
using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;

namespace BlokScript.FileReaders
{
	public class StoriesFileReader
	{
		public static StoryEntity[] Read (string FilePath)
		{
			return StoriesParser.Parse(StringFileReader.Read(FilePath));
		}
	}
}
