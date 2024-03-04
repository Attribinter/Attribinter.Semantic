namespace Attribinter.Semantic.SemanticNamedArgumentParserCases;

using Microsoft.CodeAnalysis;

using Moq;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

public sealed class TryParse
{
    private bool Target(IArgumentRecorder<INamedParameter, TypedConstant> recorder, AttributeData attributeData) => Target(Context.Parser, recorder, attributeData);
    private static bool Target(ISemanticNamedArgumentParser parser, IArgumentRecorder<INamedParameter, TypedConstant> recorder, AttributeData attributeData) => parser.TryParse(recorder, attributeData);

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
        var exception = Record.Exception(() => Target(Mock.Of<IArgumentRecorder<INamedParameter, TypedConstant>>(), null!));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public async Task FalseReturningRecorder_ReturnsFalse()
    {
        var parameterName1 = "Foo1";
        var parameterName2 = "Foo2";
        var parameterName3 = "Foo3";

        var parameter1 = Mock.Of<INamedParameter>();
        var parameter2 = Mock.Of<INamedParameter>();
        var parameter3 = Mock.Of<INamedParameter>();

        var argument1 = new KeyValuePair<string, TypedConstant>(parameterName1, await TypedConstantStore.GetNext());
        var argument2 = new KeyValuePair<string, TypedConstant>(parameterName2, await TypedConstantStore.GetNext());
        var argument3 = new KeyValuePair<string, TypedConstant>(parameterName3, await TypedConstantStore.GetNext());

        Context.ParameterFactoryMock.Setup((factory) => factory.Create(parameterName1)).Returns(parameter1);
        Context.ParameterFactoryMock.Setup((factory) => factory.Create(parameterName2)).Returns(parameter2);
        Context.ParameterFactoryMock.Setup((factory) => factory.Create(parameterName3)).Returns(parameter3);

        CustomAttributeData attributeData = new() { NamedArguments = [argument1, argument2, argument3] };

        Mock<IArgumentRecorder<INamedParameter, TypedConstant>> recorderMock = new();

        recorderMock.Setup(static (recorder) => recorder.TryRecordData(It.IsAny<INamedParameter>(), It.IsAny<TypedConstant>())).Returns(true);
        recorderMock.Setup((recorder) => recorder.TryRecordData(parameter2, argument2.Value)).Returns(false);

        var result = Target(recorderMock.Object, attributeData);

        Assert.False(result);

        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter1, argument1.Value), Times.Once());
        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter2, argument2.Value), Times.Once());

        recorderMock.VerifyNoOtherCalls();

        Context.ParameterFactoryMock.Verify((factory) => factory.Create(parameterName1), Times.Once());
        Context.ParameterFactoryMock.Verify((factory) => factory.Create(parameterName2), Times.Once());

        Context.ParameterFactoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task TrueReturningRecorder_RecordsAllArguments_ReturnsTrue()
    {
        var parameterName1 = "Foo1";
        var parameterName2 = "Foo2";

        var parameter1 = Mock.Of<INamedParameter>();
        var parameter2 = Mock.Of<INamedParameter>();

        var argument1 = new KeyValuePair<string, TypedConstant>(parameterName1, await TypedConstantStore.GetNext());
        var argument2 = new KeyValuePair<string, TypedConstant>(parameterName2, await TypedConstantStore.GetNext());

        Context.ParameterFactoryMock.Setup((factory) => factory.Create(parameterName1)).Returns(parameter1);
        Context.ParameterFactoryMock.Setup((factory) => factory.Create(parameterName2)).Returns(parameter2);

        CustomAttributeData attributeData = new() { NamedArguments = [argument1, argument2] };

        Mock<IArgumentRecorder<INamedParameter, TypedConstant>> recorderMock = new();

        recorderMock.Setup(static (recorder) => recorder.TryRecordData(It.IsAny<INamedParameter>(), It.IsAny<TypedConstant>())).Returns(true);

        var result = Target(recorderMock.Object, attributeData);

        Assert.True(result);

        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter1, argument1.Value), Times.Once());
        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter2, argument2.Value), Times.Once());

        recorderMock.VerifyNoOtherCalls();

        Context.ParameterFactoryMock.Verify((factory) => factory.Create(parameterName1), Times.Once());
        Context.ParameterFactoryMock.Verify((factory) => factory.Create(parameterName2), Times.Once());

        Context.ParameterFactoryMock.VerifyNoOtherCalls();
    }
}
