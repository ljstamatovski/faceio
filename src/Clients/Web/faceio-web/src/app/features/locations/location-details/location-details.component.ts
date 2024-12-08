import { Component, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { LocationsService } from "./../locations.service";
import { NotificationService } from "./../../../core/services/notification.service";
import { take } from "rxjs/internal/operators/take";
import { ILocationDto, IUpdateLocationRequest } from "../contracts/interfaces";
import { debounceTime } from "rxjs/internal/operators/debounceTime";
import { distinctUntilChanged } from "rxjs/internal/operators/distinctUntilChanged";
import { MatTableDataSource } from "@angular/material/table";
import { IGroupDto } from "../../groups/contracts/interfaces";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { CdkDragDrop, moveItemInArray } from "@angular/cdk/drag-drop";
import { AssignGroupComponent } from "../assign-group/assign-group.component";
import { MatDialog } from "@angular/material/dialog";
import { filter } from "rxjs";
import { ConfirmDialogComponent } from "src/app/shared/confirm-dialog/confirm-dialog.component";

@Component({
  selector: "app-location-details",
  templateUrl: "./location-details.component.html",
  styleUrls: ["./location-details.component.css"],
})
export class LocationDetailsComponent implements OnInit {
  columns: string[] = ["name", "description", "createdOn", "actions"];
  dataSource = new MatTableDataSource<IGroupDto>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  public locationUid: string;
  public customerUid: string = "316f1c85-8e01-4749-845e-768b22219244";
  public locationForm: FormGroup;

  constructor(
    private _fb: FormBuilder,
    private _activatedRoute: ActivatedRoute,
    private locationsService: LocationsService,
    private router: Router,
    private notificationService: NotificationService,
    private dialog: MatDialog
  ) {
    this.locationForm = this._fb.group({
      name: ["", []],
      description: ["", []],
    });
  }

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.columns, event.previousIndex, event.currentIndex);
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  navigateToDetails(uid: string) {
    this.router.navigate(["groups", uid]);
  }

  onRemoveClick(groupUid: string) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: "600px",
      data: {
        title: "Remove group from location?",
        message: "Are you sure you want to remove this group from the location?",
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.locationsService
          .removeGroupFromLocation(this.customerUid, this.locationUid, groupUid)
          .pipe(take(1))
          .subscribe(
            () => {
              this.getGroupsWithAccessToLocation();
              this.notificationService.openSnackBar(
                "Group removed from location successfuly."
              );
            },
            () => {
              this.notificationService.openSnackBar(
                "Group removing from location failed."
              );
            }
          );
      }
    });
  }

  getGroupsWithAccessToLocation() {
    this.locationsService
      .getGroupsWithAccessToLocation(this.customerUid, this.locationUid)
      .pipe(take(1))
      .subscribe((result: IGroupDto[]) => {
        if (result) {
          this.dataSource.data = result;
          this.dataSource.sort = this.sort;
          this.dataSource.paginator = this.paginator;
          this.notificationService.openSnackBar("Groups loaded");
        }
      });
  }

  openAssignGroupModal() {
    const dialogRef = this.dialog.open(AssignGroupComponent, {
      width: "600px",
      data: { locationUid: this.locationUid },
    });

    dialogRef.afterClosed().subscribe((groupUid: string) => {
      if (groupUid) {
        this.locationsService
          .addGroupToLocation(this.customerUid, this.locationUid, groupUid)
          .pipe(take(1))
          .subscribe(
            () => {
              this.notificationService.openSnackBar(
                "Group assigned to location successfully."
              );
              this.getGroupsWithAccessToLocation();
            },
            () => {
              this.notificationService.openSnackBar("Adding group failed.");
            }
          );
      }

      this.getGroupsWithAccessToLocation();
    });
  }

  ngOnInit(): void {
    this._activatedRoute.params.subscribe((params) => {
      this.locationUid = params["locationUid"];

      if (this.locationUid) {
        this.locationsService
          .getLocation(this.customerUid, this.locationUid)
          .pipe(take(1))
          .subscribe((result: ILocationDto) => {
            if (result) {
              this.locationForm.patchValue({
                name: result.name,
                description: result.description,
              });
            }
          });

        this.getGroupsWithAccessToLocation();
      }

      this.locationForm.valueChanges
        .pipe(
          debounceTime(1000),
          distinctUntilChanged())
        .subscribe((value) => {
          let request: IUpdateLocationRequest = {
            name: this.locationForm.get("name")?.value,
            description: this.locationForm.get("description")?.value,
          };

          this.locationsService
            .updateLocation(this.customerUid, this.locationUid, request)
            .pipe(take(1))
            .subscribe(
              () => {
                this.notificationService.openSnackBar(
                  "Locations update success"
                );
              },
              () => {
                this.notificationService.openSnackBar(
                  "Locations update failed"
                );
              }
            );
        });
    });
  }
}
