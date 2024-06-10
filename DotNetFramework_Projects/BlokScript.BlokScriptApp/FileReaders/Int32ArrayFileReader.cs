using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

using BlokScript.Parsers;

namespace BlokScript.FileReaders
{
	public class Int32ArrayFileReader
	{
		public static int[] Read (string FilePath)
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
	}
}
