namespace Attribinter.Semantic.SemanticTypeArgumentParserCases;

internal sealed class ParserContext
{
    public static ParserContext Create()
    {
        SemanticTypeArgumentParser parser = new();

        return new(parser);
    }

    public SemanticTypeArgumentParser Parser { get; }

    private ParserContext(SemanticTypeArgumentParser parser)
    {
        Parser = parser;
    }
}
