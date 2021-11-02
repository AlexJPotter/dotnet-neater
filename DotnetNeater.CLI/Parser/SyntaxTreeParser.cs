using System;
using System.Linq;
using DotnetNeater.CLI.Operations;
using DotnetNeater.CLI.Parser.Declarations;
using DotnetNeater.CLI.Parser.Declarators;
using DotnetNeater.CLI.Parser.Directives;
using DotnetNeater.CLI.Parser.Expressions;
using DotnetNeater.CLI.Parser.Names;
using DotnetNeater.CLI.Parser.Other;
using DotnetNeater.CLI.Parser.Statements;
using DotnetNeater.CLI.Parser.Types;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static DotnetNeater.CLI.Operations.Operator;

namespace DotnetNeater.CLI.Parser
{
    public static class SyntaxTreeParser
    {
        public static Operation Parse(CSharpSyntaxNode syntaxNode)
        {
            var nodeKind = syntaxNode.Kind();

            switch (nodeKind)
            {
                case SyntaxKind.None:
                    throw new Exception("Syntax error");
                case SyntaxKind.List:
                    break;
                case SyntaxKind.TildeToken:
                case SyntaxKind.ExclamationToken:
                case SyntaxKind.DollarToken:
                case SyntaxKind.PercentToken:
                case SyntaxKind.CaretToken:
                case SyntaxKind.AmpersandToken:
                case SyntaxKind.AsteriskToken:
                case SyntaxKind.OpenParenToken:
                case SyntaxKind.CloseParenToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.PlusToken:
                case SyntaxKind.EqualsToken:
                case SyntaxKind.OpenBraceToken:
                case SyntaxKind.CloseBraceToken:
                case SyntaxKind.OpenBracketToken:
                case SyntaxKind.CloseBracketToken:
                case SyntaxKind.BarToken:
                case SyntaxKind.BackslashToken:
                case SyntaxKind.ColonToken:
                case SyntaxKind.SemicolonToken:
                case SyntaxKind.DoubleQuoteToken:
                case SyntaxKind.SingleQuoteToken:
                case SyntaxKind.LessThanToken:
                case SyntaxKind.CommaToken:
                case SyntaxKind.GreaterThanToken:
                case SyntaxKind.DotToken:
                case SyntaxKind.QuestionToken:
                case SyntaxKind.HashToken:
                case SyntaxKind.SlashToken:
                case SyntaxKind.DotDotToken:
                case SyntaxKind.SlashGreaterThanToken:
                case SyntaxKind.LessThanSlashToken:
                case SyntaxKind.XmlCommentStartToken:
                case SyntaxKind.XmlCommentEndToken:
                case SyntaxKind.XmlCDataStartToken:
                case SyntaxKind.XmlCDataEndToken:
                case SyntaxKind.XmlProcessingInstructionStartToken:
                case SyntaxKind.XmlProcessingInstructionEndToken:
                case SyntaxKind.BarBarToken:
                case SyntaxKind.AmpersandAmpersandToken:
                case SyntaxKind.MinusMinusToken:
                case SyntaxKind.PlusPlusToken:
                case SyntaxKind.ColonColonToken:
                case SyntaxKind.QuestionQuestionToken:
                case SyntaxKind.MinusGreaterThanToken:
                case SyntaxKind.ExclamationEqualsToken:
                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.EqualsGreaterThanToken:
                case SyntaxKind.LessThanEqualsToken:
                case SyntaxKind.LessThanLessThanToken:
                case SyntaxKind.LessThanLessThanEqualsToken:
                case SyntaxKind.GreaterThanEqualsToken:
                case SyntaxKind.GreaterThanGreaterThanToken:
                case SyntaxKind.GreaterThanGreaterThanEqualsToken:
                case SyntaxKind.SlashEqualsToken:
                case SyntaxKind.AsteriskEqualsToken:
                case SyntaxKind.BarEqualsToken:
                case SyntaxKind.AmpersandEqualsToken:
                case SyntaxKind.PlusEqualsToken:
                case SyntaxKind.MinusEqualsToken:
                case SyntaxKind.CaretEqualsToken:
                case SyntaxKind.PercentEqualsToken:
                case SyntaxKind.QuestionQuestionEqualsToken:
                case SyntaxKind.InterpolatedStringStartToken:
                case SyntaxKind.InterpolatedStringEndToken:
                case SyntaxKind.InterpolatedVerbatimStringStartToken:
                case SyntaxKind.UnderscoreToken:
                case SyntaxKind.OmittedTypeArgumentToken:
                case SyntaxKind.OmittedArraySizeExpressionToken:
                case SyntaxKind.EndOfDirectiveToken:
                case SyntaxKind.EndOfDocumentationCommentToken:
                case SyntaxKind.EndOfFileToken:
                case SyntaxKind.BadToken:
                case SyntaxKind.IdentifierToken:
                case SyntaxKind.NumericLiteralToken:
                case SyntaxKind.CharacterLiteralToken:
                case SyntaxKind.StringLiteralToken:
                case SyntaxKind.XmlEntityLiteralToken:
                case SyntaxKind.XmlTextLiteralToken:
                case SyntaxKind.XmlTextLiteralNewLineToken:
                case SyntaxKind.InterpolatedStringToken:
                case SyntaxKind.InterpolatedStringTextToken:
                    return Text(syntaxNode.GetText().ToString().Trim());
                case SyntaxKind.BoolKeyword:
                case SyntaxKind.ByteKeyword:
                case SyntaxKind.SByteKeyword:
                case SyntaxKind.ShortKeyword:
                case SyntaxKind.UShortKeyword:
                case SyntaxKind.IntKeyword:
                case SyntaxKind.UIntKeyword:
                case SyntaxKind.LongKeyword:
                case SyntaxKind.ULongKeyword:
                case SyntaxKind.DoubleKeyword:
                case SyntaxKind.FloatKeyword:
                case SyntaxKind.DecimalKeyword:
                case SyntaxKind.StringKeyword:
                case SyntaxKind.CharKeyword:
                case SyntaxKind.VoidKeyword:
                case SyntaxKind.ObjectKeyword:
                case SyntaxKind.TypeOfKeyword:
                case SyntaxKind.SizeOfKeyword:
                case SyntaxKind.NullKeyword:
                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                case SyntaxKind.IfKeyword:
                case SyntaxKind.ElseKeyword:
                case SyntaxKind.WhileKeyword:
                case SyntaxKind.ForKeyword:
                case SyntaxKind.ForEachKeyword:
                case SyntaxKind.DoKeyword:
                case SyntaxKind.SwitchKeyword:
                case SyntaxKind.CaseKeyword:
                case SyntaxKind.DefaultKeyword:
                case SyntaxKind.TryKeyword:
                case SyntaxKind.CatchKeyword:
                case SyntaxKind.FinallyKeyword:
                case SyntaxKind.LockKeyword:
                case SyntaxKind.GotoKeyword:
                case SyntaxKind.BreakKeyword:
                case SyntaxKind.ContinueKeyword:
                case SyntaxKind.ReturnKeyword:
                case SyntaxKind.ThrowKeyword:
                case SyntaxKind.PublicKeyword:
                case SyntaxKind.PrivateKeyword:
                case SyntaxKind.InternalKeyword:
                case SyntaxKind.ProtectedKeyword:
                case SyntaxKind.StaticKeyword:
                case SyntaxKind.ReadOnlyKeyword:
                case SyntaxKind.SealedKeyword:
                case SyntaxKind.ConstKeyword:
                case SyntaxKind.FixedKeyword:
                case SyntaxKind.StackAllocKeyword:
                case SyntaxKind.VolatileKeyword:
                case SyntaxKind.NewKeyword:
                case SyntaxKind.OverrideKeyword:
                case SyntaxKind.AbstractKeyword:
                case SyntaxKind.VirtualKeyword:
                case SyntaxKind.EventKeyword:
                case SyntaxKind.ExternKeyword:
                case SyntaxKind.RefKeyword:
                case SyntaxKind.OutKeyword:
                case SyntaxKind.InKeyword:
                case SyntaxKind.IsKeyword:
                case SyntaxKind.AsKeyword:
                case SyntaxKind.ParamsKeyword:
                case SyntaxKind.ArgListKeyword:
                case SyntaxKind.MakeRefKeyword:
                case SyntaxKind.RefTypeKeyword:
                case SyntaxKind.RefValueKeyword:
                case SyntaxKind.ThisKeyword:
                case SyntaxKind.BaseKeyword:
                case SyntaxKind.NamespaceKeyword:
                case SyntaxKind.UsingKeyword:
                case SyntaxKind.ClassKeyword:
                case SyntaxKind.StructKeyword:
                case SyntaxKind.InterfaceKeyword:
                case SyntaxKind.EnumKeyword:
                case SyntaxKind.DelegateKeyword:
                case SyntaxKind.CheckedKeyword:
                case SyntaxKind.UncheckedKeyword:
                case SyntaxKind.UnsafeKeyword:
                case SyntaxKind.OperatorKeyword:
                case SyntaxKind.ExplicitKeyword:
                case SyntaxKind.ImplicitKeyword:
                case SyntaxKind.YieldKeyword:
                case SyntaxKind.PartialKeyword:
                case SyntaxKind.AliasKeyword:
                case SyntaxKind.GlobalKeyword:
                case SyntaxKind.AssemblyKeyword:
                case SyntaxKind.ModuleKeyword:
                case SyntaxKind.TypeKeyword:
                case SyntaxKind.FieldKeyword:
                case SyntaxKind.MethodKeyword:
                case SyntaxKind.ParamKeyword:
                case SyntaxKind.PropertyKeyword:
                case SyntaxKind.TypeVarKeyword:
                case SyntaxKind.GetKeyword:
                case SyntaxKind.SetKeyword:
                case SyntaxKind.AddKeyword:
                case SyntaxKind.RemoveKeyword:
                case SyntaxKind.WhereKeyword:
                case SyntaxKind.FromKeyword:
                case SyntaxKind.GroupKeyword:
                case SyntaxKind.JoinKeyword:
                case SyntaxKind.IntoKeyword:
                case SyntaxKind.LetKeyword:
                case SyntaxKind.ByKeyword:
                case SyntaxKind.SelectKeyword:
                case SyntaxKind.OrderByKeyword:
                case SyntaxKind.OnKeyword:
                case SyntaxKind.EqualsKeyword:
                case SyntaxKind.AscendingKeyword:
                case SyntaxKind.DescendingKeyword:
                case SyntaxKind.NameOfKeyword:
                case SyntaxKind.AsyncKeyword:
                case SyntaxKind.AwaitKeyword:
                case SyntaxKind.WhenKeyword:
                case SyntaxKind.OrKeyword:
                case SyntaxKind.AndKeyword:
                case SyntaxKind.NotKeyword:
                case SyntaxKind.DataKeyword:
                case SyntaxKind.WithKeyword:
                case SyntaxKind.InitKeyword:
                case SyntaxKind.RecordKeyword:
                case SyntaxKind.ElifKeyword:
                case SyntaxKind.EndIfKeyword:
                case SyntaxKind.RegionKeyword:
                case SyntaxKind.EndRegionKeyword:
                case SyntaxKind.DefineKeyword:
                case SyntaxKind.UndefKeyword:
                case SyntaxKind.WarningKeyword:
                case SyntaxKind.ErrorKeyword:
                case SyntaxKind.LineKeyword:
                case SyntaxKind.PragmaKeyword:
                case SyntaxKind.HiddenKeyword:
                case SyntaxKind.ChecksumKeyword:
                case SyntaxKind.DisableKeyword:
                case SyntaxKind.RestoreKeyword:
                case SyntaxKind.ReferenceKeyword:
                case SyntaxKind.LoadKeyword:
                case SyntaxKind.NullableKeyword:
                case SyntaxKind.EnableKeyword:
                case SyntaxKind.WarningsKeyword:
                case SyntaxKind.AnnotationsKeyword:
                case SyntaxKind.VarKeyword:
                    return Text(syntaxNode.GetText().ToString().Trim());
                case SyntaxKind.EndOfLineTrivia:
                    break;
                case SyntaxKind.WhitespaceTrivia:
                    break;
                case SyntaxKind.SingleLineCommentTrivia:
                    break;
                case SyntaxKind.MultiLineCommentTrivia:
                    break;
                case SyntaxKind.DocumentationCommentExteriorTrivia:
                    break;
                case SyntaxKind.SingleLineDocumentationCommentTrivia:
                    break;
                case SyntaxKind.MultiLineDocumentationCommentTrivia:
                    break;
                case SyntaxKind.DisabledTextTrivia:
                    break;
                case SyntaxKind.PreprocessingMessageTrivia:
                    break;
                case SyntaxKind.IfDirectiveTrivia:
                    break;
                case SyntaxKind.ElifDirectiveTrivia:
                    break;
                case SyntaxKind.ElseDirectiveTrivia:
                    break;
                case SyntaxKind.EndIfDirectiveTrivia:
                    break;
                case SyntaxKind.RegionDirectiveTrivia:
                    break;
                case SyntaxKind.EndRegionDirectiveTrivia:
                    break;
                case SyntaxKind.DefineDirectiveTrivia:
                    break;
                case SyntaxKind.UndefDirectiveTrivia:
                    break;
                case SyntaxKind.ErrorDirectiveTrivia:
                    break;
                case SyntaxKind.WarningDirectiveTrivia:
                    break;
                case SyntaxKind.LineDirectiveTrivia:
                    break;
                case SyntaxKind.PragmaWarningDirectiveTrivia:
                    break;
                case SyntaxKind.PragmaChecksumDirectiveTrivia:
                    break;
                case SyntaxKind.ReferenceDirectiveTrivia:
                    break;
                case SyntaxKind.BadDirectiveTrivia:
                    break;
                case SyntaxKind.SkippedTokensTrivia:
                    break;
                case SyntaxKind.ConflictMarkerTrivia:
                    break;
                case SyntaxKind.XmlElement:
                    break;
                case SyntaxKind.XmlElementStartTag:
                    break;
                case SyntaxKind.XmlElementEndTag:
                    break;
                case SyntaxKind.XmlEmptyElement:
                    break;
                case SyntaxKind.XmlTextAttribute:
                    break;
                case SyntaxKind.XmlCrefAttribute:
                    break;
                case SyntaxKind.XmlNameAttribute:
                    break;
                case SyntaxKind.XmlName:
                    break;
                case SyntaxKind.XmlPrefix:
                    break;
                case SyntaxKind.XmlText:
                    break;
                case SyntaxKind.XmlCDataSection:
                    break;
                case SyntaxKind.XmlComment:
                    break;
                case SyntaxKind.XmlProcessingInstruction:
                    break;
                case SyntaxKind.TypeCref:
                    break;
                case SyntaxKind.QualifiedCref:
                    break;
                case SyntaxKind.NameMemberCref:
                    break;
                case SyntaxKind.IndexerMemberCref:
                    break;
                case SyntaxKind.OperatorMemberCref:
                    break;
                case SyntaxKind.ConversionOperatorMemberCref:
                    break;
                case SyntaxKind.CrefParameterList:
                    break;
                case SyntaxKind.CrefBracketedParameterList:
                    break;
                case SyntaxKind.CrefParameter:
                    break;
                case SyntaxKind.IdentifierName:
                    return IdentifierNameParser.Parse((IdentifierNameSyntax) syntaxNode);
                case SyntaxKind.QualifiedName:
                    return QualifiedNameParser.Parse((QualifiedNameSyntax) syntaxNode);
                case SyntaxKind.GenericName:
                    return GenericNameParser.Parse((GenericNameSyntax) syntaxNode);
                case SyntaxKind.TypeArgumentList:
                    return TypeArgumentListParser.Parse((TypeArgumentListSyntax) syntaxNode);
                case SyntaxKind.AliasQualifiedName:
                    break;
                case SyntaxKind.PredefinedType:
                    return PredefinedTypeParser.Parse((PredefinedTypeSyntax) syntaxNode);
                case SyntaxKind.ArrayType:
                    break;
                case SyntaxKind.ArrayRankSpecifier:
                    break;
                case SyntaxKind.PointerType:
                    break;
                case SyntaxKind.NullableType:
                    break;
                case SyntaxKind.OmittedTypeArgument:
                    break;
                case SyntaxKind.ParenthesizedExpression:
                    break;
                case SyntaxKind.ConditionalExpression:
                    break;
                case SyntaxKind.InvocationExpression:
                    break;
                case SyntaxKind.ElementAccessExpression:
                    break;
                case SyntaxKind.ArgumentList:
                    break;
                case SyntaxKind.BracketedArgumentList:
                    break;
                case SyntaxKind.Argument:
                    break;
                case SyntaxKind.NameColon:
                    break;
                case SyntaxKind.CastExpression:
                    break;
                case SyntaxKind.AnonymousMethodExpression:
                    break;
                case SyntaxKind.SimpleLambdaExpression:
                    break;
                case SyntaxKind.ParenthesizedLambdaExpression:
                    break;
                case SyntaxKind.ObjectInitializerExpression:
                    break;
                case SyntaxKind.CollectionInitializerExpression:
                    break;
                case SyntaxKind.ArrayInitializerExpression:
                    return ArrayInitializerExpressionParser.Parse((InitializerExpressionSyntax) syntaxNode);
                case SyntaxKind.AnonymousObjectMemberDeclarator:
                    break;
                case SyntaxKind.ComplexElementInitializerExpression:
                    break;
                case SyntaxKind.ObjectCreationExpression:
                    break;
                case SyntaxKind.AnonymousObjectCreationExpression:
                    break;
                case SyntaxKind.ArrayCreationExpression:
                    break;
                case SyntaxKind.ImplicitArrayCreationExpression:
                    return ImplicitArrayCreationExpressionParser.Parse((ImplicitArrayCreationExpressionSyntax)syntaxNode);
                case SyntaxKind.StackAllocArrayCreationExpression:
                    break;
                case SyntaxKind.OmittedArraySizeExpression:
                    break;
                case SyntaxKind.InterpolatedStringExpression:
                    break;
                case SyntaxKind.ImplicitElementAccess:
                    break;
                case SyntaxKind.IsPatternExpression:
                    break;
                case SyntaxKind.RangeExpression:
                    break;
                case SyntaxKind.ImplicitObjectCreationExpression:
                    break;
                case SyntaxKind.AddExpression:
                    break;
                case SyntaxKind.SubtractExpression:
                    break;
                case SyntaxKind.MultiplyExpression:
                    break;
                case SyntaxKind.DivideExpression:
                    break;
                case SyntaxKind.ModuloExpression:
                    break;
                case SyntaxKind.LeftShiftExpression:
                    break;
                case SyntaxKind.RightShiftExpression:
                    break;
                case SyntaxKind.LogicalOrExpression:
                    break;
                case SyntaxKind.LogicalAndExpression:
                    break;
                case SyntaxKind.BitwiseOrExpression:
                    break;
                case SyntaxKind.BitwiseAndExpression:
                    break;
                case SyntaxKind.ExclusiveOrExpression:
                    break;
                case SyntaxKind.EqualsExpression:
                    break;
                case SyntaxKind.NotEqualsExpression:
                    break;
                case SyntaxKind.LessThanExpression:
                    break;
                case SyntaxKind.LessThanOrEqualExpression:
                    break;
                case SyntaxKind.GreaterThanExpression:
                    break;
                case SyntaxKind.GreaterThanOrEqualExpression:
                    break;
                case SyntaxKind.IsExpression:
                    break;
                case SyntaxKind.AsExpression:
                    break;
                case SyntaxKind.CoalesceExpression:
                    break;
                case SyntaxKind.SimpleMemberAccessExpression:
                    break;
                case SyntaxKind.PointerMemberAccessExpression:
                    break;
                case SyntaxKind.ConditionalAccessExpression:
                    break;
                case SyntaxKind.MemberBindingExpression:
                    break;
                case SyntaxKind.ElementBindingExpression:
                    break;
                case SyntaxKind.SimpleAssignmentExpression:
                    break;
                case SyntaxKind.AddAssignmentExpression:
                    break;
                case SyntaxKind.SubtractAssignmentExpression:
                    break;
                case SyntaxKind.MultiplyAssignmentExpression:
                    break;
                case SyntaxKind.DivideAssignmentExpression:
                    break;
                case SyntaxKind.ModuloAssignmentExpression:
                    break;
                case SyntaxKind.AndAssignmentExpression:
                    break;
                case SyntaxKind.ExclusiveOrAssignmentExpression:
                    break;
                case SyntaxKind.OrAssignmentExpression:
                    break;
                case SyntaxKind.LeftShiftAssignmentExpression:
                    break;
                case SyntaxKind.RightShiftAssignmentExpression:
                    break;
                case SyntaxKind.CoalesceAssignmentExpression:
                    break;
                case SyntaxKind.UnaryPlusExpression:
                    break;
                case SyntaxKind.UnaryMinusExpression:
                    break;
                case SyntaxKind.BitwiseNotExpression:
                    break;
                case SyntaxKind.LogicalNotExpression:
                    break;
                case SyntaxKind.PreIncrementExpression:
                    break;
                case SyntaxKind.PreDecrementExpression:
                    break;
                case SyntaxKind.PointerIndirectionExpression:
                    break;
                case SyntaxKind.AddressOfExpression:
                    break;
                case SyntaxKind.PostIncrementExpression:
                    break;
                case SyntaxKind.PostDecrementExpression:
                    break;
                case SyntaxKind.AwaitExpression:
                    break;
                case SyntaxKind.IndexExpression:
                    break;
                case SyntaxKind.ThisExpression:
                    break;
                case SyntaxKind.BaseExpression:
                    break;
                case SyntaxKind.ArgListExpression:
                    break;
                case SyntaxKind.NumericLiteralExpression:
                    break;
                case SyntaxKind.StringLiteralExpression:
                    return StringLiteralExpressionParser.Parse((LiteralExpressionSyntax) syntaxNode);
                case SyntaxKind.CharacterLiteralExpression:
                    break;
                case SyntaxKind.TrueLiteralExpression:
                    break;
                case SyntaxKind.FalseLiteralExpression:
                    break;
                case SyntaxKind.NullLiteralExpression:
                    break;
                case SyntaxKind.DefaultLiteralExpression:
                    break;
                case SyntaxKind.TypeOfExpression:
                    break;
                case SyntaxKind.SizeOfExpression:
                    break;
                case SyntaxKind.CheckedExpression:
                    break;
                case SyntaxKind.UncheckedExpression:
                    break;
                case SyntaxKind.DefaultExpression:
                    break;
                case SyntaxKind.MakeRefExpression:
                    break;
                case SyntaxKind.RefValueExpression:
                    break;
                case SyntaxKind.RefTypeExpression:
                    break;
                case SyntaxKind.QueryExpression:
                    break;
                case SyntaxKind.QueryBody:
                    break;
                case SyntaxKind.FromClause:
                    break;
                case SyntaxKind.LetClause:
                    break;
                case SyntaxKind.JoinClause:
                    break;
                case SyntaxKind.JoinIntoClause:
                    break;
                case SyntaxKind.WhereClause:
                    break;
                case SyntaxKind.OrderByClause:
                    break;
                case SyntaxKind.AscendingOrdering:
                    break;
                case SyntaxKind.DescendingOrdering:
                    break;
                case SyntaxKind.SelectClause:
                    break;
                case SyntaxKind.GroupClause:
                    break;
                case SyntaxKind.QueryContinuation:
                    break;
                case SyntaxKind.Block:
                    return BlockParser.Parse((BlockSyntax) syntaxNode);
                case SyntaxKind.VariableDeclaration:
                    return VariableDeclarationParser.Parse((VariableDeclarationSyntax) syntaxNode);
                case SyntaxKind.VariableDeclarator:
                    return VariableDeclaratorParser.Parse((VariableDeclaratorSyntax) syntaxNode);
                case SyntaxKind.EqualsValueClause:
                    break;
                case SyntaxKind.ExpressionStatement:
                    break;
                case SyntaxKind.EmptyStatement:
                    break;
                case SyntaxKind.LabeledStatement:
                    break;
                case SyntaxKind.GotoStatement:
                    break;
                case SyntaxKind.GotoCaseStatement:
                    break;
                case SyntaxKind.GotoDefaultStatement:
                    break;
                case SyntaxKind.BreakStatement:
                    break;
                case SyntaxKind.ContinueStatement:
                    break;
                case SyntaxKind.ReturnStatement:
                    return ReturnStatementParser.Parse((ReturnStatementSyntax) syntaxNode);
                case SyntaxKind.YieldReturnStatement:
                    break;
                case SyntaxKind.YieldBreakStatement:
                    break;
                case SyntaxKind.ThrowStatement:
                    break;
                case SyntaxKind.WhileStatement:
                    break;
                case SyntaxKind.DoStatement:
                    break;
                case SyntaxKind.ForStatement:
                    break;
                case SyntaxKind.ForEachStatement:
                    break;
                case SyntaxKind.UsingStatement:
                    break;
                case SyntaxKind.FixedStatement:
                    break;
                case SyntaxKind.CheckedStatement:
                    break;
                case SyntaxKind.UncheckedStatement:
                    break;
                case SyntaxKind.UnsafeStatement:
                    break;
                case SyntaxKind.LockStatement:
                    break;
                case SyntaxKind.IfStatement:
                    break;
                case SyntaxKind.ElseClause:
                    break;
                case SyntaxKind.SwitchStatement:
                    break;
                case SyntaxKind.SwitchSection:
                    break;
                case SyntaxKind.CaseSwitchLabel:
                    break;
                case SyntaxKind.DefaultSwitchLabel:
                    break;
                case SyntaxKind.TryStatement:
                    break;
                case SyntaxKind.CatchClause:
                    break;
                case SyntaxKind.CatchDeclaration:
                    break;
                case SyntaxKind.CatchFilterClause:
                    break;
                case SyntaxKind.FinallyClause:
                    break;
                case SyntaxKind.LocalFunctionStatement:
                    break;
                case SyntaxKind.CompilationUnit:
                    return CompilationUnitParser.Parse((CompilationUnitSyntax) syntaxNode);
                case SyntaxKind.GlobalStatement:
                    return GlobalStatementParser.Parse((GlobalStatementSyntax) syntaxNode);
                case SyntaxKind.LocalDeclarationStatement:
                    return LocalDeclarationStatementParser.Parse((LocalDeclarationStatementSyntax) syntaxNode);
                case SyntaxKind.NamespaceDeclaration:
                    return NamespaceDeclarationParser.Parse((NamespaceDeclarationSyntax) syntaxNode);
                case SyntaxKind.UsingDirective:
                    return UsingDirectiveParser.Parse((UsingDirectiveSyntax) syntaxNode);
                case SyntaxKind.ExternAliasDirective:
                    break;
                case SyntaxKind.AttributeList:
                    break;
                case SyntaxKind.AttributeTargetSpecifier:
                    break;
                case SyntaxKind.Attribute:
                    break;
                case SyntaxKind.AttributeArgumentList:
                    break;
                case SyntaxKind.AttributeArgument:
                    break;
                case SyntaxKind.NameEquals:
                    return NameEqualsParser.Parse((NameEqualsSyntax) syntaxNode);
                case SyntaxKind.ClassDeclaration:
                    return ClassDeclarationParser.Parse((ClassDeclarationSyntax) syntaxNode);
                case SyntaxKind.StructDeclaration:
                    break;
                case SyntaxKind.InterfaceDeclaration:
                    break;
                case SyntaxKind.EnumDeclaration:
                    break;
                case SyntaxKind.DelegateDeclaration:
                    break;
                case SyntaxKind.BaseList:
                    break;
                case SyntaxKind.SimpleBaseType:
                    break;
                case SyntaxKind.TypeParameterConstraintClause:
                    break;
                case SyntaxKind.ConstructorConstraint:
                    break;
                case SyntaxKind.ClassConstraint:
                    break;
                case SyntaxKind.StructConstraint:
                    break;
                case SyntaxKind.TypeConstraint:
                    break;
                case SyntaxKind.ExplicitInterfaceSpecifier:
                    break;
                case SyntaxKind.EnumMemberDeclaration:
                    break;
                case SyntaxKind.FieldDeclaration:
                    break;
                case SyntaxKind.EventFieldDeclaration:
                    break;
                case SyntaxKind.MethodDeclaration:
                    return MethodDeclarationParser.Parse((MethodDeclarationSyntax) syntaxNode);
                case SyntaxKind.OperatorDeclaration:
                    break;
                case SyntaxKind.ConversionOperatorDeclaration:
                    break;
                case SyntaxKind.ConstructorDeclaration:
                    break;
                case SyntaxKind.BaseConstructorInitializer:
                    break;
                case SyntaxKind.ThisConstructorInitializer:
                    break;
                case SyntaxKind.DestructorDeclaration:
                    break;
                case SyntaxKind.PropertyDeclaration:
                    break;
                case SyntaxKind.EventDeclaration:
                    break;
                case SyntaxKind.IndexerDeclaration:
                    break;
                case SyntaxKind.AccessorList:
                    break;
                case SyntaxKind.GetAccessorDeclaration:
                    break;
                case SyntaxKind.SetAccessorDeclaration:
                    break;
                case SyntaxKind.AddAccessorDeclaration:
                    break;
                case SyntaxKind.RemoveAccessorDeclaration:
                    break;
                case SyntaxKind.UnknownAccessorDeclaration:
                    break;
                case SyntaxKind.ParameterList:
                    break;
                case SyntaxKind.BracketedParameterList:
                    break;
                case SyntaxKind.Parameter:
                    break;
                case SyntaxKind.TypeParameterList:
                    break;
                case SyntaxKind.TypeParameter:
                    break;
                case SyntaxKind.IncompleteMember:
                    break;
                case SyntaxKind.ArrowExpressionClause:
                    break;
                case SyntaxKind.Interpolation:
                    break;
                case SyntaxKind.InterpolatedStringText:
                    break;
                case SyntaxKind.InterpolationAlignmentClause:
                    break;
                case SyntaxKind.InterpolationFormatClause:
                    break;
                case SyntaxKind.ShebangDirectiveTrivia:
                    break;
                case SyntaxKind.LoadDirectiveTrivia:
                    break;
                case SyntaxKind.TupleType:
                    break;
                case SyntaxKind.TupleElement:
                    break;
                case SyntaxKind.TupleExpression:
                    break;
                case SyntaxKind.SingleVariableDesignation:
                    break;
                case SyntaxKind.ParenthesizedVariableDesignation:
                    break;
                case SyntaxKind.ForEachVariableStatement:
                    break;
                case SyntaxKind.DeclarationPattern:
                    break;
                case SyntaxKind.ConstantPattern:
                    break;
                case SyntaxKind.CasePatternSwitchLabel:
                    break;
                case SyntaxKind.WhenClause:
                    break;
                case SyntaxKind.DiscardDesignation:
                    break;
                case SyntaxKind.RecursivePattern:
                    break;
                case SyntaxKind.PropertyPatternClause:
                    break;
                case SyntaxKind.Subpattern:
                    break;
                case SyntaxKind.PositionalPatternClause:
                    break;
                case SyntaxKind.DiscardPattern:
                    break;
                case SyntaxKind.SwitchExpression:
                    break;
                case SyntaxKind.SwitchExpressionArm:
                    break;
                case SyntaxKind.VarPattern:
                    break;
                case SyntaxKind.ParenthesizedPattern:
                    break;
                case SyntaxKind.RelationalPattern:
                    break;
                case SyntaxKind.TypePattern:
                    break;
                case SyntaxKind.OrPattern:
                    break;
                case SyntaxKind.AndPattern:
                    break;
                case SyntaxKind.NotPattern:
                    break;
                case SyntaxKind.DeclarationExpression:
                    break;
                case SyntaxKind.RefExpression:
                    break;
                case SyntaxKind.RefType:
                    break;
                case SyntaxKind.ThrowExpression:
                    break;
                case SyntaxKind.ImplicitStackAllocArrayCreationExpression:
                    break;
                case SyntaxKind.SuppressNullableWarningExpression:
                    break;
                case SyntaxKind.NullableDirectiveTrivia:
                    break;
                case SyntaxKind.FunctionPointerType:
                    break;
                case SyntaxKind.InitAccessorDeclaration:
                    break;
                case SyntaxKind.WithExpression:
                    break;
                case SyntaxKind.WithInitializerExpression:
                    break;
                case SyntaxKind.RecordDeclaration:
                    break;
                case SyntaxKind.PrimaryConstructorBaseType:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return syntaxNode.ToFullString().Split("\n").Aggregate(Nil(), (current, next) => current + Text(next));
        }
    }
}
