import { Component, OnInit } from '@angular/core';
import { IMyOptions, IMyDateModel } from 'mydatepicker';
import { DateUtils } from '../common/date-utils';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  constructor() { 
  }

  private myDatePickerOptions = DateUtils.GetMyDatePickerOptions();
  private myDatePickerPlaceholder : string = 'Select a date';

  public hiddenList = true;
  public hiddenGraph = true;

  // Pie
  public pieChartLabels:string[] = ['Antibióticos', 'Antialérgicos', 'Analgésicos'];
  public pieChartData:number[] = [10, 5, 15];
  public pieChartType:string = 'pie';
 
  // events
  public chartClicked(e:any):void {
    console.log(e);
  }
 
  public chartHovered(e:any):void {
    console.log(e);
  }  

  ngOnInit() {
  }

}
