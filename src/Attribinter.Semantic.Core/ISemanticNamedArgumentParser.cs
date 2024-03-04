namespace Attribinter.Semantic;

using Microsoft.CodeAnalysis;

/// <summary>Parses the named arguments of attributes.</summary>
public interface ISemanticNamedArgumentParser : INamedArgumentParser<TypedConstant, AttributeData> { }
