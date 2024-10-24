using System.ComponentModel.DataAnnotations;

namespace StepBook.DAL.Entities;

/// <summary>
/// Represents a group.
/// </summary>
public class Group
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    [Key]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the connections.
    /// </summary>
    public ICollection<Connection> Connections { get; set; } = [];
}