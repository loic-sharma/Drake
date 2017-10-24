using System;
using System.Linq;
using System.Threading.Tasks;
using Drake.Core;
using Drake.Indexing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Drake.Api.Controllers
{
    [Route("f")]
    public class FileController : Controller
    {
        private readonly RepositoryManager _repositoryManager;

        public FileController(RepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager ?? throw new ArgumentNullException(nameof(repositoryManager));
        }

        [HttpGet("{owner}/{repositoryName}/{*path}")]
        public string Get(string owner, string repositoryName, string path)
        {
            var filePath = $"{owner}/{repositoryName}/{path}";

            try
            {
                return _repositoryManager.ReadAllText(filePath);
            }
            catch (System.IO.FileNotFoundException)
            {
                // TODO: Log/404
                throw;
            }
        }
    }
}