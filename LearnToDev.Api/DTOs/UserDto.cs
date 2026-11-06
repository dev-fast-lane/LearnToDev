namespace LearnToDev.Api.DTOs;

public class UserDto
{
   public string Id { get; set; } = string.Empty;
   public string UserName { get; set; } = string.Empty;
   public IList<string> Roles { get; set; } = new List<string>();
}