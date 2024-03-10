namespace Attribinter.Semantic.SemanticTypeArgumentParserCases;

using Attribinter.Parameters;

using Microsoft.CodeAnalysis;

using Moq;

using System;

using Xunit;

public sealed class TryParse
{
    private bool Target(IArgumentRecorder<ITypeParameter, ITypeSymbol> recorder, AttributeData attributeData) => Target(Context.Parser, recorder, attributeData);
    private static bool Target(ISemanticTypeArgumentParser parser, IArgumentRecorder<ITypeParameter, ITypeSymbol> recorder, AttributeData attributeData) => parser.TryParse(recorder, attributeData);

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
        var exception = Record.Exception(() => Target(Mock.Of<IArgumentRecorder<ITypeParameter, ITypeSymbol>>(), null!));

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void NullAttributeClass_ReturnsFalse()
    {
        CustomAttributeData attributeData = new();

        Mock<IArgumentRecorder<ITypeParameter, ITypeSymbol>> recorderMock = new();

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

        Mock<IArgumentRecorder<ITypeParameter, ITypeSymbol>> recorderMock = new();

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

        Mock<IArgumentRecorder<ITypeParameter, ITypeSymbol>> recorderMock = new();

        var result = Target(recorderMock.Object, attributeData);

        Assert.False(result);

        recorderMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void FalseReturningRecorder_ReturnsFalse()
    {
        var parameterSymbol1 = Mock.Of<ITypeParameterSymbol>();
        var parameterSymbol2 = Mock.Of<ITypeParameterSymbol>();
        var parameterSymbol3 = Mock.Of<ITypeParameterSymbol>();

        var parameter1 = Mock.Of<ITypeParameter>();
        var parameter2 = Mock.Of<ITypeParameter>();
        var parameter3 = Mock.Of<ITypeParameter>();

        var argument1 = Mock.Of<ITypeSymbol>();
        var argument2 = Mock.Of<ITypeSymbol>();
        var argument3 = Mock.Of<ITypeSymbol>();

        Mock<INamedTypeSymbol> attributeClassMock = new();

        attributeClassMock.Setup(static (type) => type.TypeParameters).Returns([parameterSymbol1, parameterSymbol2, parameterSymbol3]);
        attributeClassMock.Setup(static (type) => type.TypeArguments).Returns([argument1, argument2, argument3]);

        Context.ParameterFactoryMock.Setup((factory) => factory.Create(parameterSymbol1)).Returns(parameter1);
        Context.ParameterFactoryMock.Setup((factory) => factory.Create(parameterSymbol2)).Returns(parameter2);
        Context.ParameterFactoryMock.Setup((factory) => factory.Create(parameterSymbol3)).Returns(parameter3);

        CustomAttributeData attributeData = new() { AttributeClass = attributeClassMock.Object };

        Mock<IArgumentRecorder<ITypeParameter, ITypeSymbol>> recorderMock = new();

        recorderMock.Setup(static (recorder) => recorder.TryRecordData(It.IsAny<ITypeParameter>(), It.IsAny<ITypeSymbol>())).Returns(true);
        recorderMock.Setup((recorder) => recorder.TryRecordData(parameter2, argument2)).Returns(false);

        var result = Target(recorderMock.Object, attributeData);

        Assert.False(result);

        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter1, argument1), Times.Once());
        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter2, argument2), Times.Once());

        recorderMock.VerifyNoOtherCalls();

        Context.ParameterFactoryMock.Verify((factory) => factory.Create(parameterSymbol1), Times.Once());
        Context.ParameterFactoryMock.Verify((factory) => factory.Create(parameterSymbol2), Times.Once());

        Context.ParameterFactoryMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void TrueReturningRecorder_RecordsAllArguments_ReturnsTrue()
    {
        var parameterSymbol1 = Mock.Of<ITypeParameterSymbol>();
        var parameterSymbol2 = Mock.Of<ITypeParameterSymbol>();

        var parameter1 = Mock.Of<ITypeParameter>();
        var parameter2 = Mock.Of<ITypeParameter>();

        var argument1 = Mock.Of<ITypeSymbol>();
        var argument2 = Mock.Of<ITypeSymbol>();

        Mock<INamedTypeSymbol> attributeClassMock = new();

        attributeClassMock.Setup(static (type) => type.TypeParameters).Returns([parameterSymbol1, parameterSymbol2]);
        attributeClassMock.Setup(static (type) => type.TypeArguments).Returns([argument1, argument2]);

        Context.ParameterFactoryMock.Setup((factory) => factory.Create(parameterSymbol1)).Returns(parameter1);
        Context.ParameterFactoryMock.Setup((factory) => factory.Create(parameterSymbol2)).Returns(parameter2);

        CustomAttributeData attributeData = new() { AttributeClass = attributeClassMock.Object };

        Mock<IArgumentRecorder<ITypeParameter, ITypeSymbol>> recorderMock = new();

        recorderMock.Setup(static (recorder) => recorder.TryRecordData(It.IsAny<ITypeParameter>(), It.IsAny<ITypeSymbol>())).Returns(true);

        var result = Target(recorderMock.Object, attributeData);

        Assert.True(result);

        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter1, argument1), Times.Once());
        recorderMock.Verify((recorder) => recorder.TryRecordData(parameter2, argument2), Times.Once());

        recorderMock.VerifyNoOtherCalls();

        Context.ParameterFactoryMock.Verify((factory) => factory.Create(parameterSymbol1), Times.Once());
        Context.ParameterFactoryMock.Verify((factory) => factory.Create(parameterSymbol2), Times.Once());

        Context.ParameterFactoryMock.VerifyNoOtherCalls();
    }
}
