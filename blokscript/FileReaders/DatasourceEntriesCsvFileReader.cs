using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;

using CsvHelper;
using CsvHelper.Configuration;

using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;
using BlokScript.EntityDataFactories;

namespace BlokScript.FileReaders
{
	public class DatasourceEntriesCsvFileReader
	{
		public static DatasourceEntryEntity[] Read (string FilePath)
		{
			List<DatasourceEntryEntity> DatasourceEntryEntityList = new List<DatasourceEntryEntity>();

			var Config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true
			};

			using (var SourceFileReader = new StreamReader(FilePath))
			{
				using (var SourceCsvReader = new CsvReader(SourceFileReader, Config))
				{
					SourceCsvReader.Read();
					SourceCsvReader.ReadHeader();

					while (SourceCsvReader.Read())
					{
						DatasourceEntryEntity Entry = new DatasourceEntryEntity();
						Entry.Name = SourceCsvReader.GetField("name");
						Entry.Value = SourceCsvReader.GetField("value");
						Entry.FilePath = FilePath;
						Entry.DataLocation = BlokScriptEntityDataLocation.FilePath;
						Entry.Data = DatasourceEntryEntityDataFactory.CreateData(Entry);
						DatasourceEntryEntityList.Add(Entry);
					}
				}
			}

			return DatasourceEntryEntityList.ToArray();
		}
	}
}
