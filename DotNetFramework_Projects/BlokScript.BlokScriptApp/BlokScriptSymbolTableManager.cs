using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlokScript.Entities;

namespace BlokScript.BlokScriptApp
{
	public class BlokScriptSymbolTableManager
	{
		public BlokScriptSymbolTableManager ()
		{
			_SymbolTableStack = new Stack<BlokScriptSymbolTable>();
		}

		public void PushSymbolTable (BlokScriptSymbolTable SymbolTableParam)
		{
			_SymbolTableStack.Push(SymbolTableParam);
		}

		public void CreateAndPushNewSymbolTable ()
		{
			_SymbolTableStack.Push(new BlokScriptSymbolTable());
		}

		public BlokScriptSymbolTable PopSymbolTable ()
		{
			return _SymbolTableStack.Pop();
		}

		public BlokScriptSymbol GetSymbol (string SymbolName)
		{
			Stack<BlokScriptSymbolTable> TempStack = new Stack<BlokScriptSymbolTable>();
			BlokScriptSymbol TargetSymbol = null;

			while (_SymbolTableStack.Count > 0 && TargetSymbol == null)
			{
				BlokScriptSymbolTable TargetSymbolTable = _SymbolTableStack.Pop();
				TempStack.Push(TargetSymbolTable);
				TargetSymbol = TargetSymbolTable.ContainsKey(SymbolName) ? TargetSymbolTable[SymbolName] : null;
			}

			while (TempStack.Count > 0)
				_SymbolTableStack.Push(TempStack.Pop());

			return TargetSymbol;
		}

		public void AddSymbol (BlokScriptSymbol SymbolParam)
		{
			BlokScriptSymbolTable TargetSymbolTable = _SymbolTableStack.Peek();

			if (TargetSymbolTable.ContainsKey(SymbolParam.Name))
				throw new SymbolConflictException(SymbolParam.Name);

			TargetSymbolTable[SymbolParam.Name] = SymbolParam;
		}


		public BlokScriptSymbol AddAndReturnSymbol (BlokScriptSymbol SymbolParam)
		{
			AddSymbol(SymbolParam);
			return SymbolParam;
		}

		public string GetSymbolValueAsString (string SymbolName)
		{
			return (string)GetSymbol(SymbolName).Value;
		}

		public int GetSymbolValueAsInt32 (string SymbolName)
		{
			return (int)GetSymbol(SymbolName).Value;
		}

		public Regex GetSymbolValueAsRegex (string SymbolName)
		{
			return (Regex)GetSymbol(SymbolName).Value;
		}

		public SpaceEntity GetSymbolValueAsSpaceEntity (string SymbolName)
		{
			return (SpaceEntity)GetSymbol(SymbolName).Value;
		}

		public BlokScriptSymbolTable[] GetSymbolTables ()
		{
			Stack<BlokScriptSymbolTable> TempStack = new Stack<BlokScriptSymbolTable>();
			List<BlokScriptSymbolTable> SymbolTableList = new List<BlokScriptSymbolTable>();

			while (_SymbolTableStack.Count > 0)
			{
				BlokScriptSymbolTable CurrentSymbolTable = _SymbolTableStack.Pop();
				TempStack.Push(CurrentSymbolTable);
			}

			while (TempStack.Count > 0)
			{
				BlokScriptSymbolTable CurrentSymbolTable = TempStack.Pop();
				_SymbolTableStack.Push(CurrentSymbolTable);
				SymbolTableList.Add(CurrentSymbolTable);
			}

			return SymbolTableList.ToArray();
		}

		private Stack<BlokScriptSymbolTable> _SymbolTableStack;
	}
}
