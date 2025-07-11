using System;

namespace ClientApp.Models;

public class AcsTokenModel
{
    public string? AcsUserId { get; set; }
    public string? Token { get; set; }
    public System.DateTimeOffset ExpiresOn { get; set; }
    public Guid GroupId { get; set; }
}