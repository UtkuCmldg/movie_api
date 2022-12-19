using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace movie_api.Controllers
{
    [ApiController]
    public class GetMoviesController : ControllerBase
    {
        private static string _sqlConnectionString = "server=localhost;database=dapper_example;uid=root;password=";


        [Route("/api/get/movie-details")]
        [HttpGet]
        public string GetMovieDetails()
        {
            return "sadds";
        }

        [Route("/api/get/movies")]
        [HttpGet]
        public string GetMovies()
        {
            return "sadds";
        }
    }
}
