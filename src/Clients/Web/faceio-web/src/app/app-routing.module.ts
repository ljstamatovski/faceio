import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { AuthGuard } from "./core/guards/auth.guard";

const appRoutes: Routes = [
  {
    path: "auth",
    loadChildren: () =>
      import("./features/auth/auth.module").then((m) => m.AuthModule),
  },
  {
    path: "dashboard",
    loadChildren: () =>
      import("./features/dashboard/dashboard.module").then(
        (m) => m.DashboardModule
      ),
    canActivate: [AuthGuard],
  },
  {
    path: "locations",
    loadChildren: () =>
      import("./features/locations/locations.module").then(
        (m) => m.LocationsModule
      ),
    canActivate: [AuthGuard],
  },
  {
    path: "persons",
    loadChildren: () =>
      import("./features/persons/persons.module").then((m) => m.PersonsModule),
    canActivate: [AuthGuard],
  },
  {
    path: "groups",
    loadChildren: () =>
      import("./features/groups/groups.module").then((m) => m.GroupsModule),
    canActivate: [AuthGuard],
  },
  {
    path: "account",
    loadChildren: () =>
      import("./features/account/account.module").then((m) => m.AccountModule),
    canActivate: [AuthGuard],
  },
  {
    path: "about",
    loadChildren: () =>
      import("./features/about/about.module").then((m) => m.AboutModule),
    canActivate: [AuthGuard],
  },
  {
    path: "**",
    redirectTo: "dashboard",
    pathMatch: "full",
  },
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule],
  providers: [],
})
export class AppRoutingModule {}
