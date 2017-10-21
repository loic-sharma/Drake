using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drake.Core;
using Drake.Indexing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Drake.Api.Controllers
{
    [Route("r")]
    public class RepositoryController : Controller
    {
        private DrakeContext _db;
        private Indexer _indexer;

        public RepositoryController(DrakeContext db, Indexer indexer)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _indexer = indexer ?? throw new ArgumentNullException(nameof(indexer));
        }

        [HttpGet]
        public async Task<IEnumerable<string>> List()
        {
            return await _db.Repositories
                            .Select(r => r.Name)
                            .ToListAsync();
        }

        [HttpGet("{owner}/{repositoryName}")]
        public async Task<object> Get(string owner, string repositoryName)
        {
            var uri = $"https://github.com/{owner}/{repositoryName}.git";

            var repository = await _db.Repositories
                                      .Where(r => r.Uri == uri)
                                      .Include(r => r.Files)
                                      .FirstOrDefaultAsync();

            if (repository == null)
            {
                repository = await _indexer.Index(uri);
            }

            return new
            {
                repository.RepositoryId,
                repository.Uri,
                repository.LastUpdate,
                Files = repository.Files.Select(f => new
                {
                    f.FileId,
                    f.Path,
                    f.Commits,
                }),
            };
        }
    }
}
