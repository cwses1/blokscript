using System;

using BlokScript.BlokScriptApp;

namespace BlokScript.Extractors
{
	public class EntityValueExtractor
	{
		public static string ExtractObjectValue (dynamic Entity, string FieldName)
		{
			return JsonExtractor.ExtractNativeValue(Entity.Data, FieldName);
		}


		public static string ExtractString (dynamic Entity, string FieldName)
		{
			return JsonExtractor.ExtractNativeValue(Entity.Data, FieldName).ToString();
		}


		public static BlokScriptSymbol ExtractSymbolFromEntity (dynamic Entity, string FieldName)
		{
			//
			// GET AN ANONYMOUS BLOKSCRIPT SYMBOL (BOTH TYPE AND VALUE) FROM THE ENTITY'S JSON DOCUMENT (THE DATA FIELD).
			//
			return JsonExtractor.ExtractSymbol(Entity.Data, FieldName);
		}
	}
}
