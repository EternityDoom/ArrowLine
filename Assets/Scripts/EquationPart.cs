using System;
using System.Linq;
using UnityEngine;
public abstract class EquationPart : MonoBehaviour
{
    public virtual AdditionSequence Add(EquationPart other)
    {
        if (other == null)
        {
            throw new ArgumentException("Can't add a null EquationPart.");
        }
        else if (other is AdditionSequence otherseq)
        {
            otherseq.Sequence.Add(this);
            return otherseq;
        }
        else
        {
            return new AdditionSequence(this, other);
        }
    }
    public virtual EquationPart Subtract(EquationPart other)
    {
        if (other == null)
        {
            throw new ArgumentException("Can't subtract a null EquationPart.");
        }
        else if (other is EquationNumber othernum)
        {
            othernum.Number *= -1;
            return Add(othernum);
        }
        else if (other is MultiplicationSequence otherseq)
        {
            for (int i = 0; i < otherseq.Sequence.Count; i++)
            {
                if (otherseq.Sequence[i] is EquationNumber innernum)
                {
                    innernum.Number *= -1;
                    return Add(otherseq);
                }
            }
            otherseq.Sequence.Add(new EquationNumber(-1));
            return Add(otherseq);
        }
        else 
        {
            return Add(new MultiplicationSequence(new EquationNumber(-1), this));
        }
    }
    public virtual MultiplicationSequence Multiply(EquationPart other)
    {
        if (other == null)
        {
            throw new ArgumentException("Can't multiply a null EquationPart.");
        }
        else if (other is MultiplicationSequence otherseq)
        {
            otherseq.Sequence.Add(this);
            return otherseq;
        }
        else
        {
            return new MultiplicationSequence(this, other);
        }
    }
    public virtual Fraction Divide(EquationPart other)
    {
        if (other == null)
        {
            throw new ArgumentException("Can't divide a null EquationPart.");
        }
        return new Fraction(this, other);
    }
    public abstract bool ContainsVariable(char v);
    public abstract bool ContainsVariables(params char[] v);
    public abstract char[] GetVariables();
    public abstract float[] GetDimensions();
}