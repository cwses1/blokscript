using System;

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
			Symbol.Type = TypeReference.GetSymbolType(TokenType);
			Symbol.Value = ExtractNativeValueFromJToken(Token);
			return Symbol;
		}

		public static object ExtractNativeValueFromJToken (JToken Token)
		{
			if (Token.Type == JTokenType.Integer)
				return Token.Value<int>();
			else if (Token.Type == JTokenType.String)
				return Token.Value<string>();
			else if (Token.Type == JTokenType.Date)
				return Token.Value<DateTime>();
			else if (Token.Type == JTokenType.Null)
				return null;
			else if (Token.Type == JTokenType.Undefined)
				return null;
			else if (Token.Type == JTokenType.Boolean)
				return Token.Value<bool>();

			throw new TypeNotAllowedException($"{Token.Type}");
		}

		public static object ExtractNativeValue (object JsonDataObject, string FieldName)
		{
			JContainer JsonObject = (JContainer)JsonDataObject;
			JToken Token = JsonObject[FieldName];
			return ExtractNativeValueFromJToken(Token);
		}
	}
}
