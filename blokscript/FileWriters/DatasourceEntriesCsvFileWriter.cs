using System.Collections;
using System.IO;
using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

using BlokScript.Entities;
using BlokScript.Serializers;
using BlokScript.EntityDataExtractors;

namespace BlokScript.FileWriters
{
	public class DatasourceEntriesCsvFileWriter
	{
		public static void Write (DatasourceEntryEntity[] SourceEntries, string FilePath)
		{
			var Config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				HasHeaderRecord = true
			};

			using (var SourceFileWriter = new StreamWriter(FilePath))
			{
				using (var SourceCsvWriter = new CsvWriter(SourceFileWriter, Config))
				{
					SourceCsvWriter.WriteField("name");
					SourceCsvWriter.WriteField("value");
					SourceCsvWriter.NextRecord();

					foreach (DatasourceEntryEntity SourceEntry in SourceEntries)
					{
						SourceCsvWriter.WriteField(SourceEntry.Name);
						SourceCsvWriter.WriteField(SourceEntry.Value);
						SourceCsvWriter.NextRecord();
					}
				}
			}
		}
	}
}
