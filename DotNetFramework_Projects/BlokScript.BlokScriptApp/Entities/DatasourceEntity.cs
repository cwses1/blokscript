using System.Collections.Generic;
using BlokScript.Common;

namespace BlokScript.Entities
{
	public class DatasourceEntity
	{
		public DatasourceEntity ()
		{
			DatasourceEntryEntitiesById = new Dictionary<string, DatasourceEntryEntity>();
			DatasourceEntryEntitiesByName = new Dictionary<string, DatasourceEntryEntity>();
			DatasourceEntriesList = new List<DatasourceEntryEntity>();
		}

		public void InsertDatasourceEntry (DatasourceEntryEntity DatasourceEntry)
		{
			DatasourceEntryEntitiesById[DatasourceEntry.DatasourceId] = DatasourceEntry;
			DatasourceEntryEntitiesByName[DatasourceEntry.Name.ToLower()] = DatasourceEntry;
			DatasourceEntriesList.Add(DatasourceEntry);
		}

		public bool HasEntryById (int DatasourceEntryId)
		{
			return HasEntryById(DatasourceEntryId.ToString());
		}

		public bool HasEntryById (string DatasourceEntryId)
		{
			return DatasourceEntryEntitiesById.ContainsKey(DatasourceEntryId);
		}

		public DatasourceEntryEntity GetEntryById (int DatasourceEntryId)
		{
			return GetEntryById(DatasourceEntryId.ToString());
		}

		public DatasourceEntryEntity GetEntryById (string DatasourceEntryId)
		{
			return DatasourceEntryEntitiesById[DatasourceEntryId];
		}

		public bool HasEntryByName (string Name)
		{
			return DatasourceEntryEntitiesByName.ContainsKey(Name.ToLower());
		}

		public DatasourceEntryEntity GetEntryByName (string Name)
		{
			return DatasourceEntryEntitiesByName[Name.ToLower()];
		}

		public DatasourceEntryEntity[] GetEntries ()
		{
			return DatasourceEntriesList.ToArray();
		}

		public string DatasourceId;
		public string Name;
		public string Slug;
		public string SpaceId;
		public object Data;
		public bool DatasourceEntriesLoaded;

		public BlokScriptEntityDataLocation DataLocation;
		public string FilePath;
		public string ServerPath;

		private Dictionary<string, DatasourceEntryEntity> DatasourceEntryEntitiesById;
		private Dictionary<string, DatasourceEntryEntity> DatasourceEntryEntitiesByName;
		private List<DatasourceEntryEntity> DatasourceEntriesList;
	}
}
