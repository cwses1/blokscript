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
			BlockSchemaEntitiesByName = new Dictionary<string, BlockSchemaEntity>();
			BlockSchemaEntitiesById = new Dictionary<string, BlockSchemaEntity>();

			StoryEntities = new Dictionary<string, StoryEntity>();
			StoryEntitiesById = new Dictionary<string, StoryEntity>();

			DatasourceEntitiesById = new Dictionary<string, DatasourceEntity>();
			DatasourceEntitiesBySlug = new Dictionary<string, DatasourceEntity>();
			DatasourceEntitiesByName = new Dictionary<string, DatasourceEntity>();
		}

		public bool ContainsBlock (string ComponentName)
		{
			return BlockSchemaEntitiesByName.ContainsKey(ComponentName);
		}

		public bool ContainsBlockById (string BlockId)
		{
			return BlockSchemaEntitiesById.ContainsKey(BlockId);
		}

		public bool ContainsStory (string Url)
		{
			return ContainsStoryByUrl(Url);
		}

		public bool ContainsStoryByUrl (string Url)
		{
			return StoryEntities.ContainsKey(Url);
		}

		public bool ContainsStory (int StoryId)
		{
			return StoryEntitiesById.ContainsKey(StoryId.ToString());
		}

		public BlockSchemaEntity GetBlock (string ComponentName)
		{
			return BlockSchemaEntitiesByName[ComponentName];
		}

		public BlockSchemaEntity GetBlockById (string BlockId)
		{
			return BlockSchemaEntitiesById[BlockId];
		}

		public StoryEntity GetStory (string Url)
		{
			return GetStoryByUrl(Url);
		}

		public StoryEntity GetStoryByUrl (string Url)
		{
			return StoryEntities[Url];
		}

		public StoryEntity GetStory (int StoryId)
		{
			return StoryEntitiesById[StoryId.ToString()];
		}

		public void InsertBlock (BlockSchemaEntity Block)
		{
			BlockSchemaEntitiesByName[Block.ComponentName] = Block;
			BlockSchemaEntitiesById[Block.BlockId] = Block;
		}

		public void InsertStories (StoryEntity[] Stories)
		{
			foreach (StoryEntity Story in Stories)
			{
				InsertStory(Story);
			}
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

			if (Story.IsFolder)
			{
				//
				// REMOVING A FOLDER STORY DESTROYS EVERYTHING UNDERNEATH THAT FOLDER IN STORYBLOK.
				// REMOVE THE FOLDER STORY AND ALL OTHER MATCHING STORIES HERE AS WELL TO MATCH.
				//
				foreach (StoryEntity ChildStory in GetStories())
				{
					if (ChildStory.Url.StartsWith(Story.Url))
						RemoveStory(ChildStory);
				}
			}
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
		public Dictionary<string, BlockSchemaEntity> BlockSchemaEntitiesByName;
		public Dictionary<string, BlockSchemaEntity> BlockSchemaEntitiesById;

		public bool StoriesLoaded;
		public Dictionary<string, StoryEntity> StoryEntities;
		public Dictionary<string, StoryEntity> StoryEntitiesById;

		public bool DatasourcesLoaded;
		public Dictionary<string, DatasourceEntity> DatasourceEntitiesById;
		public Dictionary<string, DatasourceEntity> DatasourceEntitiesBySlug;
		public Dictionary<string, DatasourceEntity> DatasourceEntitiesByName;
	}
}
