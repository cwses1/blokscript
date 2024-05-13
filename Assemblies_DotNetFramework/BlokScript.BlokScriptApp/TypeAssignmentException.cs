using System;
using BlokScript.Common;

namespace BlokScript.BlokScriptApp
{
	public class TypeAssignmentException : Exception
	{
		public TypeAssignmentException (BlokScriptSymbolType LeftType, BlokScriptSymbolType RightType) : base($"Assignment type mismatch.  Cannot assign type {RightType} (right side of = operator) to {LeftType} (left side of = operator).")
		{
		}
	}
}