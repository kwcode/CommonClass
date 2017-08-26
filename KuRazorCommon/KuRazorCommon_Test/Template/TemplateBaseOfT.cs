using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KuRazorCommon
{
    public class TemplateBase<T> : TemplateBase, ITemplate<T>
    {
        private object currentModel;
        public T Model
        {
            get { return (T)currentModel; }
            set { currentModel = value; }
        }

        public override void SetModel(object model)
        {
            Model = (T)model;
        }
    }
}