using System;

namespace Drake.Core
{
    public static class RepositoryUriHelper
    {
        public const string GitExtension = ".git";

        public static string RepositoryName(this Uri repositoryUri)
        {
            return repositoryUri.TryParseRepositoryName(out var repositoryName)
                    ? repositoryName
                    : null;
        }

        public static bool TryParseRepositoryName(this Uri repositoryUri, out string repositoryName)
        {
            try
            {
                var repositorySegment = repositoryUri.AbsolutePath;

                if (repositorySegment.Length > GitExtension.Length)
                {
                    var extension = repositorySegment.Substring(repositorySegment.Length - GitExtension.Length);

                    if (extension == GitExtension)
                    {
                        repositoryName = repositorySegment.Substring(0, repositorySegment.Length - GitExtension.Length);

                        return true;
                    }
                }
            }
            catch
            {
                // TODO: Logging errors is nice.
            }

            repositoryName = null;

            return false;
        }
    }
}