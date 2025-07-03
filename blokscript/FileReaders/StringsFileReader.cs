using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

using BlokScript.Parsers;

namespace BlokScript.FileReaders
{
	public class StringsFileReader
	{
		public static string[] Read (string FilePath)
		{
			string Ext = Path.GetExtension(FilePath);

			if (Ext == ".txt")
				return ReadText(FilePath);

			return ReadJson(FilePath);
		}

		public static string[] ReadText (string FilePath)
		{
			List<string> StringList = new List<string>();
			string CurrentLine;

			using (StreamReader SourceReader = new StreamReader(FilePath))
			{
				while((CurrentLine = SourceReader.ReadLine()) != null)
					StringList.Add(CurrentLine);
			}

			return StringList.ToArray();
		}

		public static string[] ReadJson (string FilePath)
		{
			return StringsParser.Parse(StringFileReader.Read(FilePath));
		}
	}
}
