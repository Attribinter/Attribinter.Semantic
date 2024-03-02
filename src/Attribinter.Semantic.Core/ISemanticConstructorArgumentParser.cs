namespace Attribinter.Semantic;

using Microsoft.CodeAnalysis;

/// <summary>Parses the constructor arguments of attributes.</summary>
public interface ISemanticConstructorArgumentParser
{
    /// <summary>Attempts to parse the constructor arguments of an attribute.</summary>
    /// <param name="recorder">Records the parsed arguments.</param>
    /// <param name="attributeData">The parsed attribute.</param>
    /// <returns>A <see cref="bool"/> indicating whether the attempt was successful.</returns>
    public abstract bool TryParse(ISemanticConstructorArgumentRecorder recorder, AttributeData attributeData);
}
