using Antlr4.Runtime;
using BlokScript.Parser;

namespace BlokScript.BlokScriptApp
{
	public class BlokScriptGrammarConcreteLexer : BlokScriptGrammarLexer
	{
		public BlokScriptGrammarConcreteLexer (ICharStream CharStreamParam) : base(CharStreamParam)
		{ }

		public override void Recover (LexerNoViableAltException e)
		{
			throw e;
		}

		public override void Recover (RecognitionException re)
		{
			throw re;
		}
	}
}
