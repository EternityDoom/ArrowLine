using Assets.Scripts;
using System;
using UnityEngine;
public class EquationVariable : EquationPart
{
    private EquationDigit digit;
    private char vchar;
    public char VariableChar { 
        get { return vchar; }
        set {
            if (value == 'x' || value == 'y')
            {
                vchar = value;
            }
            else if (value == 'X' || value == 'Y')
            {
                vchar = (char)((int)value + 32);
            }
            else throw new ArgumentException("Can't set an Equation Variable to something other than x or y.");
        }
    }
    public EquationVariable() { vchar = 'x'; }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        digit = EquationFactory.MakeNewEquationDigit(vchar);
        digit.transform.SetParent(transform, false);
    }
    public override bool ContainsVariable(char v)
    {
        return v == vchar || v == vchar - ' ';
    }

    public override bool ContainsVariables(params char[] v)
    {
        return (v.Length == 1 && ContainsVariable(v[0])) || v.Length == 0;
    }

    public override char[] GetVariables()
    {
        char[] vars = new char[1];
        vars[0] = vchar;
        return vars;
    }

    public override float[] GetDimensions()
    {
        float[] vars = new float[2];
        vars[0] = 1.0f;
        vars[1] = 1.0f;
        return vars;
    }

    public override void UpdateDigits()
    {
        if (Started) digit.Digit = vchar;
    }
}