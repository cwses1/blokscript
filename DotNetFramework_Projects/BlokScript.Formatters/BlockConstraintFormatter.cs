using System.Text;
using BlokScript.Entities;
using BlokScript.Filters;

namespace BlokScript.Formatters
{
	public class BlockConstraintFormatter
	{
		public static string FormatLog (BlockConstraint Constraint)
		{
			StringBuilder OutputBuilder = new StringBuilder();
			OutputBuilder.AppendLine("-----");
			OutputBuilder.AppendLine($"BlockConstraint");
			OutputBuilder.AppendLine("-----");
			OutputBuilder.AppendLine($"Field: {Constraint.Field}");
			OutputBuilder.AppendLine($"Operator: {Constraint.Operator}");
			OutputBuilder.AppendLine($"ConstraintData: {Constraint.ConstraintData}");
			OutputBuilder.AppendLine($"ConstraintDataType: {Constraint.ConstraintDataType}");
			OutputBuilder.AppendLine($"ChildConstraint: {Constraint.ChildConstraint}");
			OutputBuilder.AppendLine($"LeftChildConstraint: {Constraint.LeftChildConstraint}");
			return OutputBuilder.ToString();
		}
	}
}
