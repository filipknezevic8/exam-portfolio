using System.ComponentModel.DataAnnotations;

namespace Exam.App.Services.Dtos;

public class RegistrationDto
{
    [EmailAddress]
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
}