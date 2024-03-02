namespace Attribinter.Semantic.SemanticConstructorArgumentParserCases;

internal sealed class ParserContext
{
    public static ParserContext Create()
    {
        SemanticConstructorArgumentParser parser = new();

        return new(parser);
    }

    public SemanticConstructorArgumentParser Parser { get; }

    private ParserContext(SemanticConstructorArgumentParser parser)
    {
        Parser = parser;
    }
}
