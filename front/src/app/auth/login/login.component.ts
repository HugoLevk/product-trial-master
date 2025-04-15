import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { Router } from "@angular/router";
import { MessageService } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { InputTextModule } from "primeng/inputtext";
import { PasswordModule } from "primeng/password";
import { ToastModule } from "primeng/toast";
import { LoginDto } from "../../shared/models/auth.model";
import { AuthService } from "../../shared/services/auth.service";

@Component({
  selector: "app-login",
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    ButtonModule,
    CardModule,
    InputTextModule,
    PasswordModule,
    ToastModule,
  ],
  providers: [MessageService],
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"],
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly authService = inject(AuthService);
  private readonly messageService = inject(MessageService);

  protected loginForm: FormGroup = this.fb.group({
    email: ["", [Validators.required, Validators.email]],
    password: ["", [Validators.required, Validators.minLength(6)]],
  });

  protected onSubmit(): void {
    if (this.loginForm.invalid) return;

    const loginDto: LoginDto = this.loginForm.value;

    this.authService.login(loginDto).subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Connexion réussie",
          detail: "Vous êtes maintenant connecté",
        });
        this.router.navigate(["/products/list"]);
      },
      error: (error) => {
        this.messageService.add({
          severity: "error",
          summary: "Erreur de connexion",
          detail: error.error?.message ?? "Impossible de se connecter",
        });
      },
    });
  }

  protected navigateToRegister(): void {
    this.router.navigate(["/auth/register"]);
  }
}
