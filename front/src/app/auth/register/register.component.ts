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
import { RegisterDto } from "../../shared/models/auth.model";
import { AuthService } from "../../shared/services/auth.service";

@Component({
  selector: "app-register",
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
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.scss"],
})
export class RegisterComponent {
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly authService = inject(AuthService);
  private readonly messageService = inject(MessageService);

  protected registerForm: FormGroup = this.fb.group({
    username: ["", [Validators.required]],
    firstname: ["", [Validators.required]],
    email: ["", [Validators.required, Validators.email]],
    password: ["", [Validators.required, Validators.minLength(6)]],
  });

  protected onSubmit(): void {
    if (this.registerForm.invalid) return;

    const registerDto: RegisterDto = this.registerForm.value;

    this.authService.register(registerDto).subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Inscription réussie",
          detail: "Votre compte a été créé avec succès",
        });
        this.router.navigate(["/auth/login"]);
      },
      error: (error) => {
        this.messageService.add({
          severity: "error",
          summary: "Erreur d'inscription",
          detail: error.error?.message ?? "Impossible de créer votre compte",
        });
      },
    });
  }

  protected navigateToLogin(): void {
    this.router.navigate(["/auth/login"]);
  }
}
