using System;
using BlokScript.Common;

namespace BlokScript.BlokScriptApp
{
	public class TypeNotAllowedException : Exception
	{
		public TypeNotAllowedException (string M) : base(M)
		{
		}
	}
}