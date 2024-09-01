using System;
using System.Collections.Generic;
using System.Linq;

public class MultiplicationSequence : EquationPart
{
    private List<EquationPart> sequence = new List<EquationPart>();
    public List<EquationPart> Sequence { get { return sequence; } }

    public MultiplicationSequence(params EquationPart[] parts) 
    {
        foreach (var part in parts)
        {
            if (part != null)
            {
                if (part is MultiplicationSequence sqpart)
                {
                    sequence.AddRange(sqpart.Sequence);
                }
                else
                {
                    sequence.Add(part);
                }
            }
            else throw new ArgumentException("Multiplication Sequence can't have a null element.");
        }
    }

    public override bool ContainsVariable(char v)
    {
        foreach (var item in sequence)
        {
            if (item.ContainsVariable(v)) return true;
        }
        return false;
    }

    public override bool ContainsVariables(params char[] v)
    {
        bool[] flags = new bool[v.Length];
        foreach (var item in sequence)
        {
            for (int i = 0; i < flags.Length; i++)
            {
                if (item.ContainsVariable(v[i])) flags[i] = true;

            }
        }
        for (int i = 0;i < flags.Length; i++)
        {
            if (!flags[i]) return false;
        }
        return true;
    }

    public override char[] GetVariables()
    {
        List<char> v = new List<char>();
        foreach (var item in sequence)
        {
            foreach (char c in item.GetVariables())
            {
                if (!v.Contains(c)) v.Add(c);
            }
        }
        v.Sort();
        return v.ToArray();
    }
    public override MultiplicationSequence Multiply(EquationPart other)
    {
        if (other == null)
        {
            throw new ArgumentException("Can't multiply a null EquationPart.");
        }
        else if (other is MultiplicationSequence otherseq)
        {
            Sequence.AddRange(otherseq.Sequence);
        }
        else
        {
            Sequence.Add(other);
        }
        return this;
    }

    public void Sort()
    {
        Sequence.Sort(CompareEquationParts);
    }

    private static int CompareEquationParts(EquationPart a, EquationPart b)
    {
        // For a comparator like this, if a goes first return a positive number, if b goes first return a negative number.
        if (a == null)
        {
            if (b == null) { return 0; } else { return -1; }
        }
        else if (b == null) { return 1; }
        else
        {
            if (a is EquationNumber && b is EquationNumber) { return 0; }
            else if (a is EquationNumber) { return 1; }
            else if (b is EquationNumber) { return -1; }
            char[] avars = a.GetVariables();
            char[] bvars = b.GetVariables();
            if (avars.Length == 0 && bvars.Length == 0 && a is Fraction && b is Fraction) { return 0; }
            else if (avars.Length == 0 && a is Fraction) { return 1; }
            else if (bvars.Length == 0 && b is Fraction) { return -1; }
            if (a is EquationVariable avar && b is EquationVariable bvar) { return avar.VariableChar - bvar.VariableChar; }
            else if (a is EquationVariable) { return 1; }
            else if (b is EquationVariable) { return -1; }
            else return 0;
        }
    }
}