import { CommonModule, CurrencyPipe } from "@angular/common";
import { Component, OnInit, inject, signal } from "@angular/core";
import { Router } from "@angular/router";
import { CartService } from "app/products/data-access/cart.service";
import { Product } from "app/products/data-access/product.model";
import { ProductsService } from "app/products/data-access/products.service";
import { ProductFormComponent } from "app/products/ui/product-form/product-form.component";
import { AuthService } from "app/shared/services/auth.service";
import { MessageService } from "primeng/api";
import { BadgeModule } from "primeng/badge";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { DataViewModule } from "primeng/dataview";
import { DialogModule } from "primeng/dialog";
import { ToastModule } from "primeng/toast";

const emptyProduct: Product = {
  id: 0,
  code: "",
  name: "",
  description: "",
  image: "",
  category: "",
  price: 0,
  quantity: 0,
  internalReference: "",
  shellId: 0,
  inventoryStatus: 0,
  rating: 0,
  createdAt: new Date(),
  updatedAt: new Date(),
};

@Component({
  selector: "app-product-list",
  templateUrl: "./product-list.component.html",
  styleUrls: ["./product-list.component.scss"],
  standalone: true,
  imports: [
    DataViewModule,
    CardModule,
    ButtonModule,
    DialogModule,
    ProductFormComponent,
    CurrencyPipe,
    CommonModule,
    BadgeModule,
    ToastModule,
  ],
  providers: [MessageService],
})
export class ProductListComponent implements OnInit {
  private readonly productsService = inject(ProductsService);
  private readonly authService = inject(AuthService);
  private readonly cartService = inject(CartService);
  private readonly messageService = inject(MessageService);
  private readonly router = inject(Router);

  public readonly products = this.productsService.products;
  public readonly isAdmin = this.authService.isAdmin();
  public readonly cart = this.cartService.cart;

  public isDialogVisible = false;
  public isCreation = false;
  public readonly editedProduct = signal<Product>(emptyProduct);

  ngOnInit() {
    this.productsService.get().subscribe();
  }

  public onCreate() {
    this.isCreation = true;
    this.isDialogVisible = true;
    this.editedProduct.set(emptyProduct);
  }

  public onUpdate(product: Product) {
    this.isCreation = false;
    this.isDialogVisible = true;
    this.editedProduct.set(product);
  }

  public onDelete(product: Product) {
    this.productsService.delete(product.id).subscribe();
  }

  public onSave(product: Product) {
    if (this.isCreation) {
      this.productsService.create(product).subscribe();
    } else {
      this.productsService.update(product).subscribe();
    }
    this.closeDialog();
  }

  public onCancel() {
    this.closeDialog();
  }

  private closeDialog() {
    this.isDialogVisible = false;
  }

  public addToCart(product: Product) {
    this.cartService
      .addToCart({ productId: product.id, quantity: 1 })
      .subscribe({
        next: () => {
          this.messageService.add({
            severity: "success",
            summary: "Succès",
            detail: "Produit ajouté au panier",
          });
        },
        error: () => {
          this.messageService.add({
            severity: "error",
            summary: "Erreur",
            detail: "Impossible d'ajouter le produit au panier",
          });
        },
      });
  }

  public removeFromCart(product: Product) {
    const currentCart = this.cartService.cart();
    if (currentCart) {
      this.cartService.removeFromCart(currentCart.id, product.id).subscribe({
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

  public getCartItemCount(): number {
    return (
      this.cart()?.items.reduce((total, item) => total + item.quantity, 0) ?? 0
    );
  }

  public onViewCart() {
    this.router.navigate(["/cart"]);
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

  public getProductQuantityInCart(productId: number): number {
    const currentCart = this.cartService.cart();
    if (currentCart) {
      const cartItem = currentCart.items.find(
        (item) => item.productId === productId
      );
      return cartItem ? cartItem.quantity : 0;
    }
    return 0;
  }
}
