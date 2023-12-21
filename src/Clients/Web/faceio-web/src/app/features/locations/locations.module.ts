import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LocationsRoutingModule } from './locations-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { LocationsListComponent } from './locations-list/locations-list.component';
import { LocationDetailsComponent } from './location-details/location-details.component';

@NgModule({
    imports: [
        CommonModule,
        LocationsRoutingModule,
        SharedModule
    ],
    declarations: [
        LocationsListComponent,
        LocationDetailsComponent
    ]
})
export class LocationsModule { }
