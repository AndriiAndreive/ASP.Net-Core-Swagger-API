using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

namespace MovieAPI.Controllers
{
    /// <summary>
    /// Interface for accessing actor data in the database.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ActorsController : Controller
    {
        private readonly ILogger<ActorsController> _logger;
        private readonly MovieDbContext _context;
        public ActorsController(ILogger<ActorsController> logger, MovieDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Retrieves all actors asynchronously.
        /// </summary>
        /// <returns>A list of all actors.</returns>
        // GET: api/actors
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Actor>), StatusCodes.Status200OK)]
        public IActionResult GetActors()
        {
            var actors = _context.Actors.ToList();
            return Ok(actors);
        }

        /// <summary>
        /// Retrieves a list of actors asynchronously based on the provided IDs.
        /// </summary>
        /// <param name="id">The ID of the actors to retrieve.</param>
        /// <returns>The actor with the specified ID.</returns>
        // GET: api/actors/1
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Actor), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetActor(int id)
        {
            var actor = _context.Actors.Find(id);
            if (actor == null)
            {
                return NotFound();
            }

            return Ok(actor);
        }

        /// <summary>
        /// Adds an actor asynchronously to the repository.
        /// </summary>
        /// <param name="actor">The actor to add.</param>
        /// <returns>The added actor.</returns>
        // POST: api/actors
        [HttpPost]
        [ProducesResponseType(typeof(Actor), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateActor([FromBody] Actor actor)
        {
            if (actor == null)
                return BadRequest();

            _context.Actors.Add(actor);

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, actor);
            }
            catch (DbUpdateConcurrencyException)
            {

                return NoContent();
            }
        }

        /// <summary>
        /// Updates an existing actor asynchronously.
        /// </summary>
        /// <param name="updatedActor">The actor to update.</param>
        /// <returns>The updated actor.</returns>
        // PUT: api/actors/1
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Actor), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateActor(int id, [FromBody] Actor updatedActor)
        {
            var actor = _context.Actors.Find(id);
            if (actor == null)
                return NotFound();

            actor.Name = updatedActor.Name;
            // Update other properties as needed
            try
            {
                await _context.SaveChangesAsync();
                return Ok(actor);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Deletes an actor asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the actor to delete.</param>
        /// <returns>No content if successful.</returns>
        // DELETE: api/actors/1
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteActor(int id)
        {
            var actor = _context.Actors.Find(id);
            if (actor == null)
                return NotFound();

            _context.Actors.Remove(actor);
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
