export class ILocationDto{
    uid: string = '';
    name: string = '';
    description: string = '';
    createdOn: string = '';
}

export class IUpdateLocationRequest{
    name: string = '';
    description: string = '';
}

export class ICreateLocationRequest{
    name: string = '';
    description: string = '';
}