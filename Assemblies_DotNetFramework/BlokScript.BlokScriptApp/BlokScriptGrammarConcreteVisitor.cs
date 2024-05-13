using System;
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
using System.Runtime.InteropServices.ComTypes;
using BlokScript.Filters;

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
				CurrentSymbol.Name = "_GlobalManagementBaseUrl";
				CurrentSymbol.Type = BlokScriptSymbolType.String;
				CurrentSymbol.Value = "https://api-us.storyblok.com";
				CreatedSymbolTable[CurrentSymbol.Name] = CurrentSymbol;
			}

			{
				BlokScriptSymbol CurrentSymbol = new BlokScriptSymbol();
				CurrentSymbol.Name = "_GlobalManagementRegion";
				CurrentSymbol.Type = BlokScriptSymbolType.String;
				CurrentSymbol.Value = "us";
				CreatedSymbolTable[CurrentSymbol.Name] = CurrentSymbol;
			}

			{
				BlokScriptSymbol CurrentSymbol = new BlokScriptSymbol();
				CurrentSymbol.Name = "_GlobalVerbosity";
				CurrentSymbol.Type = BlokScriptSymbolType.String;
				CurrentSymbol.Value = BlokScriptVerbosity.Verbose;
				CreatedSymbolTable[CurrentSymbol.Name] = CurrentSymbol;
			}

			{
				BlokScriptSymbol CurrentSymbol = new BlokScriptSymbol();
				CurrentSymbol.Name = "_GlobalManagementUserName";
				CurrentSymbol.Type = BlokScriptSymbolType.String;
				CreatedSymbolTable[CurrentSymbol.Name] = CurrentSymbol;
			}

			{
				BlokScriptSymbol CurrentSymbol = new BlokScriptSymbol();
				CurrentSymbol.Name = "_GlobalManagementPassword";
				CurrentSymbol.Type = BlokScriptSymbolType.String;
				CreatedSymbolTable[CurrentSymbol.Name] = CurrentSymbol;
			}

			{
				BlokScriptSymbol CurrentSymbol = new BlokScriptSymbol();
				CurrentSymbol.Name = "_GlobalManagementToken";
				CurrentSymbol.Type = BlokScriptSymbolType.String;
				CurrentSymbol.Value = "8NiAUbm84CFQebcUP5XuWwtt-231628-YCRGz_Uz8Yt5Fj3MqHbK";
				CreatedSymbolTable[CurrentSymbol.Name] = CurrentSymbol;
			}

			{
				BlokScriptSymbol CurrentSymbol = new BlokScriptSymbol();
				CurrentSymbol.Name = "_GlobalDefaultSpaceId";
				CurrentSymbol.Type = BlokScriptSymbolType.String;
				CreatedSymbolTable[CurrentSymbol.Name] = CurrentSymbol;
			}

			_SymbolTableManager = new BlokScriptSymbolTableManager();
			_SymbolTableManager.PushSymbolTable(CreatedSymbolTable);

			//
			// CREATE THE LOCAL CACHE.
			//
			_SpaceCacheDict = new Dictionary<string, SpaceCache>();

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
			return VisitChildren(Context);
		}

		public override object VisitSpaceVarStatement ([NotNull] BlokScriptGrammarParser.SpaceVarStatementContext Context)
		{
			//
			// space sourceSpace;
			// space sourceSpace = space sourceSpaceId;
			// space sourceSpace = space '12345';
			//

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
			//
			// string sourceSpaceId;
			// string sourceSpaceId = '1019179';
			//
			/*
			stringVarStatement: 'string' VARID ('=' (STRINGLITERAL | VARID | stringExpr))?;
			*/
			string SymbolName = Context.VARID()[0].GetText();

			BlokScriptSymbol CreatedSymbol = new BlokScriptSymbol();
			CreatedSymbol.Name = SymbolName;
			CreatedSymbol.Type = BlokScriptSymbolType.String;

			if (Context.STRINGLITERAL() != null)
			{
				CreatedSymbol.Value = StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText());
			}
			else if (Context.VARID().Length > 1)
			{
				CreatedSymbol.Value = _SymbolTableManager.GetSymbolValueAsString(Context.VARID()[1].GetText());
			}
			else if (Context.stringExpr() != null)
			{
				CreatedSymbol.Value = (string)VisitStringExpr(Context.stringExpr());
			}
			else
				throw new NotImplementedException("VisitStringVarStatement");

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
			if (Context.GetChild(0).GetText() == "space")
			{
				//
				// GET THE SPACE ID.
				//
				string SpaceId = null;

				if (Context.INTLITERAL() != null)
				{
					SpaceId = Int32.Parse(Context.INTLITERAL().GetText()).ToString();
				}
				else if (Context.STRINGLITERAL() != null)
				{
					SpaceId = SpaceLiteralTrimmer.Trim(StringLiteralTrimmer.Trim(Context.STRINGLITERAL().GetText()));
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

					if (Symbol.Type == BlokScriptSymbolType.Int32)
						SpaceId = ((int)Symbol.Value).ToString();
					else if (Symbol.Type == BlokScriptSymbolType.String)
						SpaceId = (string)Symbol.Value;
					else
					{
						EchoError($"Cannot use variable {SymbolName} of type {Symbol.Type} to identify a space.");
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
					GetFrom.DataLocation = BlokScriptEntityDataLocation.OnDemand;
				}

				BlokScriptEntityDataLocation DataLocation = GetFrom.DataLocation;

				//
				// CREATE THE SPACE ENTITY.
				//
				SpaceEntity Space = new SpaceEntity();
				Space.SpaceId = SpaceId;
				Space.DataLocation = DataLocation;

				if (DataLocation == BlokScriptEntityDataLocation.OnDemand)
				{
					//
					// DATA IS PULLED ON DEMAND WHEN IT'S NEEDED. NOTHING TO DO HERE.
					//
				}
				else if (DataLocation == BlokScriptEntityDataLocation.LocalCache)
				{
					//
					// ENTITY DATA MUST BE PULLED FROM THE LOCAL CACHE RIGHT NOW.
					//
					SpaceCache TargetSpaceCache = GetSpaceCache(SpaceId);

					if (TargetSpaceCache == null)
					{
						Console.WriteLine($"Space {SpaceId} not found in local cache.");
						throw new CacheMissException(SpaceId);
					}

					Space.Data = TargetSpaceCache.Space.Data;
				}
				else if (DataLocation == BlokScriptEntityDataLocation.FilePath)
				{
					if (GetFrom.FilePath == null)
						GetFrom.FilePath = Space.SpaceId + ".json";

					EchoAction($"Loading space from file: '{GetFrom.FilePath}'.");

					Space.Data = JsonFileReader.Read(GetFrom.FilePath);
					Space.FilePath = GetFrom.FilePath;

					//
					// CACHE THE SPACE.
					//
					SpaceCache TargetSpaceCache = GetSpaceCache(SpaceId);
					TargetSpaceCache.Space = Space;
					TargetSpaceCache.SpaceDataLoaded = true;
				}
				else if (DataLocation == BlokScriptEntityDataLocation.Server)
				{
					Space = GetSpaceCacheWithSpaceDataLoaded(SpaceId).Space;
				}
				else
					throw new NotImplementedException("VisitSpaceSpec");

				return Space;
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

			throw new NotImplementedException("VisitSpaceSpec");
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
					SpaceCache SourceSpaceCache = GetSpaceCacheWithComponentsLoaded(Space.SpaceId);

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

		public override object VisitCopyStatement([NotNull] BlokScriptGrammarParser.CopyStatementContext Context) 
		{ return VisitChildren(Context); }

		public override object VisitCopyBlockStatement ([NotNull] BlokScriptGrammarParser.CopyBlockStatementContext Context) 
		{
			/*
			copyBlockStatement: 'copy' blockSpec 'to' blockOutputLocation;
			*/
			BlockSchemaEntity Block = (BlockSchemaEntity)VisitBlockSpec(Context.blockSpec());
			BlockOutputLocation TargetLocation = (BlockOutputLocation)VisitBlockOutputLocation(Context.blockOutputLocation());

			if (TargetLocation.ToConsole)
			{
				Console.WriteLine(JsonFormatter.FormatIndented(Block.Data));
			}
			else if (TargetLocation.ToLocalCache)
			{
				SpaceCache CurrentSpaceCache = GetSpaceCache(Block.SpaceId);
				CurrentSpaceCache.BlockSchemaEntities[Block.ComponentName] = Block;
			}
			else if (TargetLocation.ToFile)
			{
				if (TargetLocation.FilePath == null)
					TargetLocation.FilePath = Block.ComponentName + ".json";

				EchoAction($"Block '{Block.ComponentName}' to file: '{TargetLocation.FilePath}'.");

				using (StreamWriter TargetWriter = new StreamWriter(TargetLocation.FilePath))
				{
					TargetWriter.WriteLine(JsonFormatter.FormatIndented(Block.Data));
				}
			}
			else if (TargetLocation.ToSpace)
			{
				//
				// THIS BLOCK IS BEING COPIED TO A SPACE.
				// THIS REQUIRES A LOT OF HEAVY API CALLS, SO WE NEED TO REDUCE THIS TO AVOID TIMEOUTS AND I/O.
				// SO, CREATE A TARGET SPACE CACHE RIGHT NOW, REUSE IT FOR FURTHER CALLS.
				//

				//
				// LOAD THE TARGET SPACE INTO THE LOCAL CACHE.  THIS GETS US THE BLOCK IDENTIFIERS IN THE TARGET SPACE.
				//
				SpaceCache TargetSpaceCache = GetSpaceCacheWithComponentsLoaded(TargetLocation.SpaceId);

				//
				// CREATE THE REQUEST BODY.
				//
				string RequestBody = JsonFormatter.FormatIndented(Block.Data);

				//
				// CHECK FOR EXISTENCE OF THE BLOCK ON THE SERVER FIRST.
				// THIS DETERMINES WHETHER WE POST (CREATE) OR PUT (UPDATE).
				//
				if (!TargetSpaceCache.ContainsBlock(Block.ComponentName))
				{
					//
					// THE TARGET SPACE DOES NOT HAVE THIS BLOCK.  CREATE THE BLOCK IN THE TARGET SPACE.
					//
					string RequestPath = ManagementPathFactory.CreateComponentsPath(TargetLocation.SpaceId);

					if (_Log.IsDebugEnabled)
					{
						_Log.Debug($"Space {TargetLocation.SpaceId} does not have block {Block.ComponentName}.");
						_Log.Debug($"Creating block {Block.ComponentName} in space {TargetLocation.SpaceId}.");
						_Log.Debug($"VisitCopyBlockStatement: POST {RequestPath}");
					}

					EchoAction($"API POST {RequestPath}. Creating Block '{Block.ComponentName}' in Space '{TargetLocation.SpaceId}'.");

					GetManagementWebClient().PostJson(RequestPath, RequestBody);
				}
				else
				{
					//
					// THE TARGET SPACE HAS THIS BLOCK.  UPDATE THE BLOCK IN THE TARGET SPACE.
					//
					BlockSchemaEntity TargetBlock = TargetSpaceCache.GetBlock(Block.ComponentName);

					string RequestPath = ManagementPathFactory.CreateComponentPath(TargetBlock.BlockId, TargetLocation.SpaceId);

					if (_Log.IsDebugEnabled)
					{
						_Log.Debug($"Updating Block {Block.ComponentName} in Space {TargetLocation.SpaceId}.");
						_Log.Debug($"VisitCopyBlockStatement: PUT {RequestPath}");
					}

					EchoAction($"API PUT {RequestPath}. Updating Block '{Block.ComponentName}' in Space '{TargetLocation.SpaceId}'.");

					GetManagementWebClient().PutJson(RequestPath, RequestBody);
				}
			}

			return null;
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

		public override object VisitCopySpaceStatement ([NotNull] BlokScriptGrammarParser.CopySpaceStatementContext Context)
		{
			/*
			copySpaceStatement: 'copy' spaceSpec 'to' spaceOutputLocation;
			*/

			//
			// GET THE SPACE ENTITY.
			// WE DON'T KNOW IF IT HAS ANY DATA ATTACHED TO IT.
			//
			SpaceEntity Space = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());

			SpaceCache CurrentSpaceCache = GetSpaceCacheWithSpaceDataLoaded(Space.SpaceId);
			Space = CurrentSpaceCache.Space;

			SpaceOutputLocation Location = (SpaceOutputLocation)VisitSpaceOutputLocation(Context.spaceOutputLocation());

			if (Location.ToConsole)
			{
				EchoToConsole(SpaceFormatter.FormatConsole(Space));
			}
			else if (Location.ToFile)
			{
				string SpaceFilePath = Location.FilePath;

				if (SpaceFilePath == null)
					SpaceFilePath = Space.SpaceId + ".json";

				EchoAction($"Copying Space '{Space.SpaceId}' to file '{SpaceFilePath}'");

				using (StreamWriter SpaceWriter = new StreamWriter(SpaceFilePath))
				{
					SpaceWriter.Write(SpaceFormatter.FormatJson(Space));
				}
			}
			else
				throw new NotImplementedException("VisitCopySpaceStatement");

			return null;
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
			/*                    1              1                      1
			varGetFrom: ('on' 'demand' | 'in' 'local' 'cache' | 'on' 'server' | 'in' fileSpec);
			*/
			VarGetFromDirective GetFrom = new VarGetFromDirective();

			if (Context.fileSpec() != null)
			{
				GetFrom.DataLocation = BlokScriptEntityDataLocation.FilePath;
				GetFrom.FilePath = (string)VisitFileSpec(Context.fileSpec());
			}
			else if (Context.GetChild(1).GetText() == "demand")
			{
				GetFrom.DataLocation = BlokScriptEntityDataLocation.OnDemand;
			}
			else if (Context.GetChild(1).GetText() == "server")
			{
				GetFrom.DataLocation = BlokScriptEntityDataLocation.Server;
			}
			else if (Context.GetChild(1).GetText() == "local")
			{
				GetFrom.DataLocation = BlokScriptEntityDataLocation.LocalCache;
			}
			else
				throw new NotImplementedException("VisitVarGetFrom");

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
				foreach (string SpaceKey in _SpaceCacheDict.Keys)
				{
					SpaceEntityList.Add(_SpaceCacheDict[SpaceKey].Space);
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

					_SpaceCacheDict[CurrentSpaceEntity.SpaceId] = CreatedCache;
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
			BlokScriptSymbol TargetSymbol = _SymbolTableManager.GetSymbol(Context.VARID().GetText());

			if (TargetSymbol.Type == BlokScriptSymbolType.String)
			{
				EchoToConsole((string)TargetSymbol.Value);
			}
			else if (TargetSymbol.Type == BlokScriptSymbolType.Space)
			{
				EchoToConsole(SpaceFormatter.FormatJson((SpaceEntity)TargetSymbol.Value));
			}
			else if (TargetSymbol.Type == BlokScriptSymbolType.Block)
			{
				EchoToConsole(BlockFormatter.FormatJson((BlockSchemaEntity)TargetSymbol.Value));
			}
			else if (TargetSymbol.Type == BlokScriptSymbolType.Story)
			{
				EchoToConsole(StoryEntityFormatter.FormatEcho((StoryEntity)TargetSymbol.Value));
				EchoToConsole(StoryEntityFormatter.FormatJson((StoryEntity)TargetSymbol.Value));
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

				EchoToConsole(SpaceFormatter.FormatConsole(GetSpaceCacheWithSpaceDataLoaded(SpaceId).Space));
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
			copyBlocksStatement: 'copy' 'all'? 'blocks' ('where' blockConstraintList)? ('in' | 'from') spaceSpec 'to' blocksOutputLocation;
			*/
			SpaceEntity SourceSpace = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());

			//
			// GET ALL BLOCKS.
			//
			BlockSchemaEntity[] BlockSchemaList = GetBlocksInSpace(SourceSpace.SpaceId);

			//
			// APPLY THE BLOCK CONSTRAINT LIST.
			//
			if (Context.blockConstraintList() != null)
			{
				BlockConstraint Constraint = (BlockConstraint)VisitBlockConstraintList(Context.blockConstraintList());
				BlockSchemaList = Constraint.Evaluate(BlockSchemaList);
			}

			//
			// OUTPUT THE BLOCK TO THE TARGET LOCATION.
			//
			BlocksOutputLocation TargetLocation = (BlocksOutputLocation)VisitBlocksOutputLocation(Context.blocksOutputLocation());

			if (TargetLocation.ToConsole)
			{
				List<object> BlockDataList = new List<object>();

				foreach (BlockSchemaEntity CurrentEntity in BlockSchemaList)
				{
					BlockDataList.Add(CurrentEntity.Data);
				}

				Console.WriteLine(JsonFormatter.FormatIndented(BlockDataList));
			}
			else if (TargetLocation.ToLocalCache)
			{
				foreach (BlockSchemaEntity CurrentEntity in BlockSchemaList)
				{
					SpaceCache CurrentSpaceCache = GetSpaceCache(CurrentEntity.SpaceId);
					CurrentSpaceCache.BlockSchemaEntities[CurrentEntity.ComponentName] = CurrentEntity;
				}
			}
			else if (TargetLocation.ToFile)
			{
				List<object> BlockDataList = new List<object>();

				foreach (BlockSchemaEntity CurrentEntity in BlockSchemaList)
				{
					BlockDataList.Add(CurrentEntity.Data);
				}

				using (StreamWriter TargetWriter = new StreamWriter(TargetLocation.FilePath))
				{
					TargetWriter.WriteLine(JsonFormatter.FormatIndented(BlockDataList));
				}
			}
			else if (TargetLocation.ToFiles)
			{
				List<object> BlockDataList = new List<object>();

				foreach (BlockSchemaEntity CurrentBlock in BlockSchemaList)
				{
					string CurrentBlockFilePath = CurrentBlock.ComponentName + ".json";

					EchoAction($"Copying Block '{CurrentBlock.ComponentName}' to file '{CurrentBlockFilePath}'.");

					using (StreamWriter TargetWriter = new StreamWriter(CurrentBlockFilePath))
					{
						TargetWriter.WriteLine(BlockFormatter.FormatJson(CurrentBlock));
					}
				}
			}
			else if (TargetLocation.ToSpace)
			{
				//
				// THE SOURCE BLOCKS ARE BEING COPIED TO ANOTHER SPACE.
				// THIS REQUIRES A LOT OF HEAVY API CALLS, SO WE NEED TO REDUCE THIS TO AVOID TIMEOUTS AND I/O.
				// SO, CREATE A TARGET SPACE CACHE RIGHT NOW, REUSE IT FOR FURTHER CALLS.
				//
				SpaceCache TargetSpaceCache = GetSpaceCacheWithComponentsLoaded(TargetLocation.SpaceId);
				StoryblokManagementWebClient WebClient = GetManagementWebClient();

				foreach (BlockSchemaEntity SourceBlock in BlockSchemaList)
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
						string RequestPath = ManagementPathFactory.CreateComponentsPath(TargetLocation.SpaceId);

						if (_Log.IsDebugEnabled)
						{
							_Log.Debug($"Space {TargetLocation.SpaceId} does not have block {SourceBlock.ComponentName}.");
							_Log.Debug($"Creating block {SourceBlock.ComponentName} in space {TargetLocation.SpaceId}.");
							_Log.Debug($"VisitCopyBlocksStatement: POST {RequestPath}");
						}

						EchoAction($"API POST {RequestPath}. Creating Block '{SourceBlock.ComponentName}' in Space '{TargetLocation.SpaceId}'.");

						WebClient.PostJson(RequestPath, RequestBody);
					}
					else
					{
						//
						// THE TARGET SPACE HAS THIS BLOCK.  UPDATE THE BLOCK IN THE TARGET SPACE.
						//
						BlockSchemaEntity TargetBlock = TargetSpaceCache.GetBlock(SourceBlock.ComponentName);

						string RequestPath = ManagementPathFactory.CreateComponentPath(TargetBlock.BlockId, TargetLocation.SpaceId);

						if (_Log.IsDebugEnabled)
						{
							_Log.Debug($"Updating block {TargetBlock.ComponentName} in space {TargetLocation.SpaceId}.");
							_Log.Debug($"VisitCopyBlocksStatement: PUT {RequestPath}");
						}

						EchoAction($"API PUT {RequestPath}. Updating Block '{TargetBlock.ComponentName}' in Space '{TargetLocation.SpaceId}'.");

						WebClient.PutJson(RequestPath, RequestBody);
					}
				}
			}

			return null;
		}

		public override object VisitVerbosityStatement([NotNull] BlokScriptGrammarParser.VerbosityStatementContext Context)
		{
			/*
			verbosityStatement: 'be' ('verbose' | 'quiet');
			*/
			BlokScriptSymbol VerbositySymbol = _SymbolTableManager.GetSymbol("_GlobalVerbosity");

			if (Context.GetChild(1).GetText() == "quiet")
			{
				VerbositySymbol.Value = BlokScriptVerbosity.Quiet;
				_Log.Info("Verbosity set to quiet.");
			}
			else if (Context.GetChild(1).GetText() == "verbose")
			{
				VerbositySymbol.Value = BlokScriptVerbosity.Verbose;
				Console.WriteLine("Verbosity set to verbose.");
			}

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

			SpaceCache SpacheCache1 = GetSpaceCacheWithSpaceDataLoaded(Space1.SpaceId);
			SpaceCache SpacheCache2 = GetSpaceCacheWithSpaceDataLoaded(Space2.SpaceId);

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
			foreach (SpaceCache CurrentSpaceCache in _SpaceCacheDict.Values)
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

		public override object VisitBlockConstraintList ([NotNull] BlokScriptGrammarParser.BlockConstraintListContext Context)
		{
			_Log.Debug("VisitBlockConstraintList");

			/*
			blockConstraintList: blockConstraint ('and' blockConstraintList)?
				| blockConstraint ('or' blockConstraintList)?
				;
			*/
			BlockConstraint Constraint = (BlockConstraint)VisitBlockConstraint(Context.blockConstraint());

			if (Context.blockConstraintList() != null)
			{
				BlockConstraint ChildConstraint = (BlockConstraint)VisitBlockConstraintList(Context.blockConstraintList());

				if (Context.GetChild(1).GetText() == "and")
				{
					Constraint.AndChildConstraint = ChildConstraint;
				}
				else if (Context.GetChild(1).GetText() == "or")
				{
					Constraint.OrChildConstraint = ChildConstraint;
				}
			}

			return Constraint;
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
					Constraint.Operator = BlockConstraintOperator.StartsWith;
					Constraint.ConstraintData = VisitStringExpr(Context.stringExpr());
				}
				else if (Context.GetChild(1).GetText() == "starts")
				{
					Constraint.Operator = BlockConstraintOperator.DoesNotStartWith;
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
			regexVarStatement: 'regex' VARID ('=' (REGEXLITERAL | VARID))?;
			*/
			string SymbolName = Context.VARID()[0].GetText();

			BlokScriptSymbol CreatedSymbol = new BlokScriptSymbol();
			CreatedSymbol.Name = SymbolName;
			CreatedSymbol.Type = BlokScriptSymbolType.Regex;

			if (Context.REGEXLITERAL() != null)
			{
				CreatedSymbol.Value = new Regex(RegexLiteralTrimmer.Trim(Context.REGEXLITERAL().GetText()));
			}
			else if (Context.VARID().Length > 1)
			{
				CreatedSymbol.Value = _SymbolTableManager.GetSymbolValueAsRegex(Context.VARID()[1].GetText());
			}
			else
				throw new NotImplementedException("VisitRegexVarStatement");

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

		public override object VisitCopyStoryStatement([NotNull] BlokScriptGrammarParser.CopyStoryStatementContext Context)
		{
			/*
			copyStoryStatement: 'copy' 'story' storySpec 'to' (storyOutputLocation | VARID);
			*/
			StoryEntity Story = (StoryEntity)VisitStorySpec(Context.storySpec());

			//
			// IF THE STORY DOES NOT CONTAIN CONTENT, THEN WE NEED TO GET THAT CONTENT FIRST.
			//
			if (!Story.HasContent)
			{
				string RequestPath = ManagementPathFactory.CreateStoryPath(Story.StoryId, Story.SpaceId);
				EchoAction($"API GET {RequestPath}. Getting story content.");
				Story.Data = JsonParser.Parse(GetManagementWebClient().GetString(RequestPath));
				Story.ServerPath = RequestPath;
				Story.HasContent = true;
			}

			//
			// GET THE TARGET LOCATION.
			//
			StoryOutputLocation TargetLocation = new StoryOutputLocation();

			if (Context.storyOutputLocation() != null)
			{
				TargetLocation = (StoryOutputLocation)VisitStoryOutputLocation(Context.storyOutputLocation());
			}
			else if (Context.VARID() != null)
			{
				TargetLocation.ToSpace = true;
				TargetLocation.SpaceId = _SymbolTableManager.GetSymbolValueAsSpaceEntity(Context.VARID().GetText()).SpaceId;
			}
			else
				throw new NotImplementedException("VisitCopyStoryStatement - TargetLocation");

			//
			// OUTPUT THE STORY TO THE TARGET LOCATION.
			//
			if (TargetLocation.ToConsole)
			{
				EchoToConsole(StoryEntityFormatter.FormatJson(Story));
			}
			else if (TargetLocation.ToLocalCache)
			{
				SpaceCache CurrentSpaceCache = GetSpaceCache(Story.SpaceId);
				CurrentSpaceCache.StoryEntities[Story.Url] = Story;
			}
			else if (TargetLocation.ToFile)
			{
				if (TargetLocation.FilePath == null)
					TargetLocation.FilePath = Story.Url.Replace("/", "_") + ".json";

				EchoAction($"Copying Story '{Story.Url}' to file: '{TargetLocation.FilePath}'.");

				using (StreamWriter TargetWriter = new StreamWriter(TargetLocation.FilePath))
				{
					TargetWriter.WriteLine(JsonFormatter.FormatIndented(Story.Data));
				}
			}
			else if (TargetLocation.ToSpace)
			{
				//
				// THIS STORY IS BEING COPIED TO A SPACE.
				// THIS REQUIRES A LOT OF HEAVY API CALLS, SO WE NEED TO REDUCE THIS TO AVOID TIMEOUTS AND I/O.
				// SO, CREATE A TARGET SPACE CACHE RIGHT NOW, AND REUSE IT FOR FURTHER CALLS.
				//

				//
				// LOAD THE TARGET SPACE INTO THE LOCAL CACHE.  THIS GETS US THE BLOCK IDENTIFIERS IN THE TARGET SPACE.
				//
				SpaceCache TargetSpaceCache = GetSpaceCacheWithStoriesLoaded(TargetLocation.SpaceId);

				//
				// CREATE THE REQUEST BODY.
				//
				string RequestBody = JsonFormatter.FormatIndented(Story.Data);

				//
				// CHECK FOR EXISTENCE OF THE STORY ON THE SERVER FIRST.
				// THIS DETERMINES WHETHER WE POST (CREATE) OR PUT (UPDATE).
				//
				if (!TargetSpaceCache.ContainsStory(Story.Url))
				{
					//
					// THE TARGET SPACE DOES NOT HAVE THIS STORY.  CREATE THE STORY IN THE TARGET SPACE.
					//
					string RequestPath = ManagementPathFactory.CreateStoriesPath(TargetLocation.SpaceId);

					if (_Log.IsDebugEnabled)
					{
						_Log.Debug($"Space {TargetLocation.SpaceId} does not have story {Story.Url}.");
						_Log.Debug($"Creating story {Story.Url} in space {TargetLocation.SpaceId}.");
						_Log.Debug($"VisitCopyStoryStatement: POST {RequestPath}");
					}

					EchoAction($"API POST {RequestPath}. Creating Story '{Story.Url}' in Space '{TargetLocation.SpaceId}'.");

					try
					{
						GetManagementWebClient().PostJson(RequestPath, RequestBody);
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
					StoryEntity TargetStory = TargetSpaceCache.GetStory(Story.Url);
					string RequestPath = ManagementPathFactory.CreateStoryPath(TargetStory.StoryId, TargetLocation.SpaceId);

					if (_Log.IsDebugEnabled)
					{
						_Log.Debug($"Updating Story {TargetStory.Url} in Space {TargetLocation.SpaceId}.");
						_Log.Debug($"VisitCopyBlockStatement: PUT {RequestPath}");
					}

					EchoAction($"API PUT {RequestPath}. Updating Story '{TargetStory.Url}' in Space '{TargetLocation.SpaceId}'.");

					try
					{
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

			return null;
		}

		public override object VisitPublishStoryStatement ([NotNull] BlokScriptGrammarParser.PublishStoryStatementContext Context)
		{
			/*
			publishStoryStatement: 'publish' 'story' storySpec ('in' spaceSpec)?;
			*/

			//
			// GET THE STORY.
			//
			StoryEntity Story = (StoryEntity)VisitStorySpec(Context.storySpec());

			//
			// GET THE TARGET SPACE.
			//
			SpaceEntity TargetSpace;

			if (Context.spaceSpec() != null)
				TargetSpace = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
			else
				TargetSpace = GetSpaceCacheWithSpaceDataLoaded(Story.SpaceId).Space;

			SpaceCache TargetSpaceCache = GetSpaceCacheWithStoriesLoaded(TargetSpace.SpaceId);

			//
			// PUBLISH.
			//
			string RequestPath = ManagementPathFactory.CreatePublishStoryPath(Story.StoryId, TargetSpace.SpaceId);
			EchoAction($"API GET {RequestPath} | Publishing Story '{Story.Url}'.");
			GetManagementWebClient().GetString(RequestPath);
			return null;
		}

		public override object VisitUnpublishStoryStatement([NotNull] BlokScriptGrammarParser.UnpublishStoryStatementContext Context)
		{
			/*
			unpublishStoryStatement: 'unpublish' 'story' storySpec ('in' spaceSpec)?;
			*/

			//
			// GET THE STORY.
			//
			StoryEntity Story = (StoryEntity)VisitStorySpec(Context.storySpec());

			//
			// GET THE TARGET SPACE.
			//
			SpaceEntity TargetSpace;

			if (Context.spaceSpec() != null)
				TargetSpace = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
			else
				TargetSpace = GetSpaceCacheWithSpaceDataLoaded(Story.SpaceId).Space;

			SpaceCache TargetSpaceCache = GetSpaceCacheWithStoriesLoaded(TargetSpace.SpaceId);

			//
			// UNPUBLISH.
			//
			string RequestPath = ManagementPathFactory.CreateUnpublishStoryPath(Story.StoryId, TargetSpace.SpaceId);
			EchoAction($"API GET {RequestPath} | Unpublishing Story '{Story.Url}'.");
			GetManagementWebClient().GetString(RequestPath);
			return null;
		}

		public override object VisitDeleteStoryStatement ([NotNull] BlokScriptGrammarParser.DeleteStoryStatementContext Context)
		{
			/*
			deleteStoryStatement: 'delete' 'story' storySpec ('in' spaceSpec)?;
			*/

			//
			// GET THE STORY.
			//
			StoryEntity Story = (StoryEntity)VisitStorySpec(Context.storySpec());

			//
			// GET THE TARGET SPACE.
			//
			SpaceEntity TargetSpace;

			if (Context.spaceSpec() != null)
				TargetSpace = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
			else
				TargetSpace = GetSpaceCacheWithSpaceDataLoaded(Story.SpaceId).Space;

			SpaceCache TargetSpaceCache = GetSpaceCacheWithStoriesLoaded(TargetSpace.SpaceId);

			string RequestPath = ManagementPathFactory.CreateDeleteStoryPath(Story.StoryId, TargetSpace.SpaceId);
			EchoAction($"API DELETE {RequestPath} | Deleting Story {Story.Url}");
			GetManagementWebClient().Delete(RequestPath, "");

			//
			// REMOVE THE STORY FROM THE SPACE CACHE.
			//
			TargetSpaceCache.RemoveStory(Story);

			return null;
		}

		public override object VisitPublishStoriesStatement([NotNull] BlokScriptGrammarParser.PublishStoriesStatementContext Context)
		{
			/*
			publishStoriesStatement: 'publish' 'stories' (('where' | 'with') storyConstraintList)? ('in' | 'from') spaceSpec;
			*/
			SpaceEntity TargetSpace = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
			SpaceCache TargetSpaceCache = GetSpaceCacheWithStoriesLoaded(TargetSpace.SpaceId);
			StoryEntity[] TargetStories = TargetSpaceCache.GetStories();

			if (Context.storyConstraintList() != null)
				TargetStories =  ((StoryConstraint)VisitStoryConstraintList(Context.storyConstraintList())).Evaluate(TargetStories);

			foreach (StoryEntity Story in TargetStories)
			{
				string RequestPath = ManagementPathFactory.CreatePublishStoryPath(Story.StoryId, TargetSpace.SpaceId);
				EchoAction($"API GET {RequestPath} | Publishing Story '{Story.Url}'.");
				GetManagementWebClient().GetString(RequestPath);
			}
			return null;
		}

		public override object VisitUnpublishStoriesStatement([NotNull] BlokScriptGrammarParser.UnpublishStoriesStatementContext Context)
		{
			/*
			unpublishStoriesStatement: 'unpublish' 'stories' (('where' | 'with') storyConstraintList)? ('in' | 'from') spaceSpec;
			*/
			SpaceEntity TargetSpace = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
			SpaceCache TargetSpaceCache = GetSpaceCacheWithStoriesLoaded(TargetSpace.SpaceId);
			StoryEntity[] TargetStories = TargetSpaceCache.GetStories();

			if (Context.storyConstraintList() != null)
				TargetStories =  ((StoryConstraint)VisitStoryConstraintList(Context.storyConstraintList())).Evaluate(TargetStories);

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
			deleteStoriesStatement: 'delete' 'stories' (('where' | 'with') storyConstraintList)? ('in' | 'from') spaceSpec;
			*/
			SpaceEntity TargetSpace = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
			SpaceCache TargetSpaceCache = GetSpaceCacheWithStoriesLoaded(TargetSpace.SpaceId);
			StoryEntity[] TargetStories = TargetSpaceCache.GetStories();

			if (Context.storyConstraintList() != null)
				TargetStories =  ((StoryConstraint)VisitStoryConstraintList(Context.storyConstraintList())).Evaluate(TargetStories);

			foreach (StoryEntity Story in TargetStories)
			{
				string RequestPath = ManagementPathFactory.CreateDeleteStoryPath(Story.StoryId, TargetSpace.SpaceId);
				EchoAction($"API DELETE {RequestPath} | Deleting Story {Story.Url}");
				GetManagementWebClient().Delete(RequestPath, "");

				//
				// REMOVE THE STORY FROM THE SPACE CACHE.
				//
				TargetSpaceCache.RemoveStory(Story);
			}

			return null;
		}

		public override object VisitStoryConstraintList ([NotNull] BlokScriptGrammarParser.StoryConstraintListContext Context)
		{
			/*
			storyConstraintList: storyConstraint ('and' storyConstraintList)?
				| storyConstraint ('or' storyConstraintList)?
				;
			*/
			StoryConstraint ThisConstraint = (StoryConstraint)VisitStoryConstraint(Context.storyConstraint());

			if (Context.storyConstraintList() != null)
			{
				ThisConstraint.ChildConstraint = (StoryConstraint)VisitStoryConstraintList(Context.storyConstraintList());
				ThisConstraint.ChildConstraintOperator = Context.GetChild(1).GetText();
			}

			return ThisConstraint;
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

		public override object VisitCopyDatasourceStatement([NotNull] BlokScriptGrammarParser.CopyDatasourceStatementContext Context)
					{ return VisitChildren(Context); }

		public override object VisitCopyDatasourcesStatement([NotNull] BlokScriptGrammarParser.CopyDatasourcesStatementContext Context)
					{ return VisitChildren(Context); }

		public override object VisitDatasourceConstraintList([NotNull] BlokScriptGrammarParser.DatasourceConstraintListContext Context)
					{ return VisitChildren(Context); }

		public override object VisitDatasourceConstraint([NotNull] BlokScriptGrammarParser.DatasourceConstraintContext Context)
					{ return VisitChildren(Context); }

		public override object VisitCopyStoriesStatement ([NotNull] BlokScriptGrammarParser.CopyStoriesStatementContext Context)
		{
			/*
			copyStoriesStatement: 'copy' 'stories' ('where' storyConstraintList)? ('in' | 'from') storiesInputLocation 'to' storiesOutputLocation;
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
			if (Context.storyConstraintList() != null)
				SourceStories = ((StoryConstraint)VisitStoryConstraintList(Context.storyConstraintList())).Evaluate(SourceStories);

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
					EchoToConsole(StoryEntityFormatter.FormatJson(SourceStory));
				}
				else if (OutLocation.ToLocalCache)
				{
					SpaceCache CurrentSpaceCache = GetSpaceCache(SourceStory.SpaceId);
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
					// THIS STORY IS BEING COPIED TO A SPACE.
					// THIS REQUIRES A LOT OF HEAVY API CALLS, SO WE NEED TO REDUCE THIS TO AVOID TIMEOUTS AND I/O.
					// SO, CREATE A TARGET SPACE CACHE RIGHT NOW, AND REUSE IT FOR FURTHER CALLS.
					//

					//
					// IF THE STORY DOES NOT CONTAIN CONTENT, THEN WE NEED TO GET THAT CONTENT FIRST.
					//
					if (!SourceStory.HasContent)
					{
						string RequestPath = ManagementPathFactory.CreateStoryPath(SourceStory.StoryId, SourceStory.SpaceId);
						EchoAction($"API GET {RequestPath}. Getting story content.");
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

						EchoAction($"API POST {RequestPath}. Creating Story '{SourceStory.Url}' ({SourceStory.Name}) in Space '{TargetSpace.SpaceId}'.");

						try
						{
							string ResponseString = GetManagementWebClient().PostJson(RequestPath, RequestBody);

							//
							// GET THE CREATED STORY.
							//
							StoryEntity CreatedStory = StoryEntityParser.Parse(JsonParser.ParseAsDynamic(ResponseString).story);

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

						if (_Log.IsDebugEnabled)
						{
							_Log.Debug($"Updating Story {TargetStory.Url} in Space {TargetSpace.SpaceId}.");
							_Log.Debug($"VisitCopyBlockStatement: PUT {RequestPath}");
						}

						EchoAction($"API PUT {RequestPath}. Updating Story '{TargetStory.Url}' ({SourceStory.Name}) in Space '{TargetSpace.SpaceId}'.");

						try
						{
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
				throw new NotImplementedException("VisitIntExpr");

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
				;
			*/
			if (Context.spaceSpec() != null)
			{
				StoriesInputLocation Location = new StoriesInputLocation();
				Location.FromSpace = true;
				Location.Space = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
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
				;
			*/
			StoriesOutputLocation Location = new StoriesOutputLocation();

			if (Context.spaceSpec() != null)
			{
				Location.ToSpace = true;
				Location.Space = (SpaceEntity)VisitSpaceSpec(Context.spaceSpec());
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

		public StoryblokManagementWebClient GetManagementWebClient ()
		{
			StoryblokManagementWebClient CreatedWebClient = new StoryblokManagementWebClient();
			CreatedWebClient.BaseUrl = _SymbolTableManager.GetSymbolValueAsString("_GlobalManagementBaseUrl");
			CreatedWebClient.UserName = _SymbolTableManager.GetSymbolValueAsString("_GlobalManagementUserName");
			CreatedWebClient.Password = _SymbolTableManager.GetSymbolValueAsString("_GlobalManagementPassword");
			CreatedWebClient.Token = _SymbolTableManager.GetSymbolValueAsString("_GlobalManagementToken");
			CreatedWebClient.Region = _SymbolTableManager.GetSymbolValueAsString("_GlobalManagementRegion");
			return CreatedWebClient;
		}

		public StoryblokContentDeliveryWebClient GetContentDeliveryWebClient ()
		{
			StoryblokContentDeliveryWebClient CreatedWebClient = new StoryblokContentDeliveryWebClient();
			return CreatedWebClient;
		}

		public SpaceCache GetSpaceCache (string SpaceId)
		{
			if (!_SpaceCacheDict.ContainsKey(SpaceId))
			{
				SpaceCache CreatedCache = new SpaceCache();
				CreatedCache.SpaceId = SpaceId;
				_SpaceCacheDict[SpaceId] = CreatedCache;
			}

			return _SpaceCacheDict[SpaceId];
		}

		public SpaceCache GetSpaceCacheWithSpaceDataLoaded (string SpaceId)
		{
			SpaceCache TargetSpaceCache = GetSpaceCache(SpaceId);

			if (TargetSpaceCache.SpaceDataLoaded)
				return TargetSpaceCache;

			LoadSpaceCacheSpaceData(TargetSpaceCache);

			return TargetSpaceCache;
		}

		public SpaceCache GetSpaceCacheWithComponentsLoaded (string SpaceId)
		{
			SpaceCache TargetSpaceCache = GetSpaceCache(SpaceId);

			if (TargetSpaceCache.ComponentsLoaded)
				return TargetSpaceCache;

			LoadSpaceCacheComponents(TargetSpaceCache);

			return TargetSpaceCache;
		}

		public SpaceCache GetSpaceCacheWithStoriesLoaded (string SpaceId)
		{
			SpaceCache TargetSpaceCache = GetSpaceCache(SpaceId);

			if (TargetSpaceCache.StoriesLoaded)
				return TargetSpaceCache;

			LoadSpaceCacheStories(TargetSpaceCache);

			return TargetSpaceCache;
		}

		public void LoadSpaceCacheSpaceData (SpaceCache TargetSpaceCache)
		{
			string RequestPath = ManagementPathFactory.CreateSpacePath(TargetSpaceCache.SpaceId);

			if (_Log.IsDebugEnabled)
				_Log.Debug($"LoadSpaceCacheSpaceData: GET {RequestPath}");

			EchoAction($"API GET {RequestPath}. Copying Space '{TargetSpaceCache.SpaceId}' to Local Cache.");

			SpaceEntity Space = new SpaceEntity();
			Space.SpaceId = TargetSpaceCache.SpaceId;
			Space.DataLocation = BlokScriptEntityDataLocation.Server;
			Space.ServerPath = RequestPath;
			Space.Data = JsonParser.Parse(GetManagementWebClient().GetString(RequestPath));

			TargetSpaceCache.Space = Space;
			TargetSpaceCache.SpaceDataLoaded = true;
		}

		public void LoadSpaceCacheComponents (SpaceCache TargetSpaceCache)
		{
			string RequestPath = ManagementPathFactory.CreateComponentsPath(TargetSpaceCache.SpaceId);

			if (_Log.IsDebugEnabled)
				_Log.Debug($"LoadSpaceCacheComponents: GET {RequestPath}");

			EchoAction($"API GET {RequestPath}. Copying all blocks in Space '{TargetSpaceCache.SpaceId}' to Local Cache.");

			string ComponentsResponseString = GetManagementWebClient().GetString(RequestPath);
			BlockSchemaEntity[] TargetBlocks = ComponentsResponseReader.ReadResponseString(ComponentsResponseString, TargetSpaceCache.SpaceId);

			foreach (BlockSchemaEntity CurrentBlock in TargetBlocks)
			{
				TargetSpaceCache.InsertBlock(CurrentBlock);
			}

			TargetSpaceCache.ComponentsLoaded = true;
		}

		public void LoadSpaceCacheStories (SpaceCache TargetSpaceCache)
		{
			string RequestPath = ManagementPathFactory.CreateStoriesPath(TargetSpaceCache.SpaceId);

			if (_Log.IsDebugEnabled)
				_Log.Debug($"LoadSpaceCacheComponents: GET {RequestPath}");

			EchoAction($"API GET {RequestPath}. Copying all stories in Space '{TargetSpaceCache.SpaceId}' to Local Cache.");

			string ResponseString = GetManagementWebClient().GetString(RequestPath);

			StoryEntity[] Stories = StoriesResponseReader.ReadResponseString(ResponseString);

			foreach (StoryEntity Story in Stories)
			{
				Story.SpaceId = TargetSpaceCache.SpaceId;
				Story.DataLocation = BlokScriptEntityDataLocation.Server;
				Story.ServerPath = RequestPath;
				TargetSpaceCache.InsertStory(Story);
			}

			TargetSpaceCache.StoriesLoaded = true;
		}

		public BlockSchemaEntity[] GetBlocksInSpace (string SpaceId)
		{
			SpaceCache CurrentSpaceCache = GetSpaceCacheWithComponentsLoaded(SpaceId);

			List<BlockSchemaEntity> BlockSchemaEntity = new List<BlockSchemaEntity>();
			BlockSchemaEntity.AddRange(CurrentSpaceCache.BlockSchemaEntities.Values);
			return BlockSchemaEntity.ToArray();
		}

		public bool ShouldBeVerbose ()
		{
			return ((BlokScriptVerbosity)_SymbolTableManager.GetSymbol("_GlobalVerbosity").Value) == BlokScriptVerbosity.Verbose;
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
			Console.WriteLine(Message);
		}

		public void EchoDebug (string Message)
		{
			Console.WriteLine(Message);
		}

		public void EchoAction (string Message)
		{
			Console.WriteLine($"{_ActionNumber++}. " + Message);
		}
		
		public void EchoError (string Message)
		{
			Console.WriteLine(Message);
		}

		public void EchoWarning (string Message)
		{
			Console.WriteLine(Message);
		}

		public void EchoToConsole (string Message)
		{
			Console.WriteLine(Message);
		}

		private BlokScriptSymbolTableManager _SymbolTableManager;
		private Dictionary<string, SpaceCache> _SpaceCacheDict;
		private int _ActionNumber = 1;

		private static ILog _Log;
	}
}
