import { Component, inject } from "@angular/core";
import { Router } from "@angular/router";
import { AvatarModule } from "primeng/avatar";
import { ButtonModule } from "primeng/button";
import { AuthService } from "../../services/auth.service";

@Component({
  selector: "app-user-badge",
  standalone: true,
  imports: [AvatarModule, ButtonModule],
  templateUrl: "./user-badge.component.html",
  styleUrls: ["./user-badge.component.scss"],
})
export class UserBadgeComponent {
  protected readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  protected getUserInitials(): string {
    const auth = this.authService.auth();
    if (!auth) return "";

    if (auth?.username) {
      return auth?.username.substring(0, 2).toUpperCase() ?? "?";
    }

    if (auth?.email) {
      return auth?.email.substring(0, 2).toUpperCase() ?? "?";
    }

    return "?";
  }

  protected navigateToLogin(): void {
    this.router.navigate(["/auth/login"]);
  }

  protected onUserClick(): void {
    // Ici vous pouvez ajouter la logique pour afficher un menu ou naviguer vers le profil
    this.authService.logout();
    this.router.navigate(["/auth/login"]);
  }
}
