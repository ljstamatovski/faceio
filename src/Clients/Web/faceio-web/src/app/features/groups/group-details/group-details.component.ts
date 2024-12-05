import { NotificationService } from "src/app/core/services/notification.service";
import { Component, OnInit, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { IPersonDto } from "../../persons/contracts/interfaces";
import { CdkDragDrop, moveItemInArray } from "@angular/cdk/drag-drop";
import { FormBuilder, FormGroup } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { GroupsService } from "../groups.service";
import { debounceTime, distinctUntilChanged, filter, take } from "rxjs";
import { IGroupDto, IUpdateGroupRequest } from "../contracts/interfaces";
import { PersonsService } from "../../persons/persons.service";
import { ConfirmDialogComponent } from "src/app/shared/confirm-dialog/confirm-dialog.component";
import { MatDialog } from "@angular/material/dialog";

@Component({
  selector: "app-group-details",
  templateUrl: "./group-details.component.html",
  styleUrls: ["./group-details.component.css"],
})
export class GroupDetailsComponent implements OnInit {
  columns: string[] = ["name", "email", "phone", "createdOn", "actions"];
  dataSource = new MatTableDataSource<IPersonDto>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  public groupUid: string;
  public customerUid: string = "316f1c85-8e01-4749-845e-768b22219244";

  public groupForm: FormGroup;

  constructor(
    private _fb: FormBuilder,
    private _activatedRoute: ActivatedRoute,
    private groupsService: GroupsService,
    private personsService: PersonsService,
    private router: Router,
    private notificationService: NotificationService,
    private dialog: MatDialog
  ) {
    this.groupForm = this._fb.group({
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
    this.router.navigate(["persons", uid]);
  }

  onRemoveClick(personUid: string) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: "600px",
      data: {
        title: "Remove person from group?",
        message: "Are you sure you want to remove this person from the group?",
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.personsService
          .removePersonFromGroup(this.customerUid, this.groupUid, personUid)
          .pipe(take(1))
          .subscribe(
            () => {
              this.getPeopleInGroup();
              this.notificationService.openSnackBar(
                "Person removed from group successfuly."
              );
            },
            () => {
              this.notificationService.openSnackBar(
                "Person removing from group failed."
              );
            }
          );
      }
    });
  }

  getPeopleInGroup() {
    this.groupsService
      .getPeopleInGroup(this.customerUid, this.groupUid)
      .pipe(take(1))
      .subscribe((result: IPersonDto[]) => {
        if (result) {
          this.dataSource.data = result;
          this.dataSource.sort = this.sort;
          this.dataSource.paginator = this.paginator;
          this.notificationService.openSnackBar("People loaded");
        }
      });
  }

  ngOnInit(): void {
    this._activatedRoute.params.subscribe((params) => {
      this.groupUid = params["groupUid"];

      if (this.groupUid) {
        this.groupsService
          .getGroup(this.customerUid, this.groupUid)
          .pipe(take(1))
          .subscribe((result: IGroupDto) => {
            if (result) {
              this.groupForm.patchValue({
                name: result.name,
                description: result.description,
              });
            }
          });

        this.getPeopleInGroup();
      }

      this.groupForm.valueChanges
        .pipe(
          debounceTime(1000),
          distinctUntilChanged(),
          filter((value) => {
            const currentValues = this.groupForm.value;
            return (
              value.name !== currentValues.name ||
              value.description !== currentValues.description
            );
          })
        )
        .subscribe((value) => {
          let request: IUpdateGroupRequest = {
            name: this.groupForm.get("name")?.value,
            description: this.groupForm.get("description")?.value,
          };

          this.groupsService
            .updateGroup(this.customerUid, this.groupUid, request)
            .pipe(take(1))
            .subscribe(
              () => {
                this.notificationService.openSnackBar("Group update success");
              },
              () => {
                this.notificationService.openSnackBar("Group update failed");
              }
            );
        });
    });
  }
}
