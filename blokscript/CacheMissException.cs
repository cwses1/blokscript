using System;
using BlokScript.Common;

namespace BlokScript.BlokScriptApp
{
	public class CacheMissException : Exception
	{
		public CacheMissException (string KeyName) : base($"{KeyName} not found in local cache.")
		{
		}
	}
}