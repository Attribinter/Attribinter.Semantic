namespace Attribinter.Semantic;

using Attribinter.Parameters;

using Microsoft.CodeAnalysis;

/// <summary>Parses the type arguments of attributes.</summary>
public interface ISemanticTypeArgumentParser : IArgumentParser<ITypeParameter, ITypeSymbol, AttributeData> { }
