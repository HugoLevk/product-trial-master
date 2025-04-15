import { Product } from "./product.model";

export interface CartItem {
  id: number;
  productId: number;
  cartId: number;
  quantity: number;
  product: Product;
  productPrice: number;
  createdAt: Date;
  updatedAt: Date;
}

export interface Cart {
  id: number;
  userId: string;
  items: CartItem[];
  createdAt: Date;
  updatedAt: Date;
}

export interface AddToCartDto {
  productId: number;
  quantity: number;
}

export interface UpdateCartItemDto {
  cartId: number;
  productId: number;
  quantity: number;
}
