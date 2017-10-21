using System;
using System.Collections.Generic;

namespace Drake.Core
{
    public class Repository
    {
        public int RepositoryId { get; set; }
        public string Uri { get; set; }
        public DateTimeOffset LastUpdate { get; set; }

        public string Name
        {
            get
            {
                var name = new Uri(Uri).AbsolutePath;

                if (name.EndsWith(".git"))
                {
                    name = name.Substring(0, name.Length - ".git".Length);
                }

                return name.Trim('/');
            }
        }

        public List<File> Files { get; set; }
    }
}