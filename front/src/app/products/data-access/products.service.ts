import { HttpClient } from "@angular/common/http";
import { inject, Injectable, signal } from "@angular/core";
import { catchError, Observable, of, tap } from "rxjs";
import { CreateProductDTO, Product, UpdateProductDTO } from "./product.model";

@Injectable({
  providedIn: "root",
})
export class ProductsService {
  private readonly http = inject(HttpClient);
  private readonly path = ":5001/api/Products";

  private readonly _products = signal<Product[]>([]);

  public readonly products = this._products.asReadonly();

  public get(): Observable<Product[]> {
    return this.http.get<Product[]>(this.path).pipe(
      catchError((error) => {
        console.error("Error fetching products from API:", error);
        return this.http.get<Product[]>("assets/products.json");
      }),
      tap((products) => {
        this._products.set(products);
        console.table(products);
      })
    );
  }

  public getById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.path}/${id}`);
  }

  public create(product: CreateProductDTO): Observable<Product> {
    return this.http.post<Product>(this.path, product).pipe(
      catchError((error) => {
        console.error("Error creating product:", error);
        return of({} as Product);
      }),
      tap((product) =>
        this._products.update((products) => [product, ...products])
      )
    );
  }

  public update(product: UpdateProductDTO): Observable<Product> {
    return this.http.patch<Product>(`${this.path}/${product.id}`, product).pipe(
      catchError((error) => {
        console.error("Error updating product:", error);
        return of({} as Product);
      }),
      tap((updatedProduct) =>
        this._products.update((products) => {
          return products.map((p) =>
            p.id === updatedProduct.id ? updatedProduct : p
          );
        })
      )
    );
  }

  public delete(productId: number): Observable<void> {
    return this.http.delete<void>(`${this.path}/${productId}`).pipe(
      catchError((error) => {
        console.error("Error deleting product:", error);
        return of(void 0);
      }),
      tap(() =>
        this._products.update((products) =>
          products.filter((product) => product.id !== productId)
        )
      )
    );
  }
}
