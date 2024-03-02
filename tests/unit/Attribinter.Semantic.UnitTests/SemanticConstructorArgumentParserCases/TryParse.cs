namespace Attribinter.Semantic.SemanticConstructorArgumentParserCases;

using Microsoft.CodeAnalysis;

using Moq;

using System;
using System.Threading.Tasks;

using Xunit;

public sealed class TryParse
{
    private bool Target(ISemanticConstructorArgumentRecorder recorder, AttributeData attributeData) => Target(Context.Parser, recorder, attributeData);
    private static bool Target(ISemanticConstructorArgumentParser parser, ISemanticConstructorArgumentRecorder recorder, AttributeData attributeData) => parser.TryParse(recorder, attributeData);

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
        var exception = Record.Exception(() => Target(Mock.Of<ISemanticConstructorArgumentRecorder>(), null!));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void NullAttributeConstructor_ReturnsFalse()
    {
        CustomAttributeData attributeData = new();

        Mock<ISemanticConstructorArgumentRecorder> recorderMock = new();

        var result = Target(recorderMock.Object, attributeData);

        Assert.False(result);

        recorderMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void MoreParametersThanArguments_ReturnsFalse()
    {
        var parameter = Mock.Of<IParameterSymbol>();

        Mock<IMethodSymbol> attributeConstructorMock = new();

        attributeConstructorMock.Setup(static (method) => method.Parameters).Returns([parameter]);

        CustomAttributeData attributeData = new() { AttributeConstructor = attributeConstructorMock.Object, ConstructorArguments = [] };

        Mock<ISemanticConstructorArgumentRecorder> recorderMock = new();

        var result = Target(recorderMock.Object, attributeData);

        Assert.False(result);

        recorderMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void MoreArgumentsThanParameters_ReturnsFalse()
    {
        var argument = new TypedConstant();

        Mock<IMethodSymbol> attributeConstructorMock = new();

        attributeConstructorMock.Setup(static (method) => method.Parameters).Returns([]);

        CustomAttributeData attributeData = new() { AttributeConstructor = attributeConstructorMock.Object, ConstructorArguments = [argument] };

        Mock<ISemanticConstructorArgumentRecorder> recorderMock = new();

        var result = Target(recorderMock.Object, attributeData);

        Assert.False(result);

        recorderMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task FalseReturningRecorder_ReturnsFalse()
    {
        var parameter1 = Mock.Of<IParameterSymbol>();
        var parameter2 = Mock.Of<IParameterSymbol>();
        var parameter3 = Mock.Of<IParameterSymbol>();

        var argument1 = await TypedConstantStore.GetNext();
        var argument2 = await TypedConstantStore.GetNext();
        var argument3 = await TypedConstantStore.GetNext();

        Mock<IMethodSymbol> attributeConstructorMock = new();

        attributeConstructorMock.Setup(static (method) => method.Parameters).Returns([parameter1, parameter2, parameter3]);

        CustomAttributeData attributeData = new() { AttributeConstructor = attributeConstructorMock.Object, ConstructorArguments = [argument1, argument2, argument3] };

        Mock<ISemanticConstructorArgumentRecorder> recorderMock = new();

        recorderMock.Setup(static (recorder) => recorder.TryRecordData(It.IsAny<IParameterSymbol>(), It.IsAny<TypedConstant>())).Returns(true);
        recorderMock.Setup((recorder) => recorder.TryRecordData(parameter2, argument2)).Returns(false);

        var result = Target(recorderMock.Object, attributeData);

        Assert.False(result);

        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter1, argument1), Times.Once());
        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter2, argument2), Times.Once());

        recorderMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task TrueReturningRecorder_RecordsAllArguments_ReturnsTrue()
    {
        var parameter1 = Mock.Of<IParameterSymbol>();
        var parameter2 = Mock.Of<IParameterSymbol>();

        var argument1 = await TypedConstantStore.GetNext();
        var argument2 = await TypedConstantStore.GetNext();

        Mock<IMethodSymbol> attributeConstructorMock = new();

        attributeConstructorMock.Setup(static (method) => method.Parameters).Returns([parameter1, parameter2]);

        CustomAttributeData attributeData = new() { AttributeConstructor = attributeConstructorMock.Object, ConstructorArguments = [argument1, argument2] };

        Mock<ISemanticConstructorArgumentRecorder> recorderMock = new();

        recorderMock.Setup(static (recorder) => recorder.TryRecordData(It.IsAny<IParameterSymbol>(), It.IsAny<TypedConstant>())).Returns(true);

        var result = Target(recorderMock.Object, attributeData);

        Assert.True(result);

        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter1, argument1), Times.Once());
        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter2, argument2), Times.Once());

        recorderMock.VerifyNoOtherCalls();
    }
}
