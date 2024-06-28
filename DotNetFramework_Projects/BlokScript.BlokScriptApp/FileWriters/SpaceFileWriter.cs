using System.Collections;
using System.IO;

using BlokScript.Entities;
using BlokScript.Serializers;
using BlokScript.EntityDataExtractors;

namespace BlokScript.FileWriters
{
	public class SpaceFileWriter
	{
		public static void Write (SpaceEntity Space, string FilePath)
		{
			Hashtable DataHash = new Hashtable();
			DataHash["space"] = Space.Data;

			using (StreamWriter Writer = new StreamWriter(FilePath))
			{
				Writer.Write(JsonSerializer.Serialize(DataHash));
			}
		}
	}
}
