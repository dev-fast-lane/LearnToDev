// Typene speiler DTO-ene i LearnToDev.Api/DTOs. API-et sender camelCase,
// så UserName i C# blir userName her. Endres en DTO, må typen her endres også

// LoginDto.cs
export interface LoginDto {
  emailOrUserName: string;
  password: string;
  rememberMe: boolean;
}

// RegisterDto.cs
export interface RegisterDto {
  userName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

// UserDto.cs. Det API-et sender om en innlogget bruker. sender aldri passord-hashen.
export interface UserDto {
  id: string;
  userName: string;
  roles: string[];
}