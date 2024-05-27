using System.IO;

using BlokScript.Entities;
using BlokScript.Serializers;

namespace BlokScript.FileWriters
{
	public class JsonFileWriter
	{
		public static void Write (object Data, string FilePath)
		{
			using (StreamWriter Writer = new StreamWriter(FilePath))
			{
				Writer.Write(JsonSerializer.Serialize(Data));
			}
		}
	}
}
