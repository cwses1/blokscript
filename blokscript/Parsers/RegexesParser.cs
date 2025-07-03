using System;
using BlokScript.Entities;
using BlokScript.Parsers;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

using BlokScript.Common;
using BlokScript.FileReaders;

namespace BlokScript.Parsers
{
	public class RegexesParser
	{
		public static Regex[] Parse (string StringsJson)
		{
			return ParseDynamic(JsonParser.Parse(StringsJson));
		}


		public static Regex[] ParseDynamic (dynamic StringsJsonArray)
		{
			List<Regex> RegexList = new List<Regex>();

			foreach (dynamic JsonString in StringsJsonArray)
			{
				RegexList.Add(new Regex(JsonString));
			}

			return RegexList.ToArray();
		}
	}
}
