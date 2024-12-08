import { FormBuilder, FormGroup } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { NotificationService } from "./../../../core/services/notification.service";
import { PersonsService } from "./../persons.service";
import { Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { take } from "rxjs/internal/operators/take";
import { IPersonDto, IUpdatePersonRequest } from "../contracts/interfaces";
import { debounceTime } from "rxjs/internal/operators/debounceTime";
import { distinctUntilChanged } from "rxjs/internal/operators/distinctUntilChanged";
import { MatTableDataSource } from "@angular/material/table";
import { MatSort } from "@angular/material/sort";
import { MatPaginator } from "@angular/material/paginator";
import { CdkDragDrop, moveItemInArray } from "@angular/cdk/drag-drop";
import { IGroupDto } from "../../groups/contracts/interfaces";
import { AssignGroupComponent } from "../assign-group/assign-group.component";
import { MatDialog } from "@angular/material/dialog";
import { ConfirmDialogComponent } from "src/app/shared/confirm-dialog/confirm-dialog.component";

@Component({
  selector: "app-person-details",
  templateUrl: "./person-details.component.html",
  styleUrls: ["./person-details.component.css"],
})
export class PersonDetailsComponent implements OnInit {
  columns: string[] = ["name", "createdOn", "description", "actions"];
  dataSource = new MatTableDataSource<IGroupDto>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  @ViewChild("fileInput") fileInput!: ElementRef;

  public personUid: string;
  public customerUid: string = "316f1c85-8e01-4749-845e-768b22219244";

  public profileImageUrl: string = "assets/images/user.png";

  public personForm: FormGroup;

  constructor(
    private _fb: FormBuilder,
    private _activatedRoute: ActivatedRoute,
    private personsService: PersonsService,
    private router: Router,
    private notificationService: NotificationService,
    private dialog: MatDialog
  ) {
    this.personForm = this._fb.group({
      name: ["", []],
      email: ["", []],
      phone: ["", []],
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
        title: "Remove person from group?",
        message: "Are you sure you want to remove this person from the group?",
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.personsService
          .removePersonFromGroup(this.customerUid, groupUid, this.personUid)
          .pipe(take(1))
          .subscribe(
            () => {
              this.getPersonGroups();
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

  triggerFileUpload() {
    this.fileInput.nativeElement.click();
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.uploadImage(file);
    }
  }

  uploadImage(file: File) {
    if (this.personUid) {
      this.personsService
        .uploadPersonFace(this.customerUid, this.personUid, file)
        .pipe(take(1))
        .subscribe({
          next: (response) => {
            this.notificationService.openSnackBar("File uploaded successfully");

            this.personsService
              .getPersonFace(this.customerUid, this.personUid)
              .pipe(take(1))
              .subscribe((result: string) => {
                if (result && result !== "") {
                  this.profileImageUrl = result;
                }
              });
          },
          error: (error) => {
            this.notificationService.openSnackBar("Error uploading file");
          },
        });
    }
  }

  getPersonGroups() {
    this.personsService
      .getPersonGroups(this.customerUid, this.personUid)
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
      data: { personUid: this.personUid },
    });

    dialogRef.afterClosed().subscribe((groupUid: string) => {
      if (groupUid) {
        this.personsService
          .addPersonInGroup(this.customerUid, groupUid, this.personUid)
          .pipe(take(1))
          .subscribe(
            () => {
              this.notificationService.openSnackBar(
                "Person assigned to group successfully."
              );
              this.getPersonGroups();
            },
            () => {
              this.notificationService.openSnackBar("Adding person failed.");
            }
          );
      }

      this.getPersonGroups();
    });
  }

  ngOnInit(): void {
    this._activatedRoute.params.subscribe((params) => {
      this.personUid = params["personUid"];

      if (this.personUid) {
        this.personsService
          .getPerson(this.customerUid, this.personUid)
          .pipe(take(1))
          .subscribe((result: IPersonDto) => {
            if (result) {
              this.personForm.patchValue({
                name: result.name,
                email: result.email,
                phone: result.phone,
              });
            }
          });

        this.personsService
          .getPersonFace(this.customerUid, this.personUid)
          .pipe(take(1))
          .subscribe((result: string) => {
            if (result) {
              this.profileImageUrl = result;
            }
          });

        this.getPersonGroups();
      }

      this.personForm.valueChanges
        .pipe(
          debounceTime(1000),
          distinctUntilChanged()
        )
        .subscribe((value) => {
          let request: IUpdatePersonRequest = {
            name: this.personForm.get("name")?.value,
            email: this.personForm.get("email")?.value,
            phone: this.personForm.get("phone")?.value,
          };

          this.personsService
            .updatePerson(this.customerUid, this.personUid, request)
            .pipe(take(1))
            .subscribe(
              () => {
                this.notificationService.openSnackBar("Person update success");
              },
              () => {
                this.notificationService.openSnackBar("Person update failed");
              }
            );
        });
    });
  }
}
