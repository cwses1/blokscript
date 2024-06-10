using System;
using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;

namespace BlokScript.FileReaders
{
	public class DatasourcesFileReader
	{
		public static DatasourceEntity[] Read (string FilePath)
		{
			return DatasourcesParser.Parse(StringFileReader.Read(FilePath));
		}
	}
}
