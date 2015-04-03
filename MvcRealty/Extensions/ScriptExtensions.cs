using MvcRealty.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcRealty.Extensions
{
    public static class ScriptExtensions
    {
        private static Dictionary<string, FileInclude> GetScriptDictionary(this HtmlHelper htmlHelper, string dictionaryKey)
        {
            var contextItems = htmlHelper.ViewContext.HttpContext.Items;
            if (!contextItems.Contains(dictionaryKey))
            {
                contextItems[dictionaryKey] = new Dictionary<string, FileInclude>();
            }
            return (Dictionary<String, FileInclude>)contextItems[dictionaryKey];
        }
        
        private static void AddScriptToDictionary(this Dictionary<string, FileInclude> dict, string key, string scriptPath, uint sortOrder)
        {
            if (!dict.ContainsKey(key) || String.IsNullOrWhiteSpace(dict[key].ResourceUrl))
            {
                dict[key] = new FileInclude() { ResourceUrl = scriptPath, SortOrder = sortOrder };
            }
            else if (!dict[key].ResourceUrl.Equals(scriptPath, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception(String.Format("Failed to register script include. Script Includes Dictionary Already Includes an Item with Key {0} and resolvedUrl {1} does not match existing file path {2}", key, scriptPath, dict[key]));
            }
        }
        
        #region Script Include
        public static IHtmlString RegisterScriptInclude(this HtmlHelper htmlHelper, string key, string filePath, uint sortOrder = uint.MaxValue)
        {
            var dict = GetScriptDictionary(htmlHelper, "scriptIncludes");
            string resolvedUrl = UrlHelper.GenerateContentUrl(filePath, htmlHelper.ViewContext.HttpContext);
            dict.AddScriptToDictionary(key, resolvedUrl, sortOrder);
            return new HtmlString(null);
        }
        
        public static IHtmlString GetScriptIncludes(this HtmlHelper htmlHelper)
        {
            return new HtmlString(String.Join("\r\n", GetScriptDictionary(htmlHelper, "scriptIncludes").OrderBy(z => z.Value.SortOrder).Select(x =>
                String.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", x.Value.ResourceUrl))));
        }
        #endregion
        
        #region Ready Scripts / Require
        public static IHtmlString RegisterReadyScript(this HtmlHelper htmlHelper, string key, string script, uint sortOrder = uint.MaxValue)
        {
            var dict = GetScriptDictionary(htmlHelper, "readyScripts");
            dict.AddScriptToDictionary(key, script, sortOrder);
            return new HtmlString(null);
        }
        
        /// <summary>
        /// Registers a module with requirejs.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="moduleName">The module name to be required.</param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static IHtmlString RegisterRequireModule(this HtmlHelper htmlHelper, string moduleName, uint sortOrder = uint.MaxValue)
        {
            return RegisterRequireModule(htmlHelper, moduleName, sortOrder, null);
        }
        
        public static IHtmlString RegisterRequireModule(this HtmlHelper htmlHelper, string moduleName, params string[] callbackArgs)
        {
            return RegisterRequireModule(htmlHelper, moduleName, uint.MaxValue, callbackArgs);
        }
        
        public static IHtmlString RegisterRequireModule(this HtmlHelper htmlHelper, string moduleName, uint sortOrder = uint.MaxValue, params string[] callbackArgs)
        {
            var dict = GetScriptDictionary(htmlHelper, "requireScripts");
            string script;
            var args = callbackArgs ?? Enumerable.Empty<string>();
            if (args.Any())
            {
                var scriptBase = @"require(['{0}'], function(func) {{
                    return func({1});
                }});";
                script = String.Format(scriptBase, moduleName, String.Join(", ", args));
            }
            else
            {
                script = String.Format("require(['{0}']);", moduleName);
            }
            dict.AddScriptToDictionary(moduleName, script, sortOrder);
            return new HtmlString(null);
        }
        
        public static IHtmlString GetRequireScripts(this HtmlHelper htmlHelper)
        {
            return GetReadyScripts(htmlHelper, "requireScripts");
        }
        
        public static IHtmlString GetReadyScripts(this HtmlHelper htmlHelper)
        {
            return GetReadyScripts(htmlHelper, "readyScripts");
        }
        
        private static IHtmlString GetReadyScripts(this HtmlHelper htmlHelper, string section)
        {
            var dict = GetScriptDictionary(htmlHelper, section);
            if (!dict.Any())
                return null;
            return new HtmlString(String.Format("<script type=\"text/javascript\">jQuery(document).ready(function(){{\r\n{0}\r\n}});\r\n</script>",
                String.Join("\r\n", dict.OrderBy(z => z.Value.SortOrder).Select(x =>
                x.Value.ResourceUrl))));
        }
        #endregion
        
        #region Script Block
        public static IHtmlString RegisterScriptBlock(this HtmlHelper htmlHelper, Func<dynamic, IHtmlString> template)
        {
            var scriptBlockBuilder = htmlHelper.ViewContext.HttpContext.Items["scriptBlocks"] as StringBuilder ?? new StringBuilder();
            scriptBlockBuilder.Append("\r\n").Append(template(null).ToHtmlString());
            htmlHelper.ViewContext.HttpContext.Items["scriptBlocks"] = scriptBlockBuilder;
            return new HtmlString(null);
        }
        
        public static IHtmlString RegisterScriptBlock(this WebViewPage webPage, Func<dynamic, IHtmlString> template)
        {
            if (!webPage.IsAjax)
            {
                var scriptBlockBuilder = webPage.ViewContext.HttpContext.Items["scriptBlocks"] as StringBuilder ?? new StringBuilder();
                scriptBlockBuilder.Append("\r\n").Append(template(null).ToHtmlString());
                webPage.ViewContext.HttpContext.Items["scriptBlocks"] = scriptBlockBuilder;
                return new HtmlString(null);
            }
            return new HtmlString(template(null).ToHtmlString());
        }
        
        public static IHtmlString GetScriptBlocks(this HtmlHelper htmlHelper)
        {
            var scriptBlockBuilder = htmlHelper.ViewContext.HttpContext.Items["scriptBlocks"] as StringBuilder ?? new StringBuilder();
            return new HtmlString(scriptBlockBuilder.ToString());
        }
        #endregion
    }
}