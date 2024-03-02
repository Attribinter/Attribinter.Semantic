namespace Attribinter.Semantic;

using Microsoft.CodeAnalysis;

/// <summary>Records the arguments of type parameters.</summary>
public interface ISemanticTypeArgumentRecorder : ITypeArgumentRecorder<ITypeSymbol> { }
