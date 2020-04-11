using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.Refractory;

namespace WebApi.CalcService
{    internal static class CalcService {
        internal static CalcRes CalcRefManual(RefractoryCalculationModel Refractory)
        {
            List<DataCalc> RowRefractory = new List<DataCalc>();
            float radiusTopInner = Refractory.TopDiameter / 2;
            float radiusButtomInner = Refractory.BottomDiameter / 2;
            float radiusDelta = (radiusTopInner - radiusButtomInner) / Refractory.RowNumber;

            float RadiusIn = 0;
            float RadiusOut = 0;
            float LenghtIn = 0;
            float LenghtOut = 0;
            float a = 0;
            float b = 0;

            for (int i = 0; i < Refractory.RowNumber; i++)
            {
                RadiusIn = radiusButtomInner + i * radiusDelta;
                RadiusOut = RadiusIn + Refractory.BrickLength;
                LenghtIn = (float)(2 * 3.14 * RadiusIn);
                LenghtOut = (float)(2 * 3.14 * RadiusOut);

                a = (LenghtIn * Refractory.b2 - LenghtOut * Refractory.b1) / (Refractory.a1 * Refractory.b2 - Refractory.a2 * Refractory.b1);
                b = (LenghtOut * Refractory.a1 - LenghtIn * Refractory.a2) / (Refractory.a1 * Refractory.b2 - Refractory.a2 * Refractory.b1);

                if (a <= 0 || b <= 0)
                {
                    return null;
                }
                RowRefractory.Add(new DataCalc() { A = Math.Ceiling(a), B = Math.Ceiling(b) });
            }

            int totalA = 0;
            int totalB = 0;
            foreach (var item in RowRefractory)
            {
                totalA = totalA + (int)item.A;
                totalB = totalB + (int)item.B;
            }
            double density = Refractory.Density;
            double massAt = (density * 0.5f * (Refractory.a1 + Refractory.a2) * Refractory.BrickLength * 100 * totalA) / 1000000000;
            double massBt = (density * 0.5f * (Refractory.b1 + Refractory.b2) * Refractory.BrickLength * 100 * totalB) / 1000000000;

            double totalMass = massAt + massBt;
            double totalPrice = totalMass * Refractory.Price;

            return new CalcRes() { TotalA = totalA, TotalB = totalB, TotalMass = totalMass, TotalPrice = totalPrice };
        }


    }
}