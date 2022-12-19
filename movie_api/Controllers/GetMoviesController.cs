using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using movie_api.Models;
using MySql.Data.MySqlClient;

namespace movie_api.Controllers
{
    [ApiController]
    public class GetMoviesController : ControllerBase
    {
        private static string _sqlConnectionString = "server=localhost;database=films_api_project;uid=root;password=";


        [Route("/api/get/movie-details")]
        [HttpGet]
        public IActionResult GetMovieDetails(int movieId)
        {
            
            using (var connection = new MySqlConnection(_sqlConnectionString))
            {
                //try to open connection with database
                try
                {
                    connection.Open();
                }
                catch (Exception)
                {

                    return StatusCode(500, "Internal Server Error. Something Went Wrong !!");
                }

                //try to get movie from movies table
                try
                {
                    var movie = connection.QuerySingle<MovieModel>("SELECT movies.movie_id, movies.budget, movies.overview, movies.title, movie_genres.movie_genre, movie_original_languages.movie_original_language_name  FROM movies INNER JOIN movie_genres ON movies.movie_genre_id = movie_genres.movie_genre_id INNER JOIN movie_original_languages ON movie_original_languages.movie_original_language_id = movies.movie_original_language_id WHERE movies.movie_id = @MovieId; ", new { MovieId = movieId });

                    return Ok(movie);
                }
                catch (Exception)
                {

                    return StatusCode(404, "Not Found. Coudn't find anything that matches given movie id, try something else.");
                }


                connection.Close();

                

            }

        }

        [Route("/api/get/movies")]
        [HttpGet]
        public string GetMovies()
        {
            return "sadds";
        }
    }
}
