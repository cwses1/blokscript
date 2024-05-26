using System;
using System.Collections.Generic;
using BlokScript.Entities;
using BlokScript.Common;

namespace BlokScript.Caches
{
	public class SpaceCache
	{
		public SpaceCache ()
		{
			BlockSchemaEntities = new Dictionary<string, BlockSchemaEntity>();
			StoryEntities = new Dictionary<string, StoryEntity>();
			StoryEntitiesById = new Dictionary<string, StoryEntity>();

			DatasourceEntitiesById = new Dictionary<string, DatasourceEntity>();
			DatasourceEntitiesBySlug = new Dictionary<string, DatasourceEntity>();
			DatasourceEntitiesByName = new Dictionary<string, DatasourceEntity>();
		}

		public bool ContainsBlock (string ComponentName)
		{
			return BlockSchemaEntities.ContainsKey(ComponentName);
		}

		public bool ContainsStory (string Url)
		{
			return StoryEntities.ContainsKey(Url);
		}

		public bool ContainsStory (int StoryId)
		{
			return StoryEntitiesById.ContainsKey(StoryId.ToString());
		}

		public BlockSchemaEntity GetBlock (string ComponentName)
		{
			return BlockSchemaEntities[ComponentName];
		}

		public StoryEntity GetStory (string Url)
		{
			return StoryEntities[Url];
		}

		public StoryEntity GetStory (int StoryId)
		{
			return StoryEntitiesById[StoryId.ToString()];
		}

		public void InsertBlock (BlockSchemaEntity Block)
		{
			BlockSchemaEntities[Block.ComponentName] = Block;
		}

		public void InsertStory (StoryEntity Story)
		{
			StoryEntities[Story.Url] = Story;
			StoryEntitiesById[Story.StoryId] = Story;
		}

		public StoryEntity[] GetStories ()
		{
			List<StoryEntity> StoryList = new List<StoryEntity>();
			StoryList.AddRange(StoryEntities.Values);
			return StoryList.ToArray();
		}

		public void RemoveStory (StoryEntity Story)
		{
			StoryEntities.Remove(Story.Url);
			StoryEntitiesById.Remove(Story.StoryId);
		}

		public void InsertDatasource (DatasourceEntity Datasource)
		{
			DatasourceEntitiesById[Datasource.DatasourceId] = Datasource;
			DatasourceEntitiesBySlug[Datasource.Slug] = Datasource;
			DatasourceEntitiesByName[Datasource.Name] = Datasource;
		}

		public void RemoveDatasource (DatasourceEntity Datasource)
		{
			DatasourceEntitiesById.Remove(Datasource.DatasourceId);
			DatasourceEntitiesBySlug.Remove(Datasource.Slug);
			DatasourceEntitiesByName.Remove(Datasource.Name);
		}

		public bool ContainsDatasourceById (string DatasourceId)
		{
			return DatasourceEntitiesById.ContainsKey(DatasourceId);
		}

		public bool ContainsDatasourceById (int DatasourceId)
		{
			return ContainsDatasourceById(DatasourceId.ToString());
		}

		public DatasourceEntity GetDatasourceById (string DatasourceId)
		{
			return DatasourceEntitiesById[DatasourceId];
		}

		public DatasourceEntity GetDatasourceById (int DatasourceId)
		{
			return GetDatasourceById(DatasourceId.ToString());
		}

		public bool ContainsDatasourceBySlug (string Slug)
		{
			return DatasourceEntitiesBySlug.ContainsKey(Slug);
		}

		public DatasourceEntity GetDatasourceBySlug (string Slug)
		{
			return DatasourceEntitiesBySlug[Slug];
		}

		public bool ContainsDatasourceByName (string Name)
		{
			return DatasourceEntitiesByName.ContainsKey(Name);
		}

		public DatasourceEntity GetDatasourceByName (string Name)
		{
			return DatasourceEntitiesByName[Name];
		}

		public DatasourceEntity[] GetDatasources ()
		{
			List<DatasourceEntity> DatasourceEntityList = new List<DatasourceEntity>();
			DatasourceEntityList.AddRange(DatasourceEntitiesById.Values);
			return DatasourceEntityList.ToArray();
		}

		public string SpaceId;
		public string SpaceName;
		public bool SpaceDataLoaded;
		public SpaceEntity Space;

		public bool ComponentsLoaded;
		public Dictionary<string, BlockSchemaEntity> BlockSchemaEntities;

		public bool StoriesLoaded;
		public Dictionary<string, StoryEntity> StoryEntities;
		public Dictionary<string, StoryEntity> StoryEntitiesById;

		public bool DatasourcesLoaded;
		public Dictionary<string, DatasourceEntity> DatasourceEntitiesById;
		public Dictionary<string, DatasourceEntity> DatasourceEntitiesBySlug;
		public Dictionary<string, DatasourceEntity> DatasourceEntitiesByName;
	}
}
