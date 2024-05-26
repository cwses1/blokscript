using BlokScript.Parsers;
using BlokScript.Entities;
using BlokScript.Common;
using BlokScript.Formatters;

namespace BlokScript.ResponseReaders
{
	public class CreateComponentResponseReader
	{
		public static BlockSchemaEntity ReadResponseString (string ResponseString)
		{
			dynamic ComponentJson = JsonParser.ParseAsDynamic(ResponseString).component;

			BlockSchemaEntity Block = new BlockSchemaEntity();
			Block.BlockId = ComponentJson.id.ToString();
			Block.ComponentName = ComponentJson.name;
			Block.Data = ComponentJson;
			return Block;
		}
	}
}
