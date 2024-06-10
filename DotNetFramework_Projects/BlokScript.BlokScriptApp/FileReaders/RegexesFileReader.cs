using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

using BlokScript.Parsers;

namespace BlokScript.FileReaders
{
	public class RegexesFileReader
	{
		public static Regex[] Read (string FilePath)
		{
			string Ext = Path.GetExtension(FilePath);

			if (Ext == ".txt")
				return ReadText(FilePath);

			return ReadJson(FilePath);
		}

		public static Regex[] ReadText (string FilePath)
		{
			List<Regex> RegexList = new List<Regex>();
			string CurrentLine;

			using (StreamReader SourceReader = new StreamReader(FilePath))
			{
				while((CurrentLine = SourceReader.ReadLine()) != null)
					RegexList.Add(new Regex(CurrentLine));
			}

			return RegexList.ToArray();
		}

		public static Regex[] ReadJson (string FilePath)
		{
			return RegexesParser.Parse(StringFileReader.Read(FilePath));
		}
	}
}
