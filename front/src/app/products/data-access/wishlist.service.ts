import { HttpClient } from "@angular/common/http";
import { Injectable, inject, signal } from "@angular/core";
import { Observable, tap } from "rxjs";
import { Wishlist } from "./wishlist.model";

@Injectable({
  providedIn: "root",
})
export class WishlistService {
  private readonly http = inject(HttpClient);
  private readonly path = "/api/wishlist";

  private readonly _wishlist = signal<Wishlist | null>(null);
  public readonly wishlist = this._wishlist.asReadonly();

  public getWishlist(): Observable<Wishlist> {
    return this.http
      .get<Wishlist>(this.path, { withCredentials: true })
      .pipe(tap((wishlist) => this._wishlist.set(wishlist)));
  }

  public addToWishlist(productId: number): Observable<Wishlist> {
    return this.http
      .post<Wishlist>(`${this.path}/items/${productId}`, {}, { withCredentials: true })
      .pipe(tap((wishlist) => this._wishlist.set(wishlist)));
  }

  public removeFromWishlist(
    wishlistId: number,
    productId: number
  ): Observable<void> {
    return this.http
      .delete<void>(`${this.path}/items/${wishlistId}/${productId}`, { withCredentials: true })
      .pipe(
        tap(() => {
          const currentWishlist = this._wishlist();
          if (currentWishlist) {
            this._wishlist.set({
              ...currentWishlist,
              items: currentWishlist.items.filter(
                (item) => item.productId !== productId
              ),
            });
          }
        })
      );
  }

  public clearWishlist(): Observable<void> {
    return this.http
      .delete<void>(this.path, { withCredentials: true })
      .pipe(tap(() => this._wishlist.set(null)));
  }
}
