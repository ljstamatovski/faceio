export class IPersonDto{
    uid: string = '';
    name: string = '';
    createdOn: string = '';
}

export class IUpdatePersonRequest{
    name: string = '';
}

export class ICreatePersonRequest{
    name: string = '';
}