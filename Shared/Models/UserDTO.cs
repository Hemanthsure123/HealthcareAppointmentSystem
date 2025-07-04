using System;

namespace Shared.Models;
public class UserDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // Patient, Doctor, Admin
}