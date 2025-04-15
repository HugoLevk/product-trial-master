export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  username: string;
  firstname: string;
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  username: string;
  email: string;
}

