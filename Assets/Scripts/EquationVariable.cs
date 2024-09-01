using System;
public class EquationVariable : EquationPart
{
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
    public EquationVariable() { VariableChar = 'x'; }
    public EquationVariable(char vchar) { VariableChar = vchar; }
    public override bool ContainsVariable(char v)
    {
        return v == vchar || v == vchar - ' ';
    }

    public override bool ContainsVariables(params char[] v)
    {
        return v.Length == 1 && ContainsVariable(v[0]);
    }

    public override char[] GetVariables()
    {
        char[] vars = new char[1];
        vars[0] = vchar;
        return vars;
    }
}