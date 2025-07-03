using System.IO;

using BlokScript.Models;
using BlokScript.StringWriters;

namespace BlokScript.FileWriters
{
	public class SelectTableFileWriter
	{
		public static void Write (SelectTable Table, string FilePath)
		{
			using (StreamWriter Writer = new StreamWriter(FilePath))
			{
				Writer.Write(SelectTableStringWriter.Write(Table));
			}
		}
	}
}
