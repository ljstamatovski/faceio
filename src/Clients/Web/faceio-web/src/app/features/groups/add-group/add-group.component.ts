import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ICreateGroupRequest } from '../contracts/interfaces';

@Component({
  selector: 'app-add-group',
  templateUrl: './add-group.component.html',
  styleUrls: ['./add-group.component.css']
})
export class AddGroupComponent {

  public name: string = "";
  public description: string = "";

  constructor(private dialogRef: MatDialogRef<AddGroupComponent>) {}


  onAdd() {
    let request: ICreateGroupRequest = {
      name: this.name,
      description: this.description,
    };

    this.dialogRef.close(request);
  }

  onCancel() {
    this.dialogRef.close();
  }
}
