export class IGroupDto{
    uid: string = '';
    name: string = '';
    createdOn: string = '';
    description: string = '';
}

export class IUpdateGroupRequest{
    name: string = '';
    description: string = '';
}

export class ICreateGroupRequest{
    name: string = '';
    description: string = '';
}