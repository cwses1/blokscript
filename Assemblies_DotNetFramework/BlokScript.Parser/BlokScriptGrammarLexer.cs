//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from BlokScriptGrammar.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace BlokScript.Parser {
using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public partial class BlokScriptGrammarLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, T__23=24, 
		T__24=25, T__25=26, T__26=27, T__27=28, T__28=29, T__29=30, T__30=31, 
		T__31=32, T__32=33, T__33=34, T__34=35, T__35=36, T__36=37, T__37=38, 
		T__38=39, T__39=40, T__40=41, T__41=42, T__42=43, T__43=44, T__44=45, 
		T__45=46, T__46=47, T__47=48, T__48=49, T__49=50, T__50=51, T__51=52, 
		T__52=53, T__53=54, T__54=55, T__55=56, T__56=57, T__57=58, T__58=59, 
		T__59=60, T__60=61, T__61=62, T__62=63, T__63=64, T__64=65, T__65=66, 
		T__66=67, T__67=68, T__68=69, T__69=70, T__70=71, T__71=72, T__72=73, 
		STATEMENTEND=74, WS=75, STRINGLITERAL=76, VARID=77, INTLITERAL=78, REGEXLITERAL=79;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"T__9", "T__10", "T__11", "T__12", "T__13", "T__14", "T__15", "T__16", 
		"T__17", "T__18", "T__19", "T__20", "T__21", "T__22", "T__23", "T__24", 
		"T__25", "T__26", "T__27", "T__28", "T__29", "T__30", "T__31", "T__32", 
		"T__33", "T__34", "T__35", "T__36", "T__37", "T__38", "T__39", "T__40", 
		"T__41", "T__42", "T__43", "T__44", "T__45", "T__46", "T__47", "T__48", 
		"T__49", "T__50", "T__51", "T__52", "T__53", "T__54", "T__55", "T__56", 
		"T__57", "T__58", "T__59", "T__60", "T__61", "T__62", "T__63", "T__64", 
		"T__65", "T__66", "T__67", "T__68", "T__69", "T__70", "T__71", "T__72", 
		"STATEMENTEND", "WS", "STRINGLITERAL", "VARID", "INTLITERAL", "REGEXLITERAL"
	};


	public BlokScriptGrammarLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public BlokScriptGrammarLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'login'", "'with'", "'global'", "'username'", "'password'", "'token'", 
		"'and'", "'space'", "'='", "'block'", "'string'", "'regex'", "'story'", 
		"'in'", "'from'", "'copy'", "'to'", "'all'", "'spaces'", "'print'", "'symbol'", 
		"'tables'", "'local'", "'cache'", "'server'", "'console'", "'file'", "'files'", 
		"'on'", "'demand'", "'blocks'", "'where'", "'or'", "'id'", "'!='", "'not'", 
		"'('", "')'", "'name'", "'matches'", "'does'", "'match'", "'like'", "'starts'", 
		"'start'", "'ends'", "'end'", "','", "'+'", "'-'", "'*'", "'%'", "'be'", 
		"'verbose'", "'quiet'", "'wait'", "'compare'", "'publish'", "'unpublish'", 
		"'delete'", "'stories'", "'url'", "'any'", "'tag'", "'tags'", "'do'", 
		"'no'", "'datasource'", "'datasources'", "'slug'", "'foreach'", "'{'", 
		"'}'", "';'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, "STATEMENTEND", "WS", "STRINGLITERAL", "VARID", "INTLITERAL", 
		"REGEXLITERAL"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "BlokScriptGrammar.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static BlokScriptGrammarLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,79,606,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,2,36,7,36,2,37,7,37,2,38,7,38,2,39,7,39,2,40,7,40,2,41,7,41,2,42,
		7,42,2,43,7,43,2,44,7,44,2,45,7,45,2,46,7,46,2,47,7,47,2,48,7,48,2,49,
		7,49,2,50,7,50,2,51,7,51,2,52,7,52,2,53,7,53,2,54,7,54,2,55,7,55,2,56,
		7,56,2,57,7,57,2,58,7,58,2,59,7,59,2,60,7,60,2,61,7,61,2,62,7,62,2,63,
		7,63,2,64,7,64,2,65,7,65,2,66,7,66,2,67,7,67,2,68,7,68,2,69,7,69,2,70,
		7,70,2,71,7,71,2,72,7,72,2,73,7,73,2,74,7,74,2,75,7,75,2,76,7,76,2,77,
		7,77,2,78,7,78,1,0,1,0,1,0,1,0,1,0,1,0,1,1,1,1,1,1,1,1,1,1,1,2,1,2,1,2,
		1,2,1,2,1,2,1,2,1,3,1,3,1,3,1,3,1,3,1,3,1,3,1,3,1,3,1,4,1,4,1,4,1,4,1,
		4,1,4,1,4,1,4,1,4,1,5,1,5,1,5,1,5,1,5,1,5,1,6,1,6,1,6,1,6,1,7,1,7,1,7,
		1,7,1,7,1,7,1,8,1,8,1,9,1,9,1,9,1,9,1,9,1,9,1,10,1,10,1,10,1,10,1,10,1,
		10,1,10,1,11,1,11,1,11,1,11,1,11,1,11,1,12,1,12,1,12,1,12,1,12,1,12,1,
		13,1,13,1,13,1,14,1,14,1,14,1,14,1,14,1,15,1,15,1,15,1,15,1,15,1,16,1,
		16,1,16,1,17,1,17,1,17,1,17,1,18,1,18,1,18,1,18,1,18,1,18,1,18,1,19,1,
		19,1,19,1,19,1,19,1,19,1,20,1,20,1,20,1,20,1,20,1,20,1,20,1,21,1,21,1,
		21,1,21,1,21,1,21,1,21,1,22,1,22,1,22,1,22,1,22,1,22,1,23,1,23,1,23,1,
		23,1,23,1,23,1,24,1,24,1,24,1,24,1,24,1,24,1,24,1,25,1,25,1,25,1,25,1,
		25,1,25,1,25,1,25,1,26,1,26,1,26,1,26,1,26,1,27,1,27,1,27,1,27,1,27,1,
		27,1,28,1,28,1,28,1,29,1,29,1,29,1,29,1,29,1,29,1,29,1,30,1,30,1,30,1,
		30,1,30,1,30,1,30,1,31,1,31,1,31,1,31,1,31,1,31,1,32,1,32,1,32,1,33,1,
		33,1,33,1,34,1,34,1,34,1,35,1,35,1,35,1,35,1,36,1,36,1,37,1,37,1,38,1,
		38,1,38,1,38,1,38,1,39,1,39,1,39,1,39,1,39,1,39,1,39,1,39,1,40,1,40,1,
		40,1,40,1,40,1,41,1,41,1,41,1,41,1,41,1,41,1,42,1,42,1,42,1,42,1,42,1,
		43,1,43,1,43,1,43,1,43,1,43,1,43,1,44,1,44,1,44,1,44,1,44,1,44,1,45,1,
		45,1,45,1,45,1,45,1,46,1,46,1,46,1,46,1,47,1,47,1,48,1,48,1,49,1,49,1,
		50,1,50,1,51,1,51,1,52,1,52,1,52,1,53,1,53,1,53,1,53,1,53,1,53,1,53,1,
		53,1,54,1,54,1,54,1,54,1,54,1,54,1,55,1,55,1,55,1,55,1,55,1,56,1,56,1,
		56,1,56,1,56,1,56,1,56,1,56,1,57,1,57,1,57,1,57,1,57,1,57,1,57,1,57,1,
		58,1,58,1,58,1,58,1,58,1,58,1,58,1,58,1,58,1,58,1,59,1,59,1,59,1,59,1,
		59,1,59,1,59,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,60,1,61,1,61,1,61,1,
		61,1,62,1,62,1,62,1,62,1,63,1,63,1,63,1,63,1,64,1,64,1,64,1,64,1,64,1,
		65,1,65,1,65,1,66,1,66,1,66,1,67,1,67,1,67,1,67,1,67,1,67,1,67,1,67,1,
		67,1,67,1,67,1,68,1,68,1,68,1,68,1,68,1,68,1,68,1,68,1,68,1,68,1,68,1,
		68,1,69,1,69,1,69,1,69,1,69,1,70,1,70,1,70,1,70,1,70,1,70,1,70,1,70,1,
		71,1,71,1,72,1,72,1,73,1,73,1,74,4,74,554,8,74,11,74,12,74,555,1,74,1,
		74,1,75,1,75,5,75,562,8,75,10,75,12,75,565,9,75,1,75,1,75,1,76,1,76,4,
		76,571,8,76,11,76,12,76,572,1,77,4,77,576,8,77,11,77,12,77,577,1,78,1,
		78,1,78,4,78,583,8,78,11,78,12,78,584,1,78,4,78,588,8,78,11,78,12,78,589,
		1,78,4,78,593,8,78,11,78,12,78,594,1,78,1,78,1,78,5,78,600,8,78,10,78,
		12,78,603,9,78,1,78,1,78,0,0,79,1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,
		9,19,10,21,11,23,12,25,13,27,14,29,15,31,16,33,17,35,18,37,19,39,20,41,
		21,43,22,45,23,47,24,49,25,51,26,53,27,55,28,57,29,59,30,61,31,63,32,65,
		33,67,34,69,35,71,36,73,37,75,38,77,39,79,40,81,41,83,42,85,43,87,44,89,
		45,91,46,93,47,95,48,97,49,99,50,101,51,103,52,105,53,107,54,109,55,111,
		56,113,57,115,58,117,59,119,60,121,61,123,62,125,63,127,64,129,65,131,
		66,133,67,135,68,137,69,139,70,141,71,143,72,145,73,147,74,149,75,151,
		76,153,77,155,78,157,79,1,0,9,3,0,9,10,13,13,32,32,7,0,32,32,35,35,37,
		37,45,57,65,90,95,95,97,122,2,0,65,90,97,122,4,0,48,57,65,90,95,95,97,
		122,1,0,48,57,4,0,36,36,42,42,91,91,93,94,1,0,97,122,1,0,65,90,3,0,40,
		41,43,43,45,46,618,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,
		1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,
		0,0,21,1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,
		1,0,0,0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,
		0,0,43,1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,
		1,0,0,0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,
		0,0,65,1,0,0,0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,1,0,0,0,0,73,1,0,0,0,0,75,
		1,0,0,0,0,77,1,0,0,0,0,79,1,0,0,0,0,81,1,0,0,0,0,83,1,0,0,0,0,85,1,0,0,
		0,0,87,1,0,0,0,0,89,1,0,0,0,0,91,1,0,0,0,0,93,1,0,0,0,0,95,1,0,0,0,0,97,
		1,0,0,0,0,99,1,0,0,0,0,101,1,0,0,0,0,103,1,0,0,0,0,105,1,0,0,0,0,107,1,
		0,0,0,0,109,1,0,0,0,0,111,1,0,0,0,0,113,1,0,0,0,0,115,1,0,0,0,0,117,1,
		0,0,0,0,119,1,0,0,0,0,121,1,0,0,0,0,123,1,0,0,0,0,125,1,0,0,0,0,127,1,
		0,0,0,0,129,1,0,0,0,0,131,1,0,0,0,0,133,1,0,0,0,0,135,1,0,0,0,0,137,1,
		0,0,0,0,139,1,0,0,0,0,141,1,0,0,0,0,143,1,0,0,0,0,145,1,0,0,0,0,147,1,
		0,0,0,0,149,1,0,0,0,0,151,1,0,0,0,0,153,1,0,0,0,0,155,1,0,0,0,0,157,1,
		0,0,0,1,159,1,0,0,0,3,165,1,0,0,0,5,170,1,0,0,0,7,177,1,0,0,0,9,186,1,
		0,0,0,11,195,1,0,0,0,13,201,1,0,0,0,15,205,1,0,0,0,17,211,1,0,0,0,19,213,
		1,0,0,0,21,219,1,0,0,0,23,226,1,0,0,0,25,232,1,0,0,0,27,238,1,0,0,0,29,
		241,1,0,0,0,31,246,1,0,0,0,33,251,1,0,0,0,35,254,1,0,0,0,37,258,1,0,0,
		0,39,265,1,0,0,0,41,271,1,0,0,0,43,278,1,0,0,0,45,285,1,0,0,0,47,291,1,
		0,0,0,49,297,1,0,0,0,51,304,1,0,0,0,53,312,1,0,0,0,55,317,1,0,0,0,57,323,
		1,0,0,0,59,326,1,0,0,0,61,333,1,0,0,0,63,340,1,0,0,0,65,346,1,0,0,0,67,
		349,1,0,0,0,69,352,1,0,0,0,71,355,1,0,0,0,73,359,1,0,0,0,75,361,1,0,0,
		0,77,363,1,0,0,0,79,368,1,0,0,0,81,376,1,0,0,0,83,381,1,0,0,0,85,387,1,
		0,0,0,87,392,1,0,0,0,89,399,1,0,0,0,91,405,1,0,0,0,93,410,1,0,0,0,95,414,
		1,0,0,0,97,416,1,0,0,0,99,418,1,0,0,0,101,420,1,0,0,0,103,422,1,0,0,0,
		105,424,1,0,0,0,107,427,1,0,0,0,109,435,1,0,0,0,111,441,1,0,0,0,113,446,
		1,0,0,0,115,454,1,0,0,0,117,462,1,0,0,0,119,472,1,0,0,0,121,479,1,0,0,
		0,123,487,1,0,0,0,125,491,1,0,0,0,127,495,1,0,0,0,129,499,1,0,0,0,131,
		504,1,0,0,0,133,507,1,0,0,0,135,510,1,0,0,0,137,521,1,0,0,0,139,533,1,
		0,0,0,141,538,1,0,0,0,143,546,1,0,0,0,145,548,1,0,0,0,147,550,1,0,0,0,
		149,553,1,0,0,0,151,559,1,0,0,0,153,568,1,0,0,0,155,575,1,0,0,0,157,579,
		1,0,0,0,159,160,5,108,0,0,160,161,5,111,0,0,161,162,5,103,0,0,162,163,
		5,105,0,0,163,164,5,110,0,0,164,2,1,0,0,0,165,166,5,119,0,0,166,167,5,
		105,0,0,167,168,5,116,0,0,168,169,5,104,0,0,169,4,1,0,0,0,170,171,5,103,
		0,0,171,172,5,108,0,0,172,173,5,111,0,0,173,174,5,98,0,0,174,175,5,97,
		0,0,175,176,5,108,0,0,176,6,1,0,0,0,177,178,5,117,0,0,178,179,5,115,0,
		0,179,180,5,101,0,0,180,181,5,114,0,0,181,182,5,110,0,0,182,183,5,97,0,
		0,183,184,5,109,0,0,184,185,5,101,0,0,185,8,1,0,0,0,186,187,5,112,0,0,
		187,188,5,97,0,0,188,189,5,115,0,0,189,190,5,115,0,0,190,191,5,119,0,0,
		191,192,5,111,0,0,192,193,5,114,0,0,193,194,5,100,0,0,194,10,1,0,0,0,195,
		196,5,116,0,0,196,197,5,111,0,0,197,198,5,107,0,0,198,199,5,101,0,0,199,
		200,5,110,0,0,200,12,1,0,0,0,201,202,5,97,0,0,202,203,5,110,0,0,203,204,
		5,100,0,0,204,14,1,0,0,0,205,206,5,115,0,0,206,207,5,112,0,0,207,208,5,
		97,0,0,208,209,5,99,0,0,209,210,5,101,0,0,210,16,1,0,0,0,211,212,5,61,
		0,0,212,18,1,0,0,0,213,214,5,98,0,0,214,215,5,108,0,0,215,216,5,111,0,
		0,216,217,5,99,0,0,217,218,5,107,0,0,218,20,1,0,0,0,219,220,5,115,0,0,
		220,221,5,116,0,0,221,222,5,114,0,0,222,223,5,105,0,0,223,224,5,110,0,
		0,224,225,5,103,0,0,225,22,1,0,0,0,226,227,5,114,0,0,227,228,5,101,0,0,
		228,229,5,103,0,0,229,230,5,101,0,0,230,231,5,120,0,0,231,24,1,0,0,0,232,
		233,5,115,0,0,233,234,5,116,0,0,234,235,5,111,0,0,235,236,5,114,0,0,236,
		237,5,121,0,0,237,26,1,0,0,0,238,239,5,105,0,0,239,240,5,110,0,0,240,28,
		1,0,0,0,241,242,5,102,0,0,242,243,5,114,0,0,243,244,5,111,0,0,244,245,
		5,109,0,0,245,30,1,0,0,0,246,247,5,99,0,0,247,248,5,111,0,0,248,249,5,
		112,0,0,249,250,5,121,0,0,250,32,1,0,0,0,251,252,5,116,0,0,252,253,5,111,
		0,0,253,34,1,0,0,0,254,255,5,97,0,0,255,256,5,108,0,0,256,257,5,108,0,
		0,257,36,1,0,0,0,258,259,5,115,0,0,259,260,5,112,0,0,260,261,5,97,0,0,
		261,262,5,99,0,0,262,263,5,101,0,0,263,264,5,115,0,0,264,38,1,0,0,0,265,
		266,5,112,0,0,266,267,5,114,0,0,267,268,5,105,0,0,268,269,5,110,0,0,269,
		270,5,116,0,0,270,40,1,0,0,0,271,272,5,115,0,0,272,273,5,121,0,0,273,274,
		5,109,0,0,274,275,5,98,0,0,275,276,5,111,0,0,276,277,5,108,0,0,277,42,
		1,0,0,0,278,279,5,116,0,0,279,280,5,97,0,0,280,281,5,98,0,0,281,282,5,
		108,0,0,282,283,5,101,0,0,283,284,5,115,0,0,284,44,1,0,0,0,285,286,5,108,
		0,0,286,287,5,111,0,0,287,288,5,99,0,0,288,289,5,97,0,0,289,290,5,108,
		0,0,290,46,1,0,0,0,291,292,5,99,0,0,292,293,5,97,0,0,293,294,5,99,0,0,
		294,295,5,104,0,0,295,296,5,101,0,0,296,48,1,0,0,0,297,298,5,115,0,0,298,
		299,5,101,0,0,299,300,5,114,0,0,300,301,5,118,0,0,301,302,5,101,0,0,302,
		303,5,114,0,0,303,50,1,0,0,0,304,305,5,99,0,0,305,306,5,111,0,0,306,307,
		5,110,0,0,307,308,5,115,0,0,308,309,5,111,0,0,309,310,5,108,0,0,310,311,
		5,101,0,0,311,52,1,0,0,0,312,313,5,102,0,0,313,314,5,105,0,0,314,315,5,
		108,0,0,315,316,5,101,0,0,316,54,1,0,0,0,317,318,5,102,0,0,318,319,5,105,
		0,0,319,320,5,108,0,0,320,321,5,101,0,0,321,322,5,115,0,0,322,56,1,0,0,
		0,323,324,5,111,0,0,324,325,5,110,0,0,325,58,1,0,0,0,326,327,5,100,0,0,
		327,328,5,101,0,0,328,329,5,109,0,0,329,330,5,97,0,0,330,331,5,110,0,0,
		331,332,5,100,0,0,332,60,1,0,0,0,333,334,5,98,0,0,334,335,5,108,0,0,335,
		336,5,111,0,0,336,337,5,99,0,0,337,338,5,107,0,0,338,339,5,115,0,0,339,
		62,1,0,0,0,340,341,5,119,0,0,341,342,5,104,0,0,342,343,5,101,0,0,343,344,
		5,114,0,0,344,345,5,101,0,0,345,64,1,0,0,0,346,347,5,111,0,0,347,348,5,
		114,0,0,348,66,1,0,0,0,349,350,5,105,0,0,350,351,5,100,0,0,351,68,1,0,
		0,0,352,353,5,33,0,0,353,354,5,61,0,0,354,70,1,0,0,0,355,356,5,110,0,0,
		356,357,5,111,0,0,357,358,5,116,0,0,358,72,1,0,0,0,359,360,5,40,0,0,360,
		74,1,0,0,0,361,362,5,41,0,0,362,76,1,0,0,0,363,364,5,110,0,0,364,365,5,
		97,0,0,365,366,5,109,0,0,366,367,5,101,0,0,367,78,1,0,0,0,368,369,5,109,
		0,0,369,370,5,97,0,0,370,371,5,116,0,0,371,372,5,99,0,0,372,373,5,104,
		0,0,373,374,5,101,0,0,374,375,5,115,0,0,375,80,1,0,0,0,376,377,5,100,0,
		0,377,378,5,111,0,0,378,379,5,101,0,0,379,380,5,115,0,0,380,82,1,0,0,0,
		381,382,5,109,0,0,382,383,5,97,0,0,383,384,5,116,0,0,384,385,5,99,0,0,
		385,386,5,104,0,0,386,84,1,0,0,0,387,388,5,108,0,0,388,389,5,105,0,0,389,
		390,5,107,0,0,390,391,5,101,0,0,391,86,1,0,0,0,392,393,5,115,0,0,393,394,
		5,116,0,0,394,395,5,97,0,0,395,396,5,114,0,0,396,397,5,116,0,0,397,398,
		5,115,0,0,398,88,1,0,0,0,399,400,5,115,0,0,400,401,5,116,0,0,401,402,5,
		97,0,0,402,403,5,114,0,0,403,404,5,116,0,0,404,90,1,0,0,0,405,406,5,101,
		0,0,406,407,5,110,0,0,407,408,5,100,0,0,408,409,5,115,0,0,409,92,1,0,0,
		0,410,411,5,101,0,0,411,412,5,110,0,0,412,413,5,100,0,0,413,94,1,0,0,0,
		414,415,5,44,0,0,415,96,1,0,0,0,416,417,5,43,0,0,417,98,1,0,0,0,418,419,
		5,45,0,0,419,100,1,0,0,0,420,421,5,42,0,0,421,102,1,0,0,0,422,423,5,37,
		0,0,423,104,1,0,0,0,424,425,5,98,0,0,425,426,5,101,0,0,426,106,1,0,0,0,
		427,428,5,118,0,0,428,429,5,101,0,0,429,430,5,114,0,0,430,431,5,98,0,0,
		431,432,5,111,0,0,432,433,5,115,0,0,433,434,5,101,0,0,434,108,1,0,0,0,
		435,436,5,113,0,0,436,437,5,117,0,0,437,438,5,105,0,0,438,439,5,101,0,
		0,439,440,5,116,0,0,440,110,1,0,0,0,441,442,5,119,0,0,442,443,5,97,0,0,
		443,444,5,105,0,0,444,445,5,116,0,0,445,112,1,0,0,0,446,447,5,99,0,0,447,
		448,5,111,0,0,448,449,5,109,0,0,449,450,5,112,0,0,450,451,5,97,0,0,451,
		452,5,114,0,0,452,453,5,101,0,0,453,114,1,0,0,0,454,455,5,112,0,0,455,
		456,5,117,0,0,456,457,5,98,0,0,457,458,5,108,0,0,458,459,5,105,0,0,459,
		460,5,115,0,0,460,461,5,104,0,0,461,116,1,0,0,0,462,463,5,117,0,0,463,
		464,5,110,0,0,464,465,5,112,0,0,465,466,5,117,0,0,466,467,5,98,0,0,467,
		468,5,108,0,0,468,469,5,105,0,0,469,470,5,115,0,0,470,471,5,104,0,0,471,
		118,1,0,0,0,472,473,5,100,0,0,473,474,5,101,0,0,474,475,5,108,0,0,475,
		476,5,101,0,0,476,477,5,116,0,0,477,478,5,101,0,0,478,120,1,0,0,0,479,
		480,5,115,0,0,480,481,5,116,0,0,481,482,5,111,0,0,482,483,5,114,0,0,483,
		484,5,105,0,0,484,485,5,101,0,0,485,486,5,115,0,0,486,122,1,0,0,0,487,
		488,5,117,0,0,488,489,5,114,0,0,489,490,5,108,0,0,490,124,1,0,0,0,491,
		492,5,97,0,0,492,493,5,110,0,0,493,494,5,121,0,0,494,126,1,0,0,0,495,496,
		5,116,0,0,496,497,5,97,0,0,497,498,5,103,0,0,498,128,1,0,0,0,499,500,5,
		116,0,0,500,501,5,97,0,0,501,502,5,103,0,0,502,503,5,115,0,0,503,130,1,
		0,0,0,504,505,5,100,0,0,505,506,5,111,0,0,506,132,1,0,0,0,507,508,5,110,
		0,0,508,509,5,111,0,0,509,134,1,0,0,0,510,511,5,100,0,0,511,512,5,97,0,
		0,512,513,5,116,0,0,513,514,5,97,0,0,514,515,5,115,0,0,515,516,5,111,0,
		0,516,517,5,117,0,0,517,518,5,114,0,0,518,519,5,99,0,0,519,520,5,101,0,
		0,520,136,1,0,0,0,521,522,5,100,0,0,522,523,5,97,0,0,523,524,5,116,0,0,
		524,525,5,97,0,0,525,526,5,115,0,0,526,527,5,111,0,0,527,528,5,117,0,0,
		528,529,5,114,0,0,529,530,5,99,0,0,530,531,5,101,0,0,531,532,5,115,0,0,
		532,138,1,0,0,0,533,534,5,115,0,0,534,535,5,108,0,0,535,536,5,117,0,0,
		536,537,5,103,0,0,537,140,1,0,0,0,538,539,5,102,0,0,539,540,5,111,0,0,
		540,541,5,114,0,0,541,542,5,101,0,0,542,543,5,97,0,0,543,544,5,99,0,0,
		544,545,5,104,0,0,545,142,1,0,0,0,546,547,5,123,0,0,547,144,1,0,0,0,548,
		549,5,125,0,0,549,146,1,0,0,0,550,551,5,59,0,0,551,148,1,0,0,0,552,554,
		7,0,0,0,553,552,1,0,0,0,554,555,1,0,0,0,555,553,1,0,0,0,555,556,1,0,0,
		0,556,557,1,0,0,0,557,558,6,74,0,0,558,150,1,0,0,0,559,563,5,39,0,0,560,
		562,7,1,0,0,561,560,1,0,0,0,562,565,1,0,0,0,563,561,1,0,0,0,563,564,1,
		0,0,0,564,566,1,0,0,0,565,563,1,0,0,0,566,567,5,39,0,0,567,152,1,0,0,0,
		568,570,7,2,0,0,569,571,7,3,0,0,570,569,1,0,0,0,571,572,1,0,0,0,572,570,
		1,0,0,0,572,573,1,0,0,0,573,154,1,0,0,0,574,576,7,4,0,0,575,574,1,0,0,
		0,576,577,1,0,0,0,577,575,1,0,0,0,577,578,1,0,0,0,578,156,1,0,0,0,579,
		601,5,47,0,0,580,600,7,5,0,0,581,583,7,6,0,0,582,581,1,0,0,0,583,584,1,
		0,0,0,584,582,1,0,0,0,584,585,1,0,0,0,585,600,1,0,0,0,586,588,7,7,0,0,
		587,586,1,0,0,0,588,589,1,0,0,0,589,587,1,0,0,0,589,590,1,0,0,0,590,600,
		1,0,0,0,591,593,7,4,0,0,592,591,1,0,0,0,593,594,1,0,0,0,594,592,1,0,0,
		0,594,595,1,0,0,0,595,600,1,0,0,0,596,600,7,8,0,0,597,598,5,92,0,0,598,
		600,5,47,0,0,599,580,1,0,0,0,599,582,1,0,0,0,599,587,1,0,0,0,599,592,1,
		0,0,0,599,596,1,0,0,0,599,597,1,0,0,0,600,603,1,0,0,0,601,599,1,0,0,0,
		601,602,1,0,0,0,602,604,1,0,0,0,603,601,1,0,0,0,604,605,5,47,0,0,605,158,
		1,0,0,0,11,0,555,561,563,572,577,584,589,594,599,601,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace BlokScript.Parser
