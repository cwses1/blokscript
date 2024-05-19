using System;
using BlokScript.Common;

namespace BlokScript.BlokScriptApp
{
	public class SpaceObjectNotFoundException : Exception
	{
		public SpaceObjectNotFoundException (string Message) : base(Message)
		{
		}
	}
}
