﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DevTools.Analyzers.Documentation {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DevTools.Analyzers.Documentation.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All potential exceptions should be documented..
        /// </summary>
        internal static string ExceptionNotDocumentedDiagnosticDescription {
            get {
                return ResourceManager.GetString("ExceptionNotDocumentedDiagnosticDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception &apos;{0}&apos; is not documented.
        /// </summary>
        internal static string ExceptionNotDocumentedDiagnosticMessageFormat {
            get {
                return ResourceManager.GetString("ExceptionNotDocumentedDiagnosticMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception is not documented.
        /// </summary>
        internal static string ExceptionNotDocumentedDiagnosticTitle {
            get {
                return ResourceManager.GetString("ExceptionNotDocumentedDiagnosticTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All documented exceptions should be thrown..
        /// </summary>
        internal static string ExceptionNotThrownDiagnosticDescription {
            get {
                return ResourceManager.GetString("ExceptionNotThrownDiagnosticDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception &apos;{0}&apos; is not thrown.
        /// </summary>
        internal static string ExceptionNotThrownDiagnosticMessageFormat {
            get {
                return ResourceManager.GetString("ExceptionNotThrownDiagnosticMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Documented exception is not thrown.
        /// </summary>
        internal static string ExceptionNotThrownDiagnosticTitle {
            get {
                return ResourceManager.GetString("ExceptionNotThrownDiagnosticTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exceptions should be documented properly..
        /// </summary>
        internal static string IncorrectExceptionDocumentationDiagnosticDescription {
            get {
                return ResourceManager.GetString("IncorrectExceptionDocumentationDiagnosticDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception is not documented property. {0}.
        /// </summary>
        internal static string IncorrectExceptionDocumentationDiagnosticMessageFormat {
            get {
                return ResourceManager.GetString("IncorrectExceptionDocumentationDiagnosticMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Incorrect exception documentation.
        /// </summary>
        internal static string IncorrectExceptionDocumentationDiagnosticTitle {
            get {
                return ResourceManager.GetString("IncorrectExceptionDocumentationDiagnosticTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All potential exceptions should be documented..
        /// </summary>
        internal static string InnerExceptionNotDocumentedDiagnosticDescription {
            get {
                return ResourceManager.GetString("InnerExceptionNotDocumentedDiagnosticDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception &apos;{0}&apos; is not documented.
        /// </summary>
        internal static string InnerExceptionNotDocumentedDiagnosticMessageFormat {
            get {
                return ResourceManager.GetString("InnerExceptionNotDocumentedDiagnosticMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inner exception is not documented.
        /// </summary>
        internal static string InnerExceptionNotDocumentedDiagnosticTitle {
            get {
                return ResourceManager.GetString("InnerExceptionNotDocumentedDiagnosticTitle", resourceCulture);
            }
        }
    }
}