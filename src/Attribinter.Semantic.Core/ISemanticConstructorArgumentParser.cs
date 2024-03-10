namespace Attribinter.Semantic;

using Attribinter.Parameters;

using Microsoft.CodeAnalysis;

/// <summary>Parses the constructor arguments of attributes.</summary>
public interface ISemanticConstructorArgumentParser : IArgumentParser<IConstructorParameter, TypedConstant, AttributeData> { }
