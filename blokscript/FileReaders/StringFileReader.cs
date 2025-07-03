using System.IO;
using Newtonsoft.Json;
using BlokScript.Parsers;

namespace BlokScript.FileReaders
{
	public class StringFileReader
	{
		public static string Read (string FilePath)
		{
			using (StreamReader SourceReader = new StreamReader(FilePath))
			{
				return SourceReader.ReadToEnd();
			}
		}
	}
}
