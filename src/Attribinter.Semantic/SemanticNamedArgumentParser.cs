namespace Attribinter.Semantic;

using Attribinter.Parameters;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;

/// <inheritdoc cref="ISemanticNamedArgumentParser"/>
public sealed class SemanticNamedArgumentParser : ISemanticNamedArgumentParser
{
    private readonly INamedParameterFactory ParameterFactory;

    /// <summary>Instantiates a <see cref="SemanticNamedArgumentParser"/>, parsing the named arguments of attributes.</summary>
    /// <param name="parameterFactory">Handles creation of <see cref="INamedParameter"/>.</param>
    public SemanticNamedArgumentParser(INamedParameterFactory parameterFactory)
    {
        ParameterFactory = parameterFactory ?? throw new ArgumentNullException(nameof(parameterFactory));
    }

    bool IArgumentParser<INamedParameter, TypedConstant, AttributeData>.TryParse(IArgumentRecorder<INamedParameter, TypedConstant> recorder, AttributeData attribute)
    {
        if (recorder is null)
        {
            throw new ArgumentNullException(nameof(recorder));
        }

        if (attribute is null)
        {
            throw new ArgumentNullException(nameof(attribute));
        }

        return TryRecordArguments(recorder, attribute.NamedArguments);
    }

    private bool TryRecordArguments(IArgumentRecorder<INamedParameter, TypedConstant> recorder, IReadOnlyList<KeyValuePair<string, TypedConstant>> arguments) => arguments.All((argument) => TryRecordArgument(recorder, argument.Key, argument.Value));
    private bool TryRecordArgument(IArgumentRecorder<INamedParameter, TypedConstant> recorder, string parameterName, TypedConstant argument) => recorder.TryRecordData(ParameterFactory.Create(parameterName), argument);
}
