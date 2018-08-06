import { ErrorHandler, Inject, NgZone, Injectable, isDevMode } from '@angular/core';
import { PopupService } from './app.popup-service';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class AppErrorHandler implements ErrorHandler {
    constructor(
        private ngZone: NgZone){

    }

    handleError(error: any): void {
        if(!isDevMode){
            console.log("errorxxxx", error);
        }
        

        this.ngZone.run(() => {
            alert("error");
        });

        
        // log on the server

        

    }
}