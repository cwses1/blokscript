using BlokScript.Parsers;
using BlokScript.Entities;

namespace BlokScript.ResponseReaders
{
	public class CreateSpaceResponseReader
	{
		public static SpaceEntity ReadResponseString (string ResponseString)
		{
			return SpaceParser.Parse(ResponseString);
		}
	}
}
