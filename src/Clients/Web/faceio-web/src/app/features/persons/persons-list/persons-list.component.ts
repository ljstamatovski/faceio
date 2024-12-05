import { AddPersonComponent } from "./../add-person/add-person.component";
import { PersonsService } from "./../persons.service";
import { MatTableDataSource } from "@angular/material/table";
import { ICreatePersonRequest, IPersonDto } from "./../contracts/interfaces";
import { Component, OnInit, ViewChild } from "@angular/core";
import { Title } from "@angular/platform-browser";

import { take } from "rxjs/internal/operators/take";
import { NotificationService } from "./../../../core/services/notification.service";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { Router } from "@angular/router";
import { MatDialog } from "@angular/material/dialog";
import { CdkDragDrop, moveItemInArray } from "@angular/cdk/drag-drop";
import { ConfirmDialogComponent } from "src/app/shared/confirm-dialog/confirm-dialog.component";

@Component({
  selector: "app-persons-list",
  templateUrl: "./persons-list.component.html",
  styleUrls: ["./persons-list.component.css"],
})
export class PersonsListComponent implements OnInit {
  public customerUid: string = "316f1c85-8e01-4749-845e-768b22219244";

  columns: string[] = ["name", "createdOn", "actions"];
  dataSource = new MatTableDataSource<IPersonDto>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private notificationService: NotificationService,
    private titleService: Title,
    private personsService: PersonsService,
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

  openAddPersonModal() {
    const dialogRef = this.dialog.open(AddPersonComponent, {
      width: "600px",
    });

    dialogRef.afterClosed().subscribe((result: ICreatePersonRequest) => {
      if (result) {
        this.personsService
          .addPerson(this.customerUid, result)
          .pipe(take(1))
          .subscribe(
            () => {
              this.notificationService.openSnackBar(
                "Person added successfully."
              );
              this.getPersons();
            },
            () => {
              this.notificationService.openSnackBar("Adding person failed.");
            }
          );
      }

      this.getPersons();
    });
  }

  onRemoveClick(personUid: string) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: "600px",
      data: {
        title: "Delete person?",
        message: "Are you sure you want to delete this person?",
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.personsService
          .removePerson(this.customerUid, personUid)
          .pipe(take(1))
          .subscribe(
            () => {
              this.getPersons();
              this.notificationService.openSnackBar(
                "Person removed successfuly."
              );
            },
            () => {
              this.notificationService.openSnackBar("Person removing failed.");
            }
          );
      }
    });
  }

  navigateToDetails(uid: string) {
    this.router.navigate(["persons", uid]);
  }

  getPersons() {
    this.personsService
      .getPeople(this.customerUid)
      .pipe(take(1))
      .subscribe((result: IPersonDto[]) => {
        if (result) {
          this.dataSource.data = result;
          this.dataSource.sort = this.sort;
          this.dataSource.paginator = this.paginator;
          this.notificationService.openSnackBar("Persons loaded");
        }
      });
  }

  ngOnInit() {
    this.titleService.setTitle("People");
    this.getPersons();
  }
}
