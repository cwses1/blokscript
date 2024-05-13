namespace BlokScript.PathFactories
{
	public class ManagementPathFactory
	{
		public static string CreateSpacePath (string SpaceId)
		{
			return $"/v1/spaces/{SpaceId}";
		}

		public static string CreateComponentsPath (string SpaceId)
		{
			return $"/v1/spaces/{SpaceId}/components";
		}

		public static string CreateSpacesPath ()
		{
			return "/v1/spaces";
		}

		public static string CreateComponentPath (string BlockId, string SpaceId)
		{
			return $"/v1/spaces/{SpaceId}/components/{BlockId}";
		}

		public static string CreateStoriesPath (string SpaceId)
		{
			return $"/v1/spaces/{SpaceId}/stories";
		}

		public static string CreateStoryPath (string StoryId, string SpaceId)
		{
			return $"/v1/spaces/{SpaceId}/stories/{StoryId}";
		}

		public static string CreatePublishStoryPath (string StoryId, string SpaceId)
		{
			return $"/v1/spaces/{SpaceId}/stories/{StoryId}/publish";
		}

		public static string CreateUnpublishStoryPath (string StoryId, string SpaceId)
		{
			return $"/v1/spaces/{SpaceId}/stories/{StoryId}/unpublish";
		}

		public static string CreateDeleteStoryPath (string StoryId, string SpaceId)
		{
			return CreateStoryPath(StoryId, SpaceId);
		}
	}
}
