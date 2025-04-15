import { CommonModule, CurrencyPipe } from "@angular/common";
import { Component, OnInit, inject } from "@angular/core";
import { CartItem } from "app/products/data-access/cart.model";
import { CartService } from "app/products/data-access/cart.service";
import { MessageService } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { DataViewModule } from "primeng/dataview";
import { ToastModule } from "primeng/toast";

@Component({
  selector: "app-cart",
  templateUrl: "./cart.component.html",
  styleUrls: ["./cart.component.scss"],
  standalone: true,
  imports: [
    CommonModule,
    DataViewModule,
    CardModule,
    ButtonModule,
    ToastModule,
    CurrencyPipe,
  ],
  providers: [MessageService],
})
export class CartComponent implements OnInit {
  readonly cartService = inject(CartService);
  private readonly messageService = inject(MessageService);

  ngOnInit() {
    this.cartService.getCart().subscribe();
  }

  public updateQuantity(productId: number, quantity: number) {
    const currentCart = this.cartService.cart();
    if (currentCart) {
      this.cartService
        .updateCartItem({ cartId: currentCart.id, productId, quantity })
        .subscribe({
          next: () => {
            this.messageService.add({
              severity: "success",
              summary: "Succès",
              detail: "Quantité mise à jour",
            });
          },
          error: () => {
            this.messageService.add({
              severity: "error",
              summary: "Erreur",
              detail: "Impossible de mettre à jour la quantité",
            });
          },
        });
    }
  }

  public removeFromCart(productId: number) {
    const currentCart = this.cartService.cart();
    if (currentCart) {
      this.cartService.removeFromCart(currentCart.id, productId).subscribe({
        next: () => {
          this.messageService.add({
            severity: "success",
            summary: "Succès",
            detail: "Produit retiré du panier",
          });
        },
        error: () => {
          this.messageService.add({
            severity: "error",
            summary: "Erreur",
            detail: "Impossible de retirer le produit du panier",
          });
        },
      });
    }
  }

  public clearCart() {
    this.cartService.clearCart().subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Succès",
          detail: "Panier vidé",
        });
      },
      error: () => {
        this.messageService.add({
          severity: "error",
          summary: "Erreur",
          detail: "Impossible de vider le panier",
        });
      },
    });
  }

  public getTotal(): number {
    return (
      this.cartService
        .cart()
        ?.items.reduce(
          (total: number, item: CartItem) =>
            total + item.productPrice * item.quantity,
          0
        ) ?? 0
    );
  }
}
