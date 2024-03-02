namespace Attribinter.Semantic;

using Microsoft.CodeAnalysis;

/// <summary>Records the arguments of constructor parameters.</summary>
public interface ISemanticConstructorArgumentRecorder : IConstructorArgumentRecorder<TypedConstant> { }
