using System.Collections.Generic;

using BlokScript.Parsers;
using BlokScript.Entities;
using BlokScript.Common;
using BlokScript.Formatters;

namespace BlokScript.ResponseReaders
{
	public class DatasourceEntryResponseReader
	{
		public static DatasourceEntryEntity ReadResponseString (string ResponseString)
		{
			return DatasourceEntryParser.ParseDynamic(JsonParser.ParseAsDynamic(ResponseString).datasource_entry);
		}
	}
}
