namespace Attribinter.Semantic;

using Microsoft.CodeAnalysis;

/// <summary>Parses the constructor arguments of attributes.</summary>
public interface ISemanticConstructorArgumentParser : IConstructorArgumentParser<TypedConstant, AttributeData> { }
