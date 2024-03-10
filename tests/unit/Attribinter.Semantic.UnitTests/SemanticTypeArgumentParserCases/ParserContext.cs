namespace Attribinter.Semantic.SemanticTypeArgumentParserCases;

using Attribinter.Parameters;

using Moq;

internal sealed class ParserContext
{
    public static ParserContext Create()
    {
        Mock<ITypeParameterFactory> parameterFactoryMock = new();

        SemanticTypeArgumentParser parser = new(parameterFactoryMock.Object);

        return new(parser, parameterFactoryMock);
    }

    public SemanticTypeArgumentParser Parser { get; }

    public Mock<ITypeParameterFactory> ParameterFactoryMock { get; }

    private ParserContext(SemanticTypeArgumentParser parser, Mock<ITypeParameterFactory> parameterFactoryMock)
    {
        Parser = parser;

        ParameterFactoryMock = parameterFactoryMock;
    }
}
