using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace KuRazorCommon
{
    public class TemplateBase<T> : TemplateBase, ITemplate<T>
    {
        protected bool HasDynamicModel { get; private set; }
        protected TemplateBase()
        {
            HasDynamicModel = GetType().IsDefined(typeof(HasDynamicModelAttribute), true);
        }
        private object currentModel;
        public T Model
        {
            get { return (T)currentModel; }
            set
            {
                if (value is object || (value is DynamicObject) || (value is ExpandoObject))
                {
                    currentModel = new RazorDynamicObject { Model = value };
                }
                else
                {
                    currentModel = value;
                }
            }
        }

        public override void SetModel(object model)
        {
            Model = (T)model;
        }
    }
}