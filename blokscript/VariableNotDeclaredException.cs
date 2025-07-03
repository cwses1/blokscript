using System;
using BlokScript.Common;

namespace BlokScript.BlokScriptApp
{
	public class VariableNotDeclaredException : Exception
	{
		public VariableNotDeclaredException (string SymbolName) : base($"Use of undeclared variable {SymbolName}.")
		{
		}
	}
}