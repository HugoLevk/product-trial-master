import { Routes } from "@angular/router";
import { authGuard } from "app/shared/guards/auth.guard";
import { ProductListComponent } from "./features/product-list/product-list.component";

export const PRODUCTS_ROUTES: Routes = [
  {
    path: "list",
    component: ProductListComponent,
    canActivate: [authGuard],
  },
  { path: "**", redirectTo: "list" },
];
