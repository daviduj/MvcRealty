using Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace MvcRealty
{
    /// <summary>
    /// Configures the Cassette asset bundles for the web application.
    /// </summary>
    public class CassetteBundleConfiguration : IConfiguration<BundleCollection>
    {
        public void Configure(BundleCollection bundles)
        {
            // TODO: Configure your bundles here...
            // Please read http://getcassette.net/documentation/configuration

            // Make sure we don't pick up any SCSS files...
            var cssFilter = new FileSearch() { Pattern = "*.css" };

            // This default configuration treats each file as a separate 'bundle'.
            // In production the content will be minified, but the files are not combined.
            // So you probably want to tweak these defaults!
            // Stylesheets
            //bundles.AddPerIndividualFile<StylesheetBundle>("Content/styles/3rd", cssFilter, b => b.Media = "all");
            //bundles.AddPerIndividualFile<StylesheetBundle>("Content/styles/compiled", cssFilter, b => b.Media = "all");
            bundles.AddPerSubDirectory<StylesheetBundle>("Content/styles/3rd");
            bundles.AddPerSubDirectory<StylesheetBundle>("Content/styles/compiled");

            // Scripts
            bundles.AddPerSubDirectory<ScriptBundle>("~/Scripts/3rd");
            bundles.AddPerSubDirectory<ScriptBundle>("~/Scripts/compiled");

            // To combine files, try something like this instead:
            //   bundles.Add<StylesheetBundle>("Content");
            // In production mode, all of ~/Content will be combined into a single bundle.
            
            // If you want a bundle per folder, try this:
            //   bundles.AddPerSubDirectory<ScriptBundle>("Scripts");
            // Each immediate sub-directory of ~/Scripts will be combined into its own bundle.
            // This is useful when there are lots of scripts for different areas of the website.
        }
    }
}