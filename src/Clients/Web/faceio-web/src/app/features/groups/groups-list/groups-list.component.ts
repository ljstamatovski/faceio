import { Component, OnInit, ViewChild } from '@angular/core';
import { ICreateGroupRequest, IGroupDto } from '../contracts/interfaces';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { GroupsService } from '../groups.service';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { NotificationService } from 'src/app/core/services/notification.service';
import { Title } from '@angular/platform-browser';
import { take } from 'rxjs';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { AddGroupComponent } from '../add-group/add-group.component';

@Component({
  selector: 'app-groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.css']
})
export class GroupsListComponent implements OnInit {

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
          .addGroup("316f1c85-8e01-4749-845e-768b22219244", result)
          .pipe(take(1))
          .subscribe(
            () => {
              this.notificationService.openSnackBar('Location added successfully.');
              this.getGroups();
            },
            () => {
              this.notificationService.openSnackBar('Adding location failed.');
            }
          );
      }

      this.getGroups();
    });
  }

  onRemoveClick(groupUid: string) {
    this.groupsService
      .removeGroup("316f1c85-8e01-4749-845e-768b22219244", groupUid)
      .pipe(take(1))
      .subscribe(
        () => {
          this.getGroups();
          this.notificationService.openSnackBar("Group removed successfuly");
        },
        () => {
          this.notificationService.openSnackBar("Group remove failed");
        }
      );
  }

  navigateToDetails(uid: string) {
    this.router.navigate(["groups", uid]);
  }

  getGroups() {
    this.groupsService
      .getGroups("316f1c85-8e01-4749-845e-768b22219244")
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
