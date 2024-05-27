using System.IO;
using BlokScript.Models;
using BlokScript.Parsers;

namespace BlokScript.FileReaders
{
	public class BlokScriptEnvFileReader
	{
		public static BlokScriptGlobalEnv Read (string FilePath)
		{
			using (StreamReader SourceReader = new StreamReader(FilePath))
			{
				return BlokScriptGlobalEnvParser.Parse(SourceReader.ReadToEnd());
			}
		}
	}
}
