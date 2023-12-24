import { ICreatePersonRequest } from "./../contracts/interfaces";
import { MatDialogRef } from "@angular/material/dialog";
import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-add-person",
  templateUrl: "./add-person.component.html",
  styleUrls: ["./add-person.component.css"],
})
export class AddPersonComponent implements OnInit {
  public name: string = "";

  constructor(private dialogRef: MatDialogRef<AddPersonComponent>) {}

  onAdd() {
    let request: ICreatePersonRequest = {
      name: this.name,
    };

    this.dialogRef.close(request);
  }

  onCancel() {
    this.dialogRef.close();
  }

  ngOnInit(): void {}
}
