namespace Backend.Data;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Email), IsUnique = true)]
public class Users
{
    /// <summary>
    /// Primary key - unique identifier for the user.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// University email address for login (unique).
    /// </summary>
    [Required]
    [MaxLength(255)]
    [Column(TypeName = "varchar(255)")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password.
    /// </summary>
    [Required]
    [Column(TypeName = "text")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// User's first name.
    /// </summary>
    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name.
    /// </summary>
    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// University foreign key (optional).
    /// </summary>
    [ForeignKey(nameof(University))]
    public Guid? UniversityId { get; set; }

    /// <summary>
    /// Navigation to the University entity.
    /// </summary>
    public University? University { get; set; }

    /// <summary>
    /// Timestamp of user creation in UTC.
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public List<Items>? Items { get; set; }
    public List<Booking>? Bookings { get; set; }
    public List<Comments>? Comments { get; set; }

    [InverseProperty(nameof(Review.Reviewer))]
    public List<Review>? ReviewsWritten { get; set; }

    [InverseProperty(nameof(Review.TargetUser))]
    public List<Review>? ReviewsReceived { get; set; }
}