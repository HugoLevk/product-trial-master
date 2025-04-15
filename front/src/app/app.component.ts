import { Component, effect, inject, signal } from "@angular/core";
import { Router, RouterModule } from "@angular/router";
import { BadgeModule } from "primeng/badge";
import { ButtonModule } from "primeng/button";
import { SplitterModule } from "primeng/splitter";
import { ToolbarModule } from "primeng/toolbar";
import { CartService } from "./products/data-access/cart.service";
import { UserBadgeComponent } from "./shared/components/user-badge/user-badge.component";
import { AuthService } from "./shared/services/auth.service";
import { PanelMenuComponent } from "./shared/ui/panel-menu/panel-menu.component";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
  standalone: true,
  imports: [
    RouterModule,
    SplitterModule,
    ToolbarModule,
    PanelMenuComponent,
    UserBadgeComponent,
    ButtonModule,
    BadgeModule,
  ],
})
export class AppComponent {
  title = "ALTEN SHOP";
  private readonly router = inject(Router);
  readonly cartService = inject(CartService);
  readonly authService = inject(AuthService);

  public cartItemCount = signal(0);

  constructor() {
    effect(() => {
      if (this.authService.auth()) {
        this.cartService.getCart().subscribe((cart) => {
          this.cartItemCount.set(
            cart.items.reduce((total, item) => total + item.quantity, 0)
          );
        });
      }
    });
  }

  public onViewCart(): void {
    this.router.navigate(["/cart"]);
  }
}
