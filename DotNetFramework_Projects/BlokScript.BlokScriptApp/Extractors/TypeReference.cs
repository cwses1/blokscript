using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using BlokScript.Common;

namespace BlokScript.Extractors
{
	public class TypeReference
	{
		static TypeReference ()
		{
			_JsonToSymbolTypeDict = new Dictionary<JTokenType, BlokScriptSymbolType>();
			_JsonToSymbolTypeDict[JTokenType.String] = BlokScriptSymbolType.String;
			_JsonToSymbolTypeDict[JTokenType.Integer] = BlokScriptSymbolType.Int32;
			_JsonToSymbolTypeDict[JTokenType.Boolean] = BlokScriptSymbolType.Boolean;
			_JsonToSymbolTypeDict[JTokenType.Date] = BlokScriptSymbolType.DateTime;
		}

		public static BlokScriptSymbolType GetSymbolType (JTokenType JTokenTypeParam)
		{
			return _JsonToSymbolTypeDict[JTokenTypeParam];
		}

		private static Dictionary<JTokenType, BlokScriptSymbolType> _JsonToSymbolTypeDict;
	}
}
