import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { delay, map } from 'rxjs/operators';
import * as moment from 'moment';

import { of } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class AuthenticationService {

    constructor(private http: HttpClient,
        @Inject('LOCALSTORAGE') private localStorage: Storage) {
    }

    login(email: string, password: string) {
        return of(true)
            .pipe(delay(1000),
                map((/*response*/) => {
                    // set token property
                    // const decodedToken = jwt_decode(response['token']);

                    // store email and jwt token in local storage to keep user logged in between page refreshes
                    this.localStorage.setItem('currentUser', JSON.stringify({
                        token: 'aisdnaksjdn,axmnczm',
                        isAdmin: true,
                        email: email,
                        id: '1',
                        alias: email.split('@')[0],
                        expiration: moment().add(1, 'days').toDate(),
                        fullName: email
                    }));

                    return true;
                }));
    }

    logout(): void {
        // clear token remove user from local storage to log user out
        this.localStorage.removeItem('currentUser');
    }

    getCurrentUser(): any {
        let currentUser = this.localStorage.getItem('currentUser');

        if(currentUser){
            return JSON.parse(currentUser);
        }

        return null;

        // TODO: Enable after implementation
        // return JSON.parse(this.localStorage.getItem('currentUser'));
        // return {
        //     token: 'aisdnaksjdn,axmnczm',
        //     isAdmin: true,
        //     email: 'john.doe@gmail.com',
        //     id: '12312323232',
        //     alias: 'john.doe@gmail.com'.split('@')[0],
        //     expiration: moment().add(1, 'days').toDate(),
        //     fullName: 'John Doe'
        // };
    }

    passwordResetRequest(email: string) {
        return of(true).pipe(delay(1000));
    }

    changePassword(email: string, currentPwd: string, newPwd: string) {
        return of(true).pipe(delay(1000));
    }

    passwordReset(email: string, token: string, password: string, confirmPassword: string): any {
        return of(true).pipe(delay(1000));
    }
}
