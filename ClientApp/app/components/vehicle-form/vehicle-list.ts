import { VehicleService } from './../../services/vehicle.service';
import { Vehicle, KeyValuePair } from './../../models/vehicle';
import { OnInit, Component } from '@angular/core';

@Component({
    templateUrl: 'vehicle-list.html'
})

export class VehicleListComponent implements OnInit {
    private readonly PAGE_SIZE = 3;

    queryResult : any = {};
    makes: KeyValuePair[] = [];
    models: KeyValuePair[] = [];
    query: any = { 
        pageSize: this.PAGE_SIZE
    };
    columns = [
        {title: 'Id'},
        {title: 'Contact Name', key: 'id', isSortable : true},
        {title: 'Make', key: 'make', isSortable : true},
        {title: 'Model', key: 'model', isSortable : true},
        { }
    ];

    constructor(private vehicleService: VehicleService){

    }

    ngOnInit(){
        this.vehicleService.getMakes()
            .subscribe(makes => this.makes = makes);

        this.populateVehicles();
    }


    private populateVehicles(){
        this.vehicleService.getVehicles(this.query)
            .subscribe(result => this.queryResult = result);
    }

    onFilterChange(){
        this.query.page = 1;
        this.populateVehicles();
    }

    resetFilter(){
        this.query = {
            page: 1,
            pageSize: this.PAGE_SIZE
        };
        this.populateVehicles();
    }

    sortBy(columnName: any){
        if(this.query.sortBy === columnName){
            this.query.isSortAscending = !this.query.isSortAscending;
        }else{
            this.query.sortBy = columnName;
            this.query.isSortAscending = false;
        }
        this.populateVehicles();
    }

    onPageChange(page: any){
        this.query.page = page;
        this.populateVehicles();
    }

}