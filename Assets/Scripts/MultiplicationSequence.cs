using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiplicationSequence : EquationPart
{
    private List<EquationDigit> operators = new List<EquationDigit>();

    private List<EquationPart> sequence = new List<EquationPart>();
    public List<EquationPart> Sequence { get { return sequence; } }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        UpdateDigits();
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
        else if (other is AdditionSequence otherseq)
        {
            EquationPart[] vals = new EquationPart[Sequence.Count + otherseq.Sequence.Count];
            for (int i = 0; i < Sequence.Count; i++)
            {
                vals[i] = Sequence[i];
            }
            for (int i = Sequence.Count; i < Sequence.Count + otherseq.Sequence.Count; i++)
            {
                vals[i] = otherseq.Sequence[i];
            }
            MultiplicationSequence result = EquationFactory.MakeNewMultiplicationSequence(vals);
            if (Started) other.transform.SetParent(transform, false);
            return result;
        }
        else
        {
            EquationPart[] vals = new EquationPart[Sequence.Count + 1];
            for (int i = 0; i < Sequence.Count; i++)
            {
                vals[i] = Sequence[i];
            }
            vals[Sequence.Count] = other;
            MultiplicationSequence result = EquationFactory.MakeNewMultiplicationSequence(vals);
            if (Started) other.transform.SetParent(transform, false);
            return result;
        }
    }

    /// <summary>
    /// Sorts the MultiplicationSequence's children by type. It puts
    /// EquationNumbers first, then Fractions that have no variables,
    /// then EquationVariables, and then everything more complicated
    /// goes last.
    /// Why that pattern? Well, it makes it easy to see if the sequence
    /// is simple!
    /// </summary>
    public void Sort()
    {
        sequence.Sort(CompareEquationParts);
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
            if (a is EquationNumber anum && b is EquationNumber bnum) { return anum.Number >= bnum.Number ? 1 : -1; }
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

    public override float[] GetDimensions()
    {
        float[] vars = new float[2];
        if (sequence.Count == 0) return vars;
        if (IsSimple())
        {
            sequence[0].GetDimensions().CopyTo(vars, 0);
            vars[0] += sequence.Count - 1;
        }
        else
        {
            vars[0] = -1;
            vars[1] = 1;
            for (int i = 0; i < sequence.Count; i++)
            {
                float[] subDs = sequence[i].GetDimensions();
                vars[0] += subDs[0] + 1;
                vars[1] = vars[1] < subDs[1] ? subDs[1] : vars[1];
            }
        }
        return vars;
    }

    /// <summary>
    /// Checks to see if this MultiplicationSequence is simple, in which
    /// case it doesn't need multiplication signs. A simple MultiplicationSequence
    /// may or may not have an EquationNumber or a simple Fraction as its 
    /// first part, but any other parts must be EquationVariables.
    /// </summary>
    /// <returns>True if it's simple, false otherwise.</returns>
    public bool IsSimple()
    {
        // This isn't the best way to check if the first element is a
        // number or a simple fraction. I should fix it later.
        if (sequence[0].GetVariables().Length == 0)
        {
            for (int i = 1; i < sequence.Count; i++)
            {
                if (!(sequence[i] is EquationVariable)) return false;
            }
        }
        else
        {
            for (int i = 0; i < sequence.Count; i++)
            {
                if (!(sequence[i] is EquationVariable)) return false;
            }
        }
        return true;
    }

    public override void UpdateDigits()
    {
        if (!Started) return;
        foreach (var item in sequence)
        {
            item.UpdateDigits();
        }
        if (IsSimple()) 
        {
            for (int i = 0; i < operators.Count; i++)
            {
                Destroy(operators[i].gameObject);
            }
            operators.Clear();
            float positiondex = 0.0f;
            for (int i = 0; i < sequence.Count; i++)
            {
                sequence[i].transform.SetParent(transform, false);
                sequence[i].transform.SetLocalPositionAndRotation(new Vector3(positiondex, 0, 0), new Quaternion());
                positiondex += sequence[i].GetDimensions()[0];
            }
        } else {
            for (int i = sequence.Count - 1; i < operators.Count; i++)
            {
                Destroy(operators[i].gameObject);
            }
            if (sequence.Count - 1 < operators.Count)
            {
                operators.RemoveRange(sequence.Count - 1, operators.Count - (sequence.Count - 1));
            }
            for (int i = operators.Count; i < sequence.Count - 1; i++)
            {
                EquationDigit plussign = EquationFactory.MakeNewEquationDigit('×');
                plussign.transform.SetParent(transform, false);
                operators.Add(plussign);
            }
            sequence[0].transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), new Quaternion());
            float positiondex = sequence[0].GetDimensions()[0];
            for (int i = 1; i < sequence.Count; i++)
            {
                operators[i - 1].transform.SetLocalPositionAndRotation(new Vector3(positiondex, 0, 0), new Quaternion());
                positiondex++;
                sequence[i].transform.SetParent(transform, false);
                sequence[i].transform.SetLocalPositionAndRotation(new Vector3(positiondex, 0, 0), new Quaternion());
                positiondex += sequence[i].GetDimensions()[0];
            }
        }
    }
}