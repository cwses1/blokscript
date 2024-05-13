namespace BlokScript.Utils
{
	public class TokenTrimmer
	{
		public static string TrimRegexLiteral (string RegexLiteral)
		{
			return RegexLiteral.Trim(new char[]{'/'});
		}
	}
}
