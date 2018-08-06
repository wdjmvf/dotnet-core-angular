import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core';

@Injectable()
export class PopupService {

  constructor(private toastr: ToastrService) {
  }

  public throwErrorPopup(textOfError: string) {
    this.toastr.error(
        'An unexpected error happened'
        , 'Error'
        , { timeOut:5000 });
  }

}