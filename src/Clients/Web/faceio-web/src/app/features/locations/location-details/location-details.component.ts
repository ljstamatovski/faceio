import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { LocationsService } from './../locations.service';
import { NotificationService } from './../../../core/services/notification.service';
import { take } from 'rxjs/internal/operators/take';
import { ILocationDto, IUpdateLocationRequest } from '../contracts/interfaces';
import { debounceTime } from 'rxjs/internal/operators/debounceTime';
import { distinctUntilChanged } from 'rxjs/internal/operators/distinctUntilChanged';

@Component({
  selector: 'app-location-details',
  templateUrl: './location-details.component.html',
  styleUrls: ['./location-details.component.css']
})
export class LocationDetailsComponent implements OnInit {

  locationUid: string;

  public locationForm: FormGroup;

  constructor(private _fb: FormBuilder,
    private _activatedRoute: ActivatedRoute,
    private locationsService: LocationsService,
    private notificationService: NotificationService,
  ) {
    this.locationForm = this._fb.group({
      name: ['', []],
      description: ['', []]
    });
  }

  ngOnInit(): void {
    this._activatedRoute.params.subscribe(params => {
      this.locationUid = params['locationUid'];

      if (this.locationUid) {
        this.locationsService.getLocation('316f1c85-8e01-4749-845e-768b22219244', this.locationUid)
          .pipe(take(1))
          .subscribe((result: ILocationDto) => {
            if (result) {
              this.locationForm.patchValue({
                name: result.name,
                description: result.description
              });
            }
          });
      }

      this.locationForm.valueChanges.pipe(
        debounceTime(1000),
        distinctUntilChanged()
      )
        .subscribe(value => {
          let request: IUpdateLocationRequest = {
            name: this.locationForm.get('name')?.value,
            description: this.locationForm.get('description')?.value
          };

          this.locationsService.updateLocation('316f1c85-8e01-4749-845e-768b22219244', this.locationUid, request)
            .pipe(take(1))
            .subscribe(
              () => {
                this.notificationService.openSnackBar('Locations update success');
              },
              () => {
                this.notificationService.openSnackBar('Locations update failed');
              }
            );
        });

    });
  }

}
