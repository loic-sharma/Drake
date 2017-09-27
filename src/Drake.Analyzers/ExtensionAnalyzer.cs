using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drake.Analyzers
{
    public class ExtensionAnalyzer : IAnalyzer
    {
        private IAnalyzer _previous;
        private HashSet<string> _extensionWhiteList;

        // TODO: Config files are nice. Maybe use a blocklist instead?
        private static readonly string[] DefaultExtensionWhiteList =
        {
            ".cmd",
            ".cs",
            ".js",
            ".md",
            ".php",
            ".sh",
        };

        public ExtensionAnalyzer(
            IAnalyzer previous,
            IEnumerable<string> extensionWhitelist = null)
        {
            _previous = previous ?? throw new ArgumentNullException(nameof(previous));
            _extensionWhiteList = new HashSet<string>(extensionWhitelist ?? DefaultExtensionWhiteList);
        }

        public async Task<IEnumerable<AnalysisResult>> AnalyzeAsync(string path)
        {
            // Reset the weight of all results whose file's extension are not whitelisted.
            return (await _previous.AnalyzeAsync(path)).Select(result =>
            {
                var extension = ParseExtensionFromPath(result.Path);

                if (!_extensionWhiteList.Contains(extension))
                {
                    result.Weight = 0;
                }

                return result;
            });
        }

        private string ParseExtensionFromPath(string path)
        {
            var extensionPosition = path.LastIndexOf('.');

            return (extensionPosition != -1)
                ? path.Substring(extensionPosition)
                : string.Empty;
        }
    }
}