namespace Attribinter.Semantic;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;

/// <inheritdoc cref="ISemanticTypeArgumentParser"/>
public sealed class SemanticTypeArgumentParser : ISemanticTypeArgumentParser
{
    /// <summary>Instantiates a <see cref="SemanticTypeArgumentParser"/>, parsing the type arguments of attributes.</summary>
    public SemanticTypeArgumentParser() { }

    bool ISemanticTypeArgumentParser.TryParse(ISemanticTypeArgumentRecorder recorder, AttributeData attributeData)
    {
        if (recorder is null)
        {
            throw new ArgumentNullException(nameof(recorder));
        }

        if (attributeData is null)
        {
            throw new ArgumentNullException(nameof(attributeData));
        }

        if (attributeData.AttributeClass is not INamedTypeSymbol attributeClass)
        {
            return false;
        }

        return TryRecordArguments(recorder, attributeClass.TypeParameters, attributeClass.TypeArguments);
    }

    private bool TryRecordArguments(ISemanticTypeArgumentRecorder recorder, IReadOnlyList<ITypeParameterSymbol> parameters, IReadOnlyList<ITypeSymbol> arguments)
    {
        if (parameters.Count != arguments.Count)
        {
            return false;
        }

        for (var i = 0; i < parameters.Count; i++)
        {
            if (TryRecordArgument(recorder, parameters[i], arguments[i]) is false)
            {
                return false;
            }
        }

        return true;
    }

    private bool TryRecordArgument(ISemanticTypeArgumentRecorder recorder, ITypeParameterSymbol parameter, ITypeSymbol argument) => recorder.TryRecordData(parameter, argument);
}
