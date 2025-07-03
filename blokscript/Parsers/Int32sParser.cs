using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using BlokScript.Common;
using BlokScript.FileReaders;
using BlokScript.Entities;
using BlokScript.Parsers;

namespace BlokScript.Parsers
{
	public class Int32sParser
	{
		public static int[] Parse (string JsonDoc)
		{
			return ParseDynamic(JsonParser.Parse(JsonDoc));
		}


		public static int[] ParseDynamic (dynamic Int32sJsonArray)
		{
			List<int> Int32List = new List<int>();

			foreach (dynamic JsonString in Int32sJsonArray)
			{
				Int32List.Add(Int32.Parse(JsonString));
			}

			return Int32List.ToArray();
		}
	}
}
