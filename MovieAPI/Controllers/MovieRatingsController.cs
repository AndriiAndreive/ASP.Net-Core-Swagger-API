using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

namespace MovieAPI.Controllers
{
    /// <summary>
    /// Interface for accessing movie rating data in the database.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MovieRatingsController : Controller
    {
        private readonly ILogger<MovieRatingsController> _logger;
        private readonly MovieDbContext _context;
        public MovieRatingsController(ILogger<MovieRatingsController> logger, MovieDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Retrieves all movie ratings asynchronously.
        /// </summary>
        /// <returns>A list of all movie ratings.</returns>
        // GET: api/movieratings
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MovieRating>), StatusCodes.Status200OK)]
        public IActionResult GetMovieRatings()
        {
            var movieratings = _context.MovieRatings.ToList();
            return Ok(movieratings);
        }

        /// <summary>
        /// Retrieves a movie rating asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie rating to retrieve.</param>
        /// <returns>The movie rating with the specified ID.</returns>
        // GET: api/movieratings/1
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MovieRating), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovieRating(int id)
        {
            var movierating = _context.MovieRatings.Find(id);
            if (movierating == null)
            {
                return NotFound();
            }

            return Ok(movierating);
        }

        /// <summary>
        /// Adds a movie rating asynchronously to the repository.
        /// </summary>
        /// <param name="movierating">The movie rating to add.</param>
        /// <returns>The added movie rating.</returns>
        // POST: api/movieratings
        [HttpPost]
        [ProducesResponseType(typeof(MovieRating), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMovieRating([FromBody] MovieRating movierating)
        {
            if (movierating == null)
                return BadRequest();

            _context.MovieRatings.Add(movierating);

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetMovieRating), new { id = movierating.Id }, movierating);
            }
            catch (DbUpdateConcurrencyException)
            {

                return NoContent();
            }
        }

        /// <summary>
        /// Updates an existing movie rating asynchronously.
        /// </summary>
        /// <param name="id">The ID of the movie rating to update.</param>
        /// <param name="updatedMovieRating">The updated movie rating.</param>
        /// <returns>The updated movie rating.</returns>
        // PUT: api/movieratings/1
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MovieRating), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMovieRating(int id, [FromBody] MovieRating updatedMovieRating)
        {
            var movierating = _context.MovieRatings.Find(id);
            if (movierating == null)
                return NotFound();

            movierating.MovieId = updatedMovieRating.MovieId;
            movierating.Rating = updatedMovieRating.Rating;
            // Update other properties as needed
            try
            {
                await _context.SaveChangesAsync();
                return Ok(movierating);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Deletes a movie rating asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the movie rating to delete.</param>
        /// <returns>No content if successful.</returns>
        // DELETE: api/movieratings/1
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMovieRating(int id)
        {
            var movierating = _context.MovieRatings.Find(id);
            if (movierating == null)
                return NotFound();

            _context.MovieRatings.Remove(movierating);
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
