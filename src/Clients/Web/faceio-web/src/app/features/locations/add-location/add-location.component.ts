import { Component, OnInit } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { ICreateLocationRequest } from "../contracts/interfaces";

@Component({
  selector: "app-add-location",
  templateUrl: "./add-location.component.html",
  styleUrls: ["./add-location.component.css"],
})
export class AddLocationComponent implements OnInit {
  public name: string = "";
  public description: string = "";

  constructor(private dialogRef: MatDialogRef<AddLocationComponent>) {}

  onAdd() {
    let request: ICreateLocationRequest = {
      name: this.name,
      description: this.description,
    };

    this.dialogRef.close(request);
  }

  onCancel() {
    this.dialogRef.close();
  }

  ngOnInit(): void {}
}
