import { AddLocationComponent } from "./../add-location/add-location.component";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { LocationsService } from "./../locations.service";
import { Component, OnInit, ViewChild } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { NotificationService } from "./../../../core/services/notification.service";
import { ICreateLocationRequest, ILocationDto } from "../contracts/interfaces";
import { take } from "rxjs/internal/operators/take";
import { MatPaginator } from "@angular/material/paginator";
import { MatTableDataSource } from "@angular/material/table";
import { MatSort } from "@angular/material/sort";
import { CdkDragDrop, moveItemInArray } from "@angular/cdk/drag-drop";
import { Router } from "@angular/router";

@Component({
  selector: "app-locations-list",
  templateUrl: "./locations-list.component.html",
  styleUrls: ["./locations-list.component.css"],
})
export class LocationsListComponent implements OnInit {
  columns: string[] = ["name", "createdOn", "description", "actions"];
  dataSource = new MatTableDataSource<ILocationDto>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private notificationService: NotificationService,
    private titleService: Title,
    private locationsService: LocationsService,
    private router: Router,
    private dialog: MatDialog
  ) {}

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

  openAddLocationModal() {
    const dialogRef = this.dialog.open(AddLocationComponent, {
      width: "600px",
    });

    dialogRef.afterClosed().subscribe((result: ICreateLocationRequest) => {
      if (result) {
        this.locationsService
          .addLocation("316f1c85-8e01-4749-845e-768b22219244", result)
          .pipe(take(1))
          .subscribe(
            () => {
              this.notificationService.openSnackBar('Location added successfully.');
              this.getLocations();
            },
            () => {
              this.notificationService.openSnackBar('Adding location failed.');
            }
          );
      }

      this.getLocations();
    });
  }

  onRemoveClick(locationUid: string) {
    this.locationsService
      .removeLocation("316f1c85-8e01-4749-845e-768b22219244", locationUid)
      .pipe(take(1))
      .subscribe(
        () => {
          this.getLocations();
          this.notificationService.openSnackBar("Location removed successfuly");
        },
        () => {
          this.notificationService.openSnackBar("Location remove failed");
        }
      );
  }

  navigateToDetails(uid: string) {
    this.router.navigate(["locations", uid]);
  }

  getLocations() {
    this.locationsService
      .getLocations("316f1c85-8e01-4749-845e-768b22219244")
      .pipe(take(1))
      .subscribe((result: ILocationDto[]) => {
        if (result) {
          this.dataSource.data = result;
          this.dataSource.sort = this.sort;
          this.dataSource.paginator = this.paginator;
          this.notificationService.openSnackBar("Locations loaded");
        }
      });
  }

  ngOnInit() {
    this.titleService.setTitle("Locations");
    this.getLocations();
  }
}
