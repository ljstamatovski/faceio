import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LocationsRoutingModule } from './locations-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { LocationsListComponent } from './locations-list/locations-list.component';
import { LocationDetailsComponent } from './location-details/location-details.component';
import { AddLocationComponent } from './add-location/add-location.component';
import { AssignGroupComponent } from './assign-group/assign-group.component';

@NgModule({
    imports: [
        CommonModule,
        LocationsRoutingModule,
        SharedModule
    ],
    declarations: [
        LocationsListComponent,
        LocationDetailsComponent,
        AddLocationComponent,
        AssignGroupComponent
    ]
})
export class LocationsModule { }
