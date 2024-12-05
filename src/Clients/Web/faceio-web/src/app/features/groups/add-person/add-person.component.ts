import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PersonsService } from '../../persons/persons.service';
import { GroupsService } from '../groups.service';
import { IPersonDto } from '../../persons/contracts/interfaces';
import { take } from 'rxjs';

@Component({
  selector: 'app-add-person',
  templateUrl: './add-person.component.html',
  styleUrls: ['./add-person.component.css']
})
export class AddPersonComponent implements OnInit {
  public customerUid: string = "316f1c85-8e01-4749-845e-768b22219244";

  public groupPeople: IPersonDto[];
  public availablePeople: IPersonDto[];
  public selectedPersonUid: string = "";

  constructor(
    private dialogRef: MatDialogRef<AddPersonComponent>,
    private personsService: PersonsService,
    private groupsService: GroupsService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  onAssign() {
    this.dialogRef.close(this.selectedPersonUid);
  }

  onClose() {
    this.dialogRef.close();
  }

  ngOnInit(): void {
    this.groupsService
      .getPeopleInGroup(
        this.customerUid,
        this.data.groupUid
      )
      .pipe(take(1))
      .subscribe((result: IPersonDto[]) => {
        if (result) {
          this.groupPeople = result;
          this.personsService
            .getPeople(this.customerUid)
            .pipe(take(1))
            .subscribe((people: IPersonDto[]) => {
              if (people) {
                this.availablePeople = people.filter(
                  (person) =>
                    !this.groupPeople.some((pg) => pg.uid === person.uid)
                );
              }
            });
        }
      });
  }
}