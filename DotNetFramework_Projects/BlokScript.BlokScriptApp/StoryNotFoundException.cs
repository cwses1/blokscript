using System;
using BlokScript.Common;

namespace BlokScript.BlokScriptApp
{
	public class StoryNotFoundException : Exception
	{
		public StoryNotFoundException (string Url, string SpaceId) : base($"Story {Url} not found in space {SpaceId}.")
		{
		}


		public StoryNotFoundException (int StoryId, string SpaceId) : base($"Story {StoryId} not found in space {SpaceId}.")
		{
		}
	}
}
