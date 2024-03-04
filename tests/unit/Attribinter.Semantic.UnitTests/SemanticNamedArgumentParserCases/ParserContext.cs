namespace Attribinter.Semantic.SemanticNamedArgumentParserCases;

using Moq;

internal sealed class ParserContext
{
    public static ParserContext Create()
    {
        Mock<INamedParameterFactory> parameterFactoryMock = new();

        SemanticNamedArgumentParser parser = new(parameterFactoryMock.Object);

        return new(parser, parameterFactoryMock);
    }

    public SemanticNamedArgumentParser Parser { get; }

    public Mock<INamedParameterFactory> ParameterFactoryMock { get; }

    private ParserContext(SemanticNamedArgumentParser parser, Mock<INamedParameterFactory> parameterFactoryMock)
    {
        Parser = parser;

        ParameterFactoryMock = parameterFactoryMock;
    }
}
