namespace ApplicationCore.Interfaces;

/// <summary>
/// Represents elements which have an ID and a name
/// </summary>
public interface INamed : IIdentified
{
    /// <summary>
    /// Name of the entity
    /// </summary>
    string Name { get; }
}