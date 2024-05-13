using System;

namespace BlokScript.BlokScriptApp
{
	public class SymbolConflictException : Exception
	{
		public SymbolConflictException (string SymbolName) : base($"{SymbolName} already added to symbol table.")
		{
		}
	}
}
