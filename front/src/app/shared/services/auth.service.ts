import { HttpClient } from "@angular/common/http";
import { Injectable, inject, signal } from "@angular/core";
import { Observable, tap } from "rxjs";
import { AuthResponseDto, LoginDto, RegisterDto } from "../models/auth.model";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly _auth = signal<AuthResponseDto | null>(null);
  public readonly auth = this._auth.asReadonly();

  public register(registerDto: RegisterDto): Observable<AuthResponseDto> {
    return this.http
      .post<AuthResponseDto>("/account", registerDto)
      .pipe(tap((response) => this._auth.set(response)));
  }

  public login(loginDto: LoginDto): Observable<AuthResponseDto> {
    return this.http
      .post<AuthResponseDto>("/token", loginDto)
      .pipe(tap((response) => this._auth.set(response)));
  }

  public logout(): Observable<void> {
    return this.http
      .post<void>("/logout", {})
      .pipe(tap(() => this._auth.set(null)));
  }

  public isAuthenticated(): boolean {
    return this._auth() !== null;
  }

  public getToken(): string | null {
    return this._auth()?.token ?? null;
  }
}
