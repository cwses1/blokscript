using BlokScript.Parsers;

namespace BlokScript.FileReaders
{
	public class JsonFileReader
	{
		public static object Read (string FilePath)
		{
			return JsonParser.Parse(StringFileReader.Read(FilePath));
		}
	}
}
