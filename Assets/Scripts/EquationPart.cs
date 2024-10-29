using System;
using System.Linq;
using UnityEngine;
public abstract class EquationPart : MonoBehaviour
{
    private bool started = false;
    public bool Started { get { return started; } }
    public virtual void Start()
    {
        started = true;
    }
    public virtual AdditionSequence Add(EquationPart other)
    {
        if (other == null)
        {
            throw new ArgumentException("Can't add a null EquationPart.");
        }
        else if (other is AdditionSequence otherseq)
        {
            return otherseq.Add(this);
        }
        else
        {
            return EquationFactory.MakeNewAdditionSequence(transform, this, other);
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
            return Add(EquationFactory.MakeNewEquationNumber(transform, othernum.Number * -1));
        }
        else 
        {
            return Add(EquationFactory.MakeNewMultiplicationSequence(transform, EquationFactory.MakeNewEquationNumber(transform, -1), other));
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
            return otherseq.Multiply(this);
        }
        else
        {
            return EquationFactory.MakeNewMultiplicationSequence(transform, this, other);
        }
    }
    public virtual Fraction Divide(EquationPart other)
    {
        if (other == null)
        {
            throw new ArgumentException("Can't divide a null EquationPart.");
        }
        return EquationFactory.MakeNewFraction(transform, this, other);
    }
    public abstract bool ContainsVariable(char v);
    public abstract bool ContainsVariables(params char[] v);
    public abstract char[] GetVariables();
    public abstract float[] GetDimensions();

    public abstract void UpdateDigits();
}