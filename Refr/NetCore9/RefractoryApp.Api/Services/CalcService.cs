using RefractoryApp.Api.Models.Refractory;

namespace RefractoryApp.Api.Services;

public static class CalcService
{
    public static CalcRes? CalcRefManual(RefractoryCalculationModel refractory)
    {
        var rowRefractory = new List<DataCalc>();
        float radiusTopInner = refractory.TopDiameter / 2;
        float radiusBottomInner = refractory.BottomDiameter / 2;
        float radiusDelta = (radiusTopInner - radiusBottomInner) / refractory.RowNumber;

        for (int i = 0; i < refractory.RowNumber; i++)
        {
            float radiusIn = radiusBottomInner + i * radiusDelta;
            float radiusOut = radiusIn + refractory.BrickLength;
            float lengthIn = (float)(2 * Math.PI * radiusIn);
            float lengthOut = (float)(2 * Math.PI * radiusOut);

            float a = (lengthIn * refractory.b2 - lengthOut * refractory.b1) / (refractory.a1 * refractory.b2 - refractory.a2 * refractory.b1);
            float b = (lengthOut * refractory.a1 - lengthIn * refractory.a2) / (refractory.a1 * refractory.b2 - refractory.a2 * refractory.b1);

            if (a <= 0 || b <= 0)
            {
                return null;
            }
            
            rowRefractory.Add(new DataCalc { A = Math.Ceiling(a), B = Math.Ceiling(b) });
        }

        int totalA = rowRefractory.Sum(item => (int)item.A);
        int totalB = rowRefractory.Sum(item => (int)item.B);
        
        double density = refractory.Density;
        double massAt = (density * 0.5f * (refractory.a1 + refractory.a2) * refractory.BrickLength * 100 * totalA) / 1000000000;
        double massBt = (density * 0.5f * (refractory.b1 + refractory.b2) * refractory.BrickLength * 100 * totalB) / 1000000000;

        double totalMass = massAt + massBt;
        double totalPrice = totalMass * refractory.Price;

        return new CalcRes { TotalA = totalA, TotalB = totalB, TotalMass = totalMass, TotalPrice = totalPrice };
    }
}

public class DataCalc
{
    public double A { get; set; }
    public double B { get; set; }
}

public class CalcRes
{
    public double TotalA { get; set; }
    public double TotalB { get; set; }
    public double TotalMass { get; set; }
    public double TotalPrice { get; set; }
}