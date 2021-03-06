﻿namespace KuRazorCommon.CSharp
{
#if RAZOR4
        using Microsoft.AspNet.Razor.Generator;
        using Microsoft.AspNet.Razor.Text;
        using Microsoft.AspNet.Razor.Parser;
        using RazorCSharpCodeParser = Microsoft.AspNet.Razor.Parser.CSharpCodeParser;
#else
    using System.Web.Razor.Generator;
    using System.Web.Razor.Text;
    using System.Web.Razor.Parser;
    using RazorCSharpCodeParser = System.Web.Razor.Parser.CSharpCodeParser;
#endif

    using System.Security;

    /// <summary>
    /// Defines a code parser that supports the C# syntax.
    /// </summary>
#if NET45 // Razor 2 has [assembly: SecurityTransparent]
    [SecurityCritical]
#endif
    public class CSharpCodeParser : RazorCSharpCodeParser
    {
        #region Fields
        private SourceLocation? _endInheritsLocation;
        private bool _modelStatementFound;
        #endregion

        #region Constructor
        /// <summary>
        /// Initialises a new instance of <see cref="CSharpCodeParser"/>.
        /// </summary>
        public CSharpCodeParser()
        {
            MapDirectives(ModelDirective, "model");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Parses the inherits statement.
        /// </summary>
#if NET45 // Razor 2 has [assembly: SecurityTransparent]
        [SecurityCritical]
#endif
        protected override void InheritsDirective()
        {
            // Verify we're on the right keyword and accept
            AssertDirective(SyntaxConstants.CSharp.InheritsKeyword);
            AcceptAndMoveNext();
            _endInheritsLocation = CurrentLocation;

            InheritsDirectiveCore();
            CheckForInheritsAndModelStatements();
        }

        private void CheckForInheritsAndModelStatements()
        {
            if (_modelStatementFound && _endInheritsLocation.HasValue)
            {
                Context.OnError(_endInheritsLocation.Value, "The 'inherits' keyword is not allowed when a 'model' keyword is used.");
            }
        }

        /// <summary>
        /// Parses the model statement.
        /// </summary>
#if NET45 // Razor 2 has [assembly: SecurityTransparent]
        [SecurityCritical]
#endif
        protected virtual void ModelDirective()
        {
            // Verify we're on the right keyword and accept
            AssertDirective("model");
            AcceptAndMoveNext();

            SourceLocation endModelLocation = CurrentLocation;

            //BaseTypeDirective("The 'model' keyword must be followed by a type name on the same line.", CreateModelCodeGenerator);

            if (_modelStatementFound)
            {
                Context.OnError(endModelLocation, "Only one 'model' statement is allowed in a file.");
            }

            _modelStatementFound = true;

            CheckForInheritsAndModelStatements();
        }

#if NET45 // Razor 2 has [assembly: SecurityTransparent]
        [SecurityCritical]
#endif
        //private SpanCodeGenerator CreateModelCodeGenerator(string model)
        //{
        //    return new SetModelTypeCodeGenerator(model, (templateType, modelTypeName) =>
        //    {
        //        return CompilerServicesUtility.CSharpCreateGenericType(templateType, modelTypeName, true);
        //    });
        //}
        #endregion
    }
}