namespace Attribinter.Semantic.SemanticConstructorArgumentParserCases;

using Moq;

internal sealed class ParserContext
{
    public static ParserContext Create()
    {
        Mock<IConstructorParameterFactory> parameterFactoryMock = new();

        SemanticConstructorArgumentParser parser = new(parameterFactoryMock.Object);

        return new(parser, parameterFactoryMock);
    }

    public SemanticConstructorArgumentParser Parser { get; }

    public Mock<IConstructorParameterFactory> ParameterFactoryMock { get; }

    private ParserContext(SemanticConstructorArgumentParser parser, Mock<IConstructorParameterFactory> parameterFactoryMock)
    {
        Parser = parser;

        ParameterFactoryMock = parameterFactoryMock;
    }
}
