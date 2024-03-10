namespace Attribinter.Semantic;

using Attribinter.Parameters;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;

/// <inheritdoc cref="ISemanticTypeArgumentParser"/>
public sealed class SemanticTypeArgumentParser : ISemanticTypeArgumentParser
{
    private readonly ITypeParameterFactory ParameterFactory;

    /// <summary>Instantiates a <see cref="SemanticTypeArgumentParser"/>, parsing the type arguments of attributes.</summary>
    /// <param name="parameterFactory">Handles creation of <see cref="ITypeParameter"/>.</param>
    public SemanticTypeArgumentParser(ITypeParameterFactory parameterFactory)
    {
        ParameterFactory = parameterFactory ?? throw new ArgumentNullException(nameof(parameterFactory));
    }

    bool IArgumentParser<ITypeParameter, ITypeSymbol, AttributeData>.TryParse(IArgumentRecorder<ITypeParameter, ITypeSymbol> recorder, AttributeData attribute)
    {
        if (recorder is null)
        {
            throw new ArgumentNullException(nameof(recorder));
        }

        if (attribute is null)
        {
            throw new ArgumentNullException(nameof(attribute));
        }

        if (attribute.AttributeClass is not INamedTypeSymbol attributeClass)
        {
            return false;
        }

        return TryRecordArguments(recorder, attributeClass.TypeParameters, attributeClass.TypeArguments);
    }

    private bool TryRecordArguments(IArgumentRecorder<ITypeParameter, ITypeSymbol> recorder, IReadOnlyList<ITypeParameterSymbol> parameters, IReadOnlyList<ITypeSymbol> arguments)
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

    private bool TryRecordArgument(IArgumentRecorder<ITypeParameter, ITypeSymbol> recorder, ITypeParameterSymbol parameter, ITypeSymbol argument) => recorder.TryRecordData(ParameterFactory.Create(parameter), argument);
}
