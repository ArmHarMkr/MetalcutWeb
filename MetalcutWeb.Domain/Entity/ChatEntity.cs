using System.ComponentModel.DataAnnotations;

namespace MetalcutWeb.Domain.Entity;

public class ChatEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public AppUser UserOne { get; set; }
    public AppUser UserTwo { get; set; }
    public string ChatName { get; set; }
}