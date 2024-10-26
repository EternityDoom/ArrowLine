using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
public class EquationNumber : EquationPart
{
    [SerializeField] GameObject decimalPoint;
    private List<EquationDigit> digits = new List<EquationDigit>();
    private double number = 0;
    public double Number { get => number; set { number = value; UpdateDigits(); } }
    public EquationNumber() { }
    public EquationNumber(double number) {  Number = number; }

    // Start is called before the first frame update
    void Start()
    {
        UpdateDigits();
    }
    public override bool ContainsVariable(char v)
    {
        return false;
    }

    public override bool ContainsVariables(params char[] v)
    {
        return false;
    }

    public override char[] GetVariables()
    {
        return new char[0];
    }
    public bool IsInteger() {  return (int)Number == Number; }
    public int GetIntegerValue()
    {
        int i = (int)Number;
        if (i == Number) return i;
        else throw new ArgumentException("Can't get integer value of a decimal.");
    }

    public override float[] GetDimensions()
    {
        float[] vars = new float[2];
        string textform = Number.ToString();
        if (textform.Contains('.'))
        {
            vars[0] = textform.Length - 1;
        }
        else {
            vars[0] = textform.Length;
        }
        vars[1] = 1.0f;
        return vars;
    }

    private void UpdateDigits()
    {
        foreach (EquationDigit digit in digits)
        {
            Destroy(digit.gameObject);
        }
        digits.Clear();
        string textform = Number.ToString();
        bool decimalflag = false;
        for(int i = 0; i < textform.Length; i++)
        {
            if (textform[i] == '.')
            {
                decimalflag = true;
                decimalPoint.transform.localPosition.Set(i - 0.5f, 0, 0);
            }
            else 
            {
                EquationDigit digit = EquationFactory.MakeNewEquationDigit(transform, textform[i]);
                if (decimalflag) digit.transform.localPosition.Set(i - 1, 0, 0);
                else digit.transform.localPosition.Set(i, 0, 0);
                digits.Add(digit);
            }
        }
        if (decimalflag) decimalPoint.gameObject.SetActive(true);
        else decimalPoint.gameObject.SetActive(false);
    }
}