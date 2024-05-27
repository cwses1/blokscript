using System.Collections.Generic;

using BlokScript.Parsers;
using BlokScript.Entities;
using BlokScript.Common;
using BlokScript.Formatters;

namespace BlokScript.ResponseReaders
{
	public class SpaceResponseReader
	{
		public static SpaceEntity ReadResponseString (string ResponseString)
		{
			return SpaceParser.Parse(ResponseString);
		}
	}
}
