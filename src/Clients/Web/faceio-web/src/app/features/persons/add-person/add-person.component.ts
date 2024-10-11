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
  public email: string = "";
  public phone: string = "";

  constructor(private dialogRef: MatDialogRef<AddPersonComponent>) {}

  onAdd() {
    let request: ICreatePersonRequest = {
      name: this.name,
      email: this.email,
      phone: this.phone,
    };

    this.dialogRef.close(request);
  }

  onCancel() {
    this.dialogRef.close();
  }

  ngOnInit(): void {}
}
