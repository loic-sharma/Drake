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
        [HttpGet("{owner}/{repositoryName}/{*path}")]
        public async Task<object> Get(string owner, string repositoryName, string path)
        {
            return $"{owner}/{repositoryName}/{path}";
        }
    }
}