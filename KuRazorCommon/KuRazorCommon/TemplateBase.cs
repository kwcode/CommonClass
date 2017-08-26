using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace KuRazorCommon
{
    public abstract class TemplateBase : ITemplate
    {
        public virtual void SetModel(object model)
        {

        }
        internal TextWriter CurrentWriter { get; set; }
        //protected ExecuteContext _context;
        //public TextWriter CurrentWriter { get { return _context.CurrentWriter; } }
        public virtual void Execute() { }

        public virtual void Write(object value)
        {
            WriteTo(CurrentWriter, value);
        }

        public virtual void WriteLiteral(string literal)
        {
            WriteLiteralTo(CurrentWriter, literal);
        }
        public virtual void WriteLiteralTo(TextWriter writer, string literal)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            if (literal == null) return;
            writer.Write(literal);
        }
        public virtual void WriteTo(TextWriter writer, object value)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            if (value == null) return;
            writer.Write(value.ToString());
        }
        private static void StreamToTextWriter(MemoryStream memory, TextWriter writer)
        {
            memory.Position = 0;
            using (var r = new StreamReader(memory))
            {
                while (!r.EndOfStream)
                {
                    writer.Write(r.ReadToEnd());
                }
            }
        }
        void ITemplate.Run(TextWriter reader)
        {
            using (var memory = new MemoryStream())
            {
                using (var writer = new StreamWriter(memory))
                {
                    CurrentWriter = writer;
                    Execute();
                    writer.Flush();
                    StreamToTextWriter(memory, reader);
                }
            }
        }
        public virtual void WriteAttribute(string name, PositionTagged<string> prefix, PositionTagged<string> suffix, params AttributeValue[] values)
        {
            WriteAttributeTo(CurrentWriter, name, prefix, suffix, values);
        }
        public virtual void WriteAttributeTo(TextWriter writer, string name, PositionTagged<string> prefix, PositionTagged<string> suffix, params AttributeValue[] values)
        {
            bool first = true;
            bool wroteSomething = false;
            if (values.Length == 0)
            {
                // Explicitly empty attribute, so write the prefix and suffix
                WritePositionTaggedLiteral(writer, prefix);
                WritePositionTaggedLiteral(writer, suffix);
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    AttributeValue attrVal = values[i];
                    PositionTagged<object> val = attrVal.Value;

                    bool? boolVal = null;
                    if (val.Value is bool)
                    {
                        boolVal = (bool)val.Value;
                    }

                    if (val.Value != null && (boolVal == null || boolVal.Value))
                    {
                        string valStr = val.Value as string;
                        string valToString = valStr;
                        if (valStr == null)
                        {
                            valToString = val.Value.ToString();
                        }
                        if (boolVal != null)
                        {
                            Debug.Assert(boolVal.Value);
                            valToString = name;
                        }

                        if (first)
                        {
                            WritePositionTaggedLiteral(writer, prefix);
                            first = false;
                        }
                        else
                        {
                            WritePositionTaggedLiteral(writer, attrVal.Prefix);
                        }

                        if (attrVal.Literal)
                        {
                            WriteLiteralTo(writer, valToString);
                        }
                        else
                        {
                            if (val.Value is IEncodedString && boolVal == null)
                            {
                                WriteTo(writer, val.Value); // Write value
                            }
                            else
                            {
                                WriteTo(writer, valToString); // Write value
                            }
                        }
                        wroteSomething = true;
                    }
                }
                if (wroteSomething)
                {
                    WritePositionTaggedLiteral(writer, suffix);
                }
            }
        }
        /// <summary>
        /// Writes a <see cref="PositionTagged{}" /> literal to the result.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The literal to be written.</param>
        private void WritePositionTaggedLiteral(TextWriter writer, PositionTagged<string> value)
        {
            WriteLiteralTo(writer, value.Value);
        }
    }
}
