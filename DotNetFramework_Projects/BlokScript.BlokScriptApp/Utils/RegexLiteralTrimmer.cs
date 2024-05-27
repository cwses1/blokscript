namespace BlokScript.Utils
{
	public class RegexLiteralTrimmer
	{
		public static string Trim (string RegexLiteral)
		{
			return RegexLiteral.Trim(new char[]{'/'});
		}
	}
}
