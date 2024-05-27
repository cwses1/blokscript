using Newtonsoft.Json;

using BlokScript.Models;

namespace BlokScript.Parsers
{
	public class BlokScriptGlobalEnvParser
	{
		public static BlokScriptGlobalEnv Parse (string JsonString)
		{
			return (BlokScriptGlobalEnv)JsonConvert.DeserializeObject(JsonString, typeof(BlokScriptGlobalEnv));
		}
	}
}
