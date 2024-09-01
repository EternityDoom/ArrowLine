using System;
public class EquationNumber : EquationPart
{
    public double Number { get; set; }
    public EquationNumber() { Number = 0; }
    public EquationNumber(double number) {  Number = number; }
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
}