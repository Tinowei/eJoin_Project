using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.WebApiServices.SearchServices;

namespace Web.WebApi.Search
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly SearchApiService _searchApiService;

        public SearchController(SearchApiService searchApiService)
        {
            _searchApiService = searchApiService;
        }

        [HttpPost]
        public IActionResult Search(SearchDTO searchDTO)
        {
            var result = _searchApiService.GetSearchEvents(searchDTO);
            return Ok(result);
        }
    }
}
