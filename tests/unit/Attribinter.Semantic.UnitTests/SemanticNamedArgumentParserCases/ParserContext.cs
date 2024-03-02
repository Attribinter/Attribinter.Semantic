namespace Attribinter.Semantic.SemanticNamedArgumentParserCases;

internal sealed class ParserContext
{
    public static ParserContext Create()
    {
        SemanticNamedArgumentParser parser = new();

        return new(parser);
    }

    public SemanticNamedArgumentParser Parser { get; }

    private ParserContext(SemanticNamedArgumentParser parser)
    {
        Parser = parser;
    }
}
