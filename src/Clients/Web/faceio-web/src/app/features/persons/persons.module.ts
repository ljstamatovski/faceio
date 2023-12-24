import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PersonsRoutingModule } from './persons-routing.module';
import { SharedModule } from './../../shared/shared.module';
import { PersonsListComponent } from './persons-list/persons-list.component';
import { AddPersonComponent } from './add-person/add-person.component';
import { PersonDetailsComponent } from './person-details/person-details.component';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    PersonsRoutingModule
  ],
  declarations: [PersonsListComponent, AddPersonComponent, PersonDetailsComponent]
})
export class PersonsModule { }
