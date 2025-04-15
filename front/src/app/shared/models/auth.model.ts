export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  email: string;
  password: string;
  firstName: string;
  userName: string;
}

export interface AuthResponseDto {
  token: string;
  userName: string;
  email: string;
}
