using System;
using System.Collections.Generic;

using BlokScript.Entities;
using BlokScript.Parsers;
using BlokScript.Common;

namespace BlokScript.Parsers
{
	public class DatasourcesParser
	{
		public static DatasourceEntity[] Parse (string JsonString)
		{
			return ParseDynamic(JsonParser.Parse(JsonString));
		}

		public static DatasourceEntity[] ParseDynamic (dynamic DatasourcesJson)
		{
			List<DatasourceEntity> DatasourceList = new List<DatasourceEntity>();

			foreach (dynamic DatasourceJson in DatasourcesJson)
			{
				DatasourceList.Add(DatasourceParser.ParseDynamic(DatasourceJson));
			}

			return DatasourceList.ToArray();
		}
	}
}
