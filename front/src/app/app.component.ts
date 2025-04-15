import { Component, inject } from "@angular/core";
import { Router, RouterModule } from "@angular/router";
import { BadgeModule } from "primeng/badge";
import { ButtonModule } from "primeng/button";
import { SplitterModule } from "primeng/splitter";
import { ToolbarModule } from "primeng/toolbar";
import { CartService } from "./products/data-access/cart.service";
import { UserBadgeComponent } from "./shared/components/user-badge/user-badge.component";
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
  private readonly cartService = inject(CartService);

  public onViewCart(): void {
    this.router.navigate(["/cart"]);
  }

  public getCartItemCount(): number {
    return this.cartService.cart()?.items.length ?? 0;
  }
}
