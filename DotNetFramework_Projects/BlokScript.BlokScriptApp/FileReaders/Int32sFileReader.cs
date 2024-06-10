using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

using BlokScript.Parsers;

namespace BlokScript.FileReaders
{
	public class Int32sFileReader
	{
		public static int[] Read (string FilePath)
		{
			string Ext = Path.GetExtension(FilePath);

			if (Ext == ".txt")
				return ReadText(FilePath);

			return ReadJson(FilePath);
		}

		public static int[] ReadText (string FilePath)
		{
			List<int> IntList = new List<int>();
			string CurrentLine;

			using (StreamReader SourceReader = new StreamReader(FilePath))
			{
				while((CurrentLine = SourceReader.ReadLine()) != null)
					IntList.Add(int.Parse(CurrentLine));
			}

			return IntList.ToArray();
		}


		public static int[] ReadJson (string FilePath)
		{
			return Int32sParser.Parse(StringFileReader.Read(FilePath));
		}
	}
}
