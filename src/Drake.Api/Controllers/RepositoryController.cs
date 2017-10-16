using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drake.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Drake.Api.Controllers
{
    [Route("api/[controller]")]
    public class RepositoryController : Controller
    {
		private static readonly Uri GitHubUri = new Uri("https://github.com/");

        private DrakeContext _db;

        public RepositoryController(DrakeContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        // GET api/repository/{owner}/{repository}
        [HttpGet("{owner}/{repository}")]
        public async Task<object> Get(string owner, string repositoryName)
        {
            var uri = new Uri(GitHubUri, $"{owner}/{repositoryName}.git").ToString();
            var repository = await _db.Repositories
                                      .Where(r => r.RepositoryUri == uri)
                                      .Include(r => r.Files)
                                      .FirstAsync();

            return new
            {
				repository.RepositoryId,
                repository.RepositoryUri,
                repository.LastPullTime,
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
