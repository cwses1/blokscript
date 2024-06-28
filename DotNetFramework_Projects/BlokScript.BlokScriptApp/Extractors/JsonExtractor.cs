using BlokScript.BlokScriptApp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlokScript.Extractors
{
	public class JsonExtractor
	{
		public static BlokScriptSymbol ExtractSymbol (object JsonDataObject, string FieldName)
		{
			JContainer JsonObject = (JContainer)JsonDataObject;

			JToken Token = JsonObject[FieldName];
			JTokenType TokenType = Token.Type;

			BlokScriptSymbol Symbol = new BlokScriptSymbol();

			return Symbol;
		}
	}
}
