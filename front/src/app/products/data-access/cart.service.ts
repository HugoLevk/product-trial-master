import { HttpClient } from "@angular/common/http";
import { Injectable, inject, signal } from "@angular/core";
import { Observable, tap } from "rxjs";
import { AddToCartDto, Cart, UpdateCartItemDto } from "./cart.model";

@Injectable({
  providedIn: "root",
})
export class CartService {
  private readonly http = inject(HttpClient);
  private readonly path = "/api/cart";

  private readonly _cart = signal<Cart | null>(null);
  public readonly cart = this._cart.asReadonly();

  public getCart(): Observable<Cart> {
    return this.http
      .get<Cart>(this.path, { withCredentials: true })
      .pipe(tap((cart) => this._cart.set(cart)));
  }

  public addToCart(addToCartDto: AddToCartDto): Observable<Cart> {
    return this.http
      .post<Cart>(`${this.path}/items`, addToCartDto, { withCredentials: true })
      .pipe(tap((cart) => this._cart.set(cart)));
  }

  public updateCartItem(
    updateCartItemDto: UpdateCartItemDto
  ): Observable<Cart> {
    return this.http
      .put<Cart>(`${this.path}/items`, updateCartItemDto, { withCredentials: true })
      .pipe(tap((cart) => this._cart.set(cart)));
  }

  public removeFromCart(cartId: number, productId: number): Observable<void> {
    return this.http
      .delete<void>(`${this.path}/items/${cartId}/${productId}`, { withCredentials: true })
      .pipe(
        tap(() => {
          const currentCart = this._cart();
          if (currentCart) {
            this._cart.set({
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
      .pipe(tap(() => this._cart.set(null)));
  }
}
