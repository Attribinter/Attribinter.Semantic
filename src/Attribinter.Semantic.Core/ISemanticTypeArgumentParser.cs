namespace Attribinter.Semantic;

using Microsoft.CodeAnalysis;

/// <summary>Parses the type arguments of attributes.</summary>
public interface ISemanticTypeArgumentParser : ITypeArgumentParser<ITypeSymbol, AttributeData> { }
