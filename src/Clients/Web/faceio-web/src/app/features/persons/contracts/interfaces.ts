export class IPersonDto{
    uid: string = '';
    name: string = '';
    createdOn: string = '';
    email: string = '';
    phone: string = '';
}

export class IUpdatePersonRequest{
    name: string = '';
    email: string = '';
    phone: string = '';
}

export class ICreatePersonRequest{
    name: string = '';
    email: string = '';
    phone: string = '';
}