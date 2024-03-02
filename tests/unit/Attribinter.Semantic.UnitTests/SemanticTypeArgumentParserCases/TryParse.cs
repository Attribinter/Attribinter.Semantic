namespace Attribinter.Semantic.SemanticTypeArgumentParserCases;

using Microsoft.CodeAnalysis;

using Moq;

using System;

using Xunit;

public sealed class TryParse
{
    private bool Target(ISemanticTypeArgumentRecorder recorder, AttributeData attributeData) => Target(Context.Parser, recorder, attributeData);
    private static bool Target(ISemanticTypeArgumentParser parser, ISemanticTypeArgumentRecorder recorder, AttributeData attributeData) => parser.TryParse(recorder, attributeData);

    private readonly ParserContext Context = ParserContext.Create();

    [Fact]
    public void NullRecorder_ThrowsArgumentNullException()
    {
        var exception = Record.Exception(() => Target(null!, Mock.Of<AttributeData>()));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void NullAttributeData_ThrowsArgumentNullException()
    {
        var exception = Record.Exception(() => Target(Mock.Of<ISemanticTypeArgumentRecorder>(), null!));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void NullAttributeClass_ReturnsFalse()
    {
        CustomAttributeData attributeData = new();

        Mock<ISemanticTypeArgumentRecorder> recorderMock = new();

        var result = Target(recorderMock.Object, attributeData);

        Assert.False(result);

        recorderMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void MoreParametersThanArguments_ReturnsFalse()
    {
        var parameter = Mock.Of<ITypeParameterSymbol>();

        Mock<INamedTypeSymbol> attributeClassMock = new();

        attributeClassMock.Setup(static (type) => type.TypeParameters).Returns([parameter]);
        attributeClassMock.Setup(static (type) => type.TypeArguments).Returns([]);

        CustomAttributeData attributeData = new() { AttributeClass = attributeClassMock.Object };

        Mock<ISemanticTypeArgumentRecorder> recorderMock = new();

        var result = Target(recorderMock.Object, attributeData);

        Assert.False(result);

        recorderMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void MoreArgumentsThanParameters_ReturnsFalse()
    {
        var argument = Mock.Of<ITypeSymbol>();

        Mock<INamedTypeSymbol> attributeClassMock = new();

        attributeClassMock.Setup(static (type) => type.TypeParameters).Returns([]);
        attributeClassMock.Setup(static (type) => type.TypeArguments).Returns([argument]);

        CustomAttributeData attributeData = new() { AttributeClass = attributeClassMock.Object };

        Mock<ISemanticTypeArgumentRecorder> recorderMock = new();

        var result = Target(recorderMock.Object, attributeData);

        Assert.False(result);

        recorderMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void FalseReturningRecorder_ReturnsFalse()
    {
        var parameter1 = Mock.Of<ITypeParameterSymbol>();
        var parameter2 = Mock.Of<ITypeParameterSymbol>();
        var parameter3 = Mock.Of<ITypeParameterSymbol>();

        var argument1 = Mock.Of<ITypeSymbol>();
        var argument2 = Mock.Of<ITypeSymbol>();
        var argument3 = Mock.Of<ITypeSymbol>();

        Mock<INamedTypeSymbol> attributeClassMock = new();

        attributeClassMock.Setup(static (type) => type.TypeParameters).Returns([parameter1, parameter2, parameter3]);
        attributeClassMock.Setup(static (type) => type.TypeArguments).Returns([argument1, argument2, argument3]);

        CustomAttributeData attributeData = new() { AttributeClass = attributeClassMock.Object };

        Mock<ISemanticTypeArgumentRecorder> recorderMock = new();

        recorderMock.Setup(static (recorder) => recorder.TryRecordData(It.IsAny<ITypeParameterSymbol>(), It.IsAny<ITypeSymbol>())).Returns(true);
        recorderMock.Setup((recorder) => recorder.TryRecordData(parameter2, argument2)).Returns(false);

        var result = Target(recorderMock.Object, attributeData);

        Assert.False(result);

        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter1, argument1), Times.Once());
        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter2, argument2), Times.Once());

        recorderMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void TrueReturningRecorder_RecordsAllArguments_ReturnsTrue()
    {
        var parameter1 = Mock.Of<ITypeParameterSymbol>();
        var parameter2 = Mock.Of<ITypeParameterSymbol>();

        var argument1 = Mock.Of<ITypeSymbol>();
        var argument2 = Mock.Of<ITypeSymbol>();

        Mock<INamedTypeSymbol> attributeClassMock = new();

        attributeClassMock.Setup(static (type) => type.TypeParameters).Returns([parameter1, parameter2]);
        attributeClassMock.Setup(static (type) => type.TypeArguments).Returns([argument1, argument2]);

        CustomAttributeData attributeData = new() { AttributeClass = attributeClassMock.Object };

        Mock<ISemanticTypeArgumentRecorder> recorderMock = new();

        recorderMock.Setup(static (recorder) => recorder.TryRecordData(It.IsAny<ITypeParameterSymbol>(), It.IsAny<ITypeSymbol>())).Returns(true);

        var result = Target(recorderMock.Object, attributeData);

        Assert.True(result);

        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter1, argument1), Times.Once());
        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter2, argument2), Times.Once());

        recorderMock.VerifyNoOtherCalls();
    }
}
