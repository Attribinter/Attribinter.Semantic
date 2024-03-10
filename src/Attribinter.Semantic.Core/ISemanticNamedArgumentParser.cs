namespace Attribinter.Semantic;

using Attribinter.Parameters;

using Microsoft.CodeAnalysis;

/// <summary>Parses the named arguments of attributes.</summary>
public interface ISemanticNamedArgumentParser : IArgumentParser<INamedParameter, TypedConstant, AttributeData> { }
