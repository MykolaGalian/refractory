import { Component, OnInit } from '@angular/core';
import { RefractoryService } from 'src/app/shared/refractory.service';
import { NgForm } from '@angular/forms';
import { FormGroup, FormControl } from '@angular/forms';
import { Refcalc } from 'src/app/model/refractory/refcalc';

@Component({
  selector: 'app-ref-calc',
  templateUrl: './ref-calc.component.html',
  styleUrls: ['./ref-calc.component.css']
})
export class RefCalcComponent implements OnInit {

  constructor(private refService: RefractoryService) { }

  ngOnInit() {
  }

  onSubmit(form: NgForm) {   
      this.CalcRefForm(form);     
   }

   CalcRefForm(data: any){
       let FirstBrickA1 = parseFloat(data.value.FirstBrickA1); 
       let FirstBrickA2 = parseFloat(data.value.FirstBrickA2); 
       let SecondBrickA1 = parseFloat(data.value.SecondBrickA1); 
       let SecondBrickA2 = parseFloat(data.value.SecondBrickA2); 
       let BrickLength = parseFloat(data.value.BrickLength); 
       let TopDiameter = parseFloat(data.value.TopDiameter); 
       let BottomDiameter = parseFloat(data.value.BottomDiameter); 
       let RowNumber = parseFloat(data.value.RowNumber); 

       const datas: Refcalc = {
       a1: FirstBrickA1.toString(),
       a2: FirstBrickA2.toString(),
       b1: SecondBrickA1.toString(),
       b2: SecondBrickA2.toString(),
       BrickLength:BrickLength.toString(),
       TopDiameter : TopDiameter.toString(),
       BottomDiameter : BottomDiameter.toString(),
       RowNumber : RowNumber.toString(),
       Density : this.refService.refractory.Density,
       Price: this.refService.refractory.Price
      }
          
       this.refService.GetRefrCalc(datas);
   }
}
