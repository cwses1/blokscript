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
			return StoryParser.Parse(JsonFileReader.Read(FilePath));
		}
	}
}
