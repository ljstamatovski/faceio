import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PersonsRoutingModule } from './persons-routing.module';
import { SharedModule } from './../../shared/shared.module';
import { PersonsListComponent } from './persons-list/persons-list.component';
import { AddPersonComponent } from './add-person/add-person.component';
import { PersonDetailsComponent } from './person-details/person-details.component';
import { AssignGroupComponent } from './assign-group/assign-group.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    PersonsRoutingModule
  ],
  declarations: [PersonsListComponent, AddPersonComponent, PersonDetailsComponent, AssignGroupComponent]
})
export class PersonsModule { }
