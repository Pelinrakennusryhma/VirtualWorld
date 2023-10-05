using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockPattern
{
    public double Day1Max;
    public double Day1Min;
    public double Day2Max;
    public double Day2Min;
    public double Day3Max;
    public double Day3Min;
    public double Day4Max;
    public double Day4Min;
    public double Day5Max;
    public double Day5Min;
    public double Day6Max;
    public double Day6Min;
    public double Day7Max;
    public double Day7Min;

    public StockPattern(double Day1Max, double Day1Min, double Day2Max, double Day2Min, double Day3Max, double Day3Min, double Day4Max, double Day4Min, double Day5Max, double Day5Min, double Day6Max, double Day6Min, double Day7Max, double Day7Min)
    {
        this.Day1Max = Day1Max;
        this.Day1Min = Day1Min;
        this.Day2Max = Day2Max;
        this.Day2Min = Day2Min;
        this.Day3Max = Day3Max;
        this.Day3Min = Day3Min;
        this.Day4Max = Day4Max;
        this.Day4Min = Day4Min;
        this.Day5Max = Day5Max;
        this.Day5Min = Day5Min;
        this.Day6Max = Day6Max;
        this.Day6Min = Day6Min;
        this.Day7Max = Day7Max;
        this.Day7Min = Day7Min;
    }
}
