import { Component, Inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { PersonsService } from "../persons.service";
import { GroupsService } from "../../groups/groups.service";
import { IGroupDto } from "../../groups/contracts/interfaces";
import { take } from "rxjs";
import { NotificationService } from "src/app/core/services/notification.service";

@Component({
  selector: "app-assign-group",
  templateUrl: "./assign-group.component.html",
  styleUrls: ["./assign-group.component.css"],
})
export class AssignGroupComponent implements OnInit {
  public personGroups: IGroupDto[];
  public availableGroups: IGroupDto[];
  public selectedGroupUid: string = "";

  constructor(
    private dialogRef: MatDialogRef<AssignGroupComponent>,
    private personsService: PersonsService,
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
    this.personsService
      .getPersonGroups(
        "316f1c85-8e01-4749-845e-768b22219244",
        this.data.personUid
      )
      .pipe(take(1))
      .subscribe((result: IGroupDto[]) => {
        if (result) {
          this.personGroups = result;
          this.groupsService
            .getGroups("316f1c85-8e01-4749-845e-768b22219244")
            .pipe(take(1))
            .subscribe((groups: IGroupDto[]) => {
              if (groups) {
                this.availableGroups = groups.filter(
                  (group) =>
                    !this.personGroups.some((pg) => pg.uid === group.uid)
                );
              }
            });
        }
      });
  }
}
