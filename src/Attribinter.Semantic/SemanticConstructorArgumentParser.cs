namespace Attribinter.Semantic;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;

/// <inheritdoc cref="ISemanticConstructorArgumentParser"/>
public sealed class SemanticConstructorArgumentParser : ISemanticConstructorArgumentParser
{
    private readonly IConstructorParameterFactory ParameterFactory;

    /// <summary>Instantiates a <see cref="SemanticConstructorArgumentParser"/>, parsing the constructor arguments of attributes.</summary>
    /// <param name="parameterFactory">Handles creation of <see cref="IConstructorParameter"/>.</param>
    public SemanticConstructorArgumentParser(IConstructorParameterFactory parameterFactory)
    {
        ParameterFactory = parameterFactory ?? throw new ArgumentNullException(nameof(parameterFactory));
    }

    bool IArgumentParser<IConstructorParameter, TypedConstant, AttributeData>.TryParse(IArgumentRecorder<IConstructorParameter, TypedConstant> recorder, AttributeData attribute)
    {
        if (recorder is null)
        {
            throw new ArgumentNullException(nameof(recorder));
        }

        if (attribute is null)
        {
            throw new ArgumentNullException(nameof(attribute));
        }

        if (attribute.AttributeConstructor is not IMethodSymbol targetConstructor)
        {
            return false;
        }

        return TryRecordArguments(recorder, targetConstructor.Parameters, attribute.ConstructorArguments);
    }

    private bool TryRecordArguments(IArgumentRecorder<IConstructorParameter, TypedConstant> recorder, IReadOnlyList<IParameterSymbol> parameters, IReadOnlyList<TypedConstant> arguments)
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

    private bool TryRecordArgument(IArgumentRecorder<IConstructorParameter, TypedConstant> recorder, IParameterSymbol parameter, TypedConstant argument) => recorder.TryRecordData(ParameterFactory.Create(parameter), argument);
}
