using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Razor;

namespace KuRazorCommon
{
    /// <summary>
    /// 动态生成Razor 语法的dll和读取 Razor语法的dll文件转换成html代码
    /// </summary>
    public class KuRazor
    {
        /// <summary>
        /// 是否调试
        /// 调试模式 则会存在.pdb 文件和其他的cs 随机文件
        /// </summary>
        public static bool IsDebug = false;
        private readonly static IDictionary<string, ITemplate> templateCache = new ConcurrentDictionary<string, ITemplate>();
        #region 公共方法

        #region 创建DLL文件 重复方法 则覆盖
        /// <summary>
        /// 创建DLL文件 重复方法 则覆盖
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="razorTemplate">razor语法的Html模板</param>
        /// <param name="model">实体一般Model</param>
        /// <param name="keyName">dll名称不含后缀</param>
        /// <param name="dllDir">dll生成目录</param>
        public static void CreateDLL<T>(string razorTemplate, T model, string keyName, string dllDir = ".")
        {
            //1、创建一个C#的对象
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            var check = KuRazor.CheckModel(model);
            Type modelType = check.Item2;

            modelType = modelType ?? typeof(System.Dynamic.DynamicObject);
            Type templateType = typeof(TemplateBase<>);

            RazorCodeLanguage language = new CSharpRazorCodeLanguage();
            RazorEngineHost host = new RazorEngineHost(language);

            host.DefaultBaseClass = KuRazor.BuildTypeName(templateType, modelType);
            host.DefaultClassName = keyName;
            host.DefaultNamespace = "KuRazorCommon.Dynamic";
            List<string> namespaceImports = new List<string>();
            namespaceImports.Add("System");
            namespaceImports.Add("System.Collections.Generic");
            namespaceImports.Add("System.Linq");
            foreach (string ns in namespaceImports)
                host.NamespaceImports.Add(ns);
            RazorTemplateEngine engine = new RazorTemplateEngine(host);
            GeneratorResults razorResult;
            using (var reader = new StringReader(razorTemplate))
                razorResult = engine.GenerateCode(reader);
            CodeCompileUnit razorCode = razorResult.GeneratedCode;

            string generatedCode;
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder, CultureInfo.InvariantCulture))
            {
                provider.GenerateCodeFromCompileUnit(razorCode, writer, new CodeGeneratorOptions());
                generatedCode = builder.ToString();
            }

            string dllName = string.Format("{0}.dll", keyName);
            CompilerResults results = CompileCode(provider, generatedCode, dllName, dllDir);
        }

        #endregion

        #region 读取DLL模板文件的内容并编译生Html代码返回
        /// <summary>
        /// 读取DLL模板文件的内容并编译生Html代码返回Html代码
        /// </summary> 
        /// <param name="model">实体</param>
        /// <param name="dllPath">dll完整的路径</param>
        /// <param name="isCache">是否缓存 false 每次都重新加载dll里面的内容</param>
        /// <returns></returns>
        public static string GetRazorHtml<T>(T model, string dllPath, bool isCache = true)
        {
            string html = string.Empty;
            ITemplate instance;
            string keyName = System.IO.Path.GetFileNameWithoutExtension(dllPath);//如mk_C_333
            if (isCache)
            {
                if (templateCache.ContainsKey(keyName))
                {
                    templateCache.TryGetValue(keyName, out instance);
                }
                else
                {
                    instance = GetDllITemplate(keyName, dllPath);
                    if (instance != null)
                    {
                        templateCache.Add(keyName, instance);
                    }
                }
            }
            else
            {
                instance = GetDllITemplate(keyName, dllPath);

            }
            if (instance != null)
            {
                html = Run<T>(instance, model);
            }
            return html;
        }

        private static ITemplate GetDllITemplate(string keyName, string dllPath)
        {
            ITemplate instance = null;
            byte[] buff = System.IO.File.ReadAllBytes(dllPath);
            Assembly assem = Assembly.Load(buff);
            Type typeFromHandle = typeof(ITemplate);
            Type[] tys = assem.GetTypes();//只好得到所有的类型名，然后遍历，通过类型名字来区别了 
            foreach (Type ty in tys)//huoquleiming
            {
                if (ty.Name == keyName)
                {
                    instance = (ITemplate)Activator.CreateInstance(ty);
                    break;
                }
            }
            return instance;
        }
        private static string Run<T>(ITemplate instance, T model)
        {
            string html = string.Empty;
            if (model != null)
            {
                instance.SetModel(model);
            }
            using (var writer = new System.IO.StringWriter())
            {
                instance.Run(writer);
                html = writer.ToString();
            }
            return html;
        }
        #endregion

        #endregion

        #region 私有方法
        /// <summary>
        /// 把代码生成dll文件
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="sourceCode"></param>
        /// <param name="dllName"></param>
        /// <param name="dllDir"></param>
        /// <returns></returns>
        private static CompilerResults CompileCode(CodeDomProvider provider, string sourceCode, string dllName, string dllDir)
        {

            CompilerParameters cp = new CompilerParameters();
            cp.GenerateInMemory = false;
            cp.GenerateExecutable = false;
            cp.IncludeDebugInformation = IsDebug;
            cp.TreatWarningsAsErrors = false;
            cp.CompilerOptions = "/target:library /optimize /define:RAZORENGINE";
            var assemblyName = Path.Combine(dllDir, dllName);
            cp.OutputAssembly = assemblyName;//;//dll文件名 
            //string[] fileAssemblies = { "System", "System.Collections.Generic", "System.Linq" };
            var domain = AppDomain.CurrentDomain;
            Assembly[] arry = domain.GetAssemblies();
            foreach (Assembly item in arry)
            {
                try
                {
                    if (!cp.ReferencedAssemblies.Contains(item.Location.ToString()))
                    {
                        cp.ReferencedAssemblies.Add(item.Location.ToString());
                    }
                }
                catch { }
            }

            //设置一个临时文件集合。
            // TempFileCollection存储临时文件
            //在当前目录中生成的时候生成，
            //并且在编译后不删除它们。
            cp.TempFiles = new TempFileCollection(dllDir, IsDebug);
            //cp.TempFiles.AddFile(assemblyName, true);
            //cp.TempFiles.AddFile(new TempFileCollection(dllDir), true);
            //2、把源码编译成一个程序集
            CompilerResults cr = provider.CompileAssemblyFromSource(cp, sourceCode);
            System.Text.StringBuilder errorTxt = new StringBuilder();
            if (cr.Errors.Count > 0)
            {
                foreach (CompilerError ce in cr.Errors)
                {
                    //错误信息
                    string error = string.Format("{0}", ce.ToString());
                    errorTxt.Append(error + "\r\n");
                }
                throw new Exception(errorTxt.ToString());
            }
            else
            {

            }

            return cr;
        }
        private static Tuple<object, Type> CheckModel(object model)
        {
            if (model == null)
            {
                return Tuple.Create((object)null, (Type)null);
            }
            Type modelType = (model == null) ? typeof(object) : model.GetType();

            bool isAnon = CompilerServicesUtility.IsAnonymousTypeRecursive(modelType);
            if (isAnon ||
                CompilerServicesUtility.IsDynamicType(modelType))
            {
                modelType = null;
                if (isAnon)
                {
                    //model =  DynamicObject.Create(model, Configuration.AllowMissingPropertiesOnDynamic);
                }
            }
            return Tuple.Create(model, modelType);
        }
        private static string BuildTypeName(Type templateType, Type modelType)
        {
            if (templateType == null)
                throw new ArgumentNullException("templateType");

            var modelTypeName = CompilerServicesUtility.ResolveCSharpTypeName(modelType);
            return CompilerServicesUtility.CSharpCreateGenericType(templateType, modelTypeName, false);
        }


        #endregion
    }
}
