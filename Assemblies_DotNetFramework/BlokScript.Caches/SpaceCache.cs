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

		public string SpaceId;
		public bool SpaceDataLoaded;
		public SpaceEntity Space;

		public bool ComponentsLoaded;
		public Dictionary<string, BlockSchemaEntity> BlockSchemaEntities;

		public bool StoriesLoaded;
		public Dictionary<string, StoryEntity> StoryEntities;
		public Dictionary<string, StoryEntity> StoryEntitiesById;
	}
}
