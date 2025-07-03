using System;
using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;

namespace BlokScript.FileReaders
{
	public class DatasourceEntriesFileReader
	{
		public static DatasourceEntryEntity[] Read (string FilePath)
		{
			return DatasourceEntriesParser.Parse(StringFileReader.Read(FilePath));
		}
	}
}
