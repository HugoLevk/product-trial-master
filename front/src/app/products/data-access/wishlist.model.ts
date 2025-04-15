import { Product } from "./product.model";

export interface WishlistItem {
  id: number;
  productId: number;
  wishlistId: number;
  product: Product;
  createdAt: Date;
  updatedAt: Date;
}

export interface Wishlist {
  id: number;
  userId: string;
  items: WishlistItem[];
  createdAt: Date;
  updatedAt: Date;
}
