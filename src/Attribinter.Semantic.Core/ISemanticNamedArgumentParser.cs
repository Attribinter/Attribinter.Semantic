namespace Attribinter.Semantic;

using Microsoft.CodeAnalysis;

/// <summary>Parses the named arguments of attributes.</summary>
public interface ISemanticNamedArgumentParser
{
    /// <summary>Attempts to parse the named arguments of an attribute.</summary>
    /// <param name="recorder">Records the parsed arguments.</param>
    /// <param name="attributeData">The parsed attribute.</param>
    /// <returns>A <see cref="bool"/> indicating whether the attempt was successful.</returns>
    public abstract bool TryParse(ISemanticNamedArgumentRecorder recorder, AttributeData attributeData);
}
