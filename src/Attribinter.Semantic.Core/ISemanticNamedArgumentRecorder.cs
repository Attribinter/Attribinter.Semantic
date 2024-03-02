namespace Attribinter.Semantic;

using Microsoft.CodeAnalysis;

/// <summary>Records the arguments of named parameters.</summary>
public interface ISemanticNamedArgumentRecorder : INamedArgumentRecorder<TypedConstant> { }
