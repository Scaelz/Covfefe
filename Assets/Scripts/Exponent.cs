using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Exponent
{
    public static string SetExponentText(double numberChange)
    {
        if (numberChange >= 1000)
        {
            var exponent = (Math.Floor(Math.Log10(Math.Abs(numberChange))));
            Math.DivRem((int)exponent, 3, out int ostatok);
            exponent -= ostatok;
            var multyplyExponent = (numberChange / Math.Pow(10, exponent));

            string tempCoinsText = multyplyExponent.ToString("F2");
            return ExponentView(exponent, tempCoinsText);
        }
        else
        {
            return numberChange.ToString("F0");
        }
    }
    private static string ExponentView(double exponent, string tempCoinsText)
    {
        List<string> charExponent = new List<string>() { "k", "M", "G", "T", "P", "E", "Z", "Y" };
        var divisionExponent = (exponent / 3) - 1;
        if (divisionExponent < charExponent.Count)
        {
            return tempCoinsText + charExponent[(int)divisionExponent];
        }
        else
        {
            return tempCoinsText + "e" + exponent;
        }
    }
}
