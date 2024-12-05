import { Component, OnInit, ViewChild } from "@angular/core";
import { ICreateGroupRequest, IGroupDto } from "../contracts/interfaces";
import { MatSort } from "@angular/material/sort";
import { MatPaginator } from "@angular/material/paginator";
import { MatTableDataSource } from "@angular/material/table";
import { GroupsService } from "../groups.service";
import { Router } from "@angular/router";
import { MatDialog } from "@angular/material/dialog";
import { NotificationService } from "src/app/core/services/notification.service";
import { Title } from "@angular/platform-browser";
import { take } from "rxjs";
import { CdkDragDrop, moveItemInArray } from "@angular/cdk/drag-drop";
import { AddGroupComponent } from "../add-group/add-group.component";
import { ConfirmDialogComponent } from "src/app/shared/confirm-dialog/confirm-dialog.component";

@Component({
  selector: "app-groups-list",
  templateUrl: "./groups-list.component.html",
  styleUrls: ["./groups-list.component.css"],
})
export class GroupsListComponent implements OnInit {
  public customerUid: string = "316f1c85-8e01-4749-845e-768b22219244";

  columns: string[] = ["name", "createdOn", "description", "actions"];
  dataSource = new MatTableDataSource<IGroupDto>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(
    private notificationService: NotificationService,
    private titleService: Title,
    private groupsService: GroupsService,
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

  openAddGroupModal() {
    const dialogRef = this.dialog.open(AddGroupComponent, {
      width: "600px",
    });

    dialogRef.afterClosed().subscribe((result: ICreateGroupRequest) => {
      if (result) {
        this.groupsService
          .addGroup(this.customerUid, result)
          .pipe(take(1))
          .subscribe(
            () => {
              this.notificationService.openSnackBar(
                "Group added successfully."
              );
              this.getGroups();
            },
            () => {
              this.notificationService.openSnackBar("Adding group failed.");
            }
          );
      }

      this.getGroups();
    });
  }

  onRemoveClick(groupUid: string) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: "600px",
      data: {
        title: "Delete group?",
        message: "Are you sure you want to delete this group?",
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.groupsService
          .removeGroup(this.customerUid, groupUid)
          .pipe(take(1))
          .subscribe(
            () => {
              this.getGroups();
              this.notificationService.openSnackBar(
                "Group removed successfuly"
              );
            },
            () => {
              this.notificationService.openSnackBar("Group remove failed");
            }
          );
      }
    });
  }

  navigateToDetails(uid: string) {
    this.router.navigate(["groups", uid]);
  }

  getGroups() {
    this.groupsService
      .getGroups(this.customerUid)
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

  ngOnInit() {
    this.titleService.setTitle("Groups");
    this.getGroups();
  }
}
