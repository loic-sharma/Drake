using System;
using System.Collections.Generic;

namespace Drake.Core
{
    public class Repository
    {
        public int RepositoryId { get; set; }
        public string Uri { get; set; }
        public DateTimeOffset LastUpdate { get; set; }

        public string Name => new Uri(Uri).RepositoryName();

        public List<File> Files { get; set; }
    }
}