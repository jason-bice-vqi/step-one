using System.Diagnostics;

namespace ViaQuestInc.StepOne.Core.Kernel;

/// <summary>
/// An attribute indicating that the property to which it's attached will be set by a particular operation, and not
/// necessarily arrive as part of an initial payload, constructor execution, etc.
/// </summary>
/// <param name="operationName">The operation responsible for setting this property.</param>
[AttributeUsage(AttributeTargets.Property)]
[Conditional("DEBUG")]
public class SetByOperationAttribute(string operationName) : Attribute
{
    /// <summary>
    /// The operation responsible for setting this property.
    /// </summary>
    public string OperationName { get; set; } = operationName;
}