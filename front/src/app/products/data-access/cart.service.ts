import { HttpClient } from "@angular/common/http";
import { Injectable, OnInit, computed, inject, signal } from "@angular/core";
import { environment } from "environments/environment";
import { Observable, tap } from "rxjs";
import { AddToCartDto, Cart, UpdateCartItemDto } from "./cart.model";

@Injectable({
  providedIn: "root",
})
export class CartService implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly path = `${environment.apiUrl}/Cart`;

  public cart = signal<Cart | null>(null);

  public cartItemCount = computed(() => {
    const currentCart = this.cart();
    if (!currentCart) return 0;
    return currentCart.items.reduce((total, item) => total + item.quantity, 0);
  });

  ngOnInit(): void {
    this.getCart();
  }

  public getCart(): Observable<Cart> {
    return this.http
      .get<Cart>(this.path, { withCredentials: true })
      .pipe(tap((cart) => this.cart.set(cart)));
  }

  public addToCart(addToCartDto: AddToCartDto): Observable<Cart> {
    return this.http
      .post<Cart>(`${this.path}/items`, addToCartDto, { withCredentials: true })
      .pipe(tap((cart) => this.cart.set(cart)));
  }

  public updateCartItem(
    updateCartItemDto: UpdateCartItemDto
  ): Observable<Cart> {
    return this.http
      .put<Cart>(`${this.path}/items`, updateCartItemDto, {
        withCredentials: true,
      })
      .pipe(tap((cart) => this.cart.set(cart)));
  }

  public removeFromCart(cartId: number, productId: number): Observable<void> {
    return this.http
      .delete<void>(`${this.path}/items/${cartId}/${productId}`, {
        withCredentials: true,
      })
      .pipe(
        tap(() => {
          const currentCart = this.cart();
          if (currentCart) {
            this.cart.set({
              ...currentCart,
              items: currentCart.items.filter(
                (item) => item.productId !== productId
              ),
            });
          }
        })
      );
  }

  public clearCart(): Observable<void> {
    return this.http
      .delete<void>(this.path, { withCredentials: true })
      .pipe(tap(() => this.cart.set(null)));
  }
}
