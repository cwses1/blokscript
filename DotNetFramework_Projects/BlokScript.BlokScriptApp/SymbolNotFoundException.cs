using System;
using BlokScript.Common;

namespace BlokScript.BlokScriptApp
{
	public class SymbolNotFoundException : Exception
	{
		public SymbolNotFoundException (string Message) : base(Message)
		{
		}
	}
}