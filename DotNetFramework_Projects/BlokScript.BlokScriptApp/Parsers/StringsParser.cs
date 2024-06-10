using System;
using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using BlokScript.FileReaders;

namespace BlokScript.Parsers
{
	public class StringsParser
	{
		public static string[] Parse (string StringsJson)
		{
			return ParseDynamic(JsonParser.Parse(StringsJson));
		}


		public static string[] ParseDynamic (dynamic StringsJsonArray)
		{
			List<string> StringList = new List<string>();

			foreach (dynamic JsonString in StringsJsonArray)
			{
				StringList.Add(JsonString);
			}

			return StringList.ToArray();
		}
	}
}
