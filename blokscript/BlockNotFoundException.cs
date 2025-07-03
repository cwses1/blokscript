using System;
using BlokScript.Common;

namespace BlokScript.BlokScriptApp
{
	public class BlockNotFoundException : Exception
	{
		public BlockNotFoundException (string BlockComponentName, string SpaceId) : base($"Block {BlockComponentName} not found in space {SpaceId}.")
		{
		}
	}
}
