import { FormBuilder, FormGroup } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { NotificationService } from "./../../../core/services/notification.service";
import { PersonsService } from "./../persons.service";
import { Component, OnInit } from "@angular/core";
import { take } from "rxjs/internal/operators/take";
import { IPersonDto, IUpdatePersonRequest } from "../contracts/interfaces";
import { debounceTime } from "rxjs/internal/operators/debounceTime";
import { distinctUntilChanged } from "rxjs/internal/operators/distinctUntilChanged";

@Component({
  selector: "app-person-details",
  templateUrl: "./person-details.component.html",
  styleUrls: ["./person-details.component.css"],
})
export class PersonDetailsComponent implements OnInit {
  personUid: string;

  public personForm: FormGroup;

  constructor(
    private _fb: FormBuilder,
    private _activatedRoute: ActivatedRoute,
    private personsService: PersonsService,
    private notificationService: NotificationService
  ) {
    this.personForm = this._fb.group({
      name: ["", []],
      description: ["", []],
    });
  }

  ngOnInit(): void {
    this._activatedRoute.params.subscribe((params) => {
      this.personUid = params["personUid"];

      if (this.personUid) {
        this.personsService
          .getPerson("316f1c85-8e01-4749-845e-768b22219244", this.personUid)
          .pipe(take(1))
          .subscribe((result: IPersonDto) => {
            if (result) {
              this.personForm.patchValue({
                name: result.name,
              });
            }
          });
      }

      this.personForm.valueChanges
        .pipe(debounceTime(1000), distinctUntilChanged())
        .subscribe((value) => {
          let request: IUpdatePersonRequest = {
            name: this.personForm.get("name")?.value,
          };

          this.personsService
            .updatePerson(
              "316f1c85-8e01-4749-845e-768b22219244",
              this.personUid,
              request
            )
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
