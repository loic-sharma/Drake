using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drake.Analyzers
{
    public class ExtensionAnalyzer
    {
        private HashSet<string> _extensionWhiteList;

        // TODO: Config files are nice
        // TODO: Maybe use a blacklist instead?
        private static readonly string[] DefaultExtensionWhiteList =
        {
            ".cmd",
            ".cs",
            ".js",
            ".md",
            ".php",
            ".sh",
        };

        public ExtensionAnalyzer(IEnumerable<string> extensionWhitelist = null)
        {
            _extensionWhiteList = new HashSet<string>(extensionWhitelist ?? DefaultExtensionWhiteList);
        }

        public int Analyze(string path)
        {
            var extension = ParseExtensionFromPath(path);

            return _extensionWhiteList.Contains(extension) ? 1 : 0;
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