namespace MovieAPI.Models
{
    /// <summary>
    /// Represents an actor entity.
    /// </summary>
    public class Actor
    {
        /// <summary>
        /// Gets or sets the unique identifier for the actor.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the actor.
        /// </summary>
        public string? Name { get; set; }
    }
}
