using BlokScript.Entities;

namespace BlokScript.Parsers
{
	public class DatasourceParser
	{
		public static DatasourceEntity Parse (string ResponseString)
		{
			return ParseDynamic(JsonParser.ParseAsDynamic(ResponseString).datasource);
		}

		public static DatasourceEntity ParseDynamic (dynamic Data)
		{
			DatasourceEntity Datasource = new DatasourceEntity();
			Datasource.DatasourceId = Data.id.ToString();
			Datasource.Name = Data.name;
			Datasource.Slug = Data.slug;
			Datasource.Data = Data;
			return Datasource;
		}
	}
}
