namespace Attribinter.Semantic.SemanticNamedArgumentParserCases;

using Microsoft.CodeAnalysis;

using Moq;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

public sealed class TryParse
{
    private bool Target(ISemanticNamedArgumentRecorder recorder, AttributeData attributeData) => Target(Context.Parser, recorder, attributeData);
    private static bool Target(ISemanticNamedArgumentParser parser, ISemanticNamedArgumentRecorder recorder, AttributeData attributeData) => parser.TryParse(recorder, attributeData);

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
        var exception = Record.Exception(() => Target(Mock.Of<ISemanticNamedArgumentRecorder>(), null!));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public async Task FalseReturningRecorder_ReturnsFalse()
    {
        var argument1 = new KeyValuePair<string, TypedConstant>("argument1", await TypedConstantStore.GetNext());
        var argument2 = new KeyValuePair<string, TypedConstant>("argument2", await TypedConstantStore.GetNext());
        var argument3 = new KeyValuePair<string, TypedConstant>("argument3", await TypedConstantStore.GetNext());

        CustomAttributeData attributeData = new() { NamedArguments = [argument1, argument2, argument3] };

        Mock<ISemanticNamedArgumentRecorder> recorderMock = new();

        recorderMock.Setup(static (recorder) => recorder.TryRecordData(It.IsAny<string>(), It.IsAny<TypedConstant>())).Returns(true);
        recorderMock.Setup((recorder) => recorder.TryRecordData(argument2.Key, argument2.Value)).Returns(false);

        var result = Target(recorderMock.Object, attributeData);

        Assert.False(result);

        recorderMock.Verify((recorder) => recorder.TryRecordData(argument1.Key, argument1.Value), Times.Once());
        recorderMock.Verify((recorder) => recorder.TryRecordData(argument2.Key, argument2.Value), Times.Once());

        recorderMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task TrueReturningRecorder_RecordsAllArguments_ReturnsTrue()
    {
        var argument1 = new KeyValuePair<string, TypedConstant>("argument1", await TypedConstantStore.GetNext());
        var argument2 = new KeyValuePair<string, TypedConstant>("argument2", await TypedConstantStore.GetNext());

        CustomAttributeData attributeData = new() { NamedArguments = [argument1, argument2] };

        Mock<ISemanticNamedArgumentRecorder> recorderMock = new();

        recorderMock.Setup(static (recorder) => recorder.TryRecordData(It.IsAny<string>(), It.IsAny<TypedConstant>())).Returns(true);

        var result = Target(recorderMock.Object, attributeData);

        Assert.True(result);

        recorderMock.Verify((recorder) => recorder.TryRecordData(argument1.Key, argument1.Value), Times.Once());
        recorderMock.Verify((recorder) => recorder.TryRecordData(argument2.Key, argument2.Value), Times.Once());

        recorderMock.VerifyNoOtherCalls();
    }
}
