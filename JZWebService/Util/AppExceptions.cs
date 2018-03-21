using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Web;

namespace JZWebService.Util
{
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
    internal class AppExceptions
    {
        private static CultureInfo resourceCulture;

        private static ResourceManager resourceMan;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return AppExceptions.resourceCulture;
            }
            set
            {
                AppExceptions.resourceCulture = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (AppExceptions.resourceMan == null)
                {
                    AppExceptions.resourceMan = new ResourceManager("Supply.Utils.Resources", typeof(AppExceptions).Assembly);
                }
                return AppExceptions.resourceMan;
            }
        }

        internal static string Terminator_ExceptionTemplate
        {
            get
            {
                return AppExceptions.ResourceManager.GetString("Terminator_ExceptionTemplate", AppExceptions.resourceCulture);
            }
        }

        internal AppExceptions()
        {
        }
    }
}