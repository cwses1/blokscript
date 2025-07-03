using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

using BlokScript.Parsers;

namespace BlokScript.FileReaders
{
	public class StringArrayFileReader
	{
		public static string[] Read (string FilePath)
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
	}
}
