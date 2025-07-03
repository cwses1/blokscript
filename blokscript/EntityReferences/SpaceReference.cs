using System.Collections.Generic;

using BlokScript.Common;

namespace BlokScript.EntityReferences
{
	public class SpaceReference
	{
		static SpaceReference ()
		{
			List<EntityField> EntityFieldList = new List<EntityField>();

			{
				EntityField Field = new EntityField();
				Field.Name = "name";
				Field.SymbolType = BlokScriptSymbolType.String;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "id";
				Field.SymbolType = BlokScriptSymbolType.Int32;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "euid";
				Field.SymbolType = BlokScriptSymbolType.String;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "region";
				Field.SymbolType = BlokScriptSymbolType.String;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "owner_id";
				Field.SymbolType = BlokScriptSymbolType.Int32;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "updated_at";
				Field.SymbolType = BlokScriptSymbolType.String;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "fe_version";
				Field.SymbolType = BlokScriptSymbolType.String;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "plan";
				Field.SymbolType = BlokScriptSymbolType.String;
				EntityFieldList.Add(Field);
			}
			
			{
				EntityField Field = new EntityField();
				Field.Name = "plan_level";
				Field.SymbolType = BlokScriptSymbolType.Int32;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "pending_traffic_boosts";
				Field.SymbolType = BlokScriptSymbolType.Boolean;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "trial";
				Field.SymbolType = BlokScriptSymbolType.Boolean;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "requires_2fa";
				Field.SymbolType = BlokScriptSymbolType.Boolean;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "created_at";
				Field.SymbolType = BlokScriptSymbolType.DateTime;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "org_id";
				Field.SymbolType = BlokScriptSymbolType.Int32;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "partner_id";
				Field.SymbolType = BlokScriptSymbolType.Int32;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "subscription_status";
				Field.SymbolType = BlokScriptSymbolType.String;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "canceled_at";
				Field.SymbolType = BlokScriptSymbolType.DateTime;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "org_requires_2fa";
				Field.SymbolType = BlokScriptSymbolType.Boolean;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "unpaid_at";
				Field.SymbolType = BlokScriptSymbolType.DateTime;
				EntityFieldList.Add(Field);
			}

			{
				EntityField Field = new EntityField();
				Field.Name = "blocked_at";
				Field.SymbolType = BlokScriptSymbolType.DateTime;
				EntityFieldList.Add(Field);
			}

			List<string> FieldList = new List<string>();
			_FieldTypesDict = new Dictionary<string, BlokScriptSymbolType>();

			foreach (EntityField CurrentField in EntityFieldList)
			{
				FieldList.Add(CurrentField.Name);
				_FieldTypesDict[CurrentField.Name] = CurrentField.SymbolType;
			}

			_SelectStarFields = FieldList.ToArray();
		}

		public static string[] SelectStarFields
		{
			get
			{
				return _SelectStarFields;
			}
		}

		public static BlokScriptSymbolType GetTypeForFieldName (string FieldName)
		{
			if (!_FieldTypesDict.ContainsKey(FieldName))
				throw new KeyNotFoundException(FieldName);

			return _FieldTypesDict[FieldName];
		}

		private static string[] _SelectStarFields;
		private static Dictionary<string, BlokScriptSymbolType> _FieldTypesDict;
	}
}
