using System.Collections.Generic;

using BlokScript.Entities;

namespace BlokScript.EntityDataExtractors
{
	public class DatasourceEntriesDataExtractor
	{
		public static object[] ExtractDataObjects (DatasourceEntryEntity[] SourceEntries)
		{
			List<object> DataList = new List<object>();

			foreach (DatasourceEntryEntity Entry in SourceEntries)
			{
				DataList.Add(Entry.Data);
			}

			return DataList.ToArray();
		}
	}
}
