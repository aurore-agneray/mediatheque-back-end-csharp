namespace ApplicationCore.Interfaces.Entities
{
    /// <summary>
    /// Describes all entities with an ID property
    /// </summary>
    public interface IIdentified
    {
        /// <summary>
        /// ID of the entity
        /// </summary>
        int Id { get; set; }
    }
}