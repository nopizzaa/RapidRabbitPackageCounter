using System.ComponentModel.DataAnnotations.Schema;
using RapidRabbitPancakeCounter.Domain.Common;

namespace RapidRabbitPancakeCounter.Domain.Entities;

[Table("users")]
public class User : BaseEntity
{
    public required string Username { get; init; }

    /// <summary>
    /// In the comments username will have a random tag to distinct similar usernames
    /// Login will use the Email property
    /// </summary>
    public required string RandomTag { get; init; }

    public required string PasswordHash { get; init; }

    public required string Salt { get; init; }
    
    public required string Email { get; init; }

    public ushort HeightInCm { get; init; }
}
