using System;

namespace Drake.Core
{
    public static class RepositoryUriHelper
    {
        public const string GitExtension = ".git";

        public static string RepositoryName(this Uri repositoryUri)
        {
            var name = repositoryUri.AbsolutePath;

            if (name.EndsWith(GitExtension))
            {
                name = name.Substring(0, name.Length - GitExtension.Length);
            }

            return name.Trim('/');
        }
    }
}