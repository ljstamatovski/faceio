import { Component, Inject, OnInit } from '@angular/core';
import { IGroupDto } from '../../groups/contracts/interfaces';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { LocationsService } from '../locations.service';
import { GroupsService } from '../../groups/groups.service';
import { NotificationService } from 'src/app/core/services/notification.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-assign-group',
  templateUrl: './assign-group.component.html',
  styleUrls: ['./assign-group.component.css']
})
export class AssignGroupComponent implements OnInit {
  public customerUid: string = "316f1c85-8e01-4749-845e-768b22219244";

  public locationGroups: IGroupDto[];
  public availableGroups: IGroupDto[];
  public selectedGroupUid: string = "";

  constructor(
    private dialogRef: MatDialogRef<AssignGroupComponent>,
    private locationsService: LocationsService,
    private groupsService: GroupsService,
    private notificationService: NotificationService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  onAssign() {
    this.dialogRef.close(this.selectedGroupUid);
  }

  onClose() {
    this.dialogRef.close();
  }
  ngOnInit(): void {
    this.locationsService
      .getGroupsWithAccessToLocation(
        this.customerUid,
        this.data.locationUid
      )
      .pipe(take(1))
      .subscribe((result: IGroupDto[]) => {
        if (result) {
          this.locationGroups = result;
          this.groupsService
            .getGroups(this.customerUid)
            .pipe(take(1))
            .subscribe((groups: IGroupDto[]) => {
              if (groups) {
                this.availableGroups = groups.filter(
                  (group) =>
                    !this.locationGroups.some((pg) => pg.uid === group.uid)
                );
              }
            });
        }
      });
  }

}