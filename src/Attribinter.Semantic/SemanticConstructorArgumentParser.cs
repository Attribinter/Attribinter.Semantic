namespace Attribinter.Semantic;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;

/// <inheritdoc cref="ISemanticConstructorArgumentParser"/>
public sealed class SemanticConstructorArgumentParser : ISemanticConstructorArgumentParser
{
    /// <summary>Instantiates a <see cref="SemanticConstructorArgumentParser"/>, parsing the constructor arguments of attributes.</summary>
    public SemanticConstructorArgumentParser() { }

    bool ISemanticConstructorArgumentParser.TryParse(ISemanticConstructorArgumentRecorder recorder, AttributeData attributeData)
    {
        if (recorder is null)
        {
            throw new ArgumentNullException(nameof(recorder));
        }

        if (attributeData is null)
        {
            throw new ArgumentNullException(nameof(attributeData));
        }

        if (attributeData.AttributeConstructor is not IMethodSymbol targetConstructor)
        {
            return false;
        }

        return TryRecordArguments(recorder, targetConstructor.Parameters, attributeData.ConstructorArguments);
    }

    private bool TryRecordArguments(ISemanticConstructorArgumentRecorder recorder, IReadOnlyList<IParameterSymbol> parameters, IReadOnlyList<TypedConstant> arguments)
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

    private bool TryRecordArgument(ISemanticConstructorArgumentRecorder recorder, IParameterSymbol parameter, TypedConstant argument) => recorder.TryRecordData(parameter, argument);
}
