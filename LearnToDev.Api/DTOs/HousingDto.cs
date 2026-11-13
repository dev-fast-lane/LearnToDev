namespace LearnToDev.Api.DTOs;

//What the API sends about a housing. Players are not included
public record HousingDto(int Id, string Name, int Order, int HoursPerDay, int Rent);