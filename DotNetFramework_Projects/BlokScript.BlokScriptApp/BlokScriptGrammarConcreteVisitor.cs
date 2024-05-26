using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using log4net;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json.Linq;

using BlokScript.Common;
using BlokScript.Entities;
using BlokScript.WebClients;
using BlokScript.Utils;
using BlokScript.Formatters;
using BlokScript.Parser;
using BlokScript.Parsers;
using BlokScript.Caches;
using BlokScript.PathFactories;
using BlokScript.ResponseReaders;
using BlokScript.Comparators;
using BlokScript.Models;
using BlokScript.FileReaders;
using BlokScript.Filters;
using BlokScript.Serializers;
using BlokScript.FileWriters;
using System.Data;
using System.Runtime.Remoting.Contexts;
using System.Net;
using BlokScript.EntityDataFactories;

namespace BlokScript.BlokScriptApp
{
	public class BlokScriptGrammarConcreteVisitor : BlokScriptGrammarBaseVisitor<object>
	{
		static BlokScriptGrammarConcreteVisitor ()
		{
			_Log = LogManager.GetLogger(typeof(BlokScriptGrammarConcreteVisitor));
		}

		public BlokScriptGrammarConcreteVisitor ()
		{
		}

		public override object VisitScript([NotNull] BlokScriptGrammarParser.ScriptContext context)
		{
			//
			// CREATE THE GLOBAL SYMBOL TABLE.
			//
			BlokScriptSymbolTable CreatedSymbolTable = new BlokScriptSymbolTable();

			{
				BlokScriptSymbol CurrentSymbol = new BlokScriptSymbol();
				CurrentSymbol.Name = "_GlobalManagementApiBaseUrl";
				CurrentSymbol.Type = BlokScriptSymbolType.String;
				CreatedSymbolTable[CurrentSymbol.Name] = CurrentSymbol;
			}

			{
				BlokScriptSymbol CurrentSymbol = new BlokScriptSymbol();
				CurrentSymbol.Name = "_GlobalContentDeliveryApiBaseUrl";
				CurrentSymbol.Type = BlokScriptSymbolType.String;
				CreatedSymbolTable[CurrentSymbol.Name] = CurrentSymbol;
			}

			{
				BlokScriptSymbol CurrentSymbol = new BlokScriptSymbol();
				CurrentSymbol.Name = "_GlobalPersonalAccessToken";
				CurrentSymbol.Type = BlokScriptSymbolType.String;
				CreatedSymbolTable[CurrentSymbol.Name] = CurrentSymbol;
			}

			{
				BlokScriptSymbol CurrentSymbol = new BlokScriptSymbol();
				CurrentSymbol.Name = "_GlobalVerbosity";
				CurrentSymbol.Type = BlokScriptSymbolType.Int32;
				CurrentSymbol.Value = (int)BlokScriptVerbosity.Verbose;
				CreatedSymbolTable[CurrentSymbol.Name] = CurrentSymbol;
			}

			_SymbolTableManager = new BlokScriptSymbolTableManager();
			_SymbolTableManager.PushSymbolTable(CreatedSymbolTable);

			//
			// CREATE THE LOCAL CACHE.
			//
			_SpaceCacheByIdDict = new Dictionary<string, SpaceCache>();
			_SpaceCacheByNameDict = new Dictionary<string, SpaceCache>();

			//
			// READ THE blokscript-env.json FILE.
			//
			string BlokScriptEnvFilePath = _WorkingDir + "\\blokscript-env.json";

			if (File.Exists(BlokScriptEnvFilePath))
			{
				BlokScriptGlobalEnv GlobalEnv = BlokScriptEnvFileReader.Read(BlokScriptEnvFilePath);

				if (GlobalEnv.ManagementApiBaseUrl != null)
					_SymbolTableManager.GetSymbol("_GlobalManagementApiBaseUrl").Value = GlobalEnv.ManagementApiBaseUrl;

				if (GlobalEnv.ContentDeliveryApiBaseUrl != null)
					_SymbolTableManager.GetSymbol("_GlobalContentDeliveryApiBaseUrl").Value = GlobalEnv.ContentDeliveryApiBaseUrl;

				if (GlobalEnv.PersonalAccessToken != null)
					_SymbolTableManager.GetSymbol("_GlobalPersonalAccessToken").Value = GlobalEnv.PersonalAccessToken;

				if (GlobalEnv.Verbosity != null)
					_SymbolTableManager.GetSymbol("_GlobalVerbosity").Value = (int)BlokScriptVerbosityParser.Parse(GlobalEnv.Verbosity);
			}

			return VisitChildren(context);
		}

		public override object VisitStatement([NotNull] BlokScriptGrammarParser.StatementContext context)
		{
			return VisitChildren(context);
		}

		public override object VisitLoginStatement([NotNull] BlokScriptGrammarParser.LoginStatementContext Context)
		{
			return VisitChildren(Context);
		}

		public override object VisitLoginOnlyStatement([NotNull] BlokScriptGrammarParser.LoginOnlyStatementContext Context)
		{

			return VisitChildren(Context);
		}

		public override object VisitLoginWithGlobalUserNameStatement([NotNull] BlokScriptGrammarParser.LoginWithGlobalUserNameStatementContext Context)
		{

			return VisitChildren(Context);
		}

		public override object VisitLoginWithGlobalPasswordStatement([NotNull] BlokScriptGrammarParser.LoginWithGlobalPasswordStatementContext Context)
		{
			return VisitChildren(Context);
		}

		public override object VisitLoginWithGlobalTokenStatement([NotNull] BlokScriptGrammarParser.LoginWithGlobalTokenStatementContext Context)
		{
			return VisitChildren(Context);
		}

		public override object VisitLoginWithGlobalUserNameAndPasswordStatement([NotNull] BlokScriptGrammarParser.LoginWithGlobalUserNameAndPasswordStatementContext Context)
		{
			return VisitChildren(Context);
		}

		public override object VisitVarStatement ([NotNull] BlokScriptGrammarParser.VarStatementContext Context)
		{
			/*
			varStatement: spaceVarStatement
				| blockVarStatement
				| stringVarStatement
				| regexVarStatement
				| storyVarStatement
				| datasourceEntryVarStatement
				| 'var' VARID '=' (VARID | spaceSpec | blockSpec | stringExpr | regexExpr | storySpec | intExpr | datasourceEntrySpec | datasourceSpec)
				;
			*/
			if (Context.spaceVarStatement() != null)
				VisitSpaceVarStatement(Context.spaceVarStatement());
			else if (Context.blockVarStatement() != null)
				VisitBlockVarStatement(Context.blockVarStatement());
			else if (Context.stringVarStatement() != null)
				VisitStringVarStatement(Context.stringVarStatement());
			else if (Context.regexVarStatement() != null)
				VisitRegexVarStatement(Context.regexVarStatement());
			else if (Context.storyVarStatement() != null)
				VisitStoryVarStatement(Context.storyVarStatement());
			else if (Context.datasourceEntryVarStatement() != null)
				VisitDatasourceEntryVarStatement(Context.datasourceEntryVarStatement());
			else if (Context.VARID() != null)
			{
				BlokScriptSymbol Symbol = new BlokScriptSymbol();
				Symbol.Name = Context.VARID()[0].GetText();

				if (Context.VARID().Length > 1)
				{
					string RightSideSymbolName = Context.VARID()[1].GetText();
					BlokScriptSymbol RightSideSymbol = _SymbolTableManager.GetSymbol(RightSideSymbolName);

					if (RightSideSymbol == null)
					{
						string ErrorMessage = $"Variable '{RightSideSymbolName}' not found.";
						EchoError(ErrorMessage);
						throw new SymbolNotFoundException(ErrorMessage);
					}

					Symbol.Type = RightSideSymbol.Type;
					Symbol.Value = RightSideSymbol.Value;
				}
				else if (Context.spaceSpec() != null)
				{
					Symbol.Type = BlokScriptSymbolType.Space;
					Symbol.Value = VisitSpaceSpec(Context.spaceSpec());
				}
				else if (Context.blockSpec() != null)
				{
					Symbol.Type = BlokScriptSymbolType.Block;
					Symbol.Value = VisitBlockSpec(Context.blockSpec());
				}
				else if (Context.stringExpr() != null)
				{
					Symbol.Type = BlokScriptSymbolType.String;
					Symbol.Value = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.regexExpr() != null)
				{
					Symbol.Type = BlokScriptSymbolType.Regex;
					Symbol.Value = VisitRegexExpr(Context.regexExpr());
				}
				else if (Context.storySpec() != null)
				{
					Symbol.Type = BlokScriptSymbolType.Story;
					Symbol.Value = VisitStorySpec(Context.storySpec());
				}
				else if (Context.intExpr() != null)
				{
					Symbol.Type = BlokScriptSymbolType.Int32;
					Symbol.Value = VisitIntExpr(Context.intExpr());
				}
				else if (Context.datasourceEntrySpec() != null)
				{
					Symbol.Type = BlokScriptSymbolType.DatasourceEntry;
					Symbol.Value = VisitDatasourceEntrySpec(Context.datasourceEntrySpec());
				}
				else if (Context.datasourceSpec() != null)
				{
					Symbol.Type = BlokScriptSymbolType.Datasource;
					Symbol.Value = VisitDatasourceSpec(Context.datasourceSpec());
				}
				else
					throw new NotImplementedException("VisitVarStatement");

				_SymbolTableManager.AddSymbol(Symbol);
			}

			return null;
		}

		public override object VisitSpaceVarStatement ([NotNull] BlokScriptGrammarParser.SpaceVarStatementContext Context)
		{
			/*
			spaceVarStatement: 'space' VARID ('=' spaceSpec)?;
			*/
			string SymbolName = Context.VARID().GetText();

			SpaceEntity Space = null;

			if (Context.spaceSpec() != null)
			{
				Space = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
			}

			BlokScriptSymbol CreatedSymbol = new BlokScriptSymbol();
			CreatedSymbol.Name = SymbolName;
			CreatedSymbol.Type = BlokScriptSymbolType.Space;
			CreatedSymbol.Value = Space;
			_SymbolTableManager.AddSymbol(CreatedSymbol);

			return null;
		}

		public override object VisitBlockVarStatement ([NotNull] BlokScriptGrammarParser.BlockVarStatementContext Context)
		{
			//
			// EXAMPLES:
			//
			// block targetBlock;
			// block targetBlock = sourceBlock;
			// block targetBlock = block 'oilAnalysisAdditiveDataPoint';
			//
			// GRAMMAR:
			//
			// blockVarStatement: 'block' VARID ('=' blockSpec)?;
			//
			string SymbolName = Context.VARID().GetText();
			BlockSchemaEntity BlockSchema = Context.blockSpec() != null ? (BlockSchemaEntity)VisitBlockSpec(Context.blockSpec()) : null;

			BlokScriptSymbol CreatedSymbol = new BlokScriptSymbol();
			CreatedSymbol.Name = SymbolName;
			CreatedSymbol.Type = BlokScriptSymbolType.Block;
			CreatedSymbol.Value = BlockSchema;
			_SymbolTableManager.AddSymbol(CreatedSymbol);

			return null;
		}

		public override object VisitStringVarStatement ([NotNull] BlokScriptGrammarParser.StringVarStatementContext Context)
		{
			/*
			stringVarStatement: 'string' VARID ('=' stringExpr)?;
			*/
			string SymbolName = Context.VARID().GetText();

			BlokScriptSymbol CreatedSymbol = new BlokScriptSymbol();
			CreatedSymbol.Name = SymbolName;
			CreatedSymbol.Type = BlokScriptSymbolType.String;

			if (Context.stringExpr() != null)
				CreatedSymbol.Value = VisitStringExpr(Context.stringExpr());

			_SymbolTableManager.AddSymbol(CreatedSymbol);

			return null;
		}

		public override object VisitSpaceSpec ([NotNull] BlokScriptGrammarParser.SpaceSpecContext Context) 
		{
			//
			// RETURNS A SPACE ENTITY.
			//
			/*
			spaceSpec: 'space' (INTLITERAL | STRINGLITERAL | VARID) (varGetFrom)?
				| VARID
				;
			*/
			Regex IdRegex = new Regex("^[0-9]+$");
			Regex CopiedRegex = new Regex("^#[0-9]+$");

			if (Context.GetChild(0).GetText() == "space")
			{
				//
				// GET THE SPACE ID.
				//
				string SpaceId = null;
				string SpaceName = null;

				if (Context.INTLITERAL() != null)
				{
					//
					// GET THE SPACE BY ID (INTEGER PROVIDED).
					//
					SpaceId = Int32.Parse(Context.INTLITERAL().GetText()).ToString();
				}
				else if (Context.STRINGLITERAL() != null)
				{
					//
					// IF WE GET A STRING LITERAL, THIS CAN TAKE THE FOLLOWING FORMS:
					// '1234567'
					// '#1234567' (COPIED AND PASTED FROM STORYBLOK)
					// 'Space Name'
					//
					string SpaceNameOrId = SpaceLiteralTrimmer.Trim(StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText()));

					if (IdRegex.IsMatch(SpaceNameOrId))
					{
						//
						// '1234567'
						//
						SpaceId = SpaceNameOrId;
					}
					else if (CopiedRegex.IsMatch(SpaceNameOrId))
					{
						//
						// '#1234567' (COPIED AND PASTED FROM STORYBLOK)
						//
						SpaceId = SpaceNameOrId.Substring(1);
					}
					else
					{
						//
						// 'Space Name'
						//
						SpaceName = SpaceNameOrId;
					}
				}
				else if (Context.VARID() != null)
				{
					string SymbolName = Context.VARID().GetText();
					BlokScriptSymbol Symbol = _SymbolTableManager.GetSymbol(SymbolName);

					if (Symbol == null)
					{
						EchoError($"Variable '{SymbolName}' not defined.");
						throw new VariableNotDeclaredException(SymbolName);
					}
					else if (Symbol.Type == BlokScriptSymbolType.Int32)
					{
						SpaceId = ((int)Symbol.Value).ToString();
					}
					else if (Symbol.Type == BlokScriptSymbolType.String)
					{
						string SymbolValue = (string)Symbol.Value;

						if (IdRegex.IsMatch(SymbolValue))
						{
							//
							// '1234567'
							//
							SpaceId = SymbolValue;
						}
						else if (CopiedRegex.IsMatch(SymbolValue))
						{
							//
							// '#1234567' (COPIED AND PASTED FROM STORYBLOK)
							//
							SpaceId = SymbolValue.Substring(1);
						}
						else
						{
							//
							// 'Space Name'
							//
							SpaceName = SymbolValue;
						}
					}
					else if (Symbol.Type == BlokScriptSymbolType.Space)
					{
						return Symbol.Value;
					}
					else
					{
						EchoError($"Cannot use variable '{SymbolName}' of type '{Symbol.Type}' to identify a space.");
						throw new NotImplementedException("VisitSpaceSpec");
					}
				}
				else
					throw new NotImplementedException("VisitSpaceSpec");

				//
				// GET THE DATA LOCATION.
				//
				VarGetFromDirective GetFrom;

				if (Context.varGetFrom() != null)
				{
					GetFrom = (VarGetFromDirective)VisitVarGetFrom(Context.varGetFrom());
				}
				else
				{
					GetFrom = new VarGetFromDirective();
					GetFrom.DataLocation = BlokScriptEntityDataLocation.Server;
				}

				BlokScriptEntityDataLocation DataLocation = GetFrom.DataLocation;

				//
				// SINCE WE ARE GETTING A SPACE, WE START BY GETTING ALL SPACES.
				//
				EnsureSpaceDictsLoaded();

				//
				// GET THE SPACE.
				//
				if (DataLocation == BlokScriptEntityDataLocation.FilePath)
				{
					return LoadSpaceFromFile(SpaceId, SpaceName, GetFrom.FilePath);
				}
				else if (DataLocation == BlokScriptEntityDataLocation.Server)
				{
					if (SpaceId != null)
						return GetSpaceCacheWithSpaceDataLoadedById(SpaceId).Space;
					else if (SpaceName != null)
						return GetSpaceCacheWithSpaceDataLoadedByName(SpaceName).Space;
					else
						throw new NotImplementedException();
				}
				else
					throw new NotImplementedException($"BlokScriptEntityDataLocation not implemented: '{{DataLocation}}'.");
			}
			else
			{
				//
				// GET THE SYMBOL.
				//
				string SymbolName = Context.VARID().GetText();
				BlokScriptSymbol TargetSymbol = _SymbolTableManager.GetSymbol(SymbolName);

				if (TargetSymbol == null)
				{
					//
					// UNDEFINED SYMBOL.
					//
					int Line = Context.VARID().Symbol.Line;
					int Column = Context.VARID().Symbol.Column;

					Console.WriteLine($"{Line}:{Column}. Variable {SymbolName} not defined.");

					throw new VariableNotDeclaredException(SymbolName);
				}
				else if (TargetSymbol.Type == BlokScriptSymbolType.Space)
				{
					return (SpaceEntity)TargetSymbol.Value;
				}
				else if (TargetSymbol.Type == BlokScriptSymbolType.String)
				{
					string SpaceId = StringLiteralTrimmer.TrimSpaceId((string)TargetSymbol.Value);

					SpaceEntity Space = new SpaceEntity();
					Space.SpaceId = SpaceId;
					return Space;
				}
				else
				{
					string ErrorMessage = $"Variable '{SymbolName}' with type '{TargetSymbol.Type}' cannot be used as an identifier in a space literal statement. ";
					EchoError(ErrorMessage);
					throw new SymbolTypeException(ErrorMessage);
				}
			}
		}

		public SpaceEntity LoadSpaceFromFile (string SpaceId, string SpaceName, string FilePath)
		{
			string EffectiveFilePath = FilePath;

			if (EffectiveFilePath == null)
				EffectiveFilePath = (SpaceId != null ? SpaceId : SpaceName) + ".json";

			EchoAction($"Loading space from file: '{EffectiveFilePath}'.");
			SpaceEntity Space = SpaceFileReader.Read(EffectiveFilePath);
			Space.DataLocation = BlokScriptEntityDataLocation.FilePath;
			Space.FilePath = EffectiveFilePath;

			//
			// CACHE THE SPACE.
			//
			CacheSpace(Space);

			//
			// SPACE DATA ON FILE IS ALWAYS COMPLETE, SO MARK THE SPACE DATA AS LOADED.
			//
			SpaceCache TargetSpaceCache = GetSpaceCacheById(SpaceId);
			TargetSpaceCache.SpaceDataLoaded = true;

			return Space;
		}

		public override object VisitBlockSpec ([NotNull] BlokScriptGrammarParser.BlockSpecContext Context) 
		{
			/*
			blockSpec: 'block' STRINGLITERAL 'in' (spaceSpec | fileSpec)
				| 'block' VARID
				;
			*/
			if (Context.STRINGLITERAL() != null)
			{
				string BlockComponentName = StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText());

				if (Context.spaceSpec() != null)
				{
					//
					// GET THE SPACE.
					//
					SpaceEntity Space = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());

					//
					// WE HAVE THE SPACE.
					// GET ALL THE BLOCKS AND PICK OUT THE ONE WE NEED THAT MATCHES THE NAME.
					//
					SpaceCache SourceSpaceCache = GetSpaceCacheWithBlocksLoaded(Space.SpaceId);

					if (SourceSpaceCache.ContainsBlock(BlockComponentName))
						return SourceSpaceCache.GetBlock(BlockComponentName);
				
					throw new BlockNotFoundException(BlockComponentName, Space.SpaceId);				
				}
				else if (Context.fileSpec() != null)
				{
					string FilePath = (string)VisitFileSpec(Context.fileSpec());

					if (FilePath == null)
						FilePath = BlockComponentName + ".json";

					return BlockSchemaEntityFileReader.Read(FilePath);
				}
				else
					throw new NotImplementedException("VisitBlockSpec");
			}
			else if (Context.VARID() != null)
			{
				string SymbolName = Context.VARID().GetText();
				return (BlockSchemaEntity)_SymbolTableManager.GetSymbol(SymbolName).Value;
			}
			else
				throw new NotImplementedException("VisitBlockSpec");
		}

		public override object VisitAssignmentStatement ([NotNull] BlokScriptGrammarParser.AssignmentStatementContext Context) 
		{
			//
			// sourceSpaceId = sourceSpaceId;
			//
			/*
			assignmentStatement: VARID '=' VARID
			*/
			if (Context.VARID().Length > 1)
			{
				string LeftSymbolName = Context.VARID()[0].GetText();
				string RightSymbolName = Context.VARID()[1].GetText();

				BlokScriptSymbol LeftSymbol = _SymbolTableManager.GetSymbol(LeftSymbolName);
				BlokScriptSymbol RightSymbol = _SymbolTableManager.GetSymbol(RightSymbolName);

				if (LeftSymbol.Type == RightSymbol.Type)
					LeftSymbol.Value = RightSymbol.Value;
				else
					throw new TypeAssignmentException(LeftSymbol.Type, RightSymbol.Type);

				return null;
			}

			return VisitChildren(Context);
		}

		public override object VisitSpaceAssignmentStatement ([NotNull] BlokScriptGrammarParser.SpaceAssignmentStatementContext Context) 
		{
			//
			// sourceSpace = sourceSpaceId;
			// sourceSpace = space sourceSpaceId;
			// sourceSpace = space '12345';
			//
			/*
			spaceAssignmentStatement: VARID '=' spaceSpec;
			*/

			//
			// GET THE SPACE.
			//
			SpaceEntity Space = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());

			//
			// GET THE SYMBOL.
			//
			string SymbolName = Context.VARID().GetText();
			BlokScriptSymbol LeftSymbol = _SymbolTableManager.GetSymbol(SymbolName);

			if (LeftSymbol.Type == BlokScriptSymbolType.Space)
				LeftSymbol.Value = Space;
			else
				throw new TypeAssignmentException(LeftSymbol.Type, BlokScriptSymbolType.Space);

			return null;
		}

		public override object VisitBlockAssignmentStatement ([NotNull] BlokScriptGrammarParser.BlockAssignmentStatementContext Context) 
		{
			//
			// blockAssignmentStatement: VARID '=' blockSpec;
			//
			string SymbolName = Context.GetChild(0).GetText();
			BlokScriptSymbol LeftSymbol = _SymbolTableManager.GetSymbol(SymbolName);

			if (Context.VARID() != null)
			{
				string RightSymbolName = Context.VARID().GetText();
				BlokScriptSymbol RightSymbol = _SymbolTableManager.GetSymbol(RightSymbolName);

				if (LeftSymbol.Type == RightSymbol.Type)
					LeftSymbol.Value = (BlockSchemaEntity)RightSymbol.Value;
				else
					throw new TypeAssignmentException(LeftSymbol.Type, RightSymbol.Type);
			}
			else if (Context.blockSpec() != null)
			{
				BlockSchemaEntity Block = (BlockSchemaEntity)VisitChildren(Context.blockSpec());

				if (LeftSymbol.Type == BlokScriptSymbolType.Block)
					LeftSymbol.Value = Block;
				else
					throw new TypeAssignmentException(LeftSymbol.Type, BlokScriptSymbolType.Block);
			}

			return new NotImplementedException("VisitBlockAssignmentStatement");
		}

		public override object VisitStringAssignmentStatement ([NotNull] BlokScriptGrammarParser.StringAssignmentStatementContext Context) 
		{
			//
			// sourceSpaceId = '1019179';
			//
			/*
			stringAssignmentStatement: VARID '=' STRINGLITERAL;
			*/
			string SymbolName = Context.VARID().GetText();
			BlokScriptSymbol TargetSymbol = _SymbolTableManager.GetSymbol(SymbolName);

			if (TargetSymbol == null)
				throw new VariableNotDeclaredException(SymbolName);
			else if (TargetSymbol.Type == BlokScriptSymbolType.String)
				TargetSymbol.Value = StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText());
			else
				throw new TypeAssignmentException(TargetSymbol.Type, BlokScriptSymbolType.String);

			return VisitChildren(Context);
		}

		public override object VisitBlockOutputLocation ([NotNull] BlokScriptGrammarParser.BlockOutputLocationContext Context) 
		{
			/*
			blockOutputLocation: 'console'
				| 'local' 'cache'
				| 'file' STRINGLITERAL?
				| spaceSpec
				;
			*/
			BlockOutputLocation Location = new BlockOutputLocation();

			if (Context.spaceSpec() != null)
			{
				Location.ToSpace = true;
				Location.SpaceId = ((SpaceEntity)VisitSpaceSpec(Context.spaceSpec())).SpaceId;
			}
			else if (Context.GetChild(0).GetText() == "file")
			{
				Location.ToFile = true;

				if (Context.STRINGLITERAL() != null)
					Location.FilePath = StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText());
			}
			else if (Context.GetChild(0).GetText() == "console")
			{
				Location.ToConsole = true;
			}
			else if (Context.GetChild(0).GetText() == "local")
			{
				Location.ToLocalCache = true;
			}

			return Location;
		}

		public override object VisitSpaceOutputLocation ([NotNull] BlokScriptGrammarParser.SpaceOutputLocationContext Context)
		{
			/*
			spaceOutputLocation: 'console'
				| 'local' 'cache'
				| fileSpec
				;
			*/
			SpaceOutputLocation Location = new SpaceOutputLocation();

			if (Context.fileSpec() != null)
			{
				Location.ToFile = true;
				Location.FilePath = (string)VisitFileSpec(Context.fileSpec());
			}
			else if (Context.GetChild(0).GetText() == "console")
			{
				Location.ToConsole = true;
			}
			else if (Context.GetChild(0).GetText() == "local")
			{
				Location.ToLocalCache = true;
			}
			else
				throw new NotImplementedException("VisitSpaceOutputLocation");

			return Location;
		}

		public override object VisitVarGetFrom ([NotNull] BlokScriptGrammarParser.VarGetFromContext Context)
		{
			/*                    1              1   
			varGetFrom: ('on' 'server' | 'in' fileSpec);
			*/
			VarGetFromDirective GetFrom = new VarGetFromDirective();

			if (Context.fileSpec() != null)
			{
				GetFrom.DataLocation = BlokScriptEntityDataLocation.FilePath;
				GetFrom.FilePath = (string)VisitFileSpec(Context.fileSpec());
			}
			else if (Context.GetChild(1).GetText() == "server")
			{
				GetFrom.DataLocation = BlokScriptEntityDataLocation.Server;
			}
			else
				throw new NotImplementedException();

			return GetFrom;
		}

		public override object VisitCopySpacesStatement ([NotNull] BlokScriptGrammarParser.CopySpacesStatementContext Context)
		{
			/*
			copySpacesStatement: 'copy' 'all'? 'spaces' 'from' realDataLocation 'to' spacesOutputLocation;
			
			copy all spaces from server to console;
			copy all spaces from server to file 'spaces.json';
			copy all spaces from server to local cache;
			copy all spaces from local cache to console;

			copySpacesStatement: 'copy' 'all'? 'spaces' 'from' realDataLocation 'to' spacesOutputLocation;
			*/
			RealDataLocation SourceLocation = (RealDataLocation)VisitRealDataLocation(Context.realDataLocation());

			List<SpaceEntity> SpaceEntityList = new List<SpaceEntity>();

			if (SourceLocation == RealDataLocation.LocalCache)
			{
				foreach (string SpaceKey in _SpaceCacheByIdDict.Keys)
				{
					SpaceEntityList.Add(_SpaceCacheByIdDict[SpaceKey].Space);
				}
			}
			else if (SourceLocation == RealDataLocation.Server)
			{
				string RequestPath = ManagementPathFactory.CreateSpacesPath();

				if (_Log.IsDebugEnabled)
					_Log.Debug(RequestPath);

				EchoAction($"API GET {RequestPath}. Loading all spaces.");

				dynamic ResponseModel = JsonParser.ParseAsDynamic(GetManagementWebClient().GetString(RequestPath));

				foreach (dynamic SpaceData in ResponseModel.spaces)
				{
					SpaceEntity CurrentSpaceEntity = new SpaceEntity();
					CurrentSpaceEntity.SpaceId = SpaceData.id.ToString();
					CurrentSpaceEntity.Data = SpaceData;
					CurrentSpaceEntity.DataLocation = BlokScriptEntityDataLocation.Server;
					CurrentSpaceEntity.ServerPath = RequestPath;
					SpaceEntityList.Add(CurrentSpaceEntity);
				}
			}
			else
				throw new NotImplementedException("SourceLocation == RealDataLocation in VisitCopySpacesStatement.");

			//
			// GET THE TARGET LOCATION.
			//
			SpacesOutputLocation TargetLocation = (SpacesOutputLocation)VisitSpacesOutputLocation(Context.spacesOutputLocation());

			if (TargetLocation.ToConsole)
			{
				List<object> SpaceDataList = new List<object>();

				foreach (SpaceEntity CurrentEntity in SpaceEntityList)
				{
					SpaceDataList.Add(CurrentEntity.Data);
				}

				Console.WriteLine(JsonFormatter.FormatIndented(SpaceDataList));
			}
			else if (TargetLocation.ToLocalCache)
			{
				foreach (SpaceEntity CurrentSpaceEntity in SpaceEntityList)
				{
					SpaceCache CreatedCache = new SpaceCache();
					CreatedCache.Space = CurrentSpaceEntity;
					CreatedCache.SpaceDataLoaded = true;

					_SpaceCacheByIdDict[CurrentSpaceEntity.SpaceId] = CreatedCache;
				}
			}
			else if (TargetLocation.ToFile)
			{
				List<object> SpaceDataList = new List<object>();

				foreach (SpaceEntity CurrentEntity in SpaceEntityList)
				{
					SpaceDataList.Add(CurrentEntity.Data);
				}

				using (StreamWriter SpaceWriter = new StreamWriter(TargetLocation.FilePath))
				{
					SpaceWriter.WriteLine(JsonFormatter.FormatIndented(SpaceDataList));
				}
			}
			else
				throw new NotImplementedException("TargetLocation in VisitCopySpacesStatement");

			return null;
		}

		public override object VisitPrintStatement ([NotNull] BlokScriptGrammarParser.PrintStatementContext Context)
		{
			return VisitChildren(Context);
		}

		public override object VisitPrintSpacesStatement ([NotNull] BlokScriptGrammarParser.PrintSpacesStatementContext Context)
		{
			return VisitChildren(Context);
		}

		public override object VisitPrintVarStatement ([NotNull] BlokScriptGrammarParser.PrintVarStatementContext Context)
		{
			/*
			printVarStatement: 'print' VARID;
			*/
			string TargetSymbolName = Context.VARID().GetText();
			BlokScriptSymbol TargetSymbol = _SymbolTableManager.GetSymbol(TargetSymbolName);

			if (TargetSymbol == null)
			{
				string ErrorMessage = $"Variable not found: '{TargetSymbolName}'.";
				EchoError(ErrorMessage);
				throw new SymbolNotFoundException(ErrorMessage);
			}

			if (TargetSymbol.Type == BlokScriptSymbolType.String)
			{
				EchoToConsole((string)TargetSymbol.Value);
			}
			else if (TargetSymbol.Type == BlokScriptSymbolType.Space)
			{
				EchoToConsole(SpaceFormatter.FormatConsole((SpaceEntity)TargetSymbol.Value));
			}
			else if (TargetSymbol.Type == BlokScriptSymbolType.Block)
			{
				EchoToConsole(BlockFormatter.FormatConsole((BlockSchemaEntity)TargetSymbol.Value));
			}
			else if (TargetSymbol.Type == BlokScriptSymbolType.Story)
			{
				EchoToConsole(StoryFormatter.FormatEcho((StoryEntity)TargetSymbol.Value));
				EchoToConsole(StoryFormatter.FormatJson((StoryEntity)TargetSymbol.Value));
			}
			else if (TargetSymbol.Type == BlokScriptSymbolType.DatasourceEntry)
			{
				EchoToConsole(DatasourceEntryFormatter.FormatJson((DatasourceEntryEntity)TargetSymbol.Value));
			}
			else if (TargetSymbol.Type == BlokScriptSymbolType.Datasource)
			{
				EchoToConsole(DatasourceFormatter.FormatJson((DatasourceEntity)TargetSymbol.Value));
			}
			else
				throw new NotImplementedException("VisitPrintVarStatement");

			return null;
		}

		public override object VisitPrintSpaceStatement ([NotNull] BlokScriptGrammarParser.PrintSpaceStatementContext Context)
		{
			/*
			printSpaceStatement: 'print' 'space' (VARID | STRINGLITERAL);
			*/
			if (Context.VARID() != null)
			{
				BlokScriptSymbol TargetSymbol = _SymbolTableManager.GetSymbol(Context.VARID().GetText());

				EchoToConsole(SpaceFormatter.FormatConsole((SpaceEntity)TargetSymbol.Value));
			}
			else if (Context.STRINGLITERAL() != null)
			{
				string SpaceId = StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText());

				EchoToConsole(SpaceFormatter.FormatConsole(GetSpaceCacheWithSpaceDataLoadedById(SpaceId).Space));
			}

			return null;
		}

		public override object VisitRealDataLocation ([NotNull] BlokScriptGrammarParser.RealDataLocationContext Context)
		{
			/*
			realDataLocation: ('server' | 'local' 'cache');
			*/
			return StringLiteralTrimmer.Trim(Context.GetChild(0).GetText()) == "server" ? RealDataLocation.Server : RealDataLocation.LocalCache;
		}

		public override object VisitSpacesOutputLocation ([NotNull] BlokScriptGrammarParser.SpacesOutputLocationContext Context)
		{
			/*
			spacesOutputLocation: 'console'
				| 'local' 'cache'
				| fileSpec
				;
			*/
			SpacesOutputLocation Location = new SpacesOutputLocation();

			if (Context.fileSpec() != null)
			{
				Location.ToFile = true;
				Location.FilePath = (string)VisitFileSpec(Context.fileSpec());
			}
			else if (Context.GetChild(0).GetText() == "console")
				Location.ToConsole = true;
			else if (Context.GetChild(0).GetText() == "local")
				Location.ToLocalCache = true;
			else
				throw new NotImplementedException("VisitSpacesOutputLocation");

			return Location;
		}

		public override object VisitFileSpec ([NotNull] BlokScriptGrammarParser.FileSpecContext Context)
		{
			/*
			fileSpec: 'file' (STRINGLITERAL | VARID)?;
			*/
			if (Context.STRINGLITERAL() != null)
			{
				return StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText());
			}
			else if (Context.VARID() != null)
			{
				return _SymbolTableManager.GetSymbolValueAsString(Context.VARID().GetText());
			}
			else
				return null;
		}

		public override object VisitCopyBlocksStatement ([NotNull] BlokScriptGrammarParser.CopyBlocksStatementContext Context)
		{
			/*
			copyBlocksStatement: 'copy' 'blocks' ('in' | 'from') spaceSpec 'to' blocksOutputLocation ('where' blockConstraintExprList)?;
			*/
			SpaceEntity SourceSpace = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());

			//
			// GET ALL BLOCKS.
			//
			BlockSchemaEntity[] Blocks = GetBlocksInSpace(SourceSpace.SpaceId);

			//
			// APPLY THE BLOCK CONSTRAINT LIST.
			//
			if (Context.blockConstraintExprList() != null)
			{
				BlockConstraint Constraint = (BlockConstraint)VisitBlockConstraintExprList(Context.blockConstraintExprList());
				Blocks = Constraint.Evaluate(Blocks);
			}

			//
			// COPY THE BLOCKS TO THE TARGET LOCATION.
			//
			CopyBlocksToLocation(Blocks, (BlocksOutputLocation)VisitBlocksOutputLocation(Context.blocksOutputLocation()));
			return null;
		}

		public void CopyBlocksToLocation (BlockSchemaEntity[] Blocks, BlocksOutputLocation TargetLocation)
		{
			if (TargetLocation.ToConsole)
				CopyBlocksToConsole(Blocks);
			else if (TargetLocation.ToLocalCache)
				CopyBlocksToLocalCache(Blocks);
			else if (TargetLocation.ToFile)
				CopyBlocksToFile(Blocks, TargetLocation.FilePath);
			else if (TargetLocation.ToFiles)
				CopyBlocksToFiles(Blocks);
			else if (TargetLocation.ToSpace)
				CopyBlocksToSpace(Blocks, TargetLocation.SpaceId);
			else
				throw new NotImplementedException("CopyBlocksToLocation");
		}

		public void CopyBlocksToConsole (BlockSchemaEntity[] Blocks)
		{
			List<object> BlockDataList = new List<object>();

			foreach (BlockSchemaEntity CurrentEntity in Blocks)
			{
				BlockDataList.Add(CurrentEntity.Data);
			}

			Console.WriteLine(JsonFormatter.FormatIndented(BlockDataList));
		}

		public void CopyBlocksToLocalCache (BlockSchemaEntity[] Blocks)
		{
			foreach (BlockSchemaEntity CurrentEntity in Blocks)
			{
				SpaceCache CurrentSpaceCache = GetSpaceCacheById(CurrentEntity.SpaceId);
				CurrentSpaceCache.BlockSchemaEntities[CurrentEntity.ComponentName] = CurrentEntity;
			}
		}

		public void CopyBlocksToFile (BlockSchemaEntity[] Blocks, string FilePath)
		{
			using (StreamWriter TargetWriter = new StreamWriter(FilePath))
			{
				foreach (BlockSchemaEntity CurrentEntity in Blocks)
				{
					TargetWriter.WriteLine(JsonFormatter.FormatIndented(CurrentEntity.Data));
				}
			}
		}

		public void CopyBlocksToFiles (BlockSchemaEntity[] Blocks)
		{
			foreach (BlockSchemaEntity CurrentBlock in Blocks)
			{
				string CurrentBlockFilePath = CurrentBlock.ComponentName + ".json";

				EchoAction($"Copying Block '{CurrentBlock.ComponentName}' to file '{CurrentBlockFilePath}'.");

				using (StreamWriter TargetWriter = new StreamWriter(CurrentBlockFilePath))
				{
					TargetWriter.WriteLine(BlockFormatter.FormatJson(CurrentBlock));
				}
			}
		}

		public void CopyBlocksToSpace (BlockSchemaEntity[] Blocks, string SpaceId)
		{
			//
			// THE SOURCE BLOCKS ARE BEING COPIED TO ANOTHER SPACE.
			// THIS REQUIRES A LOT OF HEAVY API CALLS, SO WE NEED TO REDUCE THIS TO AVOID TIMEOUTS AND I/O.
			// SO, CREATE A TARGET SPACE CACHE RIGHT NOW, REUSE IT FOR FURTHER CALLS.
			//
			SpaceCache TargetSpaceCache = GetSpaceCacheWithBlocksLoaded(SpaceId);
			SpaceEntity TargetSpace = TargetSpaceCache.Space;
			StoryblokManagementWebClient WebClient = GetManagementWebClient();

			foreach (BlockSchemaEntity SourceBlock in Blocks)
			{
				string RequestBody = JsonFormatter.FormatIndented(SourceBlock.Data);

				//
				// CHECK FOR EXISTENCE OF THE BLOCK ON THE SERVER FIRST.
				// THIS DETERMINES WHETHER WE POST (CREATE) OR PUT (UPDATE).
				//
				if (!TargetSpaceCache.ContainsBlock(SourceBlock.ComponentName))
				{
					//
					// THE TARGET SPACE DOES NOT HAVE THIS BLOCK.  CREATE THE BLOCK IN THE TARGET SPACE.
					//
					string RequestPath = ManagementPathFactory.CreateComponentsPath(SpaceId);

					if (_Log.IsDebugEnabled)
					{
						_Log.Debug($"Space {SpaceId} does not have block {SourceBlock.ComponentName}.");
						_Log.Debug($"Creating block {SourceBlock.ComponentName} in space {SpaceId}.");
						_Log.Debug($"VisitCopyBlocksStatement: POST {RequestPath}");
					}

					EchoAction($"API POST {RequestPath}. Creating block {BlockFormatter.FormatHumanFriendly(SourceBlock)} in space {SpaceFormatter.FormatHumanFriendly(TargetSpace)}.");

					string CreatedBlockString = WebClient.PostJson(RequestPath, RequestBody);

					BlockSchemaEntity CreatedBlock =  CreateComponentResponseReader.ReadResponseString(CreatedBlockString);
					CreatedBlock.SpaceId = SpaceId;
					CreatedBlock.DataLocation = BlokScriptEntityDataLocation.Server;
					CreatedBlock.ServerPath = RequestPath;
					TargetSpaceCache.InsertBlock(CreatedBlock);
				}
				else
				{
					//
					// THE TARGET SPACE HAS THIS BLOCK.  UPDATE THE BLOCK IN THE TARGET SPACE.
					//
					BlockSchemaEntity TargetBlock = TargetSpaceCache.GetBlock(SourceBlock.ComponentName);

					string RequestPath = ManagementPathFactory.CreateComponentPath(TargetBlock.BlockId, SpaceId);
					EchoAction($"API PUT {RequestPath}. Updating block {BlockFormatter.FormatHumanFriendly(TargetBlock)} in space {SpaceFormatter.FormatHumanFriendly(TargetSpace)}.");

					WebClient.PutJson(RequestPath, RequestBody);
				}
			}
		}

		public override object VisitVerbosityStatement([NotNull] BlokScriptGrammarParser.VerbosityStatementContext Context)
		{
			/*
			verbosityStatement: 'be'? ('quiet' | 'verbose' | 'debugger');
			*/
			BlokScriptSymbol VerbositySymbol = _SymbolTableManager.GetSymbol("_GlobalVerbosity");
			string Token = Context.GetChild(1).GetText();

			if (Token == "quiet")
			{
				VerbositySymbol.Value = (int)BlokScriptVerbosity.Quiet;
				_Log.Info("Verbosity set to 'quiet'.");
			}
			else if (Token == "verbose")
			{
				VerbositySymbol.Value = (int)BlokScriptVerbosity.Verbose;
				Console.WriteLine("Verbosity set to 'verbose'.");
			}
			else if (Token == "debugger")
			{
				VerbositySymbol.Value = (int)BlokScriptVerbosity.Debugger;
				Console.WriteLine("Verbosity set to 'debugger'.");
			}
			else
				throw new NotImplementedException();

			return null;
		}

		public override object VisitWaitStatement([NotNull] BlokScriptGrammarParser.WaitStatementContext Context)
		{
			/*
			waitStatement: 'wait' INTLITERAL;
			*/
			int WaitTime = Int32.Parse(Context.INTLITERAL().GetText());

			Console.WriteLine($"Waiting {WaitTime} milliseconds.");

			System.Threading.Thread.Sleep(WaitTime);

			return null;
		}

		public override object VisitCompareSpacesStatement ([NotNull] BlokScriptGrammarParser.CompareSpacesStatementContext Context)
		{
			/*
			compareSpacesStatement: 'compare' spaceSpec 'and' spaceSpec;
			*/
			SpaceEntity Space1 = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec()[0]);
			SpaceEntity Space2 = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec()[1]);

			SpaceCache SpacheCache1 = GetSpaceCacheWithSpaceDataLoadedById(Space1.SpaceId);
			SpaceCache SpacheCache2 = GetSpaceCacheWithSpaceDataLoadedById(Space2.SpaceId);

			Diff[] Diffs = DiffNode.Compare((JObject)SpacheCache1.Space.Data, (JObject)SpacheCache2.Space.Data, 0);

			int DiffNumber = 1;

			foreach (Diff CurrentDiff in Diffs)
			{
				Console.WriteLine($"Diff {DiffNumber}");
				Console.WriteLine($"-----");
				Console.WriteLine($"PropertyName: {CurrentDiff.PropertyName}");
				Console.WriteLine($"Category: {CurrentDiff.Category}");
				Console.WriteLine($"AreEqual: {CurrentDiff.AreEqual}");
				Console.WriteLine($"Type1: {CurrentDiff.Type1}");
				Console.WriteLine($"Type2: {CurrentDiff.Type2}");
				Console.WriteLine($"Value1: {CurrentDiff.Value1}");
				Console.WriteLine($"Value2: {CurrentDiff.Value2}");
				Console.WriteLine();
				DiffNumber++;
			}

			return null;
		}

		public override object VisitCompareBlocksStatement ([NotNull] BlokScriptGrammarParser.CompareBlocksStatementContext Context)
		{
			/*
			compareBlocksStatement: 'compare' blockSpec 'and' blockSpec;
			*/
			BlockSchemaEntity Block1 = (BlockSchemaEntity)VisitBlockSpec(Context.blockSpec()[0]);
			BlockSchemaEntity Block2 = (BlockSchemaEntity)VisitBlockSpec(Context.blockSpec()[1]);



			return null;
		}

		public override object VisitCompareAllBlocksStatement ([NotNull] BlokScriptGrammarParser.CompareAllBlocksStatementContext Context)
		{
			/*
			compareAllBlocksStatement: 'compare' 'all' 'blocks' 'in' spaceSpec 'and' spaceSpec;
			*/
			SpaceEntity Space1 = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec()[0]);
			SpaceEntity Space2 = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec()[1]);

			return null;
		}

		public override object VisitStringExpr ([NotNull] BlokScriptGrammarParser.StringExprContext Context)
		{
			/*
			stringExpr: (STRINGLITERAL | VARID) ('+' stringExpr)?;
			*/
			if (Context.stringExpr() != null)
			{
				string StringExpr = (string)VisitStringExpr(Context.stringExpr());

				if (Context.VARID() != null)
				{
					return _SymbolTableManager.GetSymbolValueAsString(Context.VARID().GetText()) + StringExpr;
				}
				else if (Context.STRINGLITERAL() != null)
				{
					return StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText()) + StringExpr;
				}
				else
					throw new NotImplementedException("VisitStringExpr");
			}
			else
			{
				if (Context.VARID() != null)
				{
					return _SymbolTableManager.GetSymbolValueAsString(Context.VARID().GetText());
				}
				else if (Context.STRINGLITERAL() != null)
				{
					return StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText());
				}
				else
					throw new NotImplementedException("VisitStringExpr");
			}
		}

		public override object VisitPrintStringLiteralStatement([NotNull] BlokScriptGrammarParser.PrintStringLiteralStatementContext Context)
		{
			/*
			printStringLiteralStatement: 'print' STRINGLITERAL;
			*/
			Console.WriteLine(StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText()));
			return null;
		}

		public override object VisitPrintSymbolTableStatement([NotNull] BlokScriptGrammarParser.PrintSymbolTableStatementContext Context)
		{
			/*
			printSymbolTableStatement: 'print' 'symbol' 'tables';
			*/
			int SymbolTableNum = 0;

			foreach (BlokScriptSymbolTable CurrentSymbolTable in _SymbolTableManager.GetSymbolTables())
			{
				Console.WriteLine("-----");
				Console.WriteLine($"SYMBOL TABLE: {SymbolTableNum}");
				Console.WriteLine("-----");
				Console.WriteLine();

				foreach (BlokScriptSymbol CurrentSymbol in CurrentSymbolTable.Values)
				{
					Console.WriteLine("-----");
					Console.WriteLine($"Name: '{CurrentSymbol.Name}'");
					Console.WriteLine($"Type: '{CurrentSymbol.Type}'");
					Console.WriteLine($"Value: '{CurrentSymbol.Value}'");
					Console.WriteLine("-----");
					Console.WriteLine();
				}
			}

			return null;
		}

		public override object VisitPrintLocalCacheStatement([NotNull] BlokScriptGrammarParser.PrintLocalCacheStatementContext Context)
		{
			/*
			printLocalCacheStatement: 'print' 'local' 'cache';
			*/
			foreach (SpaceCache CurrentSpaceCache in _SpaceCacheByIdDict.Values)
			{
				Console.WriteLine("-----");
				Console.WriteLine($"SPACE CACHE: {CurrentSpaceCache.SpaceId}");
				Console.WriteLine($"SpaceDataLoaded: {CurrentSpaceCache.SpaceDataLoaded}");
				Console.WriteLine($"ComponentsLoaded: {CurrentSpaceCache.ComponentsLoaded}");
				Console.WriteLine("-----");
				Console.WriteLine();
				Console.WriteLine("-----");
				Console.WriteLine("SPACE:");
				Console.WriteLine($"SpaceId: '{CurrentSpaceCache.Space.SpaceId}'");
				Console.WriteLine($"DataLocation: '{CurrentSpaceCache.Space.DataLocation}'");
				Console.WriteLine($"FilePath: '{CurrentSpaceCache.Space.FilePath}'");
				Console.WriteLine($"ServerPath: '{CurrentSpaceCache.Space.ServerPath}'");
				Console.WriteLine("-----");
				Console.WriteLine();

				foreach (BlockSchemaEntity CurrentBlock in CurrentSpaceCache.BlockSchemaEntities.Values)
				{
					Console.WriteLine("-----");
					Console.WriteLine("BLOCK:");
					Console.WriteLine($"BlockId: '{CurrentBlock.BlockId}'");
					Console.WriteLine($"ComponentName: '{CurrentBlock.ComponentName}'");
					Console.WriteLine($"SpaceId: '{CurrentBlock.SpaceId}'");
					Console.WriteLine($"DataLocation: '{CurrentBlock.DataLocation}'");
					Console.WriteLine("-----");
					Console.WriteLine();
				}
			}

			return null;
		}

		public override object VisitBlocksOutputLocation ([NotNull] BlokScriptGrammarParser.BlocksOutputLocationContext Context)
		{
			/*
			blocksOutputLocation: 'console'
				| 'local' 'cache'
				| fileSpec
				| filesSpec
				| spaceSpec
				;
			*/
			BlocksOutputLocation Location = new BlocksOutputLocation();

			if (Context.fileSpec() != null)
			{
				Location.ToFile = true;
				Location.FilePath = (string)VisitFileSpec(Context.fileSpec());
			}
			else if (Context.filesSpec() != null)
			{
				Location.ToFiles = true;
			}
			else if (Context.spaceSpec() != null)
			{
				Location.ToSpace = true;
				Location.SpaceId = ((SpaceEntity)VisitSpaceSpec(Context.spaceSpec())).SpaceId;
			}
			else if (Context.GetChild(0).GetText() == "console")
			{
				Location.ToConsole = true;
			}
			else if (Context.GetChild(0).GetText() == "local")
			{
				Location.ToLocalCache = true;
			}

			return Location;
		}

		public override object VisitBlockConstraint ([NotNull] BlokScriptGrammarParser.BlockConstraintContext Context)
		{
			_Log.Debug("VisitBlockConstraint");

			/*
			blockConstraint: 'id' ('=' | '!=') intExpr
				| 'id' 'not'? 'in' '(' intExprList ')'
				| 'name' ('=' | '!=') stringExpr
				| 'name' 'not'? 'in' '(' stringExprList ')'
				| 'name' ('matches' | 'does' 'not' 'match') 'regex'? regexExpr
				| 'name' 'not'? 'in' '(' regexExprList ')'
				| 'name' 'not'? 'like' stringExpr
				| 'name' ('starts' | 'does' 'not' 'start') 'with' stringExpr
				| 'name' ('ends' | 'does' 'not' 'end') 'with' stringExpr
				;
			*/
			BlockConstraint Constraint = new BlockConstraint();

			if (Context.GetChild(0).GetText() == "id")
			{
				//
				// CONSTRAIN BY ID.
				//
				Constraint.Field = BlockConstraintField.Id;

				if (Context.GetChild(1).GetText() == "=")
				{
					Constraint.Operator = BlockConstraintOperator.Equals;
					Constraint.ConstraintData = VisitIntExpr(Context.intExpr());
				}
				else if (Context.GetChild(1).GetText() == "!=")
				{
					Constraint.Operator = BlockConstraintOperator.NotEquals;
					Constraint.ConstraintData = VisitIntExpr(Context.intExpr());
				}
				else if (Context.GetChild(1).GetText() == "in")
				{
					Constraint.Operator = BlockConstraintOperator.In;
					Constraint.ConstraintData = VisitIntExprList(Context.intExprList());
				}
				else if (Context.GetChild(1).GetText() == "not" && Context.GetChild(2).GetText() == "in")
				{
					Constraint.Operator = BlockConstraintOperator.NotIn;
					Constraint.ConstraintData = VisitIntExprList(Context.intExprList());
				}
				else
					throw new NotImplementedException("VisitBlockConstraint");
			}
			if (Context.GetChild(0).GetText() == "name")
			{
				//
				// CONSTRAIN BY NAME.
				//
				Constraint.Field = BlockConstraintField.Name;

				if (Context.GetChild(1).GetText() == "=")
				{
					Constraint.Operator = BlockConstraintOperator.Equals;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				if (Context.GetChild(1).GetText() == "!=")
				{
					Constraint.Operator = BlockConstraintOperator.NotEquals;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "not" && Context.GetChild(2).GetText() == "in")
				{
					Constraint.Operator = BlockConstraintOperator.NotIn;

					if (Context.stringExprList() != null)
					{
						Constraint.ConstraintData = VisitStringExprList(Context.stringExprList());
						Constraint.ConstraintDataType = BlockConstraintDataType.StringList;
					}
					else if (Context.regexExprList() != null)
					{
						Constraint.ConstraintData = VisitRegexExprList(Context.regexExprList());
						Constraint.ConstraintDataType = BlockConstraintDataType.RegexList;
					}
				}
				else if (Context.GetChild(1).GetText() == "in")
				{
					Constraint.Operator = BlockConstraintOperator.In;

					if (Context.stringExprList() != null)
					{
						Constraint.ConstraintData = VisitStringExprList(Context.stringExprList());
						Constraint.ConstraintDataType = BlockConstraintDataType.StringList;
					}
					else if (Context.regexExprList() != null)
					{
						Constraint.ConstraintData = VisitRegexExprList(Context.regexExprList());
						Constraint.ConstraintDataType = BlockConstraintDataType.RegexList;
					}
				}
				else if (Context.GetChild(1).GetText() == "matches")
				{
					Constraint.Operator = BlockConstraintOperator.MatchesRegex;
					Constraint.ConstraintData = VisitRegexExpr(Context.regexExpr());
				}
				else if (Context.GetChild(1).GetText() == "does" && Context.GetChild(2).GetText() == "not" && Context.GetChild(3).GetText() == "match")
				{
					Constraint.Operator = BlockConstraintOperator.DoesNotMatchRegex;
					Constraint.ConstraintData = VisitRegexExpr(Context.regexExpr());
				}
				else if (Context.GetChild(1).GetText() == "not" && Context.GetChild(2).GetText() == "like")
				{
					Constraint.Operator = BlockConstraintOperator.NotLike;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "like")
				{
					Constraint.Operator = BlockConstraintOperator.Like;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "does" && Context.GetChild(2).GetText() == "not" && Context.GetChild(3).GetText() == "start")
				{
					Constraint.Operator = BlockConstraintOperator.DoesNotStartWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "starts")
				{
					Constraint.Operator = BlockConstraintOperator.StartsWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "does" && Context.GetChild(2).GetText() == "not" && Context.GetChild(3).GetText() == "end")
				{
					Constraint.Operator = BlockConstraintOperator.DoesNotEndWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "ends")
				{
					Constraint.Operator = BlockConstraintOperator.EndsWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else
					throw new NotImplementedException("VisitBlockConstraint");
			}

			return Constraint;
		}

		public override object VisitRegexVarStatement([NotNull] BlokScriptGrammarParser.RegexVarStatementContext Context)
		{
			/*
			regexVarStatement: 'regex' VARID ('=' regexExpr)?;
			*/
			string SymbolName = Context.VARID().GetText();

			BlokScriptSymbol CreatedSymbol = new BlokScriptSymbol();
			CreatedSymbol.Name = SymbolName;
			CreatedSymbol.Type = BlokScriptSymbolType.Regex;

			if (Context.regexExpr() != null)
				CreatedSymbol.Value = VisitRegexExpr(Context.regexExpr());

			_SymbolTableManager.AddSymbol(CreatedSymbol);

			return null;
		}

		public override object VisitStoryVarStatement ([NotNull] BlokScriptGrammarParser.StoryVarStatementContext Context)
		{
			/*
			storyVarStatement: 'story' VARID ('=' storySpec)?;
			*/
			string SymbolName = Context.VARID().GetText();

			StoryEntity Story = null;

			if (Context.storySpec() != null)
				Story = (StoryEntity)VisitStorySpec(Context.storySpec());

			BlokScriptSymbol CreatedSymbol = new BlokScriptSymbol();
			CreatedSymbol.Name = SymbolName;
			CreatedSymbol.Type = BlokScriptSymbolType.Story;
			CreatedSymbol.Value = Story;
			_SymbolTableManager.AddSymbol(CreatedSymbol);

			return null;
		}

		public override object VisitStorySpec ([NotNull] BlokScriptGrammarParser.StorySpecContext Context)
		{
			/*
			storySpec: (VARID | INTLITERAL | STRINGLITERAL) ('in' | 'from') (spaceSpec | fileSpec)
				| VARID
				;
			*/
			if (Context.spaceSpec() != null || Context.fileSpec() != null)
			{
				//
				// GET THE SPACE.
				//
				if (Context.spaceSpec() != null)
				{
					SpaceEntity Space = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
					SpaceCache CurrentSpaceCache = GetSpaceCacheWithStoriesLoaded(Space.SpaceId);

					if (Context.STRINGLITERAL() != null)
					{
						string StoryUrl = StringLiteralTrimmer.Trim((Context.STRINGLITERAL().GetText()));

						if (CurrentSpaceCache.ContainsStory(StoryUrl))
							return CurrentSpaceCache.GetStory(StoryUrl);
				
						EchoError($"Story '{StoryUrl}' not found in Space '{Space.SpaceId}'.");
						throw new StoryNotFoundException(StoryUrl, Space.SpaceId);				
					}
					else if (Context.INTLITERAL() != null)
					{
						int StoryId = Int32.Parse(Context.INTLITERAL().GetText());

						if (CurrentSpaceCache.ContainsStory(StoryId))
							return CurrentSpaceCache.GetStory(StoryId);

						EchoError($"Story '{StoryId}' not found in Space '{Space.SpaceId}'.");
						throw new StoryNotFoundException(StoryId, Space.SpaceId);				
					}
					else if (Context.VARID() != null)
					{
						//
						// THE VARIABLE MUST BE AN INTEGER OR A STRING.
						//
						string SymbolName = Context.VARID().GetText();
						BlokScriptSymbol Symbol = _SymbolTableManager.GetSymbol(SymbolName);

						if (Symbol.Type == BlokScriptSymbolType.Int32)
						{
							int StoryId = (int)Symbol.Value;

							if (CurrentSpaceCache.ContainsStory(StoryId))
								return CurrentSpaceCache.GetStory(StoryId);

							EchoError($"Story '{StoryId}' not found in Space '{Space.SpaceId}'.");
							throw new StoryNotFoundException(StoryId, Space.SpaceId);
						}
						else if (Symbol.Type == BlokScriptSymbolType.String)
						{
							string StoryUrl = (string)Symbol.Value;

							if (CurrentSpaceCache.ContainsStory(StoryUrl))
								return CurrentSpaceCache.GetStory(StoryUrl);

							EchoError($"Story '{StoryUrl}' not found in Space '{Space.SpaceId}'.");
							throw new StoryNotFoundException(StoryUrl, Space.SpaceId);
						}
						else
						{
							EchoError($"Variable '{SymbolName}' with type '{Symbol.Type}' cannot be used in a story specification statement.");
							throw new ArgumentException();
						}
					}
					else
						throw new NotImplementedException("VisitStorySpec - Don't know what to use to get the story.");
				}
				else if (Context.fileSpec() != null)
				{
					string FilePath = (string)VisitFileSpec(Context.fileSpec());

					if (FilePath == null)
					{
						if (Context.STRINGLITERAL() != null)
						{
							string StoryUrl = StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText());
							FilePath = StoryUrl.Replace("/", "_") + ".json";
						}
						else if (Context.INTLITERAL() != null)
						{
							int StoryId = Int32.Parse(Context.INTLITERAL().GetText());
							FilePath = StoryId.ToString() + ".json";
						}
						else
							throw new NotImplementedException("VisitStorySpec - FilePath");
					}

					return StoryEntityFileReader.Read(FilePath);
				}
				else
					throw new NotImplementedException("VisitStorySpec");
			}
			else
			{
				return (StoryEntity)_SymbolTableManager.GetSymbol(Context.VARID().GetText()).Value;
			}
		}

		public override object VisitStoryOutputLocation([NotNull] BlokScriptGrammarParser.StoryOutputLocationContext Context)
		{
			/*
			storyOutputLocation: 'console'
				| 'local' 'cache'
				| fileSpec
				| spaceSpec
				;
			*/
			StoryOutputLocation Location = new StoryOutputLocation();

			if (Context.spaceSpec() != null)
			{
				Location.ToSpace = true;
				Location.SpaceId = ((SpaceEntity)VisitSpaceSpec(Context.spaceSpec())).SpaceId;
			}
			else if (Context.fileSpec() != null)
			{
				Location.ToFile = true;
				Location.FilePath = (string)VisitFileSpec(Context.fileSpec());
			}
			else if (Context.GetChild(0).GetText() == "console")
			{
				Location.ToConsole = true;
			}
			else if (Context.GetChild(0).GetText() == "local")
			{
				Location.ToLocalCache = true;
			}

			return Location;
		}

		public override object VisitPublishStoriesStatement([NotNull] BlokScriptGrammarParser.PublishStoriesStatementContext Context)
		{
			/*
			publishStoriesStatement: 'publish' 'stories' ('in' | 'from') spaceSpec ('where' storyConstraintExprList)?;
			*/
			SpaceEntity TargetSpace = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
			SpaceCache TargetSpaceCache = GetSpaceCacheWithStoriesLoaded(TargetSpace.SpaceId);
			StoryEntity[] TargetStories = TargetSpaceCache.GetStories();

			if (Context.storyConstraintExprList() != null)
				TargetStories =  ((StoryConstraint)VisitStoryConstraintExprList(Context.storyConstraintExprList())).Evaluate(TargetStories);

			int ConstrainedStoryCount = TargetStories.Length;
			EchoVerbose($"Publishing {ConstrainedStoryCount} stories in space {SpaceFormatter.FormatHumanFriendly(TargetSpace)}.");

			foreach (StoryEntity Story in TargetStories)
			{
				string RequestPath = ManagementPathFactory.CreatePublishStoryPath(Story.StoryId, TargetSpace.SpaceId);
				EchoAction($"API GET {RequestPath}. Publishing story {StoryFormatter.FormatHumanFriendly(Story)} in space {SpaceFormatter.FormatHumanFriendly(TargetSpace)}.");
				GetManagementWebClient().GetString(RequestPath);
			}
			return null;
		}

		public override object VisitUnpublishStoriesStatement([NotNull] BlokScriptGrammarParser.UnpublishStoriesStatementContext Context)
		{
			/*
			unpublishStoriesStatement: 'unpublish' 'stories' ('in' | 'from') spaceSpec ('where' storyConstraintExprList)?;
			*/
			SpaceEntity TargetSpace = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
			SpaceCache TargetSpaceCache = GetSpaceCacheWithStoriesLoaded(TargetSpace.SpaceId);
			StoryEntity[] TargetStories = TargetSpaceCache.GetStories();

			if (Context.storyConstraintExprList() != null)
				TargetStories =  ((StoryConstraint)VisitStoryConstraintExprList(Context.storyConstraintExprList())).Evaluate(TargetStories);

			int ConstrainedStoryCount = TargetStories.Length;
			EchoVerbose($"Unpublishing {ConstrainedStoryCount} stories in space {SpaceFormatter.FormatHumanFriendly(TargetSpace)}.");

			foreach (StoryEntity Story in TargetStories)
			{
				string RequestPath = ManagementPathFactory.CreateUnpublishStoryPath(Story.StoryId, TargetSpace.SpaceId);
				EchoAction($"API GET {RequestPath} | Unpublishing Story '{Story.Url}'.");
				GetManagementWebClient().GetString(RequestPath);
			}

			return null;
		}

		public override object VisitDeleteStoriesStatement([NotNull] BlokScriptGrammarParser.DeleteStoriesStatementContext Context)
		{
			/*
			deleteStoriesStatement: 'delete' 'stories' ('in' | 'from') (spaceSpec | shortSpaceSpec) ('where' storyConstraintExprList)?;
			*/
			SpaceEntity TargetSpace;

			if (Context.spaceSpec() != null)
				TargetSpace = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
			else
				TargetSpace = (SpaceEntity)VisitShortSpaceSpec(Context.shortSpaceSpec());

			SpaceCache TargetSpaceCache = GetSpaceCacheWithStoriesLoaded(TargetSpace.SpaceId);
			StoryEntity[] TargetStories = TargetSpaceCache.GetStories();

			int TotalStoryCount = TargetStories.Length;

			if (Context.storyConstraintExprList() != null)
				TargetStories =  ((StoryConstraint)VisitStoryConstraintExprList(Context.storyConstraintExprList())).Evaluate(TargetStories);

			int ConstrainedStoryCount = TargetStories.Length;

			if (ConstrainedStoryCount > 1)
				EchoVerbose($"Deleting {ConstrainedStoryCount} stories in space {SpaceFormatter.FormatHumanFriendly(TargetSpace)}.");
			else if (ConstrainedStoryCount == 1)
				EchoVerbose($"Deleting {ConstrainedStoryCount} story in space {SpaceFormatter.FormatHumanFriendly(TargetSpace)}.");
			else if (ConstrainedStoryCount == 0)
				EchoVerbose($"Deleting {ConstrainedStoryCount} stories in space {SpaceFormatter.FormatHumanFriendly(TargetSpace)}.");

			foreach (StoryEntity Story in TargetStories)
			{
				string RequestPath = ManagementPathFactory.CreateDeleteStoryPath(Story.StoryId, TargetSpace.SpaceId);
				EchoAction($"API DELETE {RequestPath}. Deleting story {StoryFormatter.FormatHumanFriendly(Story)} in space {SpaceFormatter.FormatHumanFriendly(TargetSpace)}.");
				GetManagementWebClient().Delete(RequestPath, "");

				//
				// REMOVE THE STORY FROM THE SPACE CACHE.
				//
				TargetSpaceCache.RemoveStory(Story);
			}

			return null;
		}

		public override object VisitStoryConstraint ([NotNull] BlokScriptGrammarParser.StoryConstraintContext Context)
		{
			_Log.Debug("VisitStoryConstraint");
			/*
			storyConstraint: 'id' ('=' | '!=') intExpr
				| 'id' 'not'? 'in' '(' intExprList ')'
				| ('name' | 'url') ('=' | '!=') stringExpr
				| ('name' | 'url') 'not'? 'in' '(' stringExprList ')'
				| ('name' | 'url') ('matches' | 'does' 'not' 'match') 'regex'? (stringExpr | REGEXLITERAL)
				| ('name' | 'url') 'not'? 'in' '(' stringExprList ')'
				| ('name' | 'url') 'not'? 'like' (STRINGLITERAL | VARID)
				| ('name' | 'url') ('starts' | 'does' 'not' 'start') 'with' stringExpr
				| ('name' | 'url') ('ends' | 'does' 'not' 'end') 'with' (stringExpr)
				| (('any'? 'tag') | ('all'? 'tags')) '=' stringExpr
				| (('any'? 'tag') | ('all'? 'tags')) 'in' '(' stringExprList ')'
				| (('any'? 'tag' 'matches') | ('all'? 'tags' 'match')) 'regex'? regexExpr
				| (('any'? 'tag' 'matches') | ('all'? 'tags' 'match')) 'in' '(' regexExprList ')'
				| (('any'? 'tag') | ('all'? 'tags')) 'like' stringExpr
				| (('any'? 'tag') | ('all'? 'tags')) 'starts' 'with' stringExpr
				| (('any'? 'tag') | ('all'? 'tags')) 'ends' 'with' stringExpr
				;
			*/
			StoryConstraint Constraint = new StoryConstraint();

			if (Context.GetChild(0).GetText() == "id")
			{
				Constraint.Field = StoryConstraintField.Id;

				if (Context.GetChild(1).GetText() == "=")
				{
					Constraint.Operator = StoryConstraintOperator.Equals;
					Constraint.ConstraintData = VisitIntExpr(Context.intExpr());
				}
				else if (Context.GetChild(1).GetText() == "!=")
				{
					Constraint.Operator = StoryConstraintOperator.NotEquals;
					Constraint.ConstraintData = VisitIntExpr(Context.intExpr());
				}
				else if (Context.GetChild(1).GetText() == "in")
				{
					Constraint.Operator = StoryConstraintOperator.In;
					Constraint.ConstraintData = VisitIntExprList(Context.intExprList());
				}
				else if (Context.GetChild(1).GetText() == "not" && Context.GetChild(2).GetText() == "in")
				{
					Constraint.Operator = StoryConstraintOperator.NotIn;
					Constraint.ConstraintData = VisitIntExprList(Context.intExprList());
				}
				else
					throw new NotImplementedException("VisitStoryConstraint");
			}
			else if (Context.GetChild(0).GetText() == "name" || Context.GetChild(0).GetText() == "url")
			{
				if (Context.GetChild(0).GetText() == "name")
					Constraint.Field = StoryConstraintField.Name;
				else if (Context.GetChild(0).GetText() == "url")
					Constraint.Field = StoryConstraintField.Url;
				else
					throw new NotImplementedException("VisitStoryConstraint");

				if (Context.GetChild(1).GetText() == "=")
				{
					Constraint.Operator = StoryConstraintOperator.Equals;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "!=")
				{
					Constraint.Operator = StoryConstraintOperator.NotEquals;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "not" && Context.GetChild(2).GetText() == "in")
				{
					Constraint.Operator = StoryConstraintOperator.NotIn;

					if (Context.stringExprList() != null)
					{
						Constraint.ConstraintData = VisitStringExprList(Context.stringExprList());
						Constraint.ConstraintDataType = StoryConstraintDataType.StringList;
					}
					else if (Context.regexExprList() != null)
					{
						Constraint.ConstraintData = VisitRegexExprList(Context.regexExprList());
						Constraint.ConstraintDataType = StoryConstraintDataType.RegexList;
					}
					else
						throw new NotImplementedException("");
				}
				else if (Context.GetChild(1).GetText() == "in")
				{
					Constraint.Operator = StoryConstraintOperator.In;

					if (Context.stringExprList() != null)
					{
						Constraint.ConstraintData = VisitStringExprList(Context.stringExprList());
						Constraint.ConstraintDataType = StoryConstraintDataType.StringList;
					}
					else if (Context.regexExprList() != null)
					{
						Constraint.ConstraintData = VisitRegexExprList(Context.regexExprList());
						Constraint.ConstraintDataType = StoryConstraintDataType.RegexList;
					}
					else
						throw new NotImplementedException("");
				}
				else if (Context.GetChild(1).GetText() == "matches")
				{
					Constraint.Operator = StoryConstraintOperator.MatchesRegex;
					Constraint.ConstraintData = VisitRegexExpr(Context.regexExpr());
				}
				else if (Context.GetChild(1).GetText() == "does" && Context.GetChild(2).GetText() == "not" && Context.GetChild(3).GetText() == "match")
				{
					Constraint.Operator = StoryConstraintOperator.DoesNotMatchRegex;
					Constraint.ConstraintData = VisitRegexExpr(Context.regexExpr());
				}
				else if (Context.GetChild(1).GetText() == "not" && Context.GetChild(2).GetText() == "like")
				{
					Constraint.Operator = StoryConstraintOperator.NotLike;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "like")
				{
					Constraint.Operator = StoryConstraintOperator.Like;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "does" && Context.GetChild(2).GetText() == "not" && Context.GetChild(3).GetText() == "start")
				{
					Constraint.Operator = StoryConstraintOperator.DoesNotStartWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "starts")
				{
					Constraint.Operator = StoryConstraintOperator.StartsWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "does" && Context.GetChild(2).GetText() == "not" && Context.GetChild(3).GetText() == "end")
				{
					Constraint.Operator = StoryConstraintOperator.DoesNotEndWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "ends")
				{
					Constraint.Operator = StoryConstraintOperator.EndsWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else
					throw new NotImplementedException("VisitStoryConstraint");
			}
			else if (Context.GetChild(0).GetText() == "any" || Context.GetChild(0).GetText() == "tag" || Context.GetChild(0).GetText() == "no")
			{
				Constraint.Field = StoryConstraintField.AnyTag;

				string OperatorToken = null;
				int i = 1;

				while (OperatorToken == null)
				{
					string Token = Context.GetChild(i).GetText();
					string NextToken = Context.GetChild(i + 1) != null ? Context.GetChild(i + 1).GetText() : null;
					string NexterToken = Context.GetChild(i + 2) != null ? Context.GetChild(i + 2).GetText() : null;

					if (Token == "=")
						OperatorToken = "=";
					else if (Token == "!=")
						OperatorToken = "!=";
					else if (Token == "not" && NextToken == "in")
						OperatorToken = "notin";
					else if (Token == "in")
						OperatorToken = "in";
					else if (Token == "matches")
						OperatorToken = "matches";
					else if (Token == "does" && NextToken == "not" && NexterToken == "match")
						OperatorToken = "doesnotmatch";
					else if (Token == "not" && NextToken == "like")
						OperatorToken = "notlike";
					else if (Token == "like")
						OperatorToken = "like";
					else if (Token == "starts")
						OperatorToken = "starts";
					else if (Token == "doesnotstart")
						OperatorToken = "doesnotstart";
					else if (Token == "ends")
						OperatorToken = "ends";
					else if (Token == "doesnotend")
						OperatorToken = "doesnotend";
					else if (Token == "tags")
					{
						if (Context.GetChild(0).GetText() == "any")
							OperatorToken = "anytags";
						else if (Context.GetChild(0).GetText() == "no")
							OperatorToken = "notags";
						else
							throw new NotImplementedException("VisitStoryConstraint");
					}

					i++;
				}

				if (OperatorToken == "=")
				{
					Constraint.Operator = StoryConstraintOperator.Equals;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (OperatorToken == "!=")
				{
					Constraint.Operator = StoryConstraintOperator.NotEquals;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (OperatorToken == "notin")
				{
					Constraint.Operator = StoryConstraintOperator.NotIn;

					if (Context.stringExprList() != null)
					{
						Constraint.ConstraintData = VisitStringExprList(Context.stringExprList());
						Constraint.ConstraintDataType = StoryConstraintDataType.StringList;
					}
					else if (Context.regexExprList() != null)
					{
						Constraint.ConstraintData = VisitRegexExprList(Context.regexExprList());
						Constraint.ConstraintDataType = StoryConstraintDataType.RegexList;
					}
					else
						throw new NotImplementedException("");
				}
				else if (OperatorToken == "in")
				{
					Constraint.Operator = StoryConstraintOperator.In;

					if (Context.stringExprList() != null)
					{
						Constraint.ConstraintData = VisitStringExprList(Context.stringExprList());
						Constraint.ConstraintDataType = StoryConstraintDataType.StringList;
					}
					else if (Context.regexExprList() != null)
					{
						Constraint.ConstraintData = VisitRegexExprList(Context.regexExprList());
						Constraint.ConstraintDataType = StoryConstraintDataType.RegexList;
					}
					else
						throw new NotImplementedException("VisitStoryConstraint");
				}
				else if (OperatorToken == "matches")
				{
					Constraint.Operator = StoryConstraintOperator.MatchesRegex;
					Constraint.ConstraintData = VisitRegexExpr(Context.regexExpr());
				}
				else if (OperatorToken == "doesnotmatch")
				{
					Constraint.Operator = StoryConstraintOperator.DoesNotMatchRegex;
					Constraint.ConstraintData = VisitRegexExpr(Context.regexExpr());
				}
				else if (OperatorToken == "notlike")
				{
					Constraint.Operator = StoryConstraintOperator.NotLike;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (OperatorToken == "like")
				{
					Constraint.Operator = StoryConstraintOperator.Like;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (OperatorToken == "starts")
				{
					Constraint.Operator = StoryConstraintOperator.StartsWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (OperatorToken == "doesnotstart")
				{
					Constraint.Operator = StoryConstraintOperator.DoesNotStartWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "ends")
				{
					Constraint.Operator = StoryConstraintOperator.EndsWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (OperatorToken == "doesnotend")
				{
					Constraint.Operator = StoryConstraintOperator.DoesNotEndWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (OperatorToken == "notags")
				{
					Constraint.Operator = StoryConstraintOperator.NoTags;
				}
				else if (OperatorToken == "anytags")
				{
					Constraint.Operator = StoryConstraintOperator.AnyTags;
				}
				else
					throw new NotImplementedException("VisitStoryConstraint");
			}
			else if (Context.GetChild(0).GetText() == "all" || Context.GetChild(0).GetText() == "tags")
			{
				Constraint.Field = StoryConstraintField.AllTags;

				string OperatorToken = null;
				int i = 1;

				while (OperatorToken == null)
				{
					string Token = Context.GetChild(i).GetText();
					string NextToken = Context.GetChild(i + 1) != null ? Context.GetChild(i + 1).GetText() : null;
					string NexterToken = Context.GetChild(i + 2) != null ? Context.GetChild(i + 2).GetText() : null;

					if (Token == "=")
						OperatorToken = "=";
					else if (Token == "!=")
						OperatorToken = "!=";
					else if (Token == "not" && NextToken == "in")
						OperatorToken = "notin";
					else if (Token == "in")
						OperatorToken = "in";
					else if (Token == "match")
						OperatorToken = "match";
					else if (Token == "do" && NextToken == "not" && NexterToken == "match")
						OperatorToken = "donotmatch";
					else if (Token == "not" && NextToken == "like")
						OperatorToken = "notlike";
					else if (Token == "like")
						OperatorToken = "like";
					else if (Token == "start")
						OperatorToken = "startwith";
					else if (Token == "do" && NextToken == "not" && NexterToken == "start")
						OperatorToken = "donotstartwith";
					else if (Token == "end")
						OperatorToken = "endwith";
					else if (Token == "do" && NextToken == "not" && NexterToken == "end")
						OperatorToken = "donotendwith";

					i++;
				}

				if (OperatorToken == "=")
				{
					Constraint.Operator = StoryConstraintOperator.Equals;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (OperatorToken == "!=")
				{
					Constraint.Operator = StoryConstraintOperator.NotEquals;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (OperatorToken == "notin")
				{
					Constraint.Operator = StoryConstraintOperator.NotIn;

					if (Context.stringExprList() != null)
					{
						Constraint.ConstraintData = VisitStringExprList(Context.stringExprList());
						Constraint.ConstraintDataType = StoryConstraintDataType.StringList;
					}
					else if (Context.regexExprList() != null)
					{
						Constraint.ConstraintData = VisitRegexExprList(Context.regexExprList());
						Constraint.ConstraintDataType = StoryConstraintDataType.RegexList;
					}
					else
						throw new NotImplementedException("");
				}
				else if (OperatorToken == "in")
				{
					Constraint.Operator = StoryConstraintOperator.In;

					if (Context.stringExprList() != null)
					{
						Constraint.ConstraintData = VisitStringExprList(Context.stringExprList());
						Constraint.ConstraintDataType = StoryConstraintDataType.StringList;
					}
					else if (Context.regexExprList() != null)
					{
						Constraint.ConstraintData = VisitRegexExprList(Context.regexExprList());
						Constraint.ConstraintDataType = StoryConstraintDataType.RegexList;
					}
					else
						throw new NotImplementedException("");
				}
				else if (OperatorToken == "match")
				{
					Constraint.Operator = StoryConstraintOperator.MatchesRegex;
					Constraint.ConstraintData = VisitRegexExpr(Context.regexExpr());
				}
				else if (OperatorToken == "donotmatch")
				{
					Constraint.Operator = StoryConstraintOperator.DoesNotMatchRegex;
					Constraint.ConstraintData = VisitRegexExpr(Context.regexExpr());
				}
				else if (OperatorToken == "notlike")
				{
					Constraint.Operator = StoryConstraintOperator.NotLike;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (OperatorToken == "like")
				{
					Constraint.Operator = StoryConstraintOperator.Like;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (OperatorToken == "startwith")
				{
					Constraint.Operator = StoryConstraintOperator.StartsWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (OperatorToken == "donotstartwith")
				{
					Constraint.Operator = StoryConstraintOperator.DoesNotStartWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (OperatorToken == "endwith")
				{
					Constraint.Operator = StoryConstraintOperator.EndsWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (OperatorToken == "donotendwith")
				{
					Constraint.Operator = StoryConstraintOperator.DoesNotEndWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else
					throw new NotImplementedException("VisitStoryConstraint");
			}

			return Constraint;
		}

		public override object VisitCopyDatasourcesStatement([NotNull] BlokScriptGrammarParser.CopyDatasourcesStatementContext Context)
		{
			/*
			copyDatasourcesStatement: 'copy' 'datasources' ('from' | 'in') spaceSpec 'to' spaceSpec ('where' datasourceConstraintExprList)?;
			*/
			SpaceEntity SourceSpace = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec()[0]);
			EnsureDatasourcesAreLoadedIntoSpace(SourceSpace);

			DatasourceEntity[] SourceDatasources = GetDatasourcesInSpace(SourceSpace);

			if (Context.datasourceConstraintExprList() != null)
			{
				DatasourceConstraint Constraint = (DatasourceConstraint)VisitDatasourceConstraintExprList(Context.datasourceConstraintExprList());
				SourceDatasources = Constraint.Evaluate(SourceDatasources);
			}

			CopyDatasourcesToSpace(SourceDatasources, (SpaceEntity)VisitSpaceSpec(Context.spaceSpec()[1]));
			return null;
		}

		public void CopyDatasourcesToSpace (DatasourceEntity[] SourceDatasources, SpaceEntity TargetSpace)
		{
			foreach (DatasourceEntity Datasource in SourceDatasources)
			{
				CreateDatasourceInSpace(Datasource, TargetSpace);
			}
		}

		public void CreateDatasourceInSpace (DatasourceEntity SourceDatasource, SpaceEntity TargetSpace)
		{
			DatasourceEntity TargetDatasource = new DatasourceEntity();
			TargetDatasource.Name = SourceDatasource.Name;
			TargetDatasource.Slug = SourceDatasource.Slug;
			TargetDatasource.Data = DatasourceEntityDataFactory.CreateData(TargetDatasource);

			string RequestPath = ManagementPathFactory.CreateDatasourcesPath(TargetSpace.SpaceId);
			EchoAction($"API POST {RequestPath}. Creating datasource {DatasourceFormatter.FormatHumanFriendly(SourceDatasource)} in space {SpaceFormatter.FormatHumanFriendly(TargetSpace)}.");

			string RequestBody = JsonFormatter.FormatIndented(TargetDatasource.Data);
			EchoDebug("RequestBody", RequestBody);

			string ResponseString;

			try
			{
				ResponseString = GetManagementWebClient().PostJson(RequestPath, RequestBody);
				EchoDebug("ResponseString", ResponseString);
			}
			catch (WebException E)
			{
				int StatusCode = (int)((HttpWebResponse)E.Response).StatusCode;

				if (StatusCode == 422)
				{
					EchoError($"Creation of datasource {DatasourceFormatter.FormatHumanFriendly(SourceDatasource)} in space {SpaceFormatter.FormatHumanFriendly(TargetSpace)} was rejected by Storyblok.  This can happen if you are missing a required field such as the name or slug, or if the datasource already exists.");
					EchoError(E.Message);
				}

				EchoDebug(E);
				throw;
			}

			DatasourceEntity CreatedDatasource = DatasourceParser.Parse(ResponseString);
			TargetDatasource.DatasourceId = CreatedDatasource.DatasourceId;
			TargetDatasource.Name = CreatedDatasource.Name;
			TargetDatasource.Slug = CreatedDatasource.Slug;
			TargetDatasource.SpaceId = TargetSpace.SpaceId;
			TargetDatasource.Data = CreatedDatasource.Data;
			TargetDatasource.DataLocation = BlokScriptEntityDataLocation.Server;
			TargetDatasource.ServerPath = RequestPath;

			GetSpaceCacheById(TargetSpace.SpaceId).InsertDatasource(TargetDatasource);
		}

		public void PersistDatasource (DatasourceEntity Datasource)
		{
			SpaceEntity Space = GetSpaceCacheById(Datasource.SpaceId).Space;

			string RequestPath = ManagementPathFactory.CreateDatasourcePath(Datasource.SpaceId, Datasource.DatasourceId);
			EchoAction($"API PUT {RequestPath}. Updating datasource {DatasourceFormatter.FormatHumanFriendly(Datasource)} in space {SpaceFormatter.FormatHumanFriendly(Space)}.");

			string RequestBody = JsonFormatter.FormatIndented(Datasource.Data);
			EchoDebug("RequestBody", RequestBody);

			try
			{
				GetManagementWebClient().PutJson(RequestPath, RequestBody);
			}
			catch (WebException E)
			{
				int StatusCode = (int)((HttpWebResponse)E.Response).StatusCode;

				if (StatusCode == 422)
				{
					EchoError($"Update of datasource {DatasourceFormatter.FormatHumanFriendly(Datasource)} in space {SpaceFormatter.FormatHumanFriendly(Space)} was rejected by Storyblok.  This can happen if you are missing a required field.");
					EchoError(E.Message);
				}

				EchoDebug(E);
				throw;
			}
		}

		public override object VisitDatasourceConstraint ([NotNull] BlokScriptGrammarParser.DatasourceConstraintContext Context)
		{
			/*
			datasourceConstraint: 'id' ('=' | '!=') intExpr
				| 'id' 'not'? 'in' '(' intExprList ')'
				| ('name' | 'slug') ('=' | '!=') stringExpr
				| ('name' | 'slug') 'not'? 'in' '(' stringExprList ')'
				| ('name' | 'slug') ('matches' | 'does' 'not' 'match') 'regex'? regexExpr
				| ('name' | 'slug') 'not'? 'in' '(' regexExprList ')'
				| ('name' | 'slug') 'not'? 'like' stringExpr
				| ('name' | 'slug') ('starts' | 'does' 'not' 'start') 'with' stringExpr
				| ('name' | 'slug') ('ends' | 'does' 'not' 'end') 'with' (stringExpr)
				;
			*/
			DatasourceConstraint Constraint = new DatasourceConstraint();

			if (Context.GetChild(0).GetText() == "id")
			{
				//
				// CONSTRAIN BY ID
				//
				Constraint.Field = DatasourceConstraintField.Id;

				if (Context.GetChild(1).GetText() == "=")
				{
					Constraint.Operator = DatasourceConstraintOperator.Equals;
					Constraint.ConstraintData = VisitIntExpr(Context.intExpr());
				}
				else if (Context.GetChild(1).GetText() == "!=")
				{
					Constraint.Operator = DatasourceConstraintOperator.NotEquals;
					Constraint.ConstraintData = VisitIntExpr(Context.intExpr());
				}
				else if (Context.GetChild(1).GetText() == "in")
				{
					Constraint.Operator = DatasourceConstraintOperator.In;
					Constraint.ConstraintData = VisitIntExprList(Context.intExprList());
				}
				else if (Context.GetChild(1).GetText() == "not" && Context.GetChild(2).GetText() == "in")
				{
					Constraint.Operator = DatasourceConstraintOperator.NotIn;
					Constraint.ConstraintData = VisitIntExprList(Context.intExprList());
				}
				else
					throw new NotImplementedException("VisitDatasourceConstraint");
			}
			else if (Context.GetChild(0).GetText() == "name" || Context.GetChild(0).GetText() == "slug")
			{
				//
				// CONSTRAIN BY NAME OR SLUG.
				//
				if (Context.GetChild(0).GetText() == "name")
					Constraint.Field = DatasourceConstraintField.Name;
				else if (Context.GetChild(0).GetText() == "slug")
					Constraint.Field = DatasourceConstraintField.Slug;
				else
					throw new NotImplementedException();

				if (Context.GetChild(1).GetText() == "=")
				{
					Constraint.Operator = DatasourceConstraintOperator.Equals;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "!=")
				{
					Constraint.Operator = DatasourceConstraintOperator.NotEquals;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "not" && Context.GetChild(2).GetText() == "in")
				{
					Constraint.Operator = DatasourceConstraintOperator.NotIn;

					if (Context.stringExprList() != null)
					{
						Constraint.ConstraintData = VisitStringExprList(Context.stringExprList());
						Constraint.ConstraintDataType = DatasourceConstraintDataType.StringList;
					}
					else if (Context.regexExprList() != null)
					{
						Constraint.ConstraintData = VisitRegexExprList(Context.regexExprList());
						Constraint.ConstraintDataType = DatasourceConstraintDataType.RegexList;
					}
					else
						throw new NotImplementedException("");
				}
				else if (Context.GetChild(1).GetText() == "in")
				{
					Constraint.Operator = DatasourceConstraintOperator.In;

					if (Context.stringExprList() != null)
					{
						Constraint.ConstraintData = VisitStringExprList(Context.stringExprList());
						Constraint.ConstraintDataType = DatasourceConstraintDataType.StringList;
					}
					else if (Context.regexExprList() != null)
					{
						Constraint.ConstraintData = VisitRegexExprList(Context.regexExprList());
						Constraint.ConstraintDataType = DatasourceConstraintDataType.RegexList;
					}
					else
						throw new NotImplementedException("");
				}
				else if (Context.GetChild(1).GetText() == "matches")
				{
					Constraint.Operator = DatasourceConstraintOperator.MatchesRegex;
					Constraint.ConstraintData = VisitRegexExpr(Context.regexExpr());
				}
				else if (Context.GetChild(1).GetText() == "does" && Context.GetChild(2).GetText() == "not" && Context.GetChild(3).GetText() == "match")
				{
					Constraint.Operator = DatasourceConstraintOperator.DoesNotMatchRegex;
					Constraint.ConstraintData = VisitRegexExpr(Context.regexExpr());
				}
				else if (Context.GetChild(1).GetText() == "not" && Context.GetChild(2).GetText() == "like")
				{
					Constraint.Operator = DatasourceConstraintOperator.NotLike;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "like")
				{
					Constraint.Operator = DatasourceConstraintOperator.Like;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "does" && Context.GetChild(2).GetText() == "not" && Context.GetChild(3).GetText() == "start")
				{
					Constraint.Operator = DatasourceConstraintOperator.DoesNotStartWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "starts")
				{
					Constraint.Operator = DatasourceConstraintOperator.StartsWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "does" && Context.GetChild(2).GetText() == "not" && Context.GetChild(3).GetText() == "end")
				{
					Constraint.Operator = DatasourceConstraintOperator.DoesNotEndWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "ends")
				{
					Constraint.Operator = DatasourceConstraintOperator.EndsWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else
					throw new NotImplementedException();
			}

			return Constraint;
		}

		public override object VisitCopyStoriesStatement ([NotNull] BlokScriptGrammarParser.CopyStoriesStatementContext Context)
		{
			/*
			copyStoriesStatement: 'copy' 'stories' ('in' | 'from') storiesInputLocation 'to' storiesOutputLocation ('where' storyConstraintExprList)?;
			*/
			StoriesInputLocation InputLocation = (StoriesInputLocation)VisitStoriesInputLocation(Context.storiesInputLocation());
			StoryEntity[] SourceStories = new StoryEntity[]{};

			if (InputLocation.FromSpace)
			{
				SpaceEntity SourceSpace = InputLocation.Space;
				SpaceCache SourceSpaceCache = GetSpaceCacheWithStoriesLoaded(SourceSpace.SpaceId);
				SourceStories = SourceSpaceCache.GetStories();
			}

			//
			// APPLY CONSTRAINTS.
			//
			if (Context.storyConstraintExprList() != null)
				SourceStories = ((StoryConstraint)VisitStoryConstraintExprList(Context.storyConstraintExprList())).Evaluate(SourceStories);

			//
			// OUTPUT TO SPECIFIED LOCATION.
			//
			StoriesOutputLocation OutLocation = (StoriesOutputLocation)VisitStoriesOutputLocation(Context.storiesOutputLocation());

			SpaceEntity TargetSpace = null;
			SpaceCache TargetSpaceCache = null;

			if (OutLocation.ToSpace)
			{
				TargetSpace = OutLocation.Space;
				TargetSpaceCache = GetSpaceCacheWithStoriesLoaded(TargetSpace.SpaceId);
			}

			foreach (StoryEntity SourceStory in SourceStories)
			{
				if (OutLocation.ToConsole)
				{
					EchoToConsole(StoryFormatter.FormatJson(SourceStory));
				}
				else if (OutLocation.ToLocalCache)
				{
					SpaceCache CurrentSpaceCache = GetSpaceCacheById(SourceStory.SpaceId);
					CurrentSpaceCache.StoryEntities[SourceStory.Url] = SourceStory;
				}
				else if (OutLocation.ToFile)
				{
					if (OutLocation.FilePath == null)
						OutLocation.FilePath = SourceStory.Url.Replace("/", "_") + ".json";

					EchoAction($"Copying Story '{SourceStory.Url}' to file: '{OutLocation.FilePath}'.");

					using (StreamWriter TargetWriter = new StreamWriter(OutLocation.FilePath))
					{
						TargetWriter.WriteLine(JsonFormatter.FormatIndented(SourceStory.Data));
					}
				}
				else if (OutLocation.ToSpace)
				{
					//
					// IF THE STORY DOES NOT CONTAIN CONTENT, THEN WE NEED TO GET THAT CONTENT FIRST.
					//
					if (!SourceStory.HasContent)
					{
						string RequestPath = ManagementPathFactory.CreateStoryPath(SourceStory.StoryId, SourceStory.SpaceId);
						EchoAction($"API GET {RequestPath}. Caching content for story {StoryFormatter.FormatHumanFriendly(SourceStory)} in space {SpaceFormatter.FormatHumanFriendly(TargetSpace)}.");
						SourceStory.Data = JsonParser.Parse(GetManagementWebClient().GetString(RequestPath));
						SourceStory.ServerPath = RequestPath;
						SourceStory.HasContent = true;
					}

					//
					// CREATE THE REQUEST BODY.
					//
					string RequestBody = JsonFormatter.FormatIndented(SourceStory.Data);

					//
					// CHECK FOR EXISTENCE OF THE STORY ON THE SERVER FIRST.
					// THIS DETERMINES WHETHER WE POST (CREATE) OR PUT (UPDATE).
					//
					if (!TargetSpaceCache.ContainsStory(SourceStory.Url))
					{
						//
						// THE TARGET SPACE DOES NOT HAVE THIS STORY.  CREATE THE STORY IN THE TARGET SPACE.
						//
						string RequestPath = ManagementPathFactory.CreateStoriesPath(TargetSpace.SpaceId);

						if (_Log.IsDebugEnabled)
						{
							_Log.Debug($"Space {TargetSpace.SpaceId} does not have story {SourceStory.Url}.");
							_Log.Debug($"Creating story {SourceStory.Url} in space {TargetSpace.SpaceId}.");
							_Log.Debug($"VisitCopyStoryStatement: POST {RequestPath}");
						}

						EchoAction($"API POST {RequestPath}. Creating story {StoryFormatter.FormatHumanFriendly(SourceStory)} in space {SpaceFormatter.FormatHumanFriendly(TargetSpace)}.");

						try
						{
							string ResponseString = GetManagementWebClient().PostJson(RequestPath, RequestBody);

							//
							// GET THE CREATED STORY.
							//
							StoryEntity CreatedStory = StoryParser.Parse(JsonParser.ParseAsDynamic(ResponseString).story);

							//
							// UPDATE THE TARGET CACHE.
							//
							TargetSpaceCache.InsertStory(CreatedStory);
						}
						catch (Exception E)
						{
							_Log.Error(E);
							EchoError(E.Message);
							throw;
						}
					}
					else
					{
						//
						// THE TARGET SPACE HAS THIS STORY.  UPDATE THE STORY IN THE TARGET SPACE.
						//
						StoryEntity TargetStory = TargetSpaceCache.GetStory(SourceStory.Url);
						string RequestPath = ManagementPathFactory.CreateStoryPath(TargetStory.StoryId, TargetSpace.SpaceId);

						try
						{
							EchoAction($"API PUT {RequestPath}. Updating story {StoryFormatter.FormatHumanFriendly(TargetStory)} in space {SpaceFormatter.FormatHumanFriendly(TargetSpace)}.");
							GetManagementWebClient().PutJson(RequestPath, RequestBody);
						}
						catch (Exception E)
						{
							_Log.Error(E);
							EchoError(E.Message);
							throw;
						}
					}
				}
				else
					throw new NotImplementedException("VisitCopyStoriesStatement");
			}

			return null;
		}

		public override object VisitIntExpr ([NotNull] BlokScriptGrammarParser.IntExprContext Context)
		{
			/*
			intExpr: (INTLITERAL | VARID) (('+' | '-' | '*' | '%') intExpr)?;
			*/
			int LeftOperand;

			if (Context.INTLITERAL() != null)
			{
				LeftOperand = Int32.Parse(Context.INTLITERAL().GetText());
			}
			else if (Context.VARID() != null)
			{
				LeftOperand = _SymbolTableManager.GetSymbolValueAsInt32(Context.VARID().GetText());
			}
			else
				throw new NotImplementedException();

			if (Context.intExpr() != null)
			{
				int RightOperand = (int)VisitIntExpr(Context.intExpr());

				if (Context.GetChild(1).GetText() == "+")
					return LeftOperand + RightOperand;
				else if (Context.GetChild(1).GetText() == "-")
					return LeftOperand - RightOperand;
				else if (Context.GetChild(1).GetText() == "*")
					return LeftOperand * RightOperand;
				else if (Context.GetChild(1).GetText() == "%")
					return LeftOperand % RightOperand;
				else
					throw new NotImplementedException("VisitIntExpr");
			}
			else
				return LeftOperand;
		}

		public override object VisitIntExprList ([NotNull] BlokScriptGrammarParser.IntExprListContext Context)
		{
			/*
			intExprList: intExpr ('+' intExprList)?;
			*/
			List<int> IntList = new List<int>();
			IntList.Add((int)VisitIntExpr(Context.intExpr()));

			if (Context.intExprList() != null)
				IntList.AddRange((int[])VisitIntExprList(Context.intExprList()));

			return IntList.ToArray();
		}

		public override object VisitStoriesInputLocation ([NotNull] BlokScriptGrammarParser.StoriesInputLocationContext Context)
		{
			/*
			storiesInputLocation: 'local' 'cache'
				| fileSpec
				| filesSpec
				| spaceSpec
				| shortSpaceSpec
				;
			*/
			if (Context.spaceSpec() != null)
			{
				StoriesInputLocation Location = new StoriesInputLocation();
				Location.FromSpace = true;
				Location.Space = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
				return Location;
			}
			else if (Context.shortSpaceSpec() != null)
			{
				StoriesInputLocation Location = new StoriesInputLocation();
				Location.FromSpace = true;
				Location.Space = (SpaceEntity)VisitShortSpaceSpec(Context.shortSpaceSpec());
				return Location;
			}
			else
				throw new NotImplementedException("VisitStoriesInputLocation");
		}

		public override object VisitStoriesOutputLocation ([NotNull] BlokScriptGrammarParser.StoriesOutputLocationContext Context)
		{
			/*
			storiesOutputLocation: 'console'
				| 'local' 'cache'
				| fileSpec
				| filesSpec
				| spaceSpec
				| shortSpaceSpec
				;
			*/
			StoriesOutputLocation Location = new StoriesOutputLocation();

			if (Context.spaceSpec() != null)
			{
				Location.ToSpace = true;
				Location.Space = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
			}
			else if (Context.shortSpaceSpec() != null)
			{
				Location.ToSpace = true;
				Location.Space = (SpaceEntity)VisitShortSpaceSpec(Context.shortSpaceSpec());
			}
			else if (Context.filesSpec() != null)
			{
				Location.ToFiles = true;
				//Location.F = (string)VisitFilesSpec(Context.filesSpec());
			}
			else if (Context.fileSpec() != null)
			{
				Location.ToFile = true;
				Location.FilePath = (string)VisitFileSpec(Context.fileSpec());
			}
			else if (Context.GetChild(0).GetText() == "console")
			{
				Location.ToConsole = true;
			}
			else if (Context.GetChild(0).GetText() == "local")
			{
				Location.ToLocalCache = true;
			}
			else
				throw new NotImplementedException("VisitStoriesOutputLocation");

			return Location;
		}

		public override object VisitRegexExprList ([NotNull] BlokScriptGrammarParser.RegexExprListContext Context)
		{
			/*
			regexExprList: regexExpr (',' regexExprList)?;
			*/
			List<Regex> RegexList = new List<Regex>();
			RegexList.Add((Regex)VisitRegexExpr(Context.regexExpr()));
			
			if (Context.regexExprList() != null)
				RegexList.AddRange((Regex[])VisitRegexExprList(Context.regexExprList()));

			return RegexList.ToArray();
		}

		public override object VisitRegexExpr ([NotNull] BlokScriptGrammarParser.RegexExprContext Context)
		{
			/*
			regexExpr: STRINGLITERAL | REGEXLITERAL | VARID;
			*/
			if (Context.STRINGLITERAL() != null)
				return new Regex(StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText()));
			else if (Context.REGEXLITERAL() != null)
				return new Regex(RegexLiteralTrimmer.Trim(Context.REGEXLITERAL().GetText()));
			else if (Context.VARID() != null)
			{
				BlokScriptSymbol Symbol = _SymbolTableManager.GetSymbol(Context.VARID().GetText());

				if (Symbol.Type == BlokScriptSymbolType.String)
					return new Regex((string)Symbol.Value);
				else if (Symbol.Type == BlokScriptSymbolType.Regex)
					return (Regex)Symbol.Value;
				else
					throw new NotImplementedException("VisitRegexExpr");
			}
			else
				throw new NotImplementedException("VisitRegexExpr");
		}

		public override object VisitStringExprList ([NotNull] BlokScriptGrammarParser.StringExprListContext Context)
		{
			/*
			stringExprList: stringExpr (',' stringExprList)?;
			*/
			List<string> RegexList = new List<string>();
			RegexList.Add((string)VisitStringExpr(Context.stringExpr()));
			
			if (Context.stringExprList() != null)
				RegexList.AddRange((string[])VisitStringExprList(Context.stringExprList()));

			return RegexList.ToArray();
		}

		public override object VisitDatasourceEntryVarStatement ([NotNull] BlokScriptGrammarParser.DatasourceEntryVarStatementContext Context)
		{
			/*
			datasourceEntryVarStatement: 'datasource entry' VARID ('=' datasourceEntrySpec)?;
			*/
			string SymbolName = Context.VARID().GetText();

			DatasourceEntryEntity DatasourceEntry = null;

			if (Context.datasourceEntrySpec() != null)
			{
				DatasourceEntry = (DatasourceEntryEntity)VisitDatasourceEntrySpec(Context.datasourceEntrySpec());
			}

			BlokScriptSymbol CreatedSymbol = new BlokScriptSymbol();
			CreatedSymbol.Name = SymbolName;
			CreatedSymbol.Type = BlokScriptSymbolType.DatasourceEntry;
			CreatedSymbol.Value = DatasourceEntry;
			_SymbolTableManager.AddSymbol(CreatedSymbol);

			return null;
		}

		public override object VisitDatasourceEntrySpec ([NotNull] BlokScriptGrammarParser.DatasourceEntrySpecContext Context)
		{
			/*
			datasourceEntrySpec: 'datasource' 'entry' (intExpr | stringExpr | VARID) ('from' | 'in') datasourceSpec
				| VARID
				;
			*/
			if (Context.datasourceSpec() != null)
			{
				DatasourceEntity Datasource = (DatasourceEntity)VisitDatasourceSpec(Context.datasourceSpec());
				EnsureDatasourceHasEntriesLoaded(Datasource);

				if (Context.VARID() != null)
				{
					string SymbolName = Context.VARID().GetText();
					BlokScriptSymbol Symbol = _SymbolTableManager.GetSymbol(SymbolName);

					if (Symbol.Type == BlokScriptSymbolType.String)
					{
						string Name = (string)Symbol.Value;

						if (!Datasource.HasEntryByName(Name))
						{
							string ErrorMessage = $"Datasource entry with name '{Name}' not found in datasource '{Datasource.Name} ({Datasource.DatasourceId})' in space id '{Datasource.SpaceId}'.";
							EchoError(ErrorMessage);
							throw new SpaceObjectNotFoundException(ErrorMessage);
						}

						return Datasource.GetEntryByName(Name);
					}
					else if (Symbol.Type == BlokScriptSymbolType.Int32)
					{
						int EntryId = (int)Symbol.Value;

						if (!Datasource.HasEntryById(EntryId))
						{
							string ErrorMessage = $"Datasource entry with id '{EntryId}' not found in datasource '{Datasource.Name} ({Datasource.DatasourceId})' in space id '{Datasource.SpaceId}'.";
							EchoError(ErrorMessage);
							throw new SpaceObjectNotFoundException(ErrorMessage);
						}

						return Datasource.GetEntryById(EntryId);
					}
					else
					{
						string ErrorMessage = $"Cannot use variable '{Symbol.Name}' of type '{Symbol.Type}' in a datasource entry literal expression.  Variable type must be 'string' or 'int'.";
						EchoError(ErrorMessage);
						throw new TypeNotAllowedException(ErrorMessage);
					}
				}
				else if (Context.stringExpr() != null)
				{
					string Name = (string)VisitStringExpr(Context.stringExpr());

					if (!Datasource.HasEntryByName(Name))
					{
						string ErrorMessage = $"Datasource entry with name '{Name}' not found in datasource '{Datasource.Name} ({Datasource.DatasourceId})' in space id '{Datasource.SpaceId}'.";
						EchoError(ErrorMessage);
						throw new SpaceObjectNotFoundException(ErrorMessage);
					}

					return Datasource.GetEntryByName(Name);
				}
				else if (Context.intExpr() != null)
				{
					int EntryId = (int)VisitIntExpr(Context.intExpr());

					if (!Datasource.HasEntryById(EntryId))
					{
						string ErrorMessage = $"Datasource entry with id '{EntryId}' not found in datasource '{Datasource.Name} ({Datasource.DatasourceId})' in space id '{Datasource.SpaceId}'.";
						EchoError(ErrorMessage);
						throw new SpaceObjectNotFoundException(ErrorMessage);
					}

					return Datasource.GetEntryById(EntryId);
				}
				else
					throw new NotImplementedException("VisitDatasourceEntrySpec");
			}
			else
			{
				BlokScriptSymbol Symbol = _SymbolTableManager.GetSymbol(Context.VARID().GetText());

				if (Symbol.Type != BlokScriptSymbolType.DatasourceEntry)
				{
					string ErrorMessage = $"Variable '{Symbol.Name}' has wrong type '{Symbol.Type}'.  Variable type must be '{BlokScriptSymbolType.DatasourceEntry}'.";
					EchoError(ErrorMessage);
					throw new TypeNotAllowedException(ErrorMessage);
				}

				return Symbol.Value;
			}
		}

		public override object VisitDatasourceSpec ([NotNull] BlokScriptGrammarParser.DatasourceSpecContext Context)
		{
			/*
			datasourceSpec: 'datasource' (VARID | INTLITERAL | STRINGLITERAL) 'in' spaceSpec
				| VARID
				;
			*/
			if (Context.spaceSpec() != null)
			{
				SpaceEntity Space = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());

				if (Context.VARID() != null)
				{
					string SymbolName = Context.VARID().GetText();
					BlokScriptSymbol Symbol = _SymbolTableManager.GetSymbol(SymbolName);

					if (Symbol.Type == BlokScriptSymbolType.String)
					{
						string NameOrSlug = (string)Symbol.Value;
						SpaceCache CurrentSpaceCache = GetSpaceCacheWithDataSourcesLoaded(Space.SpaceId);

						if (CurrentSpaceCache.ContainsDatasourceBySlug(NameOrSlug))
							return CurrentSpaceCache.GetDatasourceBySlug(NameOrSlug);
						else if (CurrentSpaceCache.ContainsDatasourceByName(NameOrSlug))
							return CurrentSpaceCache.GetDatasourceByName(NameOrSlug);

						string ErrorMessage = $"Datasource '{NameOrSlug}' not found in space {SpaceFormatter.FormatHumanFriendly(Space)}.";
						EchoError(ErrorMessage);
						throw new SpaceObjectNotFoundException(ErrorMessage);
					}
					else if (Symbol.Type == BlokScriptSymbolType.Int32)
					{
						int DatasourceId = (int)Symbol.Value;
						SpaceCache CurrentSpaceCache = GetSpaceCacheWithDataSourcesLoaded(Space.SpaceId);

						if (!CurrentSpaceCache.ContainsDatasourceById(DatasourceId))
						{
							string ErrorMessage = $"Datasource with id '{DatasourceId}' not found in space '{Space.Name} ({Space.SpaceId})'.";
							EchoError(ErrorMessage);
							throw new SpaceObjectNotFoundException(ErrorMessage);
						}

						return CurrentSpaceCache.GetDatasourceById(DatasourceId);
					}
					else
					{
						string ErrorMessage = $"Cannot use variable '{Symbol.Name}' of type '{Symbol.Type}' in a datasource entry literal expression.  Variable type must be 'string' or 'int'.";
						EchoError(ErrorMessage);
						throw new TypeNotAllowedException(ErrorMessage);
					}
				}
				else if (Context.STRINGLITERAL() != null)
				{
					//
					// GET THE DATASOURCE USING THE SLUG.
					//
					string NameOrSlug = StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText());
					SpaceCache CurrentSpaceCache = GetSpaceCacheWithDataSourcesLoaded(Space.SpaceId);

					if (CurrentSpaceCache.ContainsDatasourceBySlug(NameOrSlug))
						return CurrentSpaceCache.GetDatasourceBySlug(NameOrSlug);
					else if (CurrentSpaceCache.ContainsDatasourceByName(NameOrSlug))
						return CurrentSpaceCache.GetDatasourceByName(NameOrSlug);

					string ErrorMessage = $"Datasource '{NameOrSlug}' not found in space {SpaceFormatter.FormatHumanFriendly(Space)}.";
					EchoError(ErrorMessage);
					throw new SpaceObjectNotFoundException(ErrorMessage);
				}
				else if (Context.INTLITERAL() != null)
				{
					int DatasourceId = Int32.Parse(Context.INTLITERAL().GetText());
					SpaceCache CurrentSpaceCache = GetSpaceCacheWithDataSourcesLoaded(Space.SpaceId);

					if (!CurrentSpaceCache.ContainsDatasourceById(DatasourceId))
					{
						string ErrorMessage = $"Datasource with id '{DatasourceId}' not found in space '{Space.Name} ({Space.SpaceId})'.";
						EchoError(ErrorMessage);
						throw new SpaceObjectNotFoundException(ErrorMessage);
					}

					return CurrentSpaceCache.GetDatasourceById(DatasourceId);
				}
				else
					throw new NotImplementedException();
			}
			else
			{
				string SymbolName = Context.VARID().GetText();
				BlokScriptSymbol Symbol = _SymbolTableManager.GetSymbol(SymbolName);

				if (Symbol.Type != BlokScriptSymbolType.Datasource)
				{
					string ErrorMessage = $"Variable '{Symbol.Name}' has wrong type '{Symbol.Type}'.";
					EchoError(ErrorMessage);
					throw new TypeNotAllowedException(ErrorMessage);
				}

				return (DatasourceEntity)Symbol.Value;
			}
		}

		public override object VisitCreateDatasourceEntryStatement ([NotNull] BlokScriptGrammarParser.CreateDatasourceEntryStatementContext Context)
		{
			/*
			createDatasourceEntryStatement: 'create' 'datasource' 'entry' (stringExpr | datasourceEntryUpdateList) ('for' | 'in') datasourceSpec;
			*/
			DatasourceEntity Datasource = (DatasourceEntity)VisitDatasourceSpec(Context.datasourceSpec());
			SpaceEntity Space = GetSpaceById(Datasource.SpaceId);

			DatasourceEntryEntity CreatedEntry = new DatasourceEntryEntity();

			if (Context.stringExpr() != null)
			{
				CreatedEntry.Name = (string)VisitStringExpr(Context.stringExpr());
				CreatedEntry.Value = StringFormatter.FormatSlug(CreatedEntry.Name);
			}
			else
			{
				foreach (DatasourceEntryUpdate CurrentUpdate in (DatasourceEntryUpdate[])VisitDatasourceEntryUpdateList(Context.datasourceEntryUpdateList()))
				{
					if (CurrentUpdate.Name == "name")
						CreatedEntry.Name = CurrentUpdate.Value;
					else if (CurrentUpdate.Name == "value")
						CreatedEntry.Value = CurrentUpdate.Value;
				}
			}

			CreatedEntry.DatasourceId = Datasource.DatasourceId;
			CreatedEntry.Data = CreateDatasourceEntryData(CreatedEntry);

			string RequestPath = ManagementPathFactory.CreateDatasourceEntriesPathForCreate(Datasource.SpaceId);
			string ResponseString;

			try
			{
				EchoAction($"API POST {RequestPath}. Creating datasource entry {DatasourceEntryFormatter.FormatHumanFriendly(CreatedEntry)} in datasource {DatasourceFormatter.FormatHumanFriendly(Datasource)} in space {SpaceFormatter.FormatHumanFriendly(Space)}.");
				ResponseString = GetManagementWebClient().PostJson(RequestPath, JsonSerializer.Serialize(CreatedEntry.Data));
			}
			catch (WebException E)
			{
				int StatusCode = (int)(((HttpWebResponse)E.Response).StatusCode);

				if (StatusCode == 422)
					EchoError($"Storyblok returned a status code of {StatusCode}.  This usually means that the datasource entry already exists. {E.Message}");
				else
					EchoError(E.Message);

				throw;
			}

			//
			// PARSE THE RETURN VALUE AND SAVE IT.
			//
			DatasourceEntryEntity ReturnedEntry = CreateDatasourceEntryResponseReader.ReadResponseString(ResponseString);
			CreatedEntry.Data = ReturnedEntry.Data;
			CreatedEntry.DataLocation = BlokScriptEntityDataLocation.Server;
			CreatedEntry.ServerPath = RequestPath;
			Datasource.InsertDatasourceEntry(CreatedEntry);

			return null;
		}

		public override object VisitUpdateDatasourceEntriesStatement ([NotNull] BlokScriptGrammarParser.UpdateDatasourceEntriesStatementContext Context)
		{
			/*
			updateDatasourceEntriesStatement: 'update' 'datasource' 'entries' 'in' datasourceSpec 'set' datasourceEntryUpdateList ('where' datasourceEntryConstraintExprList)?;
			*/

			//
			// GET THE DATASOURCE AND MAKE SURE THE ENTRIES ARE LOADED.
			//
			DatasourceEntity Datasource = (DatasourceEntity)VisitDatasourceSpec(Context.datasourceSpec());
			EnsureDatasourceHasEntriesLoaded(Datasource);

			//
			// GET THE ENTRIES;
			//
			DatasourceEntryEntity[] Entries = Datasource.GetEntries();

			//
			// APPLY CONSTRAINTS.
			//
			if (Context.datasourceEntryConstraintExprList() != null)
			{
				DatasourceEntryConstraint Constraint = (DatasourceEntryConstraint)VisitDatasourceEntryConstraintExprList(Context.datasourceEntryConstraintExprList());
				Entries = Constraint.Evaluate(Entries);
			}

			//
			// GET THE UPDATES WE NEED TO APPLY.
			//
			DatasourceEntryUpdate[] DatasourceEntryUpdates = (DatasourceEntryUpdate[])VisitDatasourceEntryUpdateList(Context.datasourceEntryUpdateList());

			//
			// APPLY THE UPDATES TO THE CONSTRAINED ENTITIES;
			//
			foreach (DatasourceEntryEntity CurrentEntry in Entries)
			{
				foreach (DatasourceEntryUpdate CurrentUpdate in DatasourceEntryUpdates)
				{
					if (CurrentUpdate.Name == "name")
						CurrentEntry.Name = CurrentUpdate.Value;
					else if (CurrentUpdate.Name == "value")
						CurrentEntry.Value = CurrentUpdate.Value;
				}

				//
				// REGENERATE THE JSON SO WE CAN SUBMIT IT TO THE API.
				//
				CurrentEntry.Data = CreateDatasourceEntryData(CurrentEntry);

				string RequestPath = ManagementPathFactory.CreateDatasourceEntryPath(Datasource.SpaceId, CurrentEntry.DatasourceEntryId);
				EchoAction($"API PUT {RequestPath}. Updating datasource entry in datasource '{Datasource.DatasourceId}'.");
				GetManagementWebClient().PutJson(RequestPath, JsonSerializer.Serialize(CurrentEntry.Data));
			}

			return null;
		}

		public override object VisitDeleteDatasourceEntriesStatement ([NotNull] BlokScriptGrammarParser.DeleteDatasourceEntriesStatementContext Context)
		{
			/*
			deleteDatasourceEntriesStatement: 'delete' 'datasource' 'entries' 'in' datasourceSpec ('where' datasourceEntryConstraintList)?;
			*/
			DatasourceEntity Datasource = (DatasourceEntity)VisitDatasourceSpec(Context.datasourceSpec());
			EnsureDatasourceHasEntriesLoaded(Datasource);

			//
			// GET THE ENTRIES.
			//
			DatasourceEntryEntity[] Entries = Datasource.GetEntries();

			//
			// APPLY CONSTRAINTS.
			//
			if (Context.datasourceEntryConstraintExprList() != null)
			{
				DatasourceEntryConstraint Constraint = (DatasourceEntryConstraint)VisitDatasourceEntryConstraintExprList(Context.datasourceEntryConstraintExprList());
				Entries = Constraint.Evaluate(Entries);
			}

			//
			// DELETE THE ENTRIES.
			//
			foreach (DatasourceEntryEntity CurrentEntry in Entries)
			{
				string RequestPath = ManagementPathFactory.CreateDatasourceEntryPath(Datasource.SpaceId, CurrentEntry.DatasourceEntryId);
				EchoAction($"API DELETE {RequestPath}. Deleting datasource entry in datasource '{Datasource.DatasourceId}'.");
				GetManagementWebClient().Delete(RequestPath, "");
				Datasource.InsertDatasourceEntry(CurrentEntry);
			}

			return null;
		}

		public override object VisitDatasourceEntryConstraintExpr ([NotNull] BlokScriptGrammarParser.DatasourceEntryConstraintExprContext Context)
		{
			/*
			datasourceEntryConstraintExpr: datasourceEntryConstraint (('and' | 'or') datasourceEntryConstraintExpr)?
				| '(' datasourceEntryConstraint (('and' | 'or') datasourceEntryConstraintExpr)? ')'
				| '(' datasourceEntryConstraintExpr (('and' | 'or') datasourceEntryConstraintExpr)? ')'
				;
			*/
			if (Context.GetChild(0).GetText() == "(")
			{
				DatasourceEntryConstraint RootConstraint = new DatasourceEntryConstraint();
				RootConstraint.Operator = DatasourceEntryConstraintOperator.Root;

				if (Context.datasourceEntryConstraint() != null)
				{
					DatasourceEntryConstraint ThisConstraint = (DatasourceEntryConstraint)VisitDatasourceEntryConstraint(Context.datasourceEntryConstraint());

					if (Context.datasourceEntryConstraintExpr().Length > 0)
					{
						DatasourceEntryConstraint OpConstraint = new DatasourceEntryConstraint();
						OpConstraint.LeftChildConstraint = ThisConstraint;
						OpConstraint.RightChildConstraint = (DatasourceEntryConstraint)VisitDatasourceEntryConstraintExpr(Context.datasourceEntryConstraintExpr(0));

						string OperatorToken = Context.GetChild(2).GetText();

						if (OperatorToken == "and")
							OpConstraint.Operator = DatasourceEntryConstraintOperator.Intersect;
						else if (OperatorToken == "or")
							OpConstraint.Operator = DatasourceEntryConstraintOperator.Union;
						else
							throw new NotImplementedException();

						RootConstraint.ChildConstraint = OpConstraint;
					}
					else
					{
						RootConstraint.ChildConstraint = ThisConstraint;
					}
				}
				else
				{
					DatasourceEntryConstraint OpConstraint = new DatasourceEntryConstraint();
					OpConstraint.LeftChildConstraint = (DatasourceEntryConstraint)VisitDatasourceEntryConstraintExpr(Context.datasourceEntryConstraintExpr()[0]);
					OpConstraint.RightChildConstraint = (DatasourceEntryConstraint)VisitDatasourceEntryConstraintExpr(Context.datasourceEntryConstraintExpr()[1]);

					string OperatorToken = Context.GetChild(2).GetText();

					if (OperatorToken == "and")
						OpConstraint.Operator = DatasourceEntryConstraintOperator.Intersect;
					else if (OperatorToken == "or")
						OpConstraint.Operator = DatasourceEntryConstraintOperator.Union;
					else
						throw new NotImplementedException();

					RootConstraint.ChildConstraint = OpConstraint;
				}

				return RootConstraint;
			}
			else
			{
				// datasourceEntryConstraint (('and' | 'or') datasourceEntryConstraintExpr)?

				if (Context.datasourceEntryConstraintExpr().Length > 0)
				{
					string OperatorToken = Context.GetChild(1).GetText();

					Echo(OperatorToken);

					DatasourceEntryConstraint OpConstraint = new DatasourceEntryConstraint();
					OpConstraint.LeftChildConstraint = (DatasourceEntryConstraint)VisitDatasourceEntryConstraint(Context.datasourceEntryConstraint());
					OpConstraint.RightChildConstraint = (DatasourceEntryConstraint)VisitDatasourceEntryConstraintExpr(Context.datasourceEntryConstraintExpr(0));

					if (OperatorToken == "and")
						OpConstraint.Operator = DatasourceEntryConstraintOperator.Intersect;
					else if (OperatorToken == "or")
						OpConstraint.Operator = DatasourceEntryConstraintOperator.Union;
					else
						throw new NotImplementedException();

					return OpConstraint;
				}
				else
					return (DatasourceEntryConstraint)VisitDatasourceEntryConstraint(Context.datasourceEntryConstraint());
			}
		}

		public override object VisitDatasourceEntryConstraint ([NotNull] BlokScriptGrammarParser.DatasourceEntryConstraintContext Context)
		{
			/*
			datasourceEntryConstraint: 'id' ('=' | '!=') intExpr
				| 'id' 'not'? 'in' '(' intExprList ')'
				| ('name' | 'value') ('=' | '!=') stringExpr
				| ('name' | 'value') 'not'? 'in' '(' stringExprList ')'
				| ('name' | 'value') ('matches' | 'does' 'not' 'match') 'regex'? regexExpr
				| ('name' | 'value') 'not'? 'in' '(' stringExprList ')'
				| ('name' | 'value') 'not'? 'like' stringExpr
				| ('name' | 'value') ('starts' | 'does' 'not' 'start') 'with' stringExpr
				| ('name' | 'value') ('ends' | 'does' 'not' 'end') 'with' stringExpr
				;
			*/
			DatasourceEntryConstraint Constraint = new DatasourceEntryConstraint();

			if (Context.GetChild(0).GetText() == "id")
			{
				//
				// CONSTRAIN BY ID
				//
				Constraint.Field = DatasourceEntryConstraintField.Id;

				if (Context.GetChild(1).GetText() == "=")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.Equals;
					Constraint.ConstraintData = VisitIntExpr(Context.intExpr());
				}
				else if (Context.GetChild(1).GetText() == "!=")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.NotEquals;
					Constraint.ConstraintData = VisitIntExpr(Context.intExpr());
				}
				else if (Context.GetChild(1).GetText() == "in")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.In;
					Constraint.ConstraintData = VisitIntExprList(Context.intExprList());
				}
				else if (Context.GetChild(1).GetText() == "not" && Context.GetChild(2).GetText() == "in")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.NotIn;
					Constraint.ConstraintData = VisitIntExprList(Context.intExprList());
				}
				else
					throw new NotImplementedException("VisitDatasourceEntryConstraint");
			}
			else if (Context.GetChild(0).GetText() == "name" || Context.GetChild(0).GetText() == "value")
			{
				//
				// CONTRAIN BY NAME OR VALUE
				//
				if (Context.GetChild(0).GetText() == "name")
					Constraint.Field = DatasourceEntryConstraintField.Name;
				else if (Context.GetChild(0).GetText() == "value")
					Constraint.Field = DatasourceEntryConstraintField.Value;
				else
					throw new NotImplementedException("VisitDatasourceEntryConstraint");

				if (Context.GetChild(1).GetText() == "=")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.Equals;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "!=")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.NotEquals;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "not" && Context.GetChild(2).GetText() == "in")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.NotIn;

					if (Context.stringExprList() != null)
					{
						Constraint.ConstraintData = VisitStringExprList(Context.stringExprList());
						Constraint.ConstraintDataType = DatasourceEntryConstraintDataType.StringList;
					}
					else if (Context.regexExprList() != null)
					{
						Constraint.ConstraintData = VisitRegexExprList(Context.regexExprList());
						Constraint.ConstraintDataType = DatasourceEntryConstraintDataType.RegexList;
					}
					else
						throw new NotImplementedException("");
				}
				else if (Context.GetChild(1).GetText() == "in")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.In;

					if (Context.stringExprList() != null)
					{
						Constraint.ConstraintData = VisitStringExprList(Context.stringExprList());
						Constraint.ConstraintDataType = DatasourceEntryConstraintDataType.StringList;
					}
					else if (Context.regexExprList() != null)
					{
						Constraint.ConstraintData = VisitRegexExprList(Context.regexExprList());
						Constraint.ConstraintDataType = DatasourceEntryConstraintDataType.RegexList;
					}
					else
						throw new NotImplementedException("");
				}
				else if (Context.GetChild(1).GetText() == "matches")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.MatchesRegex;
					Constraint.ConstraintData = VisitRegexExpr(Context.regexExpr());
				}
				else if (Context.GetChild(1).GetText() == "does" && Context.GetChild(2).GetText() == "not" && Context.GetChild(3).GetText() == "match")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.DoesNotMatchRegex;
					Constraint.ConstraintData = VisitRegexExpr(Context.regexExpr());
				}
				else if (Context.GetChild(1).GetText() == "not" && Context.GetChild(2).GetText() == "like")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.NotLike;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "like")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.Like;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "does" && Context.GetChild(2).GetText() == "not" && Context.GetChild(3).GetText() == "start")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.DoesNotStartWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "starts")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.StartsWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "does" && Context.GetChild(2).GetText() == "not" && Context.GetChild(3).GetText() == "end")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.DoesNotEndWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "ends")
				{
					Constraint.Operator = DatasourceEntryConstraintOperator.EndsWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else
					throw new NotImplementedException("VisitDatasourceEntryConstraint");
			}

			return Constraint;
		}

		public override object VisitCopyDatasourceEntriesStatement ([NotNull] BlokScriptGrammarParser.CopyDatasourceEntriesStatementContext Context)
		{
			/*
			copyDatasourceEntriesStatement: 'copy' 'datasource' 'entries' ('from' | 'in') datasourceEntriesSourceLocation 'to' datasourceEntriesTargetLocation ('where' datasourceEntryConstraintExprList)?;
			*/
			DatasourceEntriesSourceLocation SourceLocation = (DatasourceEntriesSourceLocation)VisitDatasourceEntriesSourceLocation(Context.datasourceEntriesSourceLocation());

			//
			// GET THE SOURCE ENTRIES.
			//
			DatasourceEntryEntity[] SourceEntries;

			if (SourceLocation.FromDatasource)
			{
				DatasourceEntity SourceDatasource = SourceLocation.Datasource;
				EnsureDatasourceHasEntriesLoaded(SourceDatasource);
				SourceEntries = SourceDatasource.GetEntries();
			}
			else
				throw new NotImplementedException("VisitCopyDatasourceEntriesStatement");

			//
			// COPY TO THE TARGET LOCATION.
			//
			DatasourceEntriesTargetLocation TargetLocation = (DatasourceEntriesTargetLocation)VisitDatasourceEntriesTargetLocation(Context.datasourceEntriesTargetLocation());

			if (TargetLocation.ToDatasource)
			{
				DatasourceEntity TargetDatasource = TargetLocation.Datasource;

				foreach (DatasourceEntryEntity CurrentEntry in SourceEntries)
				{
					CopyEntryToDatasource(CurrentEntry, TargetDatasource);
				}
			}
			else
				throw new NotImplementedException("VisitCopyDatasourceEntriesStatement");

			return null;
		}

		public override object VisitDatasourceEntriesSourceLocation ([NotNull] BlokScriptGrammarParser.DatasourceEntriesSourceLocationContext Context)
		{
			/*
			datasourceEntriesSourceLocation: datasourceSpec
				| urlSpec
				| fileSpec
				| 'local cache'
				;
			*/
			DatasourceEntriesSourceLocation Location = new DatasourceEntriesSourceLocation();

			if (Context.datasourceSpec() != null)
			{
				Location.FromDatasource = true;
				Location.Datasource = (DatasourceEntity)VisitDatasourceSpec(Context.datasourceSpec());
			}
			else if (Context.urlSpec() != null)
			{
				Location.ToUrl = true;
				Location.UrlSpec = (UrlSpec)VisitUrlSpec(Context.urlSpec());
			}
			else if (Context.fileSpec() != null)
			{
				Location.FromFile = true;
				Location.FileSpec = (FileSpec)VisitFileSpec(Context.fileSpec());
			}
			else if (Context.GetChild(0).GetText() == "local")
			{
				Location.FromLocalCache = true;
			}
			else
				throw new NotImplementedException("VisitDatasourceEntriesSourceLocation");

			return Location;
		}

		public override object VisitDatasourceEntriesTargetLocation ([NotNull] BlokScriptGrammarParser.DatasourceEntriesTargetLocationContext Context)
		{
			/*
			datasourceEntriesTargetLocation: datasourceSpec
				| urlSpec
				| fileSpec
				| 'local cache'
				| 'console'
				;
			*/
			DatasourceEntriesTargetLocation Location = new DatasourceEntriesTargetLocation();

			if (Context.datasourceSpec() != null)
			{
				Location.ToDatasource = true;
				Location.Datasource = (DatasourceEntity)VisitDatasourceSpec(Context.datasourceSpec());
			}
			else if (Context.urlSpec() != null)
			{
				Location.ToUrl = true;
				Location.UrlSpec = (UrlSpec)VisitUrlSpec(Context.urlSpec());
			}
			else if (Context.fileSpec() != null)
			{
				Location.ToFile = true;
				Location.FileSpec = (FileSpec)VisitFileSpec(Context.fileSpec());
			}
			else if (Context.GetChild(0).GetText() == "local")
			{
				Location.ToLocalCache = true;
			}
			else if (Context.GetChild(0).GetText() == "console")
			{
				Location.ToConsole = true;
			}
			else
				throw new NotImplementedException("VisitDatasourceEntriesTargetLocation");

			return Location;
		}

		public override object VisitDatasourceEntryConstraintExprList ([NotNull] BlokScriptGrammarParser.DatasourceEntryConstraintExprListContext Context)
		{
			/*
			datasourceEntryConstraintExprList: datasourceEntryConstraintExpr (('and' | 'or') datasourceEntryConstraintExprList)?;
			*/
			if (Context.datasourceEntryConstraintExprList() != null)
			{
				DatasourceEntryConstraint RootConstraint = new DatasourceEntryConstraint();

				string OperatorToken = Context.GetChild(1).GetText();

				if (OperatorToken == "and")
					RootConstraint.Operator = DatasourceEntryConstraintOperator.Intersect;
				else if (OperatorToken == "or")
					RootConstraint.Operator = DatasourceEntryConstraintOperator.Union;

				RootConstraint.LeftChildConstraint = (DatasourceEntryConstraint)VisitDatasourceEntryConstraintExpr(Context.datasourceEntryConstraintExpr());
				RootConstraint.RightChildConstraint = (DatasourceEntryConstraint)VisitDatasourceEntryConstraintExprList(Context.datasourceEntryConstraintExprList());
				return RootConstraint;
			}
			else
			{
				return (DatasourceEntryConstraint)VisitDatasourceEntryConstraintExpr(Context.datasourceEntryConstraintExpr());
			}
		}

		public override object VisitUrlSpec ([NotNull] BlokScriptGrammarParser.UrlSpecContext Context)
		{
			/*
			urlSpec: ('csv' | 'json')? 'url' stringExpr;
			*/
			UrlSpec CreatedUrlSpec = new UrlSpec();
			CreatedUrlSpec.Url = (string)VisitStringExpr(Context.stringExpr());

			string Token = Context.GetChild(0).GetText();

			if (Token == "csv" || Token == "json")
				CreatedUrlSpec.ForcedMediaType = Token;

			return CreatedUrlSpec;
		}

		public override object VisitBlockConstraintExprList([NotNull] BlokScriptGrammarParser.BlockConstraintExprListContext Context)
		{
			/*
			blockConstraintExprList: blockConstraintExpr (('and' | 'or') blockConstraintExprList)?;
			*/
			BlockConstraint RootConstraint = new BlockConstraint();

			if (Context.blockConstraintExprList() != null)
			{
				string OperatorToken = Context.GetChild(1).GetText();
				Echo(OperatorToken);

				if (OperatorToken == "and")
					RootConstraint.Operator = BlockConstraintOperator.Intersect;
				else if (OperatorToken == "or")
					RootConstraint.Operator = BlockConstraintOperator.Union;
				else
					throw new NotImplementedException();

				RootConstraint.LeftChildConstraint = (BlockConstraint)VisitBlockConstraintExpr(Context.blockConstraintExpr());
				RootConstraint.RightChildConstraint = (BlockConstraint)VisitBlockConstraintExprList(Context.blockConstraintExprList());
			}
			else
			{
				RootConstraint.Operator = BlockConstraintOperator.Root;
				RootConstraint.ChildConstraint = (BlockConstraint)VisitBlockConstraintExpr(Context.blockConstraintExpr());
			}

			return RootConstraint;
		}

		public override object VisitBlockConstraintExpr([NotNull] BlokScriptGrammarParser.BlockConstraintExprContext Context)
		{
			/*
			blockConstraintExpr: blockConstraint (('and' | 'or') blockConstraintExpr)?
				| '(' blockConstraint (('and' | 'or') blockConstraintExpr)? ')'
				| '(' blockConstraintExpr (('and' | 'or') blockConstraintExpr)? ')'
				;
			*/
			if (Context.GetChild(0).GetText() == "(")
			{
				BlockConstraint RootConstraint = new BlockConstraint();
				RootConstraint.Operator = BlockConstraintOperator.Root;

				if (Context.blockConstraint() != null)
				{
					BlockConstraint ThisConstraint = (BlockConstraint)VisitBlockConstraint(Context.blockConstraint());

					if (Context.blockConstraintExpr().Length > 0)
					{
						BlockConstraint OpConstraint = new BlockConstraint();
						OpConstraint.LeftChildConstraint = ThisConstraint;
						OpConstraint.RightChildConstraint = (BlockConstraint)VisitBlockConstraintExpr(Context.blockConstraintExpr(0));

						string OperatorToken = Context.GetChild(2).GetText();

						if (OperatorToken == "and")
							OpConstraint.Operator = BlockConstraintOperator.Intersect;
						else if (OperatorToken == "or")
							OpConstraint.Operator = BlockConstraintOperator.Union;
						else
							throw new NotImplementedException();

						RootConstraint.ChildConstraint = OpConstraint;
					}
					else
					{
						RootConstraint.ChildConstraint = ThisConstraint;
					}
				}
				else
				{
					BlockConstraint OpConstraint = new BlockConstraint();
					OpConstraint.LeftChildConstraint = (BlockConstraint)VisitBlockConstraintExpr(Context.blockConstraintExpr()[0]);
					OpConstraint.RightChildConstraint = (BlockConstraint)VisitBlockConstraintExpr(Context.blockConstraintExpr()[1]);

					string OperatorToken = Context.GetChild(2).GetText();

					if (OperatorToken == "and")
						OpConstraint.Operator = BlockConstraintOperator.Intersect;
					else if (OperatorToken == "or")
						OpConstraint.Operator = BlockConstraintOperator.Union;
					else
						throw new NotImplementedException();

					RootConstraint.ChildConstraint = OpConstraint;
				}

				return RootConstraint;
			}
			else
			{
				// blockConstraint (('and' | 'or') blockConstraintExpr)?

				if (Context.blockConstraintExpr().Length > 0)
				{
					string OperatorToken = Context.GetChild(1).GetText();

					Echo(OperatorToken);

					BlockConstraint OpConstraint = new BlockConstraint();
					OpConstraint.LeftChildConstraint = (BlockConstraint)VisitBlockConstraint(Context.blockConstraint());
					OpConstraint.RightChildConstraint = (BlockConstraint)VisitBlockConstraintExpr(Context.blockConstraintExpr(0));

					if (OperatorToken == "and")
						OpConstraint.Operator = BlockConstraintOperator.Intersect;
					else if (OperatorToken == "or")
						OpConstraint.Operator = BlockConstraintOperator.Union;
					else
						throw new NotImplementedException();

					return OpConstraint;
				}
				else
					return (BlockConstraint)VisitBlockConstraint(Context.blockConstraint());
			}
		}

		public override object VisitStoryConstraintExprList([NotNull] BlokScriptGrammarParser.StoryConstraintExprListContext Context)
		{
			/*
			storyConstraintExprList: storyConstraintExpr (('and' | 'or') storyConstraintExprList)?;
			*/
			if (Context.storyConstraintExprList() != null)
			{
				StoryConstraint RootConstraint = new StoryConstraint();

				string OperatorToken = Context.GetChild(1).GetText();

				if (OperatorToken == "and")
					RootConstraint.Operator = StoryConstraintOperator.Intersect;
				else if (OperatorToken == "or")
					RootConstraint.Operator = StoryConstraintOperator.Union;

				RootConstraint.LeftChildConstraint = (StoryConstraint)VisitStoryConstraintExpr(Context.storyConstraintExpr());
				RootConstraint.RightChildConstraint = (StoryConstraint)VisitStoryConstraintExprList(Context.storyConstraintExprList());
				return RootConstraint;
			}
			else
				return VisitStoryConstraintExpr(Context.storyConstraintExpr());
		}

		public override object VisitStoryConstraintExpr([NotNull] BlokScriptGrammarParser.StoryConstraintExprContext Context)
		{
			/*
			storyConstraintExpr: storyConstraint (('and' | 'or') storyConstraintExpr)?
				| '(' storyConstraint (('and' | 'or') storyConstraintExpr)? ')'
				| '(' storyConstraintExpr (('and' | 'or') storyConstraintExpr)? ')'
				;
			*/
			if (Context.GetChild(0).GetText() == "(")
			{
				StoryConstraint RootConstraint = new StoryConstraint();
				RootConstraint.Operator = StoryConstraintOperator.Root;

				if (Context.storyConstraint() != null)
				{
					StoryConstraint ThisConstraint = (StoryConstraint)VisitStoryConstraint(Context.storyConstraint());

					if (Context.storyConstraintExpr().Length > 0)
					{
						StoryConstraint OpConstraint = new StoryConstraint();
						OpConstraint.LeftChildConstraint = ThisConstraint;
						OpConstraint.RightChildConstraint = (StoryConstraint)VisitStoryConstraintExpr(Context.storyConstraintExpr(0));

						string OperatorToken = Context.GetChild(2).GetText();

						if (OperatorToken == "and")
							OpConstraint.Operator = StoryConstraintOperator.Intersect;
						else if (OperatorToken == "or")
							OpConstraint.Operator = StoryConstraintOperator.Union;
						else
							throw new NotImplementedException();

						RootConstraint.ChildConstraint = OpConstraint;
					}
					else
					{
						RootConstraint.ChildConstraint = ThisConstraint;
					}
				}
				else
				{
					StoryConstraint OpConstraint = new StoryConstraint();
					OpConstraint.LeftChildConstraint = (StoryConstraint)VisitStoryConstraintExpr(Context.storyConstraintExpr()[0]);
					OpConstraint.RightChildConstraint = (StoryConstraint)VisitStoryConstraintExpr(Context.storyConstraintExpr()[1]);

					string OperatorToken = Context.GetChild(2).GetText();

					if (OperatorToken == "and")
						OpConstraint.Operator = StoryConstraintOperator.Intersect;
					else if (OperatorToken == "or")
						OpConstraint.Operator = StoryConstraintOperator.Union;
					else
						throw new NotImplementedException();

					RootConstraint.ChildConstraint = OpConstraint;
				}

				return RootConstraint;
			}
			else
			{
				// storyConstraint (('and' | 'or') storyConstraintExpr)?

				if (Context.storyConstraintExpr().Length > 0)
				{
					string OperatorToken = Context.GetChild(1).GetText();

					Echo(OperatorToken);

					StoryConstraint OpConstraint = new StoryConstraint();
					OpConstraint.LeftChildConstraint = (StoryConstraint)VisitStoryConstraint(Context.storyConstraint());
					OpConstraint.RightChildConstraint = (StoryConstraint)VisitStoryConstraintExpr(Context.storyConstraintExpr(0));

					if (OperatorToken == "and")
						OpConstraint.Operator = StoryConstraintOperator.Intersect;
					else if (OperatorToken == "or")
						OpConstraint.Operator = StoryConstraintOperator.Union;
					else
						throw new NotImplementedException();

					return OpConstraint;
				}
				else
					return (StoryConstraint)VisitStoryConstraint(Context.storyConstraint());
			}
		}

		public override object VisitDatasourceConstraintExprList([NotNull] BlokScriptGrammarParser.DatasourceConstraintExprListContext Context)
		{
			/*
			datasourceConstraintExprList: datasourceConstraintExpr (('and' | 'or') datasourceConstraintExprList)?;
			*/
			if (Context.datasourceConstraintExprList() != null)
			{
				DatasourceConstraint RootConstraint = new DatasourceConstraint();

				string OperatorToken = Context.GetChild(1).GetText();

				if (OperatorToken == "and")
					RootConstraint.Operator = DatasourceConstraintOperator.Intersect;
				else if (OperatorToken == "or")
					RootConstraint.Operator = DatasourceConstraintOperator.Union;

				RootConstraint.LeftChildConstraint = (DatasourceConstraint)VisitDatasourceConstraintExpr(Context.datasourceConstraintExpr());
				RootConstraint.RightChildConstraint = (DatasourceConstraint)VisitDatasourceConstraintExprList(Context.datasourceConstraintExprList());
				return RootConstraint;
			}
			else
				return VisitDatasourceConstraintExpr(Context.datasourceConstraintExpr());
		}

		public override object VisitDatasourceConstraintExpr([NotNull] BlokScriptGrammarParser.DatasourceConstraintExprContext Context)
		{
			/*
			datasourceConstraintExpr: datasourceConstraint (('and' | 'or') datasourceConstraintExpr)?
				| '(' datasourceConstraint (('and' | 'or') datasourceConstraintExpr)? ')'
				| '(' datasourceConstraintExpr (('and' | 'or') datasourceConstraintExpr)? ')'
				;
			*/
			if (Context.GetChild(0).GetText() == "(")
			{
				DatasourceConstraint RootConstraint = new DatasourceConstraint();
				RootConstraint.Operator = DatasourceConstraintOperator.Root;

				if (Context.datasourceConstraint() != null)
				{
					DatasourceConstraint ThisConstraint = (DatasourceConstraint)VisitDatasourceConstraint(Context.datasourceConstraint());

					if (Context.datasourceConstraintExpr().Length > 0)
					{
						DatasourceConstraint OpConstraint = new DatasourceConstraint();
						OpConstraint.LeftChildConstraint = ThisConstraint;
						OpConstraint.RightChildConstraint = (DatasourceConstraint)VisitDatasourceConstraintExpr(Context.datasourceConstraintExpr(0));

						string OperatorToken = Context.GetChild(2).GetText();

						if (OperatorToken == "and")
							OpConstraint.Operator = DatasourceConstraintOperator.Intersect;
						else if (OperatorToken == "or")
							OpConstraint.Operator = DatasourceConstraintOperator.Union;
						else
							throw new NotImplementedException();

						RootConstraint.ChildConstraint = OpConstraint;
					}
					else
					{
						RootConstraint.ChildConstraint = ThisConstraint;
					}
				}
				else
				{
					DatasourceConstraint OpConstraint = new DatasourceConstraint();
					OpConstraint.LeftChildConstraint = (DatasourceConstraint)VisitDatasourceConstraintExpr(Context.datasourceConstraintExpr()[0]);
					OpConstraint.RightChildConstraint = (DatasourceConstraint)VisitDatasourceConstraintExpr(Context.datasourceConstraintExpr()[1]);

					string OperatorToken = Context.GetChild(2).GetText();

					if (OperatorToken == "and")
						OpConstraint.Operator = DatasourceConstraintOperator.Intersect;
					else if (OperatorToken == "or")
						OpConstraint.Operator = DatasourceConstraintOperator.Union;
					else
						throw new NotImplementedException();

					RootConstraint.ChildConstraint = OpConstraint;
				}

				return RootConstraint;
			}
			else
			{
				// datasourceConstraint (('and' | 'or') datasourceConstraintExpr)?

				if (Context.datasourceConstraintExpr().Length > 0)
				{
					string OperatorToken = Context.GetChild(1).GetText();

					Echo(OperatorToken);

					DatasourceConstraint OpConstraint = new DatasourceConstraint();
					OpConstraint.LeftChildConstraint = (DatasourceConstraint)VisitDatasourceConstraint(Context.datasourceConstraint());
					OpConstraint.RightChildConstraint = (DatasourceConstraint)VisitDatasourceConstraintExpr(Context.datasourceConstraintExpr(0));

					if (OperatorToken == "and")
						OpConstraint.Operator = DatasourceConstraintOperator.Intersect;
					else if (OperatorToken == "or")
						OpConstraint.Operator = DatasourceConstraintOperator.Union;
					else
						throw new NotImplementedException();

					return OpConstraint;
				}
				else
					return (DatasourceConstraint)VisitDatasourceConstraint(Context.datasourceConstraint());
			}
		}

		public override object VisitDeleteBlocksStatement ([NotNull] BlokScriptGrammarParser.DeleteBlocksStatementContext Context)
		{ 
			/*
			deleteBlocksStatement: 'delete' 'blocks' ('in' | 'from') spaceSpec ('where' blockConstraintExprList)?;
			*/
			SpaceEntity SourceSpace = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());

			//
			// GET ALL BLOCKS.
			//
			BlockSchemaEntity[] Blocks = GetBlocksInSpace(SourceSpace.SpaceId);

			//
			// APPLY THE CONSTRAINT.
			//
			if (Context.blockConstraintExprList() != null)
			{
				BlockConstraint Constraint = (BlockConstraint)VisitBlockConstraintExprList(Context.blockConstraintExprList());
				Blocks = Constraint.Evaluate(Blocks);
			}

			//
			// DELETE THE BLOCKS.
			//
			DeleteBlocks(Blocks);

			return null;
		}

		public override object VisitDeleteDatasourcesStatement([NotNull] BlokScriptGrammarParser.DeleteDatasourcesStatementContext Context)
		{
			/*
			deleteDatasourcesStatement: 'delete' 'datasources' ('from' | 'in') spaceSpec ('where' datasourceConstraintExprList)?;
			*/
			SpaceEntity Space = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
			EnsureDatasourcesAreLoadedIntoSpace(Space);
			DatasourceEntity[] Datasources = GetDatasourcesInSpace(Space);

			if (Context.datasourceConstraintExprList() != null)
			{
				DatasourceConstraint Constraint = (DatasourceConstraint)VisitDatasourceConstraintExprList(Context.datasourceConstraintExprList());
				Datasources = Constraint.Evaluate(Datasources);
			}

			foreach (DatasourceEntity Datasource in Datasources)
			{
				string RequestPath = ManagementPathFactory.CreateDatasourcePath(Datasource.SpaceId, Datasource.DatasourceId);
				EchoAction($"API DELETE {RequestPath}. Deleting datasource {DatasourceFormatter.FormatHumanFriendly(Datasource)} in space {SpaceFormatter.FormatHumanFriendly(Space)}.");
				GetManagementWebClient().Delete(RequestPath, "");
				GetSpaceCacheById(Space.SpaceId).RemoveDatasource(Datasource);
			}

			return null;
		}

		public override object VisitCreateDatasourceStatement ([NotNull] BlokScriptGrammarParser.CreateDatasourceStatementContext Context)
		{
			/*
			createDatasourceStatement: 'create' 'datasource' (stringExpr | '(' datasourceUpdateList ')' ) ('for' | 'in') spaceSpec;
			*/
			SpaceEntity Space = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
			EnsureDatasourcesAreLoadedIntoSpace(Space);

			DatasourceEntity Datasource = new DatasourceEntity();
			Datasource.SpaceId = Space.SpaceId;

			if (Context.stringExpr() != null)
			{
				Datasource.Name = ((string)VisitStringExpr(Context.stringExpr())).Trim();
				Datasource.Slug = StringFormatter.FormatSlug(Datasource.Name);
			}
			else if (Context.datasourceUpdateList() != null)
			{
				foreach (UpdateModel CurrentUpdate in (UpdateModel[])VisitDatasourceUpdateList(Context.datasourceUpdateList()))
				{
					if (CurrentUpdate.Name == "name")
						Datasource.Name = (string)CurrentUpdate.Value;
					else if (CurrentUpdate.Name == "slug")
						Datasource.Slug = (string)CurrentUpdate.Value;
					else
						throw new NotImplementedException();
				}
			}
			else
				throw new NotImplementedException();

			if (Datasource.Slug == null)
				Datasource.Slug = StringFormatter.FormatSlug(Datasource.Name);

			Datasource.Data = DatasourceEntityDataFactory.CreateData(Datasource);

			CreateDatasourceInSpace(Datasource, Space);
			return null;
		}

		public override object VisitDatasourceUpdateList([NotNull] BlokScriptGrammarParser.DatasourceUpdateListContext Context)
		{
			/*
			datasourceUpdateList: datasourceUpdate (',' datasourceUpdateList)?;
			*/
			List<UpdateModel> UpdateModelList = new List<UpdateModel>();
			UpdateModelList.Add((UpdateModel)VisitDatasourceUpdate(Context.datasourceUpdate()));

			if (Context.datasourceUpdateList() != null)
				UpdateModelList.AddRange((UpdateModel[])VisitDatasourceUpdateList(Context.datasourceUpdateList()));
			
			return UpdateModelList.ToArray();
		}

		public override object VisitDatasourceUpdate ([NotNull] BlokScriptGrammarParser.DatasourceUpdateContext Context)
		{
			/*
			datasourceUpdate: 'name' '=' stringExpr
				| 'slug' '=' stringExpr
				;
			*/
			UpdateModel Update = new UpdateModel();
			Update.Name = Context.GetChild(0).GetText();
			Update.Value = (string)VisitStringExpr(Context.stringExpr());
			return Update;
		}

		public override object VisitDeleteDatasourceStatement([NotNull] BlokScriptGrammarParser.DeleteDatasourceStatementContext Context)
		{
			/*
			deleteDatasourceStatement: 'delete' 'datasource' (datasourceShortSpec | datasourceSpec);
			*/
			DatasourceEntity Datasource;

			if (Context.datasourceSpec() != null)
				Datasource = (DatasourceEntity)VisitDatasourceSpec(Context.datasourceSpec());
			else
				Datasource = (DatasourceEntity)VisitDatasourceShortSpec(Context.datasourceShortSpec());

			if (Datasource == null)
				return null;

			//EnsureDatasourcesAreLoadedIntoSpace(Space);

			DeleteDatasource(Datasource);
			return null;
		}

		public void DeleteDatasource (DatasourceEntity Datasource)
		{
			string DatasourceId = Datasource.DatasourceId;
			string SpaceId = Datasource.SpaceId;
			SpaceEntity Space = GetSpaceCacheById(SpaceId).Space;

			string RequestPath = ManagementPathFactory.CreateDatasourcePath(SpaceId, DatasourceId);
			EchoAction($"API DELETE {RequestPath}. Deleting datasource {DatasourceFormatter.FormatHumanFriendly(Datasource)}  in space {SpaceFormatter.FormatHumanFriendly(Space)}.");
			GetManagementWebClient().Delete(RequestPath, "");

			NotifyDatasourceDeleted(Datasource);
		}

		public void NotifyDatasourceDeleted (DatasourceEntity Datasource)
		{
			string SpaceId = Datasource.SpaceId;
			GetSpaceCacheById(SpaceId).RemoveDatasource(Datasource);
		}

		public override object VisitUpdateDatasourceStatement([NotNull] BlokScriptGrammarParser.UpdateDatasourceStatementContext Context)
		{
			/*
			updateDatasourceStatement: 'update' 'datasource' (datasourceShortSpec | datasourceSpec) 'set' datasourceUpdateList;
			*/
			DatasourceEntity Datasource;
			
			if (Context.datasourceSpec() != null)
				Datasource = (DatasourceEntity)VisitDatasourceSpec(Context.datasourceSpec());
			else
				Datasource = (DatasourceEntity)VisitDatasourceShortSpec(Context.datasourceShortSpec());

			foreach (UpdateModel CurrentUpdate in (UpdateModel[])VisitDatasourceUpdateList(Context.datasourceUpdateList()))
			{
				if (CurrentUpdate.Name == "name")
					Datasource.Name = (string)CurrentUpdate.Value;
				else if (CurrentUpdate.Name == "slug")
					Datasource.Slug = (string)CurrentUpdate.Value;
				else
					throw new NotImplementedException();
			}

			Datasource.Data = DatasourceEntityDataFactory.CreateData(Datasource);

			PersistDatasource(Datasource);
			return null;
		}

		public object CreateDatasourceEntryData (DatasourceEntryEntity Entry)
		{
			return DatasourceEntryEntityDataFactory.CreateData(Entry);
		}

		public override object VisitDatasourceEntryUpdateList ([NotNull] BlokScriptGrammarParser.DatasourceEntryUpdateListContext Context)
		{
			/*
			datasourceEntryUpdateList: datasourceEntryUpdate (',' datasourceEntryUpdateList)?;
			*/
			List<DatasourceEntryUpdate> UpdateList = new List<DatasourceEntryUpdate>();
			UpdateList.Add((DatasourceEntryUpdate)VisitDatasourceEntryUpdate(Context.datasourceEntryUpdate()));

			if (Context.datasourceEntryUpdateList() != null)
				UpdateList.AddRange((DatasourceEntryUpdate[])VisitDatasourceEntryUpdateList(Context.datasourceEntryUpdateList()));
			
			return UpdateList.ToArray();
		}

		public override object VisitDatasourceEntryUpdate ([NotNull] BlokScriptGrammarParser.DatasourceEntryUpdateContext Context)
		{
			/*
			datasourceEntryUpdate: 'name' '=' stringExpr
				| 'value' '=' stringExpr
				;
			*/
			DatasourceEntryUpdate Update = new DatasourceEntryUpdate();
			Update.Name = Context.GetChild(0).GetText();
			Update.Value = (string)VisitStringExpr(Context.stringExpr());
			return Update;
		}

		public override object VisitDatasourceShortSpec ([NotNull] BlokScriptGrammarParser.DatasourceShortSpecContext Context)
		{
			/*
			datasourceShortSpec: (VARID | INTLITERAL | STRINGLITERAL) 'in' spaceSpec;
			*/
			SpaceEntity Space = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());

			if (Context.VARID() != null)
			{
				string SymbolName = Context.VARID().GetText();
				BlokScriptSymbol Symbol = _SymbolTableManager.GetSymbol(SymbolName);

				if (Symbol.Type == BlokScriptSymbolType.String)
				{
					string NameOrSlug = (string)Symbol.Value;
					SpaceCache CurrentSpaceCache = GetSpaceCacheWithDataSourcesLoaded(Space.SpaceId);

					if (CurrentSpaceCache.ContainsDatasourceBySlug(NameOrSlug))
						return CurrentSpaceCache.GetDatasourceBySlug(NameOrSlug);
					else if (CurrentSpaceCache.ContainsDatasourceByName(NameOrSlug))
						return CurrentSpaceCache.GetDatasourceByName(NameOrSlug);

					string ErrorMessage = $"Datasource '{NameOrSlug}' not found in space {SpaceFormatter.FormatHumanFriendly(Space)}.";
					EchoError(ErrorMessage);
					return null;
				}
				else if (Symbol.Type == BlokScriptSymbolType.Int32)
				{
					int DatasourceId = (int)Symbol.Value;
					SpaceCache CurrentSpaceCache = GetSpaceCacheWithDataSourcesLoaded(Space.SpaceId);

					if (!CurrentSpaceCache.ContainsDatasourceById(DatasourceId))
					{
						string ErrorMessage = $"Datasource with id '{DatasourceId}' not found in space '{Space.Name} ({Space.SpaceId})'.";
						EchoError(ErrorMessage);
						return null;
					}

					return CurrentSpaceCache.GetDatasourceById(DatasourceId);
				}
				else
				{
					string ErrorMessage = $"Cannot use variable '{Symbol.Name}' of type '{Symbol.Type}' in a datasource entry literal expression.  Variable type must be 'string' or 'int'.";
					EchoError(ErrorMessage);
					return null;
				}
			}
			else if (Context.STRINGLITERAL() != null)
			{
				//
				// GET THE DATASOURCE USING THE SLUG.
				//
				string NameOrSlug = StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText());
				SpaceCache CurrentSpaceCache = GetSpaceCacheWithDataSourcesLoaded(Space.SpaceId);

				if (CurrentSpaceCache.ContainsDatasourceBySlug(NameOrSlug))
					return CurrentSpaceCache.GetDatasourceBySlug(NameOrSlug);
				else if (CurrentSpaceCache.ContainsDatasourceByName(NameOrSlug))
					return CurrentSpaceCache.GetDatasourceByName(NameOrSlug);

				string ErrorMessage = $"Datasource '{NameOrSlug}' not found in space {SpaceFormatter.FormatHumanFriendly(Space)}.";
				EchoError(ErrorMessage);
				return null;
			}
			else if (Context.INTLITERAL() != null)
			{
				int DatasourceId = Int32.Parse(Context.INTLITERAL().GetText());
				SpaceCache CurrentSpaceCache = GetSpaceCacheWithDataSourcesLoaded(Space.SpaceId);

				if (!CurrentSpaceCache.ContainsDatasourceById(DatasourceId))
				{
					string ErrorMessage = $"Datasource with id '{DatasourceId}' not found in space '{Space.Name} ({Space.SpaceId})'.";
					EchoError(ErrorMessage);
					return null;
				}

				return CurrentSpaceCache.GetDatasourceById(DatasourceId);
			}
			else
				throw new NotImplementedException();
		}

		public override object VisitShortSpaceSpec ([NotNull] BlokScriptGrammarParser.ShortSpaceSpecContext Context)
		{
			/*
			shortSpaceSpec: INTLITERAL | STRINGLITERAL;
			*/
			Regex IdRegex = new Regex("^[0-9]+$");
			Regex CopiedRegex = new Regex("^#[0-9]+$");

			string SpaceId = null;
			string SpaceName = null;

			if (Context.INTLITERAL() != null)
			{
				//
				// GET THE SPACE BY ID (INTEGER PROVIDED).
				//
				SpaceId = Int32.Parse(Context.INTLITERAL().GetText()).ToString();
			}
			else if (Context.STRINGLITERAL() != null)
			{
				//
				// IF WE GET A STRING LITERAL, THIS CAN TAKE THE FOLLOWING FORMS:
				// '1234567'
				// '#1234567' (COPIED AND PASTED FROM STORYBLOK)
				// 'Space Name'
				//
				string SpaceNameOrId = SpaceLiteralTrimmer.Trim(StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText()));

				if (IdRegex.IsMatch(SpaceNameOrId))
				{
					//
					// '1234567'
					//
					SpaceId = SpaceNameOrId;
				}
				else if (CopiedRegex.IsMatch(SpaceNameOrId))
				{
					//
					// '#1234567' (COPIED AND PASTED FROM STORYBLOK)
					//
					SpaceId = SpaceNameOrId.Substring(1);
				}
				else
				{
					//
					// 'Space Name'
					//
					SpaceName = SpaceNameOrId;
				}
			}
			else
				throw new NotImplementedException();

			//
			// SINCE WE ARE GETTING A SPACE, WE START BY GETTING ALL SPACES.
			//
			EnsureSpaceDictsLoaded();

			if (SpaceId != null)
			{
				if (!_SpaceCacheByIdDict.ContainsKey(SpaceId))
				{
					string ErrorMessage = $"Space with id '{SpaceId}' not found.";
					EchoError(ErrorMessage);
					throw new SpaceObjectNotFoundException(ErrorMessage);
				}

				return GetSpaceCacheWithSpaceDataLoadedById(SpaceId).Space;
			}
			else if (SpaceName != null)
			{
				if (!_SpaceCacheByNameDict.ContainsKey(SpaceName))
				{
					string ErrorMessage = $"Space with name '{SpaceName}' not found.";
					EchoError(ErrorMessage);
					throw new SpaceObjectNotFoundException(ErrorMessage);
				}

				return GetSpaceCacheWithSpaceDataLoadedByName(SpaceName).Space;
			}

			return null;
		}


		public void DeleteBlocks (BlockSchemaEntity[] Blocks)
		{
			foreach (BlockSchemaEntity Block in Blocks)
			{
				DeleteBlock(Block);
			}
		}

		public void DeleteBlock (BlockSchemaEntity Block)
		{
			if (Block.DataLocation == BlokScriptEntityDataLocation.Server)
				DeleteBlockFromServer(Block);
			else if (Block.DataLocation == BlokScriptEntityDataLocation.FilePath)
				DeleteBlockFromFileSystem(Block);
		}

		public void DeleteBlockFromServer (BlockSchemaEntity Block)
		{
			string RequestPath = ManagementPathFactory.CreateComponentPath(Block.BlockId, Block.SpaceId);
			EchoAction($"API DELETE {RequestPath}. Deleting block {BlockFormatter.FormatHumanFriendly(Block)} from space {SpaceFormatter.FormatHumanFriendly(GetSpaceById(Block.SpaceId))}.");
			GetManagementWebClient().Delete(RequestPath, "");
		}

		public void DeleteBlockFromFileSystem (BlockSchemaEntity Block)
		{
			File.Delete(Block.FilePath);
		}

		public StoryblokManagementWebClient GetManagementWebClient ()
		{
			StoryblokManagementWebClient CreatedWebClient = new StoryblokManagementWebClient();
			CreatedWebClient.BaseUrl = _SymbolTableManager.GetSymbolValueAsString("_GlobalManagementApiBaseUrl");
			CreatedWebClient.Token = _SymbolTableManager.GetSymbolValueAsString("_GlobalPersonalAccessToken");
			return CreatedWebClient;
		}

		public StoryblokContentDeliveryWebClient GetContentDeliveryWebClient ()
		{
			StoryblokContentDeliveryWebClient CreatedWebClient = new StoryblokContentDeliveryWebClient();
			return CreatedWebClient;
		}

		public SpaceCache GetSpaceCacheById (string SpaceId)
		{
			return _SpaceCacheByIdDict[SpaceId];
		}

		public SpaceEntity GetSpaceById (string SpaceId)
		{
			return _SpaceCacheByIdDict[SpaceId].Space;
		}

		public SpaceCache GetSpaceCacheByName (string SpaceName)
		{
			if (!_SpaceCacheByNameDict.ContainsKey(SpaceName))
			{
				SpaceCache CreatedCache = new SpaceCache();
				CreatedCache.SpaceName = SpaceName;
				_SpaceCacheByNameDict[SpaceName] = CreatedCache;
			}

			return _SpaceCacheByNameDict[SpaceName];
		}

		public SpaceCache GetSpaceCacheWithSpaceDataLoadedById (string SpaceId)
		{
			SpaceCache TargetSpaceCache = GetSpaceCacheById(SpaceId);

			if (TargetSpaceCache.SpaceDataLoaded)
				return TargetSpaceCache;

			LoadSpaceCacheSpaceData(TargetSpaceCache);

			return TargetSpaceCache;
		}

		public SpaceCache GetSpaceCacheWithSpaceDataLoadedByName (string SpaceName)
		{
			SpaceCache TargetSpaceCache = GetSpaceCacheByName(SpaceName);

			if (TargetSpaceCache.SpaceDataLoaded)
				return TargetSpaceCache;

			LoadSpaceCacheSpaceData(TargetSpaceCache);
			return TargetSpaceCache;
		}

		public SpaceCache GetSpaceCacheWithBlocksLoaded (string SpaceId)
		{
			SpaceCache TargetSpaceCache = GetSpaceCacheById(SpaceId);

			if (TargetSpaceCache.ComponentsLoaded)
				return TargetSpaceCache;

			LoadSpaceCacheBlocks(TargetSpaceCache);

			return TargetSpaceCache;
		}

		public SpaceCache GetSpaceCacheWithStoriesLoaded (string SpaceId)
		{
			SpaceCache TargetSpaceCache = GetSpaceCacheById(SpaceId);

			if (TargetSpaceCache.StoriesLoaded)
				return TargetSpaceCache;

			LoadSpaceCacheStories(TargetSpaceCache);

			return TargetSpaceCache;
		}

		public SpaceCache GetSpaceCacheWithDataSourcesLoaded (string SpaceId)
		{
			SpaceCache TargetSpaceCache = GetSpaceCacheById(SpaceId);

			if (TargetSpaceCache.DatasourcesLoaded)
				return TargetSpaceCache;

			LoadSpaceCacheDatasources(TargetSpaceCache);

			return TargetSpaceCache;
		}

		public void EnsureDatasourcesAreLoadedIntoSpace (SpaceEntity Space)
		{
			GetSpaceCacheWithDataSourcesLoaded(Space.SpaceId);
		}

		public DatasourceEntity[] GetDatasourcesInSpace (SpaceEntity Space)
		{
			return GetSpaceCacheWithDataSourcesLoaded(Space.SpaceId).GetDatasources();
		}

		public void LoadSpaceCacheSpaceData (SpaceCache TargetSpaceCache)
		{
			SpaceEntity Space = TargetSpaceCache.Space;

			string RequestPath = ManagementPathFactory.CreateSpacePath(TargetSpaceCache.SpaceId);

			EchoAction($"API GET {RequestPath}. Caching space {SpaceFormatter.FormatHumanFriendly(Space)} data.");
			SpaceEntity TargetSpace = SpaceParser.Parse(GetManagementWebClient().GetString(RequestPath));
			TargetSpace.DataLocation = BlokScriptEntityDataLocation.Server;
			TargetSpace.ServerPath = RequestPath;

			TargetSpaceCache.Space = TargetSpace;
			TargetSpaceCache.SpaceDataLoaded = true;
		}

		public void LoadSpaceCacheBlocks (SpaceCache TargetSpaceCache)
		{
			string SpaceId = TargetSpaceCache.SpaceId;
			string RequestPath = ManagementPathFactory.CreateComponentsPath(SpaceId);

			EchoAction($"API GET {RequestPath}. Getting all blocks in space '{SpaceId}'.");

			foreach (BlockSchemaEntity Block in ComponentsResponseReader.ReadResponseString(GetManagementWebClient().GetString(RequestPath)))
			{
				Block.SpaceId = SpaceId;
				Block.DataLocation = BlokScriptEntityDataLocation.Server;
				TargetSpaceCache.InsertBlock(Block);
			}

			TargetSpaceCache.ComponentsLoaded = true;
		}

		public void LoadSpaceCacheStories (SpaceCache TargetSpaceCache)
		{
			SpaceEntity Space = TargetSpaceCache.Space;
			string SpaceId = Space.SpaceId;

			string RequestPath = ManagementPathFactory.CreateStoriesPath(SpaceId);
			EchoAction($"API GET {RequestPath}. Caching stories in space {SpaceFormatter.FormatHumanFriendly(Space)}.");
			string ResponseString = GetManagementWebClient().GetString(RequestPath);

			StoryEntity[] Stories = StoriesResponseReader.ReadResponseString(ResponseString);

			foreach (StoryEntity Story in Stories)
			{
				Story.SpaceId = TargetSpaceCache.SpaceId;
				Story.DataLocation = BlokScriptEntityDataLocation.Server;
				Story.ServerPath = RequestPath;
				TargetSpaceCache.InsertStory(Story);
			}

			EchoVerbose($"Cached {Stories.Length} stories in space {SpaceFormatter.FormatHumanFriendly(Space)}.");

			TargetSpaceCache.StoriesLoaded = true;
		}

		public void LoadSpaceCacheDatasources (SpaceCache TargetSpaceCache)
		{
			string RequestPath = ManagementPathFactory.CreateDatasourcesPath(TargetSpaceCache.SpaceId);

			if (_Log.IsDebugEnabled)
				_Log.Debug($"LoadSpaceCacheDatasources: GET {RequestPath}");

			EchoAction($"API GET {RequestPath}. Getting datasources in space '{TargetSpaceCache.SpaceId}'.");

			string ResponseString = GetManagementWebClient().GetString(RequestPath);

			DatasourceEntity[] Datasources = DatasourcesResponseReader.ReadResponseString(ResponseString, TargetSpaceCache.SpaceId);

			foreach (DatasourceEntity Datasource in Datasources)
			{
				Datasource.DataLocation = BlokScriptEntityDataLocation.Server;
				Datasource.ServerPath = RequestPath;
				TargetSpaceCache.InsertDatasource(Datasource);
			}

			TargetSpaceCache.DatasourcesLoaded = true;
		}

		public void LoadDatasourceEntries (DatasourceEntity Datasource)
		{
			string DatasourceId = Datasource.DatasourceId;
			string RequestPath = ManagementPathFactory.CreateDatasourceEntriesPath(Datasource.SpaceId, DatasourceId);

			EchoAction($"API GET {RequestPath}. Getting datasource entries in datasource '{DatasourceId}' space '{Datasource.SpaceId}'.");

			string ResponseString = GetManagementWebClient().GetString(RequestPath);

			EchoDebug("ResponseString", ResponseString);

			foreach (DatasourceEntryEntity Entry in DatasourceEntriesResponseReader.ReadResponseString(ResponseString))
			{
				Entry.DatasourceId = Datasource.DatasourceId;
				Entry.DataLocation = BlokScriptEntityDataLocation.Server;
				Entry.ServerPath = RequestPath;
				Datasource.InsertDatasourceEntry(Entry);
			}

			Datasource.DatasourceEntriesLoaded = true;
		}

		public void	EnsureDatasourceHasEntriesLoaded (DatasourceEntity Datasource)
		{
			if (!Datasource.DatasourceEntriesLoaded)
				LoadDatasourceEntries(Datasource);
		}

		public void	CopyEntryToDatasource (DatasourceEntryEntity SourceEntry, DatasourceEntity Datasource)
		{
			EnsureDatasourceHasEntriesLoaded(Datasource);
			DatasourceEntryEntity TargetEntry;

			if (Datasource.HasEntryByName(SourceEntry.Name))
			{
				TargetEntry = Datasource.GetEntryByName(SourceEntry.Name);
				TargetEntry.Name = SourceEntry.Name;
				TargetEntry.Value = SourceEntry.Value;
				TargetEntry.DatasourceId = Datasource.DatasourceId;
				TargetEntry.Data = CreateDatasourceEntryData(TargetEntry);
			}
			else
			{
				TargetEntry = new DatasourceEntryEntity();
				TargetEntry.Name = SourceEntry.Name;
				TargetEntry.Value = SourceEntry.Value;
				TargetEntry.DatasourceId = Datasource.DatasourceId;
				TargetEntry.Data = CreateDatasourceEntryData(TargetEntry);
				Datasource.InsertDatasourceEntry(TargetEntry);
			}

			FlushDatasourceEntry(Datasource, TargetEntry);
		}

		public void FlushDatasourceEntry (DatasourceEntity Datasource, DatasourceEntryEntity Entry)
		{
			if (Datasource.DataLocation == BlokScriptEntityDataLocation.Server)
			{
				FlushDatasourceEntryToServer(Datasource, Entry);
			}
			else if (Datasource.DataLocation == BlokScriptEntityDataLocation.FilePath)
			{
				FlushDatasourceEntryToFile(Datasource, Entry);
			}
			else
				throw new NotImplementedException("FlushDatasourceEntry");
		}

		public void FlushDatasourceEntryToServer (DatasourceEntity Datasource, DatasourceEntryEntity Entry)
		{
			if (Entry.DatasourceEntryId == null)
			{
				//
				// THIS IS A NEW DATASOURCE ENTRY.  CREATE IT.
				//
				string RequestPath = ManagementPathFactory.CreateDatasourceEntriesPath(Datasource.SpaceId, Datasource.DatasourceId);
				EchoAction($"API POST {RequestPath}. Creating datasource entry '{Entry.Name}'.");
				GetManagementWebClient().PostJson(RequestPath, JsonSerializer.Serialize(Entry.Data));
			}
			else
			{
				//
				// THIS IS AN EXISTING DATASOURCE ENTRY.  UPDATE IT.
				//
				string RequestPath = ManagementPathFactory.CreateDatasourceEntryPath(Datasource.SpaceId, Entry.DatasourceEntryId);
				EchoAction($"API PUT {RequestPath}. Updating datasource entry '{Entry.DatasourceEntryId}'.");
				GetManagementWebClient().PutJson(RequestPath, JsonSerializer.Serialize(Entry.Data));
			}
		}

		public void FlushDatasourceEntryToFile (DatasourceEntity Datasource, DatasourceEntryEntity Entry)
		{
			JsonFileWriter.Write(Entry.Data, Entry.FilePath);
		}

		public BlockSchemaEntity[] GetBlocksInSpace (string SpaceId)
		{
			SpaceCache CurrentSpaceCache = GetSpaceCacheWithBlocksLoaded(SpaceId);

			List<BlockSchemaEntity> BlockSchemaEntity = new List<BlockSchemaEntity>();
			BlockSchemaEntity.AddRange(CurrentSpaceCache.BlockSchemaEntities.Values);
			return BlockSchemaEntity.ToArray();
		}

		public void EnsureSpaceDictsLoaded ()
		{
			if (_SpaceDictsLoaded)
				return;

			LoadSpaceDicts();
			_SpaceDictsLoaded = true;
		}

		public void LoadSpaceDicts ()
		{
			string RequestPath = ManagementPathFactory.CreateSpacesPath();
			EchoAction($"API GET {RequestPath}. Caching all spaces.");

			foreach (SpaceEntity Space in SpacesResponseReader.ReadResponseString(GetManagementWebClient().GetString(RequestPath)))
			{
				Space.DataLocation = BlokScriptEntityDataLocation.Server;
				Space.ServerPath = RequestPath;
				CacheSpace(Space);
			}
		}

		public void CacheSpace (SpaceEntity Space)
		{
			SpaceCache IdCache = new SpaceCache();
			IdCache.SpaceId = Space.SpaceId;
			IdCache.SpaceName = Space.Name;
			IdCache.Space = Space;
			_SpaceCacheByIdDict[Space.SpaceId] = IdCache;

			SpaceCache NameCache = new SpaceCache();
			NameCache.SpaceId = Space.SpaceId;
			NameCache.SpaceName = Space.Name;
			NameCache.Space = Space;
			_SpaceCacheByNameDict[Space.Name] = NameCache;
		}

		public bool ShouldBeVerbose ()
		{
			return _SymbolTableManager.GetSymbolValueAsInt32("_GlobalVerbosity") >= (int)BlokScriptVerbosity.Verbose;
		}

		public bool ShouldBeDebugger ()
		{
			return _SymbolTableManager.GetSymbolValueAsInt32("_GlobalVerbosity") >= (int)BlokScriptVerbosity.Debugger;
		}

		public void Echo (string Message)
		{
			Console.WriteLine(Message);
		}

		public void EchoInfo (string Message)
		{
			Console.WriteLine(Message);
		}

		public void EchoVerbose (string Message)
		{
			if (ShouldBeVerbose())
				Console.WriteLine(Message);
		}

		public void EchoDebug (string Message)
		{
			if (ShouldBeDebugger())
				Console.WriteLine(Message);
		}

		public void EchoDebug (Exception E)
		{
			if (ShouldBeDebugger())
			{
				Console.WriteLine(E.Message);
				Console.WriteLine(E.StackTrace);

				if (E.InnerException != null)
					EchoDebug(E.InnerException);
			}
		}

		public void EchoDebug (string Message, string Data)
		{
			if (ShouldBeDebugger())
			{
				Console.WriteLine(Message);
				Console.WriteLine(Data);
			}
		}

		public void EchoAction (string Message)
		{
			if (ShouldBeVerbose())
				Console.WriteLine($"{_ActionNumber++}. " + Message);
		}
		
		public void EchoError (string Message)
		{
			Console.WriteLine($"ERROR: {Message}");
		}

		public void EchoWarning (string Message)
		{
			Console.WriteLine($"WARNING: {Message}");
		}

		public void EchoToConsole (string Message)
		{
			Console.WriteLine(Message);
		}

		public string WorkingDir
		{
			set
			{
				_WorkingDir = value;
			}
		}

		private BlokScriptSymbolTableManager _SymbolTableManager;
		private Dictionary<string, SpaceCache> _SpaceCacheByIdDict;
		private Dictionary<string, SpaceCache> _SpaceCacheByNameDict;
		private bool _SpaceDictsLoaded;
		private int _ActionNumber = 1;
		private string _WorkingDir;

		private static ILog _Log;
	}
}
