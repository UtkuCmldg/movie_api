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
                    connection.Close();
                    return StatusCode(500, "Internal Server Error. Something Went Wrong !!");
                }

                //try to get movie from movies table
                try
                {
                    var movie = connection.QuerySingle<MovieModel>("SELECT movies.movie_id, movies.budget, movies.overview, movies.title, movie_genres.movie_genre, movie_original_languages.movie_original_language_name  FROM movies INNER JOIN movie_genres ON movies.movie_genre_id = movie_genres.movie_genre_id INNER JOIN movie_original_languages ON movie_original_languages.movie_original_language_id = movies.movie_original_language_id WHERE movies.movie_id = @MovieId; ", new { MovieId = movieId });

                    connection.Close();
                    return Ok(movie);
                }
                catch (Exception)
                {
                    connection.Close();
                    return StatusCode(404, "Not Found. Coudn't find anything that matches given movie id, try something else.");
                }



                

            }

        }

        [Route("/api/get/movies")]
        [HttpGet]
        public IActionResult GetMovies(string searchQueryInMovieTitles)
        {
            using (var connection = new MySqlConnection(_sqlConnectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception)
                {
                    connection.Close();
                    return StatusCode(500, "Internal Server Error. Something Went Wrong !!");
                }





                try
                {
                    string query = "SELECT movies.movie_id, movies.budget, movies.overview, movies.title, movie_genres.movie_genre, movie_original_languages.movie_original_language_name FROM `movies` INNER JOIN movie_genres ON movies.movie_genre_id = movie_genres.movie_genre_id INNER JOIN movie_original_languages ON movie_original_languages.movie_original_language_id = movies.movie_original_language_id WHERE movies.title LIKE '%" + searchQueryInMovieTitles + "%'";
                    var movies = connection.Query<MovieModel>(query).ToList();

                    //return 404 if search query did not match any row on database
                    if ((movies != null) && (!movies.Any()))
                    {
                        connection.Close();
                        return StatusCode(404, "Not Found. Coudn't find anything that matches given search query, try something else.");
                    }

                    connection.Close();
                    return Ok(movies);
                }
                catch (Exception)
                {

                    connection.Close();
                    return StatusCode(500, "Internal Server Error. Something Went Wrong !!");
                }


                    

            }



        }
    }
}
