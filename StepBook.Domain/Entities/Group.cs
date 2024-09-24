using System.ComponentModel.DataAnnotations;

namespace StepBook.Domain.Entities;

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
    public List<Connection> Connections { get; set; } = [];
}