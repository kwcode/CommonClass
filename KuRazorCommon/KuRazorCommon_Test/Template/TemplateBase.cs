using System;
using System.Collections.Generic;
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
            //var encodedString = value as IEncodedString;
            //if (encodedString != null)
            //{
            //    writer.Write(encodedString);
            //}
            //else
            //{ 
            //    writer.Write(encodedString);
            //}
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
    }
}
