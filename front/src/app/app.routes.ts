import { Routes } from "@angular/router";
import { LoginComponent } from "./auth/login/login.component";
import { RegisterComponent } from "./auth/register/register.component";
import { CartComponent } from "./products/features/cart/cart.component";
import { ProductListComponent } from "./products/features/product-list/product-list.component";
import { HomeComponent } from "./shared/features/home/home.component";

export const APP_ROUTES: Routes = [
  {
    path: "home",
    component: HomeComponent,
  },
  {
    path: "products",
    loadChildren: () =>
      import("./products/products.routes").then((m) => m.PRODUCTS_ROUTES),
  },
  {
    path: "auth/login",
    component: LoginComponent,
    pathMatch: "full",
  },
  {
    path: "auth/register",
    component: RegisterComponent,
    pathMatch: "full",
  },
  { path: "", redirectTo: "home", pathMatch: "full" },
  {
    path: "",
    component: ProductListComponent,
  },
  {
    path: "cart",
    component: CartComponent,
  },
];
