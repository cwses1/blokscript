using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

using BlokScript.Parsers;

namespace BlokScript.FileReaders
{
	public class RegexArrayFileReader
	{
		public static Regex[] Read (string FilePath)
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
	}
}
