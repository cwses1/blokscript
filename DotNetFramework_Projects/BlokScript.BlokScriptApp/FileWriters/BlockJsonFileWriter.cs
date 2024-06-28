using System.Collections;
using System.IO;

using BlokScript.Entities;
using BlokScript.Serializers;
using BlokScript.EntityDataExtractors;

namespace BlokScript.FileWriters
{
	public class BlockJsonFileWriter
	{
		public static void Write (BlockSchemaEntity Block, string FilePath)
		{
			Hashtable DataHash = new Hashtable();
			DataHash["component"] = Block.Data;

			using (StreamWriter Writer = new StreamWriter(FilePath))
			{
				Writer.Write(JsonSerializer.Serialize(DataHash));
			}
		}
	}
}
