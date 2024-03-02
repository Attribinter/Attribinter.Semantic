namespace Attribinter.Semantic;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;

/// <inheritdoc cref="ISemanticNamedArgumentParser"/>
public sealed class SemanticNamedArgumentParser : ISemanticNamedArgumentParser
{
    /// <summary>Instantiates a <see cref="SemanticNamedArgumentParser"/>, parsing the named arguments of attributes.</summary>
    public SemanticNamedArgumentParser() { }

    bool ISemanticNamedArgumentParser.TryParse(ISemanticNamedArgumentRecorder recorder, AttributeData attributeData)
    {
        if (recorder is null)
        {
            throw new ArgumentNullException(nameof(recorder));
        }

        if (attributeData is null)
        {
            throw new ArgumentNullException(nameof(attributeData));
        }

        return TryRecordArguments(recorder, attributeData.NamedArguments);
    }

    private bool TryRecordArguments(ISemanticNamedArgumentRecorder recorder, IReadOnlyList<KeyValuePair<string, TypedConstant>> arguments) => arguments.All((argument) => TryRecordArgument(recorder, argument.Key, argument.Value));
    private bool TryRecordArgument(ISemanticNamedArgumentRecorder recorder, string parameterName, TypedConstant argument) => recorder.TryRecordData(parameterName, argument);
}
