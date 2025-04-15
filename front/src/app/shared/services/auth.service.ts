import { HttpClient } from "@angular/common/http";
import { Injectable, signal } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment";
import { AuthResponse, LoginDto, RegisterDto } from "../models/auth.model";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/Auth`;

  auth = signal<AuthResponse | null>(null);

  constructor(private readonly http: HttpClient) {
    // Initialize auth signal from localStorage if exists
    const storedUser = localStorage.getItem("user");
    if (storedUser) {
      this.auth.set(JSON.parse(storedUser));
    }
  }

  public register(registerDto: RegisterDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/account`, registerDto, {
      withCredentials: true,
    });
  }

  public login(loginDto: LoginDto): Observable<AuthResponse> {
    let response = this.http.post<AuthResponse>(
      `${this.apiUrl}/token`,
      loginDto,
      {
        withCredentials: true,
      }
    );
    response.subscribe((response) => {
      this.auth.set(response);
    });
    return response;
  }

  public logout(): Observable<void> {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    this.auth.set(null);
    return this.http.post<void>(
      `${this.apiUrl}/logout`,
      {},
      { withCredentials: true }
    );
  }

  public isAuthenticated(): boolean {
    return this.auth() !== null;
  }

  public isAdmin(): boolean {
    return this.isAuthenticated() && this.auth()?.email == "admin@admin.com";
  }

  public getToken(): string | null {
    return this.auth()?.token ?? "";
  }

  public setAuthData(response: AuthResponse): void {
    localStorage.setItem("token", response.token);
    localStorage.setItem("username", JSON.stringify(response.username));
    localStorage.setItem("email", JSON.stringify(response.email));
    this.auth.set(response);
  }
}
