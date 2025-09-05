import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { RefractoryService } from '../../../services/refractory.service';
import { Refcalc } from '../../../models/refcalc.model';

@Component({
  selector: 'app-ref-calc',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ref-calc.component.html',
  styleUrls: ['./ref-calc.component.css']
})
export class RefCalcComponent implements OnInit {
  private refService = inject(RefractoryService);

  get refCalcRes() {
    return this.refService.refCalcRes;
  }

  get refractory() {
    return this.refService.refractory;
  }

  ngOnInit(): void {
    // Component initialization
  }

  onSubmit(form: NgForm): void {
    this.CalcRefForm(form);
  }

  CalcRefForm(data: any): void {
    const FirstBrickA1 = parseFloat(data.value.FirstBrickA1);
    const FirstBrickA2 = parseFloat(data.value.FirstBrickA2);
    const SecondBrickA1 = parseFloat(data.value.SecondBrickA1);
    const SecondBrickA2 = parseFloat(data.value.SecondBrickA2);
    const BrickLength = parseFloat(data.value.BrickLength);
    const TopDiameter = parseFloat(data.value.TopDiameter);
    const BottomDiameter = parseFloat(data.value.BottomDiameter);
    const RowNumber = parseFloat(data.value.RowNumber);

    const calcData: Refcalc = {
      a1: FirstBrickA1.toString(),
      a2: FirstBrickA2.toString(),
      b1: SecondBrickA1.toString(),
      b2: SecondBrickA2.toString(),
      BrickLength: BrickLength.toString(),
      TopDiameter: TopDiameter.toString(),
      BottomDiameter: BottomDiameter.toString(),
      RowNumber: RowNumber.toString(),
      Density: this.refService.refractory?.Density || '',
      Price: this.refService.refractory?.Price || ''
    };

    this.refService.GetRefrCalc(calcData);
  }
}