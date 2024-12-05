import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupsListComponent } from './groups-list/groups-list.component';
import { GroupDetailsComponent } from './group-details/group-details.component';
import { AddGroupComponent } from './add-group/add-group.component';
import { GroupsRoutingModule } from './groups-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { AddPersonComponent } from './add-person/add-person.component';



@NgModule({
  declarations: [
    GroupsListComponent,
    GroupDetailsComponent,
    AddGroupComponent,
    AddPersonComponent
  ],
  imports: [
    CommonModule,
    GroupsRoutingModule,
    SharedModule
  ]
})
export class GroupsModule { }
