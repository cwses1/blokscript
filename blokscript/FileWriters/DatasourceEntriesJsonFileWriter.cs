using System.Collections;
using System.IO;

using BlokScript.Entities;
using BlokScript.Serializers;
using BlokScript.EntityDataExtractors;

namespace BlokScript.FileWriters
{
	public class DatasourceEntriesJsonFileWriter
	{
		public static void Write (DatasourceEntryEntity[] SourceEntries, string FilePath)
		{
			Hashtable DataHash = new Hashtable();
			DataHash["datasource_entries"] = DatasourceEntriesDataExtractor.ExtractDataObjects(SourceEntries);

			using (StreamWriter Writer = new StreamWriter(FilePath))
			{
				Writer.Write(JsonSerializer.Serialize(DataHash));
			}
		}
	}
}
