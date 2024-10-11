using System;
using System.Collections.Generic;
using System.Linq;

public class AdditionSequence : EquationPart
{
    private List<EquationPart> sequence = new List<EquationPart>();
    public List<EquationPart> Sequence { get { return sequence; } }

    public AdditionSequence(params EquationPart[] parts) 
    {
        foreach (var part in parts)
        {
            if (part != null)
            {
                if (part is AdditionSequence sqpart)
                {
                    sequence.AddRange(sqpart.Sequence);
                } else {
                    sequence.Add(part);
                }
            }
            else throw new ArgumentException("Addition Sequence can't have a null element.");
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
    public override AdditionSequence Add(EquationPart other)
    {
        if (other == null)
        {
            throw new ArgumentException("Can't add a null EquationPart.");
        }
        else if (other is AdditionSequence otherseq)
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
            char[] avars = a.GetVariables();
            char[] bvars = b.GetVariables();
            if (avars.Length > bvars.Length) return 1;
            if (avars.Length < bvars.Length) return -1;
            for (int i = 0; i < avars.Length; i++)
            {
                if (avars[i] != bvars[i]) return avars[i] - bvars[i];
            }
            if (a is EquationNumber anum && b is EquationNumber bnum)
            {
                return (int)(anum.Number - bnum.Number);
            }
            return 0;
        }
    }

    public override float[] GetDimensions()
    {
        float[] vars = new float[2];
        if (sequence.Count == 0) return vars;
        vars[0] = -1;
        vars[1] = 1;
        for (int i = 0; i < sequence.Count; i++)
        {
            float[] subDs = sequence[i].GetDimensions();
            vars[0] += subDs[0] + 1;
            vars[1] = vars[1] < subDs[1] ? subDs[1] : vars[1];
        }
        return vars;
    }
}