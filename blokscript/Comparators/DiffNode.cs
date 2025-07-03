using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using BlokScript.Formatters;
using System;

namespace BlokScript.Comparators
{
	public class DiffNode
	{
		public static Diff[] Compare (JObject JObject1, JObject JObject2, int Level)
		{
			List<Diff> DiffList = new List<Diff>();

			//
			// EXISTENCE CHECK.
			//
			bool ExistenceEqual;

			if (JObject1 == null)
				if (JObject2 == null)
					ExistenceEqual = true;
				else
					ExistenceEqual = false;
			else
				if (JObject2 != null)
					ExistenceEqual = true;
				else
					ExistenceEqual = false;

			Diff ExistenceDiff = new Diff();
			ExistenceDiff.Category = DiffCategory.Existence;
			ExistenceDiff.AreEqual = ExistenceEqual;
			DiffList.Add(ExistenceDiff);

			if (ExistenceEqual)
			{
				//
				// TYPE CHECK.
				//
				bool TypesAreEqual = JObject1.Type == JObject2.Type;

				Diff TypeCheckDiff = new Diff();
				TypeCheckDiff.Category = DiffCategory.TypeCheck;
				TypeCheckDiff.AreEqual = TypesAreEqual;
				TypeCheckDiff.Type1 = JObject1.Type;
				TypeCheckDiff.Type2 = JObject2.Type;
				DiffList.Add(TypeCheckDiff);

				//
				// VALUE CHECK.
				//
				/*
				if (JObject1.Type == JTokenType.Property)
				{
				}
				else if (JObject1.Type == JTokenType.Array)
				{
				}
				else if (JObject1.Type == JTokenType.String)
				{
				}
				else if (JObject1.Type == JTokenType.Boolean)
				{
				}
				else if (JObject1.Type == JTokenType.Undefined)
				{
				}
				else if (JObject1.Type == JTokenType.Null)
				{
				}
				else if (JObject1.Type == JTokenType.Float)
				{
				}
				else if (JObject1.Type == JTokenType.Integer)
				{
				}
				else
					throw new NotImplementedException("DiffNode");
				*/

				//
				// COMPARE ALL CHILDREN.
				//
				foreach (JProperty CurrentJProperty in JObject1.Children())
				{
					DiffList.AddRange(Compare(CurrentJProperty, JObject2.Property(CurrentJProperty.Name), Level + 1));
				}
			}

			return DiffList.ToArray();
		}

		public static Diff[] Compare (JProperty JProperty1, JProperty JProperty2, int Level)
		{
			List<Diff> DiffList = new List<Diff>();

			//
			// EXISTENCE CHECK.
			//
			bool ExistenceEqual;

			if (JProperty1 == null)
				if (JProperty2 == null)
					ExistenceEqual = true;
				else
					ExistenceEqual = false;
			else
				if (JProperty2 != null)
					ExistenceEqual = true;
				else
					ExistenceEqual = false;

			Diff ExistenceDiff = new Diff();
			ExistenceDiff.Category = DiffCategory.Existence;
			ExistenceDiff.AreEqual = ExistenceEqual;
			DiffList.Add(ExistenceDiff);

			if (ExistenceEqual)
			{
				//
				// TYPE CHECK.
				//
				bool TypesAreEqual = JProperty1.Type == JProperty2.Type;

				Diff TypeCheckDiff = new Diff();
				TypeCheckDiff.Category = DiffCategory.TypeCheck;
				TypeCheckDiff.AreEqual = TypesAreEqual;
				TypeCheckDiff.Type1 = JProperty1.Type;
				TypeCheckDiff.Type2 = JProperty2.Type;
				DiffList.Add(TypeCheckDiff);

				//
				// VALUE CHECK.
				//
				/*
				if (JObject1.Type == JTokenType.Property)
				{
				}
				else if (JObject1.Type == JTokenType.Array)
				{
				}
				else if (JObject1.Type == JTokenType.String)
				{
				}
				else if (JObject1.Type == JTokenType.Boolean)
				{
				}
				else if (JObject1.Type == JTokenType.Undefined)
				{
				}
				else if (JObject1.Type == JTokenType.Null)
				{
				}
				else if (JObject1.Type == JTokenType.Float)
				{
				}
				else if (JObject1.Type == JTokenType.Integer)
				{
				}
				else
					throw new NotImplementedException("DiffNode");
				*/

				//
				// COMPARE ALL CHILDREN.
				//
				/*
				foreach (JToken JToken1 in JProperty1.Children())
				{
					DiffList.AddRange(Compare(JToken1, JProperty2[JToken1], Level + 1));
				}
				*/
			}

			return DiffList.ToArray();
		}
	}
}
