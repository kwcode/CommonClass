using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KuRazorCommon
{
    public interface ITemplate
    {


        #region Methods
        /// <summary>
        /// Set the model of the template (if applicable).
        /// </summary>
        /// <param name="model"></param>
        void SetModel(object model);

        /// <summary>
        /// Executes the compiled template.
        /// </summary>
#if RAZOR4
        Task Execute();
#else
        void Execute();
#endif



        /// <summary>
        /// Writes the specified object to the result.
        /// </summary>
        /// <param name="value">The value to write.</param>
        void Write(object value);

        /// <summary>
        /// Writes the specified string to the result.
        /// </summary>
        /// <param name="literal">The literal to write.</param>
        void WriteLiteral(string literal);

        void Run(TextWriter writer);
        #endregion
    }
}