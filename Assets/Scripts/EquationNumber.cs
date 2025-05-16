using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
public class EquationNumber : EquationPart
{
    [SerializeField] GameObject decimalPoint;
    private List<EquationDigit> digits = new List<EquationDigit>();
    private double number = 0;
    public double Number { get => number; 
        set { 
            number = value;
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
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

    /// <summary>
    /// Checks if this EquationNumber represents an integer.
    /// </summary>
    /// <returns>True if it's an integer, false if it's a decimal.</returns>
    public bool IsInteger() {  return number % 1 == 0; }

    /// <summary>
    /// Gets this EquationNumber's number as an integer. Be sure to
    /// check it with IsInteger() first, so you don't get an exception!
    /// </summary>
    /// <returns>The number, in integer form.</returns>
    /// <exception cref="ArgumentException">Doesn't work if the number is a decimal.</exception>
    public int GetIntegerValue()
    {
        if (IsInteger()) return (int)number;
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

    public override void UpdateDigits()
    {
        if (!Started) return;
        foreach (EquationDigit digit in digits)
        {
            Destroy(digit.gameObject);
        }
        digits.Clear();
        string textform;
        bool decimalflag = false;
        if (IsInteger()) { 
            textform = ((int)Number).ToString();
        }
        else
        {
            textform = Number.ToString();
            decimalflag = true;
        }
        for(int i = 0; i < textform.Length; i++)
        {
            if (textform[i] == '.')
            {
                decimalPoint.transform.SetLocalPositionAndRotation(new Vector3(i - 0.5f, 0, 0), new Quaternion());
            }
            else 
            {
                EquationDigit digit = EquationFactory.MakeNewEquationDigit(textform[i]);
                digit.transform.SetParent(transform, false);
                if (decimalflag) digit.transform.SetLocalPositionAndRotation(new Vector3(i - 1, 0, 0), new Quaternion());
                else digit.transform.SetLocalPositionAndRotation(new Vector3(i, 0, 0), new Quaternion());
                digits.Add(digit);
            }
        }
        if (decimalflag) decimalPoint.gameObject.SetActive(true);
        else decimalPoint.gameObject.SetActive(false);
    }

    public override EquationPart DeepCopy()
    {
        return EquationFactory.MakeNewEquationNumber(number);
    }

    /// <summary>
    /// Alternate version of the DeepCopy() function that accounts
    /// for the return type.
    /// </summary>
    /// <returns>A deep copy of this, as an EquationNumber.</returns>
    public EquationNumber DeepCopyIdentity()
    {
        return EquationFactory.MakeNewEquationNumber(number);
    }
}