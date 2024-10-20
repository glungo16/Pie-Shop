using BethanysPieShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.Controllers.Api
{
    [Route("api/[controller]")] // this is the same as: api/Search
    [ApiController]
    public class SearchController : ControllerBase // ControllerBase is the base class for Controller (which is used in the MVC)
    {
        private readonly IPieRepository _pieRepository;

        public SearchController(IPieRepository pieRepository)
        {
            _pieRepository = pieRepository;
        }

        // get all pies in json format
        [HttpGet]
        public IActionResult GetAll()
        {
            var allPies = _pieRepository.AllPies;

            // Ok = status 200 code
            return Ok(allPies); // data is automatically converted to json by aspnet core
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (!_pieRepository.AllPies.Any(p => p.PieId == id))
            {
                return NotFound(); // status 404
            }

            return Ok(_pieRepository.AllPies.Where(p => p.PieId == id));
        }

        // need to tell aspnet where to find the query -> FromBody
        [HttpPost]
        public IActionResult SearchPies([FromBody] string searchQuery) 
        {
            IEnumerable<Pie> pies = new List<Pie>();

            if(!string.IsNullOrEmpty(searchQuery))
            {
                pies = _pieRepository.SearchPies(searchQuery);
            }
            return new JsonResult(pies); // returns status 200 Ok
        }
    }
}
