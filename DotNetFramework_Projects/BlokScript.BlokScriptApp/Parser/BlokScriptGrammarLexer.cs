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
		T__94=95, T__95=96, T__96=97, T__97=98, T__98=99, T__99=100, T__100=101, 
		T__101=102, T__102=103, T__103=104, T__104=105, T__105=106, T__106=107, 
		STATEMENTEND=108, WS=109, STRINGLITERAL=110, VARID=111, INTLITERAL=112, 
		REGEXLITERAL=113, LINE_COMMENT=114, BLOCK_COMMENT=115;
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
		"T__97", "T__98", "T__99", "T__100", "T__101", "T__102", "T__103", "T__104", 
		"T__105", "T__106", "STATEMENTEND", "WS", "STRINGLITERAL", "VARID", "INTLITERAL", 
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
		null, "'pass'", "'create'", "'space'", "'('", "')'", "'copy'", "'to'", 
		"'update'", "'set'", "'delete'", "','", "'name'", "'='", "'server'", "'location'", 
		"'default'", "'content'", "'type'", "'spaces'", "'from'", "'select'", 
		"'*'", "'where'", "'in'", "'block'", "'{'", "'}'", "'datasource'", "'for'", 
		"'slug'", "'entry'", "'entries'", "'sync'", "'skip'", "'updates'", "'creates'", 
		"'value'", "'csv'", "'json'", "'url'", "'and'", "'or'", "'id'", "'!='", 
		"'not'", "'matches'", "'does'", "'match'", "'regex'", "'like'", "'starts'", 
		"'start'", "'with'", "'ends'", "'end'", "'login'", "'global'", "'username'", 
		"'password'", "'token'", "'var'", "'string'", "'story'", "'print'", "'symbol'", 
		"'tables'", "'local'", "'cache'", "'file'", "'on'", "'blocks'", "'technical'", 
		"'display'", "'nestable'", "'universal'", "'add'", "'tag'", "'remove'", 
		"'preview'", "'field'", "'template'", "'screenshot'", "'+'", "'-'", "'%'", 
		"'be'", "'quiet'", "'verbose'", "'debugger'", "'wait'", "'compare'", "'all'", 
		"'stories'", "'publish'", "'unpublish'", "'any'", "'tags'", "'do'", "'no'", 
		"'datasources'", "'include'", "'['", "']'", "'directory'", "'foreach'", 
		"'int'", "'datasource entry'", "';'"
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
		null, null, null, null, null, null, null, null, null, null, null, null, 
		"STATEMENTEND", "WS", "STRINGLITERAL", "VARID", "INTLITERAL", "REGEXLITERAL", 
		"LINE_COMMENT", "BLOCK_COMMENT"
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
		4,0,115,944,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
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
		104,2,105,7,105,2,106,7,106,2,107,7,107,2,108,7,108,2,109,7,109,2,110,
		7,110,2,111,7,111,2,112,7,112,2,113,7,113,2,114,7,114,1,0,1,0,1,0,1,0,
		1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,2,1,2,1,2,1,2,1,2,1,3,1,3,1,4,1,
		4,1,5,1,5,1,5,1,5,1,5,1,6,1,6,1,6,1,7,1,7,1,7,1,7,1,7,1,7,1,7,1,8,1,8,
		1,8,1,8,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,10,1,10,1,11,1,11,1,11,1,11,1,11,
		1,12,1,12,1,13,1,13,1,13,1,13,1,13,1,13,1,13,1,14,1,14,1,14,1,14,1,14,
		1,14,1,14,1,14,1,14,1,15,1,15,1,15,1,15,1,15,1,15,1,15,1,15,1,16,1,16,
		1,16,1,16,1,16,1,16,1,16,1,16,1,17,1,17,1,17,1,17,1,17,1,18,1,18,1,18,
		1,18,1,18,1,18,1,18,1,19,1,19,1,19,1,19,1,19,1,20,1,20,1,20,1,20,1,20,
		1,20,1,20,1,21,1,21,1,22,1,22,1,22,1,22,1,22,1,22,1,23,1,23,1,23,1,24,
		1,24,1,24,1,24,1,24,1,24,1,25,1,25,1,26,1,26,1,27,1,27,1,27,1,27,1,27,
		1,27,1,27,1,27,1,27,1,27,1,27,1,28,1,28,1,28,1,28,1,29,1,29,1,29,1,29,
		1,29,1,30,1,30,1,30,1,30,1,30,1,30,1,31,1,31,1,31,1,31,1,31,1,31,1,31,
		1,31,1,32,1,32,1,32,1,32,1,32,1,33,1,33,1,33,1,33,1,33,1,34,1,34,1,34,
		1,34,1,34,1,34,1,34,1,34,1,35,1,35,1,35,1,35,1,35,1,35,1,35,1,35,1,36,
		1,36,1,36,1,36,1,36,1,36,1,37,1,37,1,37,1,37,1,38,1,38,1,38,1,38,1,38,
		1,39,1,39,1,39,1,39,1,40,1,40,1,40,1,40,1,41,1,41,1,41,1,42,1,42,1,42,
		1,43,1,43,1,43,1,44,1,44,1,44,1,44,1,45,1,45,1,45,1,45,1,45,1,45,1,45,
		1,45,1,46,1,46,1,46,1,46,1,46,1,47,1,47,1,47,1,47,1,47,1,47,1,48,1,48,
		1,48,1,48,1,48,1,48,1,49,1,49,1,49,1,49,1,49,1,50,1,50,1,50,1,50,1,50,
		1,50,1,50,1,51,1,51,1,51,1,51,1,51,1,51,1,52,1,52,1,52,1,52,1,52,1,53,
		1,53,1,53,1,53,1,53,1,54,1,54,1,54,1,54,1,55,1,55,1,55,1,55,1,55,1,55,
		1,56,1,56,1,56,1,56,1,56,1,56,1,56,1,57,1,57,1,57,1,57,1,57,1,57,1,57,
		1,57,1,57,1,58,1,58,1,58,1,58,1,58,1,58,1,58,1,58,1,58,1,59,1,59,1,59,
		1,59,1,59,1,59,1,60,1,60,1,60,1,60,1,61,1,61,1,61,1,61,1,61,1,61,1,61,
		1,62,1,62,1,62,1,62,1,62,1,62,1,63,1,63,1,63,1,63,1,63,1,63,1,64,1,64,
		1,64,1,64,1,64,1,64,1,64,1,65,1,65,1,65,1,65,1,65,1,65,1,65,1,66,1,66,
		1,66,1,66,1,66,1,66,1,67,1,67,1,67,1,67,1,67,1,67,1,68,1,68,1,68,1,68,
		1,68,1,69,1,69,1,69,1,70,1,70,1,70,1,70,1,70,1,70,1,70,1,71,1,71,1,71,
		1,71,1,71,1,71,1,71,1,71,1,71,1,71,1,72,1,72,1,72,1,72,1,72,1,72,1,72,
		1,72,1,73,1,73,1,73,1,73,1,73,1,73,1,73,1,73,1,73,1,74,1,74,1,74,1,74,
		1,74,1,74,1,74,1,74,1,74,1,74,1,75,1,75,1,75,1,75,1,76,1,76,1,76,1,76,
		1,77,1,77,1,77,1,77,1,77,1,77,1,77,1,78,1,78,1,78,1,78,1,78,1,78,1,78,
		1,78,1,79,1,79,1,79,1,79,1,79,1,79,1,80,1,80,1,80,1,80,1,80,1,80,1,80,
		1,80,1,80,1,81,1,81,1,81,1,81,1,81,1,81,1,81,1,81,1,81,1,81,1,81,1,82,
		1,82,1,83,1,83,1,84,1,84,1,85,1,85,1,85,1,86,1,86,1,86,1,86,1,86,1,86,
		1,87,1,87,1,87,1,87,1,87,1,87,1,87,1,87,1,88,1,88,1,88,1,88,1,88,1,88,
		1,88,1,88,1,88,1,89,1,89,1,89,1,89,1,89,1,90,1,90,1,90,1,90,1,90,1,90,
		1,90,1,90,1,91,1,91,1,91,1,91,1,92,1,92,1,92,1,92,1,92,1,92,1,92,1,92,
		1,93,1,93,1,93,1,93,1,93,1,93,1,93,1,93,1,94,1,94,1,94,1,94,1,94,1,94,
		1,94,1,94,1,94,1,94,1,95,1,95,1,95,1,95,1,96,1,96,1,96,1,96,1,96,1,97,
		1,97,1,97,1,98,1,98,1,98,1,99,1,99,1,99,1,99,1,99,1,99,1,99,1,99,1,99,
		1,99,1,99,1,99,1,100,1,100,1,100,1,100,1,100,1,100,1,100,1,100,1,101,1,
		101,1,102,1,102,1,103,1,103,1,103,1,103,1,103,1,103,1,103,1,103,1,103,
		1,103,1,104,1,104,1,104,1,104,1,104,1,104,1,104,1,104,1,105,1,105,1,105,
		1,105,1,106,1,106,1,106,1,106,1,106,1,106,1,106,1,106,1,106,1,106,1,106,
		1,106,1,106,1,106,1,106,1,106,1,106,1,107,1,107,1,108,4,108,862,8,108,
		11,108,12,108,863,1,108,1,108,1,109,1,109,5,109,870,8,109,10,109,12,109,
		873,9,109,1,109,1,109,1,110,1,110,4,110,879,8,110,11,110,12,110,880,1,
		111,4,111,884,8,111,11,111,12,111,885,1,112,1,112,1,112,4,112,891,8,112,
		11,112,12,112,892,1,112,4,112,896,8,112,11,112,12,112,897,1,112,4,112,
		901,8,112,11,112,12,112,902,1,112,1,112,1,112,5,112,908,8,112,10,112,12,
		112,911,9,112,1,112,1,112,1,113,1,113,1,113,1,113,5,113,919,8,113,10,113,
		12,113,922,9,113,1,113,3,113,925,8,113,1,113,1,113,1,113,1,113,1,114,1,
		114,1,114,1,114,5,114,935,8,114,10,114,12,114,938,9,114,1,114,1,114,1,
		114,1,114,1,114,1,936,0,115,1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,9,19,
		10,21,11,23,12,25,13,27,14,29,15,31,16,33,17,35,18,37,19,39,20,41,21,43,
		22,45,23,47,24,49,25,51,26,53,27,55,28,57,29,59,30,61,31,63,32,65,33,67,
		34,69,35,71,36,73,37,75,38,77,39,79,40,81,41,83,42,85,43,87,44,89,45,91,
		46,93,47,95,48,97,49,99,50,101,51,103,52,105,53,107,54,109,55,111,56,113,
		57,115,58,117,59,119,60,121,61,123,62,125,63,127,64,129,65,131,66,133,
		67,135,68,137,69,139,70,141,71,143,72,145,73,147,74,149,75,151,76,153,
		77,155,78,157,79,159,80,161,81,163,82,165,83,167,84,169,85,171,86,173,
		87,175,88,177,89,179,90,181,91,183,92,185,93,187,94,189,95,191,96,193,
		97,195,98,197,99,199,100,201,101,203,102,205,103,207,104,209,105,211,106,
		213,107,215,108,217,109,219,110,221,111,223,112,225,113,227,114,229,115,
		1,0,10,3,0,9,10,13,13,32,32,7,0,32,32,35,35,37,37,45,57,65,90,95,95,97,
		122,2,0,65,90,97,122,4,0,48,57,65,90,95,95,97,122,1,0,48,57,4,0,36,36,
		42,42,91,91,93,94,1,0,97,122,1,0,65,90,3,0,40,41,43,43,45,46,2,0,10,10,
		13,13,959,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,
		0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,
		1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,
		0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,
		1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,
		0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,
		1,0,0,0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,1,0,0,0,0,73,1,0,0,0,0,75,1,0,0,
		0,0,77,1,0,0,0,0,79,1,0,0,0,0,81,1,0,0,0,0,83,1,0,0,0,0,85,1,0,0,0,0,87,
		1,0,0,0,0,89,1,0,0,0,0,91,1,0,0,0,0,93,1,0,0,0,0,95,1,0,0,0,0,97,1,0,0,
		0,0,99,1,0,0,0,0,101,1,0,0,0,0,103,1,0,0,0,0,105,1,0,0,0,0,107,1,0,0,0,
		0,109,1,0,0,0,0,111,1,0,0,0,0,113,1,0,0,0,0,115,1,0,0,0,0,117,1,0,0,0,
		0,119,1,0,0,0,0,121,1,0,0,0,0,123,1,0,0,0,0,125,1,0,0,0,0,127,1,0,0,0,
		0,129,1,0,0,0,0,131,1,0,0,0,0,133,1,0,0,0,0,135,1,0,0,0,0,137,1,0,0,0,
		0,139,1,0,0,0,0,141,1,0,0,0,0,143,1,0,0,0,0,145,1,0,0,0,0,147,1,0,0,0,
		0,149,1,0,0,0,0,151,1,0,0,0,0,153,1,0,0,0,0,155,1,0,0,0,0,157,1,0,0,0,
		0,159,1,0,0,0,0,161,1,0,0,0,0,163,1,0,0,0,0,165,1,0,0,0,0,167,1,0,0,0,
		0,169,1,0,0,0,0,171,1,0,0,0,0,173,1,0,0,0,0,175,1,0,0,0,0,177,1,0,0,0,
		0,179,1,0,0,0,0,181,1,0,0,0,0,183,1,0,0,0,0,185,1,0,0,0,0,187,1,0,0,0,
		0,189,1,0,0,0,0,191,1,0,0,0,0,193,1,0,0,0,0,195,1,0,0,0,0,197,1,0,0,0,
		0,199,1,0,0,0,0,201,1,0,0,0,0,203,1,0,0,0,0,205,1,0,0,0,0,207,1,0,0,0,
		0,209,1,0,0,0,0,211,1,0,0,0,0,213,1,0,0,0,0,215,1,0,0,0,0,217,1,0,0,0,
		0,219,1,0,0,0,0,221,1,0,0,0,0,223,1,0,0,0,0,225,1,0,0,0,0,227,1,0,0,0,
		0,229,1,0,0,0,1,231,1,0,0,0,3,236,1,0,0,0,5,243,1,0,0,0,7,249,1,0,0,0,
		9,251,1,0,0,0,11,253,1,0,0,0,13,258,1,0,0,0,15,261,1,0,0,0,17,268,1,0,
		0,0,19,272,1,0,0,0,21,279,1,0,0,0,23,281,1,0,0,0,25,286,1,0,0,0,27,288,
		1,0,0,0,29,295,1,0,0,0,31,304,1,0,0,0,33,312,1,0,0,0,35,320,1,0,0,0,37,
		325,1,0,0,0,39,332,1,0,0,0,41,337,1,0,0,0,43,344,1,0,0,0,45,346,1,0,0,
		0,47,352,1,0,0,0,49,355,1,0,0,0,51,361,1,0,0,0,53,363,1,0,0,0,55,365,1,
		0,0,0,57,376,1,0,0,0,59,380,1,0,0,0,61,385,1,0,0,0,63,391,1,0,0,0,65,399,
		1,0,0,0,67,404,1,0,0,0,69,409,1,0,0,0,71,417,1,0,0,0,73,425,1,0,0,0,75,
		431,1,0,0,0,77,435,1,0,0,0,79,440,1,0,0,0,81,444,1,0,0,0,83,448,1,0,0,
		0,85,451,1,0,0,0,87,454,1,0,0,0,89,457,1,0,0,0,91,461,1,0,0,0,93,469,1,
		0,0,0,95,474,1,0,0,0,97,480,1,0,0,0,99,486,1,0,0,0,101,491,1,0,0,0,103,
		498,1,0,0,0,105,504,1,0,0,0,107,509,1,0,0,0,109,514,1,0,0,0,111,518,1,
		0,0,0,113,524,1,0,0,0,115,531,1,0,0,0,117,540,1,0,0,0,119,549,1,0,0,0,
		121,555,1,0,0,0,123,559,1,0,0,0,125,566,1,0,0,0,127,572,1,0,0,0,129,578,
		1,0,0,0,131,585,1,0,0,0,133,592,1,0,0,0,135,598,1,0,0,0,137,604,1,0,0,
		0,139,609,1,0,0,0,141,612,1,0,0,0,143,619,1,0,0,0,145,629,1,0,0,0,147,
		637,1,0,0,0,149,646,1,0,0,0,151,656,1,0,0,0,153,660,1,0,0,0,155,664,1,
		0,0,0,157,671,1,0,0,0,159,679,1,0,0,0,161,685,1,0,0,0,163,694,1,0,0,0,
		165,705,1,0,0,0,167,707,1,0,0,0,169,709,1,0,0,0,171,711,1,0,0,0,173,714,
		1,0,0,0,175,720,1,0,0,0,177,728,1,0,0,0,179,737,1,0,0,0,181,742,1,0,0,
		0,183,750,1,0,0,0,185,754,1,0,0,0,187,762,1,0,0,0,189,770,1,0,0,0,191,
		780,1,0,0,0,193,784,1,0,0,0,195,789,1,0,0,0,197,792,1,0,0,0,199,795,1,
		0,0,0,201,807,1,0,0,0,203,815,1,0,0,0,205,817,1,0,0,0,207,819,1,0,0,0,
		209,829,1,0,0,0,211,837,1,0,0,0,213,841,1,0,0,0,215,858,1,0,0,0,217,861,
		1,0,0,0,219,867,1,0,0,0,221,876,1,0,0,0,223,883,1,0,0,0,225,887,1,0,0,
		0,227,914,1,0,0,0,229,930,1,0,0,0,231,232,5,112,0,0,232,233,5,97,0,0,233,
		234,5,115,0,0,234,235,5,115,0,0,235,2,1,0,0,0,236,237,5,99,0,0,237,238,
		5,114,0,0,238,239,5,101,0,0,239,240,5,97,0,0,240,241,5,116,0,0,241,242,
		5,101,0,0,242,4,1,0,0,0,243,244,5,115,0,0,244,245,5,112,0,0,245,246,5,
		97,0,0,246,247,5,99,0,0,247,248,5,101,0,0,248,6,1,0,0,0,249,250,5,40,0,
		0,250,8,1,0,0,0,251,252,5,41,0,0,252,10,1,0,0,0,253,254,5,99,0,0,254,255,
		5,111,0,0,255,256,5,112,0,0,256,257,5,121,0,0,257,12,1,0,0,0,258,259,5,
		116,0,0,259,260,5,111,0,0,260,14,1,0,0,0,261,262,5,117,0,0,262,263,5,112,
		0,0,263,264,5,100,0,0,264,265,5,97,0,0,265,266,5,116,0,0,266,267,5,101,
		0,0,267,16,1,0,0,0,268,269,5,115,0,0,269,270,5,101,0,0,270,271,5,116,0,
		0,271,18,1,0,0,0,272,273,5,100,0,0,273,274,5,101,0,0,274,275,5,108,0,0,
		275,276,5,101,0,0,276,277,5,116,0,0,277,278,5,101,0,0,278,20,1,0,0,0,279,
		280,5,44,0,0,280,22,1,0,0,0,281,282,5,110,0,0,282,283,5,97,0,0,283,284,
		5,109,0,0,284,285,5,101,0,0,285,24,1,0,0,0,286,287,5,61,0,0,287,26,1,0,
		0,0,288,289,5,115,0,0,289,290,5,101,0,0,290,291,5,114,0,0,291,292,5,118,
		0,0,292,293,5,101,0,0,293,294,5,114,0,0,294,28,1,0,0,0,295,296,5,108,0,
		0,296,297,5,111,0,0,297,298,5,99,0,0,298,299,5,97,0,0,299,300,5,116,0,
		0,300,301,5,105,0,0,301,302,5,111,0,0,302,303,5,110,0,0,303,30,1,0,0,0,
		304,305,5,100,0,0,305,306,5,101,0,0,306,307,5,102,0,0,307,308,5,97,0,0,
		308,309,5,117,0,0,309,310,5,108,0,0,310,311,5,116,0,0,311,32,1,0,0,0,312,
		313,5,99,0,0,313,314,5,111,0,0,314,315,5,110,0,0,315,316,5,116,0,0,316,
		317,5,101,0,0,317,318,5,110,0,0,318,319,5,116,0,0,319,34,1,0,0,0,320,321,
		5,116,0,0,321,322,5,121,0,0,322,323,5,112,0,0,323,324,5,101,0,0,324,36,
		1,0,0,0,325,326,5,115,0,0,326,327,5,112,0,0,327,328,5,97,0,0,328,329,5,
		99,0,0,329,330,5,101,0,0,330,331,5,115,0,0,331,38,1,0,0,0,332,333,5,102,
		0,0,333,334,5,114,0,0,334,335,5,111,0,0,335,336,5,109,0,0,336,40,1,0,0,
		0,337,338,5,115,0,0,338,339,5,101,0,0,339,340,5,108,0,0,340,341,5,101,
		0,0,341,342,5,99,0,0,342,343,5,116,0,0,343,42,1,0,0,0,344,345,5,42,0,0,
		345,44,1,0,0,0,346,347,5,119,0,0,347,348,5,104,0,0,348,349,5,101,0,0,349,
		350,5,114,0,0,350,351,5,101,0,0,351,46,1,0,0,0,352,353,5,105,0,0,353,354,
		5,110,0,0,354,48,1,0,0,0,355,356,5,98,0,0,356,357,5,108,0,0,357,358,5,
		111,0,0,358,359,5,99,0,0,359,360,5,107,0,0,360,50,1,0,0,0,361,362,5,123,
		0,0,362,52,1,0,0,0,363,364,5,125,0,0,364,54,1,0,0,0,365,366,5,100,0,0,
		366,367,5,97,0,0,367,368,5,116,0,0,368,369,5,97,0,0,369,370,5,115,0,0,
		370,371,5,111,0,0,371,372,5,117,0,0,372,373,5,114,0,0,373,374,5,99,0,0,
		374,375,5,101,0,0,375,56,1,0,0,0,376,377,5,102,0,0,377,378,5,111,0,0,378,
		379,5,114,0,0,379,58,1,0,0,0,380,381,5,115,0,0,381,382,5,108,0,0,382,383,
		5,117,0,0,383,384,5,103,0,0,384,60,1,0,0,0,385,386,5,101,0,0,386,387,5,
		110,0,0,387,388,5,116,0,0,388,389,5,114,0,0,389,390,5,121,0,0,390,62,1,
		0,0,0,391,392,5,101,0,0,392,393,5,110,0,0,393,394,5,116,0,0,394,395,5,
		114,0,0,395,396,5,105,0,0,396,397,5,101,0,0,397,398,5,115,0,0,398,64,1,
		0,0,0,399,400,5,115,0,0,400,401,5,121,0,0,401,402,5,110,0,0,402,403,5,
		99,0,0,403,66,1,0,0,0,404,405,5,115,0,0,405,406,5,107,0,0,406,407,5,105,
		0,0,407,408,5,112,0,0,408,68,1,0,0,0,409,410,5,117,0,0,410,411,5,112,0,
		0,411,412,5,100,0,0,412,413,5,97,0,0,413,414,5,116,0,0,414,415,5,101,0,
		0,415,416,5,115,0,0,416,70,1,0,0,0,417,418,5,99,0,0,418,419,5,114,0,0,
		419,420,5,101,0,0,420,421,5,97,0,0,421,422,5,116,0,0,422,423,5,101,0,0,
		423,424,5,115,0,0,424,72,1,0,0,0,425,426,5,118,0,0,426,427,5,97,0,0,427,
		428,5,108,0,0,428,429,5,117,0,0,429,430,5,101,0,0,430,74,1,0,0,0,431,432,
		5,99,0,0,432,433,5,115,0,0,433,434,5,118,0,0,434,76,1,0,0,0,435,436,5,
		106,0,0,436,437,5,115,0,0,437,438,5,111,0,0,438,439,5,110,0,0,439,78,1,
		0,0,0,440,441,5,117,0,0,441,442,5,114,0,0,442,443,5,108,0,0,443,80,1,0,
		0,0,444,445,5,97,0,0,445,446,5,110,0,0,446,447,5,100,0,0,447,82,1,0,0,
		0,448,449,5,111,0,0,449,450,5,114,0,0,450,84,1,0,0,0,451,452,5,105,0,0,
		452,453,5,100,0,0,453,86,1,0,0,0,454,455,5,33,0,0,455,456,5,61,0,0,456,
		88,1,0,0,0,457,458,5,110,0,0,458,459,5,111,0,0,459,460,5,116,0,0,460,90,
		1,0,0,0,461,462,5,109,0,0,462,463,5,97,0,0,463,464,5,116,0,0,464,465,5,
		99,0,0,465,466,5,104,0,0,466,467,5,101,0,0,467,468,5,115,0,0,468,92,1,
		0,0,0,469,470,5,100,0,0,470,471,5,111,0,0,471,472,5,101,0,0,472,473,5,
		115,0,0,473,94,1,0,0,0,474,475,5,109,0,0,475,476,5,97,0,0,476,477,5,116,
		0,0,477,478,5,99,0,0,478,479,5,104,0,0,479,96,1,0,0,0,480,481,5,114,0,
		0,481,482,5,101,0,0,482,483,5,103,0,0,483,484,5,101,0,0,484,485,5,120,
		0,0,485,98,1,0,0,0,486,487,5,108,0,0,487,488,5,105,0,0,488,489,5,107,0,
		0,489,490,5,101,0,0,490,100,1,0,0,0,491,492,5,115,0,0,492,493,5,116,0,
		0,493,494,5,97,0,0,494,495,5,114,0,0,495,496,5,116,0,0,496,497,5,115,0,
		0,497,102,1,0,0,0,498,499,5,115,0,0,499,500,5,116,0,0,500,501,5,97,0,0,
		501,502,5,114,0,0,502,503,5,116,0,0,503,104,1,0,0,0,504,505,5,119,0,0,
		505,506,5,105,0,0,506,507,5,116,0,0,507,508,5,104,0,0,508,106,1,0,0,0,
		509,510,5,101,0,0,510,511,5,110,0,0,511,512,5,100,0,0,512,513,5,115,0,
		0,513,108,1,0,0,0,514,515,5,101,0,0,515,516,5,110,0,0,516,517,5,100,0,
		0,517,110,1,0,0,0,518,519,5,108,0,0,519,520,5,111,0,0,520,521,5,103,0,
		0,521,522,5,105,0,0,522,523,5,110,0,0,523,112,1,0,0,0,524,525,5,103,0,
		0,525,526,5,108,0,0,526,527,5,111,0,0,527,528,5,98,0,0,528,529,5,97,0,
		0,529,530,5,108,0,0,530,114,1,0,0,0,531,532,5,117,0,0,532,533,5,115,0,
		0,533,534,5,101,0,0,534,535,5,114,0,0,535,536,5,110,0,0,536,537,5,97,0,
		0,537,538,5,109,0,0,538,539,5,101,0,0,539,116,1,0,0,0,540,541,5,112,0,
		0,541,542,5,97,0,0,542,543,5,115,0,0,543,544,5,115,0,0,544,545,5,119,0,
		0,545,546,5,111,0,0,546,547,5,114,0,0,547,548,5,100,0,0,548,118,1,0,0,
		0,549,550,5,116,0,0,550,551,5,111,0,0,551,552,5,107,0,0,552,553,5,101,
		0,0,553,554,5,110,0,0,554,120,1,0,0,0,555,556,5,118,0,0,556,557,5,97,0,
		0,557,558,5,114,0,0,558,122,1,0,0,0,559,560,5,115,0,0,560,561,5,116,0,
		0,561,562,5,114,0,0,562,563,5,105,0,0,563,564,5,110,0,0,564,565,5,103,
		0,0,565,124,1,0,0,0,566,567,5,115,0,0,567,568,5,116,0,0,568,569,5,111,
		0,0,569,570,5,114,0,0,570,571,5,121,0,0,571,126,1,0,0,0,572,573,5,112,
		0,0,573,574,5,114,0,0,574,575,5,105,0,0,575,576,5,110,0,0,576,577,5,116,
		0,0,577,128,1,0,0,0,578,579,5,115,0,0,579,580,5,121,0,0,580,581,5,109,
		0,0,581,582,5,98,0,0,582,583,5,111,0,0,583,584,5,108,0,0,584,130,1,0,0,
		0,585,586,5,116,0,0,586,587,5,97,0,0,587,588,5,98,0,0,588,589,5,108,0,
		0,589,590,5,101,0,0,590,591,5,115,0,0,591,132,1,0,0,0,592,593,5,108,0,
		0,593,594,5,111,0,0,594,595,5,99,0,0,595,596,5,97,0,0,596,597,5,108,0,
		0,597,134,1,0,0,0,598,599,5,99,0,0,599,600,5,97,0,0,600,601,5,99,0,0,601,
		602,5,104,0,0,602,603,5,101,0,0,603,136,1,0,0,0,604,605,5,102,0,0,605,
		606,5,105,0,0,606,607,5,108,0,0,607,608,5,101,0,0,608,138,1,0,0,0,609,
		610,5,111,0,0,610,611,5,110,0,0,611,140,1,0,0,0,612,613,5,98,0,0,613,614,
		5,108,0,0,614,615,5,111,0,0,615,616,5,99,0,0,616,617,5,107,0,0,617,618,
		5,115,0,0,618,142,1,0,0,0,619,620,5,116,0,0,620,621,5,101,0,0,621,622,
		5,99,0,0,622,623,5,104,0,0,623,624,5,110,0,0,624,625,5,105,0,0,625,626,
		5,99,0,0,626,627,5,97,0,0,627,628,5,108,0,0,628,144,1,0,0,0,629,630,5,
		100,0,0,630,631,5,105,0,0,631,632,5,115,0,0,632,633,5,112,0,0,633,634,
		5,108,0,0,634,635,5,97,0,0,635,636,5,121,0,0,636,146,1,0,0,0,637,638,5,
		110,0,0,638,639,5,101,0,0,639,640,5,115,0,0,640,641,5,116,0,0,641,642,
		5,97,0,0,642,643,5,98,0,0,643,644,5,108,0,0,644,645,5,101,0,0,645,148,
		1,0,0,0,646,647,5,117,0,0,647,648,5,110,0,0,648,649,5,105,0,0,649,650,
		5,118,0,0,650,651,5,101,0,0,651,652,5,114,0,0,652,653,5,115,0,0,653,654,
		5,97,0,0,654,655,5,108,0,0,655,150,1,0,0,0,656,657,5,97,0,0,657,658,5,
		100,0,0,658,659,5,100,0,0,659,152,1,0,0,0,660,661,5,116,0,0,661,662,5,
		97,0,0,662,663,5,103,0,0,663,154,1,0,0,0,664,665,5,114,0,0,665,666,5,101,
		0,0,666,667,5,109,0,0,667,668,5,111,0,0,668,669,5,118,0,0,669,670,5,101,
		0,0,670,156,1,0,0,0,671,672,5,112,0,0,672,673,5,114,0,0,673,674,5,101,
		0,0,674,675,5,118,0,0,675,676,5,105,0,0,676,677,5,101,0,0,677,678,5,119,
		0,0,678,158,1,0,0,0,679,680,5,102,0,0,680,681,5,105,0,0,681,682,5,101,
		0,0,682,683,5,108,0,0,683,684,5,100,0,0,684,160,1,0,0,0,685,686,5,116,
		0,0,686,687,5,101,0,0,687,688,5,109,0,0,688,689,5,112,0,0,689,690,5,108,
		0,0,690,691,5,97,0,0,691,692,5,116,0,0,692,693,5,101,0,0,693,162,1,0,0,
		0,694,695,5,115,0,0,695,696,5,99,0,0,696,697,5,114,0,0,697,698,5,101,0,
		0,698,699,5,101,0,0,699,700,5,110,0,0,700,701,5,115,0,0,701,702,5,104,
		0,0,702,703,5,111,0,0,703,704,5,116,0,0,704,164,1,0,0,0,705,706,5,43,0,
		0,706,166,1,0,0,0,707,708,5,45,0,0,708,168,1,0,0,0,709,710,5,37,0,0,710,
		170,1,0,0,0,711,712,5,98,0,0,712,713,5,101,0,0,713,172,1,0,0,0,714,715,
		5,113,0,0,715,716,5,117,0,0,716,717,5,105,0,0,717,718,5,101,0,0,718,719,
		5,116,0,0,719,174,1,0,0,0,720,721,5,118,0,0,721,722,5,101,0,0,722,723,
		5,114,0,0,723,724,5,98,0,0,724,725,5,111,0,0,725,726,5,115,0,0,726,727,
		5,101,0,0,727,176,1,0,0,0,728,729,5,100,0,0,729,730,5,101,0,0,730,731,
		5,98,0,0,731,732,5,117,0,0,732,733,5,103,0,0,733,734,5,103,0,0,734,735,
		5,101,0,0,735,736,5,114,0,0,736,178,1,0,0,0,737,738,5,119,0,0,738,739,
		5,97,0,0,739,740,5,105,0,0,740,741,5,116,0,0,741,180,1,0,0,0,742,743,5,
		99,0,0,743,744,5,111,0,0,744,745,5,109,0,0,745,746,5,112,0,0,746,747,5,
		97,0,0,747,748,5,114,0,0,748,749,5,101,0,0,749,182,1,0,0,0,750,751,5,97,
		0,0,751,752,5,108,0,0,752,753,5,108,0,0,753,184,1,0,0,0,754,755,5,115,
		0,0,755,756,5,116,0,0,756,757,5,111,0,0,757,758,5,114,0,0,758,759,5,105,
		0,0,759,760,5,101,0,0,760,761,5,115,0,0,761,186,1,0,0,0,762,763,5,112,
		0,0,763,764,5,117,0,0,764,765,5,98,0,0,765,766,5,108,0,0,766,767,5,105,
		0,0,767,768,5,115,0,0,768,769,5,104,0,0,769,188,1,0,0,0,770,771,5,117,
		0,0,771,772,5,110,0,0,772,773,5,112,0,0,773,774,5,117,0,0,774,775,5,98,
		0,0,775,776,5,108,0,0,776,777,5,105,0,0,777,778,5,115,0,0,778,779,5,104,
		0,0,779,190,1,0,0,0,780,781,5,97,0,0,781,782,5,110,0,0,782,783,5,121,0,
		0,783,192,1,0,0,0,784,785,5,116,0,0,785,786,5,97,0,0,786,787,5,103,0,0,
		787,788,5,115,0,0,788,194,1,0,0,0,789,790,5,100,0,0,790,791,5,111,0,0,
		791,196,1,0,0,0,792,793,5,110,0,0,793,794,5,111,0,0,794,198,1,0,0,0,795,
		796,5,100,0,0,796,797,5,97,0,0,797,798,5,116,0,0,798,799,5,97,0,0,799,
		800,5,115,0,0,800,801,5,111,0,0,801,802,5,117,0,0,802,803,5,114,0,0,803,
		804,5,99,0,0,804,805,5,101,0,0,805,806,5,115,0,0,806,200,1,0,0,0,807,808,
		5,105,0,0,808,809,5,110,0,0,809,810,5,99,0,0,810,811,5,108,0,0,811,812,
		5,117,0,0,812,813,5,100,0,0,813,814,5,101,0,0,814,202,1,0,0,0,815,816,
		5,91,0,0,816,204,1,0,0,0,817,818,5,93,0,0,818,206,1,0,0,0,819,820,5,100,
		0,0,820,821,5,105,0,0,821,822,5,114,0,0,822,823,5,101,0,0,823,824,5,99,
		0,0,824,825,5,116,0,0,825,826,5,111,0,0,826,827,5,114,0,0,827,828,5,121,
		0,0,828,208,1,0,0,0,829,830,5,102,0,0,830,831,5,111,0,0,831,832,5,114,
		0,0,832,833,5,101,0,0,833,834,5,97,0,0,834,835,5,99,0,0,835,836,5,104,
		0,0,836,210,1,0,0,0,837,838,5,105,0,0,838,839,5,110,0,0,839,840,5,116,
		0,0,840,212,1,0,0,0,841,842,5,100,0,0,842,843,5,97,0,0,843,844,5,116,0,
		0,844,845,5,97,0,0,845,846,5,115,0,0,846,847,5,111,0,0,847,848,5,117,0,
		0,848,849,5,114,0,0,849,850,5,99,0,0,850,851,5,101,0,0,851,852,5,32,0,
		0,852,853,5,101,0,0,853,854,5,110,0,0,854,855,5,116,0,0,855,856,5,114,
		0,0,856,857,5,121,0,0,857,214,1,0,0,0,858,859,5,59,0,0,859,216,1,0,0,0,
		860,862,7,0,0,0,861,860,1,0,0,0,862,863,1,0,0,0,863,861,1,0,0,0,863,864,
		1,0,0,0,864,865,1,0,0,0,865,866,6,108,0,0,866,218,1,0,0,0,867,871,5,39,
		0,0,868,870,7,1,0,0,869,868,1,0,0,0,870,873,1,0,0,0,871,869,1,0,0,0,871,
		872,1,0,0,0,872,874,1,0,0,0,873,871,1,0,0,0,874,875,5,39,0,0,875,220,1,
		0,0,0,876,878,7,2,0,0,877,879,7,3,0,0,878,877,1,0,0,0,879,880,1,0,0,0,
		880,878,1,0,0,0,880,881,1,0,0,0,881,222,1,0,0,0,882,884,7,4,0,0,883,882,
		1,0,0,0,884,885,1,0,0,0,885,883,1,0,0,0,885,886,1,0,0,0,886,224,1,0,0,
		0,887,909,5,47,0,0,888,908,7,5,0,0,889,891,7,6,0,0,890,889,1,0,0,0,891,
		892,1,0,0,0,892,890,1,0,0,0,892,893,1,0,0,0,893,908,1,0,0,0,894,896,7,
		7,0,0,895,894,1,0,0,0,896,897,1,0,0,0,897,895,1,0,0,0,897,898,1,0,0,0,
		898,908,1,0,0,0,899,901,7,4,0,0,900,899,1,0,0,0,901,902,1,0,0,0,902,900,
		1,0,0,0,902,903,1,0,0,0,903,908,1,0,0,0,904,908,7,8,0,0,905,906,5,92,0,
		0,906,908,5,47,0,0,907,888,1,0,0,0,907,890,1,0,0,0,907,895,1,0,0,0,907,
		900,1,0,0,0,907,904,1,0,0,0,907,905,1,0,0,0,908,911,1,0,0,0,909,907,1,
		0,0,0,909,910,1,0,0,0,910,912,1,0,0,0,911,909,1,0,0,0,912,913,5,47,0,0,
		913,226,1,0,0,0,914,915,5,47,0,0,915,916,5,47,0,0,916,920,1,0,0,0,917,
		919,8,9,0,0,918,917,1,0,0,0,919,922,1,0,0,0,920,918,1,0,0,0,920,921,1,
		0,0,0,921,924,1,0,0,0,922,920,1,0,0,0,923,925,5,13,0,0,924,923,1,0,0,0,
		924,925,1,0,0,0,925,926,1,0,0,0,926,927,5,10,0,0,927,928,1,0,0,0,928,929,
		6,113,0,0,929,228,1,0,0,0,930,931,5,47,0,0,931,932,5,42,0,0,932,936,1,
		0,0,0,933,935,9,0,0,0,934,933,1,0,0,0,935,938,1,0,0,0,936,937,1,0,0,0,
		936,934,1,0,0,0,937,939,1,0,0,0,938,936,1,0,0,0,939,940,5,42,0,0,940,941,
		5,47,0,0,941,942,1,0,0,0,942,943,6,114,0,0,943,230,1,0,0,0,14,0,863,869,
		871,880,885,892,897,902,907,909,920,924,936,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace BlokScript.Parser
