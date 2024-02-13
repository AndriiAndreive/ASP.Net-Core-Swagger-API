using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

namespace MovieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly MovieDbContext _context;

        /// <summary>
        /// Initializes a new instance of the MoviesController class.
        /// </summary>
        /// <param name="_logger">The logger for logging information.</param>
        /// <param name="_context">The data context.</param>
        public MoviesController(ILogger<MoviesController> logger, MovieDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Retrieves all movies from the database.
        /// </summary>
        /// <returns>A list of all movies.</returns>
        // GET: api/movies
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Movie>), StatusCodes.Status200OK)]
        public IActionResult GetMovies()
        {
            var movies = _context.Movies
                .GroupJoin(_context.MovieRatings,
                           movie => movie.Id,
                           movierating => movierating.MovieId,
                           (movie, ratings) => new { Movie = movie, Ratings = ratings })
                .SelectMany(x => x.Ratings.DefaultIfEmpty(), (movie, rating) => new { movie.Movie, Rating = rating })
                .ToList();
            return Ok(movies);
        }

        /// <summary>
        /// Retrieves a movie by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the movie to retrieve.</param>
        /// <returns>The movie with the specified ID.</returns>
        // GET: api/movies/1
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovie(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }



        /// <summary>
        /// Adds a new movie to the database.
        /// </summary>
        /// <param name="movie">The movie to add.</param>
        /// <returns>The added movie.</returns>
        // POST: api/movies
        [HttpPost]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreateMovie([FromBody] Movie movie)
        {
            if (movie == null)
                return BadRequest();

            _context.Movies.Add(movie);

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
            }
            catch(DbUpdateConcurrencyException) {

                return NoContent();
            }
        }

        /// <summary>
        /// Updates an existing movie in the database.
        /// </summary>
        /// <param name="updatedMovie">The movie to update.</param>
        /// <returns>The updated movie.</returns>
        // PUT: api/movies/1
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] Movie updatedMovie)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null)
                return NotFound();

            movie.Title = updatedMovie.Title;
            movie.Actors = updatedMovie.Actors;
            // Update other properties as needed
            try
            {
                await _context.SaveChangesAsync();
                return Ok(movie);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Deletes a movie from the database.
        /// </summary>
        /// <param name="id">The ID of the movie to delete.</param>
        /// <returns>No content if successful.</returns>
        // DELETE: api/movies/1
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null)
                return NotFound();

            _context.Movies.Remove(movie);
            var movieRating = _context.MovieRatings.Where(x => x.MovieId == id).FirstOrDefault();
            if(movieRating != null)
            {
                _context.MovieRatings.Remove(movieRating);
            }
            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NoContent();
            }
        }
    }
}
