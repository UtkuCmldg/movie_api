using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using movie_api.Models;
using movie_api.ViewModels;
using MySql.Data.MySqlClient;

namespace movie_api.Controllers
{
    [ApiController]
    public class GetMoviesController : ControllerBase
    {
        private readonly IMapper _mapper;
        //getting automapper object from built in DI
        public GetMoviesController(IMapper mapper)
        {
            _mapper = mapper;
        }




        //sql connection string for mysql
        //this normally set in appsettings.json but this is not production code so it's fine to hold here
        private static string _sqlConnectionString = "server=localhost;database=films_api_project;uid=root;password=";


        [Route("/api/get/movie-details")]
        [HttpGet]
        public async Task<IActionResult> GetMovieDetails(int movieId)
        {
            //If parameter sent as empty or zero returning bad request and preventing further execution of this action method
            if (movieId == 0)
            {
                return StatusCode(400, "Bad Request. Parameter cannot be empty or zero");
            }

            using (var connection = new MySqlConnection(_sqlConnectionString))
            {
                //try to open connection with database
                try
                {
                    await connection.OpenAsync();
                }
                catch (Exception)
                {
                    await connection.CloseAsync();
                    return StatusCode(500, "Internal Server Error. Something Went Wrong !!");
                }

                //try to get movie from movies table
                try
                {
                    var movie = await connection.QuerySingleAsync<MovieModel>("SELECT movies.movie_id, movies.budget, movies.overview, movies.title, movie_genres.movie_genre, movie_original_languages.movie_original_language_name  FROM movies INNER JOIN movie_genres ON movies.movie_genre_id = movie_genres.movie_genre_id INNER JOIN movie_original_languages ON movie_original_languages.movie_original_language_id = movies.movie_original_language_id WHERE movies.movie_id = @MovieId; ", new { MovieId = movieId });

                    await connection.CloseAsync();


                    //mapping MovieModel obj to MovieViewModel
                    //In this project I coudn't see anywhere that automapper is necessary but I included here anyway because of the requirements of design document of this project
                    var movieViewModelObj = _mapper.Map<MovieViewModel>(movie);
                    return Ok(movieViewModelObj);
                }
                catch (Exception)
                {
                    await connection.CloseAsync();
                    return StatusCode(404, "Not Found. Coudn't find anything that matches given movie id, try something else.");
                }



                

            }

        }



        [Route("/api/get/movies")]
        [HttpGet]
        public async Task<IActionResult> GetMovies(string? searchQueryInMovieTitles)
        {
            //If parameter sent as empty returning bad request and preventing further execution of this action method
            if (String.IsNullOrEmpty(searchQueryInMovieTitles))
            {
                return StatusCode(400, "Bad Request. Parameter cannot be empty");
            }

            using (var connection = new MySqlConnection(_sqlConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                }
                catch (Exception)
                {
                    await connection.CloseAsync();
                    return StatusCode(500, "Internal Server Error. Something Went Wrong !!");
                }





                try
                {
                    //try to get movie rows from movies table
                    string query = "SELECT movies.movie_id, movies.budget, movies.overview, movies.title, movie_genres.movie_genre, movie_original_languages.movie_original_language_name FROM `movies` INNER JOIN movie_genres ON movies.movie_genre_id = movie_genres.movie_genre_id INNER JOIN movie_original_languages ON movie_original_languages.movie_original_language_id = movies.movie_original_language_id WHERE movies.title LIKE '%" + searchQueryInMovieTitles + "%'";
                    var result = await connection.QueryAsync<MovieModel>(query);
                    var movies = result.ToList();

                    //return 404 if search query did not match any row on database
                    if ((movies != null) && (!movies.Any()))
                    {
                        await connection.CloseAsync();
                        return StatusCode(404, "Not Found. Coudn't find anything that matches given search query, try something else.");
                    }

                    await connection.CloseAsync();

                    //mapping List of MovieModel objects to List of MovieViewModel objects
                    //In this project I coudn't see anywhere that automapper is necessary but I included here anyway because of the requirements of design document of this project
                    var movieViewModelObjList = _mapper.Map<List<MovieViewModel>>(movies);
                    return Ok(movieViewModelObjList);
                }
                catch (Exception)
                {

                    await connection.CloseAsync();
                    return StatusCode(500, "Internal Server Error. Something Went Wrong !!");
                }


                    

            }



        }
    }
}
