using System;
using System.Collections.Generic;

using BlokScript.Common;

namespace BlokScript.BlokScriptApp
{
	public class BlokScriptSymbolTable : Dictionary<string, BlokScriptSymbol>
	{
		public BlokScriptSymbolTable ParentSymbolTable;
	}
}
