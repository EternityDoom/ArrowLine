using System;
using System.Collections.Generic;
using UnityEngine;
public class Fraction : EquationPart
{
    [SerializeField] GameObject bar;
    private EquationPart numerator;
    private EquationPart denominator;
    public EquationPart Numerator 
    {
        get { return numerator; }
        set 
        {
            if (value == null) { 
                throw new ArgumentException("Fraction can't have a null numerator.");
            }
            else
            {
                numerator = value;
                UpdateDigits();
            }
        }
    }
    public EquationPart Denominator { 
        get { return denominator; }
        set
        {
            if (value == null) { throw new ArgumentException("Fraction can't have a null denominator."); }
            else if (value is EquationNumber valnum && valnum.Number == 0) { throw new ArgumentException("Fraction denominator can't be 0."); }
            else 
            {
                denominator = value;
                UpdateDigits(); 
            }
        }
    }
    public Fraction()
    {
        numerator = EquationFactory.MakeNewEquationNumber(transform, 0);
        denominator = EquationFactory.MakeNewEquationNumber(transform, 1);
    }
    public Fraction(double top)
    {
        numerator = new EquationNumber(top);
        denominator = new EquationNumber(1);
    }
    public Fraction(double top, double bottom)
    {
        numerator = new EquationNumber(top);
        denominator = new EquationNumber(bottom);
    }
    public Fraction(EquationPart top)
    {
        Numerator = top;
        denominator = new EquationNumber(1);
    }
    public Fraction(EquationPart top, EquationPart bottom)
    {
        Numerator = top;
        Denominator = bottom;
    }
    public override bool ContainsVariable(char v)
    {
        return Numerator.ContainsVariable(v) || Denominator.ContainsVariable(v);
    }

    public override bool ContainsVariables(params char[] v)
    {
        bool[] flags = new bool[v.Length];
        for (int i = 0; i < flags.Length; i++)
        {
            if (Numerator.ContainsVariable(v[i]) || Denominator.ContainsVariable(v[i])) flags[i] = true;

        }
        for (int i = 0; i < flags.Length; i++)
        {
            if (!flags[i]) return false;
        }
        return true;
    }

    public override char[] GetVariables()
    {
        List<char> v = new List<char>();
        foreach (char c in Numerator.GetVariables()) 
        {
            if (!v.Contains(c)) v.Add(c);
        }
        foreach (char c in Denominator.GetVariables())
        {
            if (!v.Contains(c)) v.Add(c);
        }
        v.Sort();
        return v.ToArray();
    }

    public override float[] GetDimensions()
    {
        float[] vars = new float[2];
        float[] numerDs = Numerator.GetDimensions();
        float[] denomDs = Denominator.GetDimensions();
        vars[0] = numerDs[0] > denomDs[0] ? numerDs[0] : denomDs[0];
        vars[1] = numerDs[1] + denomDs[1];
        // If the Numerator or Denominator has a Fraction in it, shrink them both and add a little padding to each side.
        if (vars[1] > 2.0f)
        {
            vars[0] *= 0.66f;
            vars[1] *= 0.66f;
            vars[0]++;
        }
        return vars;
    }
    private void UpdateDigits()
    {
        float[] numerDs = Numerator.GetDimensions();
        float[] denomDs = Denominator.GetDimensions();
        float width = numerDs[0] > denomDs[0] ? numerDs[0] : denomDs[0];
        if (numerDs[1] > 1.0f || denomDs[1] > 1.0f)
        {
            width++;
            Numerator.gameObject.transform.localScale = new Vector3(0.66f, 0.66f, 1);
            Numerator.transform.localPosition = new Vector3((width - numerDs[0]) / 2.0f, numerDs[1] * 0.33f, 0);
            Denominator.gameObject.transform.localScale = new Vector3(0.66f, 0.66f, 1);
            Denominator.transform.localPosition = new Vector3((width - denomDs[0]) / 2.0f, denomDs[1] * -0.33f, 0);
        } else
        {
            Numerator.gameObject.transform.localScale = new Vector3(1, 1, 1);
            Numerator.transform.localPosition = new Vector3((width - numerDs[0]) / 2.0f, 0.5f, 0);
            Denominator.gameObject.transform.localScale = new Vector3(1, 1, 1);
            Denominator.transform.localPosition = new Vector3((width - denomDs[0]) / 2.0f, -0.5f, 0);
        }
        bar.transform.localScale = new Vector3(width, 0.075f, 1);
        bar.transform.localPosition = new Vector3((width - 1.0f) / 2.0f, 0, 0);
    }
}