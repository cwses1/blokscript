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
#pragma warning disable 3021
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
		T__73=74, T__74=75, T__75=76, T__76=77, T__77=78, T__78=79, T__79=80, 
		T__80=81, T__81=82, T__82=83, T__83=84, T__84=85, T__85=86, T__86=87, 
		T__87=88, T__88=89, T__89=90, T__90=91, T__91=92, T__92=93, T__93=94, 
		T__94=95, T__95=96, T__96=97, T__97=98, T__98=99, STATEMENTEND=100, WS=101, 
		STRINGLITERAL=102, VARID=103, INTLITERAL=104, REGEXLITERAL=105, LINE_COMMENT=106, 
		BLOCK_COMMENT=107;
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
		"T__73", "T__74", "T__75", "T__76", "T__77", "T__78", "T__79", "T__80", 
		"T__81", "T__82", "T__83", "T__84", "T__85", "T__86", "T__87", "T__88", 
		"T__89", "T__90", "T__91", "T__92", "T__93", "T__94", "T__95", "T__96", 
		"T__97", "T__98", "STATEMENTEND", "WS", "STRINGLITERAL", "VARID", "INTLITERAL", 
		"REGEXLITERAL", "LINE_COMMENT", "BLOCK_COMMENT"
	};


	public BlokScriptGrammarLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public BlokScriptGrammarLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'create'", "'datasource'", "'('", "')'", "'for'", "'in'", "'delete'", 
		"'update'", "'set'", "','", "'name'", "'='", "'slug'", "'entry'", "'from'", 
		"'entries'", "'where'", "'copy'", "'to'", "'sync'", "'skip'", "'updates'", 
		"'creates'", "'value'", "'local cache'", "'csv'", "'json'", "'url'", "'console'", 
		"'and'", "'or'", "'id'", "'!='", "'not'", "'matches'", "'does'", "'match'", 
		"'regex'", "'like'", "'starts'", "'start'", "'with'", "'ends'", "'end'", 
		"'login'", "'global'", "'username'", "'password'", "'token'", "'var'", 
		"'space'", "'block'", "'string'", "'story'", "'spaces'", "'print'", "'symbol'", 
		"'tables'", "'local'", "'cache'", "'server'", "'file'", "'on'", "'blocks'", 
		"'technical'", "'display'", "'type'", "'nestable'", "'content'", "'universal'", 
		"'add'", "'tag'", "'remove'", "'preview'", "'field'", "'template'", "'screenshot'", 
		"'+'", "'-'", "'*'", "'%'", "'be'", "'quiet'", "'verbose'", "'debugger'", 
		"'wait'", "'compare'", "'all'", "'stories'", "'publish'", "'unpublish'", 
		"'any'", "'tags'", "'do'", "'no'", "'datasources'", "'foreach'", "'{'", 
		"'}'", "';'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, "STATEMENTEND", "WS", "STRINGLITERAL", "VARID", 
		"INTLITERAL", "REGEXLITERAL", "LINE_COMMENT", "BLOCK_COMMENT"
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
		4,0,107,876,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
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
		7,77,2,78,7,78,2,79,7,79,2,80,7,80,2,81,7,81,2,82,7,82,2,83,7,83,2,84,
		7,84,2,85,7,85,2,86,7,86,2,87,7,87,2,88,7,88,2,89,7,89,2,90,7,90,2,91,
		7,91,2,92,7,92,2,93,7,93,2,94,7,94,2,95,7,95,2,96,7,96,2,97,7,97,2,98,
		7,98,2,99,7,99,2,100,7,100,2,101,7,101,2,102,7,102,2,103,7,103,2,104,7,
		104,2,105,7,105,2,106,7,106,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,1,1,1,1,1,1,
		1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,2,1,3,1,3,1,4,1,4,1,4,1,4,1,5,1,5,
		1,5,1,6,1,6,1,6,1,6,1,6,1,6,1,6,1,7,1,7,1,7,1,7,1,7,1,7,1,7,1,8,1,8,1,
		8,1,8,1,9,1,9,1,10,1,10,1,10,1,10,1,10,1,11,1,11,1,12,1,12,1,12,1,12,1,
		12,1,13,1,13,1,13,1,13,1,13,1,13,1,14,1,14,1,14,1,14,1,14,1,15,1,15,1,
		15,1,15,1,15,1,15,1,15,1,15,1,16,1,16,1,16,1,16,1,16,1,16,1,17,1,17,1,
		17,1,17,1,17,1,18,1,18,1,18,1,19,1,19,1,19,1,19,1,19,1,20,1,20,1,20,1,
		20,1,20,1,21,1,21,1,21,1,21,1,21,1,21,1,21,1,21,1,22,1,22,1,22,1,22,1,
		22,1,22,1,22,1,22,1,23,1,23,1,23,1,23,1,23,1,23,1,24,1,24,1,24,1,24,1,
		24,1,24,1,24,1,24,1,24,1,24,1,24,1,24,1,25,1,25,1,25,1,25,1,26,1,26,1,
		26,1,26,1,26,1,27,1,27,1,27,1,27,1,28,1,28,1,28,1,28,1,28,1,28,1,28,1,
		28,1,29,1,29,1,29,1,29,1,30,1,30,1,30,1,31,1,31,1,31,1,32,1,32,1,32,1,
		33,1,33,1,33,1,33,1,34,1,34,1,34,1,34,1,34,1,34,1,34,1,34,1,35,1,35,1,
		35,1,35,1,35,1,36,1,36,1,36,1,36,1,36,1,36,1,37,1,37,1,37,1,37,1,37,1,
		37,1,38,1,38,1,38,1,38,1,38,1,39,1,39,1,39,1,39,1,39,1,39,1,39,1,40,1,
		40,1,40,1,40,1,40,1,40,1,41,1,41,1,41,1,41,1,41,1,42,1,42,1,42,1,42,1,
		42,1,43,1,43,1,43,1,43,1,44,1,44,1,44,1,44,1,44,1,44,1,45,1,45,1,45,1,
		45,1,45,1,45,1,45,1,46,1,46,1,46,1,46,1,46,1,46,1,46,1,46,1,46,1,47,1,
		47,1,47,1,47,1,47,1,47,1,47,1,47,1,47,1,48,1,48,1,48,1,48,1,48,1,48,1,
		49,1,49,1,49,1,49,1,50,1,50,1,50,1,50,1,50,1,50,1,51,1,51,1,51,1,51,1,
		51,1,51,1,52,1,52,1,52,1,52,1,52,1,52,1,52,1,53,1,53,1,53,1,53,1,53,1,
		53,1,54,1,54,1,54,1,54,1,54,1,54,1,54,1,55,1,55,1,55,1,55,1,55,1,55,1,
		56,1,56,1,56,1,56,1,56,1,56,1,56,1,57,1,57,1,57,1,57,1,57,1,57,1,57,1,
		58,1,58,1,58,1,58,1,58,1,58,1,59,1,59,1,59,1,59,1,59,1,59,1,60,1,60,1,
		60,1,60,1,60,1,60,1,60,1,61,1,61,1,61,1,61,1,61,1,62,1,62,1,62,1,63,1,
		63,1,63,1,63,1,63,1,63,1,63,1,64,1,64,1,64,1,64,1,64,1,64,1,64,1,64,1,
		64,1,64,1,65,1,65,1,65,1,65,1,65,1,65,1,65,1,65,1,66,1,66,1,66,1,66,1,
		66,1,67,1,67,1,67,1,67,1,67,1,67,1,67,1,67,1,67,1,68,1,68,1,68,1,68,1,
		68,1,68,1,68,1,68,1,69,1,69,1,69,1,69,1,69,1,69,1,69,1,69,1,69,1,69,1,
		70,1,70,1,70,1,70,1,71,1,71,1,71,1,71,1,72,1,72,1,72,1,72,1,72,1,72,1,
		72,1,73,1,73,1,73,1,73,1,73,1,73,1,73,1,73,1,74,1,74,1,74,1,74,1,74,1,
		74,1,75,1,75,1,75,1,75,1,75,1,75,1,75,1,75,1,75,1,76,1,76,1,76,1,76,1,
		76,1,76,1,76,1,76,1,76,1,76,1,76,1,77,1,77,1,78,1,78,1,79,1,79,1,80,1,
		80,1,81,1,81,1,81,1,82,1,82,1,82,1,82,1,82,1,82,1,83,1,83,1,83,1,83,1,
		83,1,83,1,83,1,83,1,84,1,84,1,84,1,84,1,84,1,84,1,84,1,84,1,84,1,85,1,
		85,1,85,1,85,1,85,1,86,1,86,1,86,1,86,1,86,1,86,1,86,1,86,1,87,1,87,1,
		87,1,87,1,88,1,88,1,88,1,88,1,88,1,88,1,88,1,88,1,89,1,89,1,89,1,89,1,
		89,1,89,1,89,1,89,1,90,1,90,1,90,1,90,1,90,1,90,1,90,1,90,1,90,1,90,1,
		91,1,91,1,91,1,91,1,92,1,92,1,92,1,92,1,92,1,93,1,93,1,93,1,94,1,94,1,
		94,1,95,1,95,1,95,1,95,1,95,1,95,1,95,1,95,1,95,1,95,1,95,1,95,1,96,1,
		96,1,96,1,96,1,96,1,96,1,96,1,96,1,97,1,97,1,98,1,98,1,99,1,99,1,100,4,
		100,794,8,100,11,100,12,100,795,1,100,1,100,1,101,1,101,5,101,802,8,101,
		10,101,12,101,805,9,101,1,101,1,101,1,102,1,102,4,102,811,8,102,11,102,
		12,102,812,1,103,4,103,816,8,103,11,103,12,103,817,1,104,1,104,1,104,4,
		104,823,8,104,11,104,12,104,824,1,104,4,104,828,8,104,11,104,12,104,829,
		1,104,4,104,833,8,104,11,104,12,104,834,1,104,1,104,1,104,5,104,840,8,
		104,10,104,12,104,843,9,104,1,104,1,104,1,105,1,105,1,105,1,105,5,105,
		851,8,105,10,105,12,105,854,9,105,1,105,3,105,857,8,105,1,105,1,105,1,
		105,1,105,1,106,1,106,1,106,1,106,5,106,867,8,106,10,106,12,106,870,9,
		106,1,106,1,106,1,106,1,106,1,106,1,868,0,107,1,1,3,2,5,3,7,4,9,5,11,6,
		13,7,15,8,17,9,19,10,21,11,23,12,25,13,27,14,29,15,31,16,33,17,35,18,37,
		19,39,20,41,21,43,22,45,23,47,24,49,25,51,26,53,27,55,28,57,29,59,30,61,
		31,63,32,65,33,67,34,69,35,71,36,73,37,75,38,77,39,79,40,81,41,83,42,85,
		43,87,44,89,45,91,46,93,47,95,48,97,49,99,50,101,51,103,52,105,53,107,
		54,109,55,111,56,113,57,115,58,117,59,119,60,121,61,123,62,125,63,127,
		64,129,65,131,66,133,67,135,68,137,69,139,70,141,71,143,72,145,73,147,
		74,149,75,151,76,153,77,155,78,157,79,159,80,161,81,163,82,165,83,167,
		84,169,85,171,86,173,87,175,88,177,89,179,90,181,91,183,92,185,93,187,
		94,189,95,191,96,193,97,195,98,197,99,199,100,201,101,203,102,205,103,
		207,104,209,105,211,106,213,107,1,0,10,3,0,9,10,13,13,32,32,7,0,32,32,
		35,35,37,37,45,57,65,90,95,95,97,122,2,0,65,90,97,122,4,0,48,57,65,90,
		95,95,97,122,1,0,48,57,4,0,36,36,42,42,91,91,93,94,1,0,97,122,1,0,65,90,
		3,0,40,41,43,43,45,46,2,0,10,10,13,13,891,0,1,1,0,0,0,0,3,1,0,0,0,0,5,
		1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,
		0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,
		1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,
		0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,
		1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,
		0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,
		1,0,0,0,0,73,1,0,0,0,0,75,1,0,0,0,0,77,1,0,0,0,0,79,1,0,0,0,0,81,1,0,0,
		0,0,83,1,0,0,0,0,85,1,0,0,0,0,87,1,0,0,0,0,89,1,0,0,0,0,91,1,0,0,0,0,93,
		1,0,0,0,0,95,1,0,0,0,0,97,1,0,0,0,0,99,1,0,0,0,0,101,1,0,0,0,0,103,1,0,
		0,0,0,105,1,0,0,0,0,107,1,0,0,0,0,109,1,0,0,0,0,111,1,0,0,0,0,113,1,0,
		0,0,0,115,1,0,0,0,0,117,1,0,0,0,0,119,1,0,0,0,0,121,1,0,0,0,0,123,1,0,
		0,0,0,125,1,0,0,0,0,127,1,0,0,0,0,129,1,0,0,0,0,131,1,0,0,0,0,133,1,0,
		0,0,0,135,1,0,0,0,0,137,1,0,0,0,0,139,1,0,0,0,0,141,1,0,0,0,0,143,1,0,
		0,0,0,145,1,0,0,0,0,147,1,0,0,0,0,149,1,0,0,0,0,151,1,0,0,0,0,153,1,0,
		0,0,0,155,1,0,0,0,0,157,1,0,0,0,0,159,1,0,0,0,0,161,1,0,0,0,0,163,1,0,
		0,0,0,165,1,0,0,0,0,167,1,0,0,0,0,169,1,0,0,0,0,171,1,0,0,0,0,173,1,0,
		0,0,0,175,1,0,0,0,0,177,1,0,0,0,0,179,1,0,0,0,0,181,1,0,0,0,0,183,1,0,
		0,0,0,185,1,0,0,0,0,187,1,0,0,0,0,189,1,0,0,0,0,191,1,0,0,0,0,193,1,0,
		0,0,0,195,1,0,0,0,0,197,1,0,0,0,0,199,1,0,0,0,0,201,1,0,0,0,0,203,1,0,
		0,0,0,205,1,0,0,0,0,207,1,0,0,0,0,209,1,0,0,0,0,211,1,0,0,0,0,213,1,0,
		0,0,1,215,1,0,0,0,3,222,1,0,0,0,5,233,1,0,0,0,7,235,1,0,0,0,9,237,1,0,
		0,0,11,241,1,0,0,0,13,244,1,0,0,0,15,251,1,0,0,0,17,258,1,0,0,0,19,262,
		1,0,0,0,21,264,1,0,0,0,23,269,1,0,0,0,25,271,1,0,0,0,27,276,1,0,0,0,29,
		282,1,0,0,0,31,287,1,0,0,0,33,295,1,0,0,0,35,301,1,0,0,0,37,306,1,0,0,
		0,39,309,1,0,0,0,41,314,1,0,0,0,43,319,1,0,0,0,45,327,1,0,0,0,47,335,1,
		0,0,0,49,341,1,0,0,0,51,353,1,0,0,0,53,357,1,0,0,0,55,362,1,0,0,0,57,366,
		1,0,0,0,59,374,1,0,0,0,61,378,1,0,0,0,63,381,1,0,0,0,65,384,1,0,0,0,67,
		387,1,0,0,0,69,391,1,0,0,0,71,399,1,0,0,0,73,404,1,0,0,0,75,410,1,0,0,
		0,77,416,1,0,0,0,79,421,1,0,0,0,81,428,1,0,0,0,83,434,1,0,0,0,85,439,1,
		0,0,0,87,444,1,0,0,0,89,448,1,0,0,0,91,454,1,0,0,0,93,461,1,0,0,0,95,470,
		1,0,0,0,97,479,1,0,0,0,99,485,1,0,0,0,101,489,1,0,0,0,103,495,1,0,0,0,
		105,501,1,0,0,0,107,508,1,0,0,0,109,514,1,0,0,0,111,521,1,0,0,0,113,527,
		1,0,0,0,115,534,1,0,0,0,117,541,1,0,0,0,119,547,1,0,0,0,121,553,1,0,0,
		0,123,560,1,0,0,0,125,565,1,0,0,0,127,568,1,0,0,0,129,575,1,0,0,0,131,
		585,1,0,0,0,133,593,1,0,0,0,135,598,1,0,0,0,137,607,1,0,0,0,139,615,1,
		0,0,0,141,625,1,0,0,0,143,629,1,0,0,0,145,633,1,0,0,0,147,640,1,0,0,0,
		149,648,1,0,0,0,151,654,1,0,0,0,153,663,1,0,0,0,155,674,1,0,0,0,157,676,
		1,0,0,0,159,678,1,0,0,0,161,680,1,0,0,0,163,682,1,0,0,0,165,685,1,0,0,
		0,167,691,1,0,0,0,169,699,1,0,0,0,171,708,1,0,0,0,173,713,1,0,0,0,175,
		721,1,0,0,0,177,725,1,0,0,0,179,733,1,0,0,0,181,741,1,0,0,0,183,751,1,
		0,0,0,185,755,1,0,0,0,187,760,1,0,0,0,189,763,1,0,0,0,191,766,1,0,0,0,
		193,778,1,0,0,0,195,786,1,0,0,0,197,788,1,0,0,0,199,790,1,0,0,0,201,793,
		1,0,0,0,203,799,1,0,0,0,205,808,1,0,0,0,207,815,1,0,0,0,209,819,1,0,0,
		0,211,846,1,0,0,0,213,862,1,0,0,0,215,216,5,99,0,0,216,217,5,114,0,0,217,
		218,5,101,0,0,218,219,5,97,0,0,219,220,5,116,0,0,220,221,5,101,0,0,221,
		2,1,0,0,0,222,223,5,100,0,0,223,224,5,97,0,0,224,225,5,116,0,0,225,226,
		5,97,0,0,226,227,5,115,0,0,227,228,5,111,0,0,228,229,5,117,0,0,229,230,
		5,114,0,0,230,231,5,99,0,0,231,232,5,101,0,0,232,4,1,0,0,0,233,234,5,40,
		0,0,234,6,1,0,0,0,235,236,5,41,0,0,236,8,1,0,0,0,237,238,5,102,0,0,238,
		239,5,111,0,0,239,240,5,114,0,0,240,10,1,0,0,0,241,242,5,105,0,0,242,243,
		5,110,0,0,243,12,1,0,0,0,244,245,5,100,0,0,245,246,5,101,0,0,246,247,5,
		108,0,0,247,248,5,101,0,0,248,249,5,116,0,0,249,250,5,101,0,0,250,14,1,
		0,0,0,251,252,5,117,0,0,252,253,5,112,0,0,253,254,5,100,0,0,254,255,5,
		97,0,0,255,256,5,116,0,0,256,257,5,101,0,0,257,16,1,0,0,0,258,259,5,115,
		0,0,259,260,5,101,0,0,260,261,5,116,0,0,261,18,1,0,0,0,262,263,5,44,0,
		0,263,20,1,0,0,0,264,265,5,110,0,0,265,266,5,97,0,0,266,267,5,109,0,0,
		267,268,5,101,0,0,268,22,1,0,0,0,269,270,5,61,0,0,270,24,1,0,0,0,271,272,
		5,115,0,0,272,273,5,108,0,0,273,274,5,117,0,0,274,275,5,103,0,0,275,26,
		1,0,0,0,276,277,5,101,0,0,277,278,5,110,0,0,278,279,5,116,0,0,279,280,
		5,114,0,0,280,281,5,121,0,0,281,28,1,0,0,0,282,283,5,102,0,0,283,284,5,
		114,0,0,284,285,5,111,0,0,285,286,5,109,0,0,286,30,1,0,0,0,287,288,5,101,
		0,0,288,289,5,110,0,0,289,290,5,116,0,0,290,291,5,114,0,0,291,292,5,105,
		0,0,292,293,5,101,0,0,293,294,5,115,0,0,294,32,1,0,0,0,295,296,5,119,0,
		0,296,297,5,104,0,0,297,298,5,101,0,0,298,299,5,114,0,0,299,300,5,101,
		0,0,300,34,1,0,0,0,301,302,5,99,0,0,302,303,5,111,0,0,303,304,5,112,0,
		0,304,305,5,121,0,0,305,36,1,0,0,0,306,307,5,116,0,0,307,308,5,111,0,0,
		308,38,1,0,0,0,309,310,5,115,0,0,310,311,5,121,0,0,311,312,5,110,0,0,312,
		313,5,99,0,0,313,40,1,0,0,0,314,315,5,115,0,0,315,316,5,107,0,0,316,317,
		5,105,0,0,317,318,5,112,0,0,318,42,1,0,0,0,319,320,5,117,0,0,320,321,5,
		112,0,0,321,322,5,100,0,0,322,323,5,97,0,0,323,324,5,116,0,0,324,325,5,
		101,0,0,325,326,5,115,0,0,326,44,1,0,0,0,327,328,5,99,0,0,328,329,5,114,
		0,0,329,330,5,101,0,0,330,331,5,97,0,0,331,332,5,116,0,0,332,333,5,101,
		0,0,333,334,5,115,0,0,334,46,1,0,0,0,335,336,5,118,0,0,336,337,5,97,0,
		0,337,338,5,108,0,0,338,339,5,117,0,0,339,340,5,101,0,0,340,48,1,0,0,0,
		341,342,5,108,0,0,342,343,5,111,0,0,343,344,5,99,0,0,344,345,5,97,0,0,
		345,346,5,108,0,0,346,347,5,32,0,0,347,348,5,99,0,0,348,349,5,97,0,0,349,
		350,5,99,0,0,350,351,5,104,0,0,351,352,5,101,0,0,352,50,1,0,0,0,353,354,
		5,99,0,0,354,355,5,115,0,0,355,356,5,118,0,0,356,52,1,0,0,0,357,358,5,
		106,0,0,358,359,5,115,0,0,359,360,5,111,0,0,360,361,5,110,0,0,361,54,1,
		0,0,0,362,363,5,117,0,0,363,364,5,114,0,0,364,365,5,108,0,0,365,56,1,0,
		0,0,366,367,5,99,0,0,367,368,5,111,0,0,368,369,5,110,0,0,369,370,5,115,
		0,0,370,371,5,111,0,0,371,372,5,108,0,0,372,373,5,101,0,0,373,58,1,0,0,
		0,374,375,5,97,0,0,375,376,5,110,0,0,376,377,5,100,0,0,377,60,1,0,0,0,
		378,379,5,111,0,0,379,380,5,114,0,0,380,62,1,0,0,0,381,382,5,105,0,0,382,
		383,5,100,0,0,383,64,1,0,0,0,384,385,5,33,0,0,385,386,5,61,0,0,386,66,
		1,0,0,0,387,388,5,110,0,0,388,389,5,111,0,0,389,390,5,116,0,0,390,68,1,
		0,0,0,391,392,5,109,0,0,392,393,5,97,0,0,393,394,5,116,0,0,394,395,5,99,
		0,0,395,396,5,104,0,0,396,397,5,101,0,0,397,398,5,115,0,0,398,70,1,0,0,
		0,399,400,5,100,0,0,400,401,5,111,0,0,401,402,5,101,0,0,402,403,5,115,
		0,0,403,72,1,0,0,0,404,405,5,109,0,0,405,406,5,97,0,0,406,407,5,116,0,
		0,407,408,5,99,0,0,408,409,5,104,0,0,409,74,1,0,0,0,410,411,5,114,0,0,
		411,412,5,101,0,0,412,413,5,103,0,0,413,414,5,101,0,0,414,415,5,120,0,
		0,415,76,1,0,0,0,416,417,5,108,0,0,417,418,5,105,0,0,418,419,5,107,0,0,
		419,420,5,101,0,0,420,78,1,0,0,0,421,422,5,115,0,0,422,423,5,116,0,0,423,
		424,5,97,0,0,424,425,5,114,0,0,425,426,5,116,0,0,426,427,5,115,0,0,427,
		80,1,0,0,0,428,429,5,115,0,0,429,430,5,116,0,0,430,431,5,97,0,0,431,432,
		5,114,0,0,432,433,5,116,0,0,433,82,1,0,0,0,434,435,5,119,0,0,435,436,5,
		105,0,0,436,437,5,116,0,0,437,438,5,104,0,0,438,84,1,0,0,0,439,440,5,101,
		0,0,440,441,5,110,0,0,441,442,5,100,0,0,442,443,5,115,0,0,443,86,1,0,0,
		0,444,445,5,101,0,0,445,446,5,110,0,0,446,447,5,100,0,0,447,88,1,0,0,0,
		448,449,5,108,0,0,449,450,5,111,0,0,450,451,5,103,0,0,451,452,5,105,0,
		0,452,453,5,110,0,0,453,90,1,0,0,0,454,455,5,103,0,0,455,456,5,108,0,0,
		456,457,5,111,0,0,457,458,5,98,0,0,458,459,5,97,0,0,459,460,5,108,0,0,
		460,92,1,0,0,0,461,462,5,117,0,0,462,463,5,115,0,0,463,464,5,101,0,0,464,
		465,5,114,0,0,465,466,5,110,0,0,466,467,5,97,0,0,467,468,5,109,0,0,468,
		469,5,101,0,0,469,94,1,0,0,0,470,471,5,112,0,0,471,472,5,97,0,0,472,473,
		5,115,0,0,473,474,5,115,0,0,474,475,5,119,0,0,475,476,5,111,0,0,476,477,
		5,114,0,0,477,478,5,100,0,0,478,96,1,0,0,0,479,480,5,116,0,0,480,481,5,
		111,0,0,481,482,5,107,0,0,482,483,5,101,0,0,483,484,5,110,0,0,484,98,1,
		0,0,0,485,486,5,118,0,0,486,487,5,97,0,0,487,488,5,114,0,0,488,100,1,0,
		0,0,489,490,5,115,0,0,490,491,5,112,0,0,491,492,5,97,0,0,492,493,5,99,
		0,0,493,494,5,101,0,0,494,102,1,0,0,0,495,496,5,98,0,0,496,497,5,108,0,
		0,497,498,5,111,0,0,498,499,5,99,0,0,499,500,5,107,0,0,500,104,1,0,0,0,
		501,502,5,115,0,0,502,503,5,116,0,0,503,504,5,114,0,0,504,505,5,105,0,
		0,505,506,5,110,0,0,506,507,5,103,0,0,507,106,1,0,0,0,508,509,5,115,0,
		0,509,510,5,116,0,0,510,511,5,111,0,0,511,512,5,114,0,0,512,513,5,121,
		0,0,513,108,1,0,0,0,514,515,5,115,0,0,515,516,5,112,0,0,516,517,5,97,0,
		0,517,518,5,99,0,0,518,519,5,101,0,0,519,520,5,115,0,0,520,110,1,0,0,0,
		521,522,5,112,0,0,522,523,5,114,0,0,523,524,5,105,0,0,524,525,5,110,0,
		0,525,526,5,116,0,0,526,112,1,0,0,0,527,528,5,115,0,0,528,529,5,121,0,
		0,529,530,5,109,0,0,530,531,5,98,0,0,531,532,5,111,0,0,532,533,5,108,0,
		0,533,114,1,0,0,0,534,535,5,116,0,0,535,536,5,97,0,0,536,537,5,98,0,0,
		537,538,5,108,0,0,538,539,5,101,0,0,539,540,5,115,0,0,540,116,1,0,0,0,
		541,542,5,108,0,0,542,543,5,111,0,0,543,544,5,99,0,0,544,545,5,97,0,0,
		545,546,5,108,0,0,546,118,1,0,0,0,547,548,5,99,0,0,548,549,5,97,0,0,549,
		550,5,99,0,0,550,551,5,104,0,0,551,552,5,101,0,0,552,120,1,0,0,0,553,554,
		5,115,0,0,554,555,5,101,0,0,555,556,5,114,0,0,556,557,5,118,0,0,557,558,
		5,101,0,0,558,559,5,114,0,0,559,122,1,0,0,0,560,561,5,102,0,0,561,562,
		5,105,0,0,562,563,5,108,0,0,563,564,5,101,0,0,564,124,1,0,0,0,565,566,
		5,111,0,0,566,567,5,110,0,0,567,126,1,0,0,0,568,569,5,98,0,0,569,570,5,
		108,0,0,570,571,5,111,0,0,571,572,5,99,0,0,572,573,5,107,0,0,573,574,5,
		115,0,0,574,128,1,0,0,0,575,576,5,116,0,0,576,577,5,101,0,0,577,578,5,
		99,0,0,578,579,5,104,0,0,579,580,5,110,0,0,580,581,5,105,0,0,581,582,5,
		99,0,0,582,583,5,97,0,0,583,584,5,108,0,0,584,130,1,0,0,0,585,586,5,100,
		0,0,586,587,5,105,0,0,587,588,5,115,0,0,588,589,5,112,0,0,589,590,5,108,
		0,0,590,591,5,97,0,0,591,592,5,121,0,0,592,132,1,0,0,0,593,594,5,116,0,
		0,594,595,5,121,0,0,595,596,5,112,0,0,596,597,5,101,0,0,597,134,1,0,0,
		0,598,599,5,110,0,0,599,600,5,101,0,0,600,601,5,115,0,0,601,602,5,116,
		0,0,602,603,5,97,0,0,603,604,5,98,0,0,604,605,5,108,0,0,605,606,5,101,
		0,0,606,136,1,0,0,0,607,608,5,99,0,0,608,609,5,111,0,0,609,610,5,110,0,
		0,610,611,5,116,0,0,611,612,5,101,0,0,612,613,5,110,0,0,613,614,5,116,
		0,0,614,138,1,0,0,0,615,616,5,117,0,0,616,617,5,110,0,0,617,618,5,105,
		0,0,618,619,5,118,0,0,619,620,5,101,0,0,620,621,5,114,0,0,621,622,5,115,
		0,0,622,623,5,97,0,0,623,624,5,108,0,0,624,140,1,0,0,0,625,626,5,97,0,
		0,626,627,5,100,0,0,627,628,5,100,0,0,628,142,1,0,0,0,629,630,5,116,0,
		0,630,631,5,97,0,0,631,632,5,103,0,0,632,144,1,0,0,0,633,634,5,114,0,0,
		634,635,5,101,0,0,635,636,5,109,0,0,636,637,5,111,0,0,637,638,5,118,0,
		0,638,639,5,101,0,0,639,146,1,0,0,0,640,641,5,112,0,0,641,642,5,114,0,
		0,642,643,5,101,0,0,643,644,5,118,0,0,644,645,5,105,0,0,645,646,5,101,
		0,0,646,647,5,119,0,0,647,148,1,0,0,0,648,649,5,102,0,0,649,650,5,105,
		0,0,650,651,5,101,0,0,651,652,5,108,0,0,652,653,5,100,0,0,653,150,1,0,
		0,0,654,655,5,116,0,0,655,656,5,101,0,0,656,657,5,109,0,0,657,658,5,112,
		0,0,658,659,5,108,0,0,659,660,5,97,0,0,660,661,5,116,0,0,661,662,5,101,
		0,0,662,152,1,0,0,0,663,664,5,115,0,0,664,665,5,99,0,0,665,666,5,114,0,
		0,666,667,5,101,0,0,667,668,5,101,0,0,668,669,5,110,0,0,669,670,5,115,
		0,0,670,671,5,104,0,0,671,672,5,111,0,0,672,673,5,116,0,0,673,154,1,0,
		0,0,674,675,5,43,0,0,675,156,1,0,0,0,676,677,5,45,0,0,677,158,1,0,0,0,
		678,679,5,42,0,0,679,160,1,0,0,0,680,681,5,37,0,0,681,162,1,0,0,0,682,
		683,5,98,0,0,683,684,5,101,0,0,684,164,1,0,0,0,685,686,5,113,0,0,686,687,
		5,117,0,0,687,688,5,105,0,0,688,689,5,101,0,0,689,690,5,116,0,0,690,166,
		1,0,0,0,691,692,5,118,0,0,692,693,5,101,0,0,693,694,5,114,0,0,694,695,
		5,98,0,0,695,696,5,111,0,0,696,697,5,115,0,0,697,698,5,101,0,0,698,168,
		1,0,0,0,699,700,5,100,0,0,700,701,5,101,0,0,701,702,5,98,0,0,702,703,5,
		117,0,0,703,704,5,103,0,0,704,705,5,103,0,0,705,706,5,101,0,0,706,707,
		5,114,0,0,707,170,1,0,0,0,708,709,5,119,0,0,709,710,5,97,0,0,710,711,5,
		105,0,0,711,712,5,116,0,0,712,172,1,0,0,0,713,714,5,99,0,0,714,715,5,111,
		0,0,715,716,5,109,0,0,716,717,5,112,0,0,717,718,5,97,0,0,718,719,5,114,
		0,0,719,720,5,101,0,0,720,174,1,0,0,0,721,722,5,97,0,0,722,723,5,108,0,
		0,723,724,5,108,0,0,724,176,1,0,0,0,725,726,5,115,0,0,726,727,5,116,0,
		0,727,728,5,111,0,0,728,729,5,114,0,0,729,730,5,105,0,0,730,731,5,101,
		0,0,731,732,5,115,0,0,732,178,1,0,0,0,733,734,5,112,0,0,734,735,5,117,
		0,0,735,736,5,98,0,0,736,737,5,108,0,0,737,738,5,105,0,0,738,739,5,115,
		0,0,739,740,5,104,0,0,740,180,1,0,0,0,741,742,5,117,0,0,742,743,5,110,
		0,0,743,744,5,112,0,0,744,745,5,117,0,0,745,746,5,98,0,0,746,747,5,108,
		0,0,747,748,5,105,0,0,748,749,5,115,0,0,749,750,5,104,0,0,750,182,1,0,
		0,0,751,752,5,97,0,0,752,753,5,110,0,0,753,754,5,121,0,0,754,184,1,0,0,
		0,755,756,5,116,0,0,756,757,5,97,0,0,757,758,5,103,0,0,758,759,5,115,0,
		0,759,186,1,0,0,0,760,761,5,100,0,0,761,762,5,111,0,0,762,188,1,0,0,0,
		763,764,5,110,0,0,764,765,5,111,0,0,765,190,1,0,0,0,766,767,5,100,0,0,
		767,768,5,97,0,0,768,769,5,116,0,0,769,770,5,97,0,0,770,771,5,115,0,0,
		771,772,5,111,0,0,772,773,5,117,0,0,773,774,5,114,0,0,774,775,5,99,0,0,
		775,776,5,101,0,0,776,777,5,115,0,0,777,192,1,0,0,0,778,779,5,102,0,0,
		779,780,5,111,0,0,780,781,5,114,0,0,781,782,5,101,0,0,782,783,5,97,0,0,
		783,784,5,99,0,0,784,785,5,104,0,0,785,194,1,0,0,0,786,787,5,123,0,0,787,
		196,1,0,0,0,788,789,5,125,0,0,789,198,1,0,0,0,790,791,5,59,0,0,791,200,
		1,0,0,0,792,794,7,0,0,0,793,792,1,0,0,0,794,795,1,0,0,0,795,793,1,0,0,
		0,795,796,1,0,0,0,796,797,1,0,0,0,797,798,6,100,0,0,798,202,1,0,0,0,799,
		803,5,39,0,0,800,802,7,1,0,0,801,800,1,0,0,0,802,805,1,0,0,0,803,801,1,
		0,0,0,803,804,1,0,0,0,804,806,1,0,0,0,805,803,1,0,0,0,806,807,5,39,0,0,
		807,204,1,0,0,0,808,810,7,2,0,0,809,811,7,3,0,0,810,809,1,0,0,0,811,812,
		1,0,0,0,812,810,1,0,0,0,812,813,1,0,0,0,813,206,1,0,0,0,814,816,7,4,0,
		0,815,814,1,0,0,0,816,817,1,0,0,0,817,815,1,0,0,0,817,818,1,0,0,0,818,
		208,1,0,0,0,819,841,5,47,0,0,820,840,7,5,0,0,821,823,7,6,0,0,822,821,1,
		0,0,0,823,824,1,0,0,0,824,822,1,0,0,0,824,825,1,0,0,0,825,840,1,0,0,0,
		826,828,7,7,0,0,827,826,1,0,0,0,828,829,1,0,0,0,829,827,1,0,0,0,829,830,
		1,0,0,0,830,840,1,0,0,0,831,833,7,4,0,0,832,831,1,0,0,0,833,834,1,0,0,
		0,834,832,1,0,0,0,834,835,1,0,0,0,835,840,1,0,0,0,836,840,7,8,0,0,837,
		838,5,92,0,0,838,840,5,47,0,0,839,820,1,0,0,0,839,822,1,0,0,0,839,827,
		1,0,0,0,839,832,1,0,0,0,839,836,1,0,0,0,839,837,1,0,0,0,840,843,1,0,0,
		0,841,839,1,0,0,0,841,842,1,0,0,0,842,844,1,0,0,0,843,841,1,0,0,0,844,
		845,5,47,0,0,845,210,1,0,0,0,846,847,5,47,0,0,847,848,5,47,0,0,848,852,
		1,0,0,0,849,851,8,9,0,0,850,849,1,0,0,0,851,854,1,0,0,0,852,850,1,0,0,
		0,852,853,1,0,0,0,853,856,1,0,0,0,854,852,1,0,0,0,855,857,5,13,0,0,856,
		855,1,0,0,0,856,857,1,0,0,0,857,858,1,0,0,0,858,859,5,10,0,0,859,860,1,
		0,0,0,860,861,6,105,0,0,861,212,1,0,0,0,862,863,5,47,0,0,863,864,5,42,
		0,0,864,868,1,0,0,0,865,867,9,0,0,0,866,865,1,0,0,0,867,870,1,0,0,0,868,
		869,1,0,0,0,868,866,1,0,0,0,869,871,1,0,0,0,870,868,1,0,0,0,871,872,5,
		42,0,0,872,873,5,47,0,0,873,874,1,0,0,0,874,875,6,106,0,0,875,214,1,0,
		0,0,14,0,795,801,803,812,817,824,829,834,839,841,852,856,868,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace BlokScript.Parser
