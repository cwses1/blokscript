using System;
using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;

namespace BlokScript.FileReaders
{
	public class StoryEntityFileReader
	{
		public static StoryEntity Read (string FilePath)
		{
			return StoryEntityParser.Parse(JsonFileReader.Read(FilePath));
		}
	}
}
