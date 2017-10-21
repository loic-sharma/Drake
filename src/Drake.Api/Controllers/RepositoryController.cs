using System;
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
        private DrakeContext _db;

        public RepositoryController(DrakeContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

		// GET api/repository/{owner}/{repositoryName}
		[HttpGet("{owner}/{repositoryName}")]
        public async Task<object> Get(string owner, string repositoryName)
        {
            var uri = $"https://github.com/{owner}/{repositoryName}.git";
            var repository = await _db.Repositories
                                      .Where(r => r.Uri == uri)
                                      .Include(r => r.Files)
                                      .FirstAsync();

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
