using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a piece of an equation, such as a number, a variable, or a group of those that
/// an operation is being performed on.
/// These are visualized in the game world using EquationDigit objects. An EquationPart might
/// have some visible EquationDigits as children, or it might have other EquationParts as
/// children that have EquationDigits, or maybe some combination of the two.
/// Typically, you shouldn't modify an EquationPart's data after it's created. Instead, create
/// a new EquationPart with the data you want and delete the original.
/// </summary>
public abstract class EquationPart : MonoBehaviour
{
    private bool started = false;

    /// <summary>
    /// Becomes true after the Start() function has been called.
    /// Unity's supposed to have a way of checking if a GameObject has been
    /// started yet, but it wasn't working correctly for me, so I made
    /// this as a replacement.
    /// </summary>
    public bool Started { get { return started; } }
    
    /// <summary>
    /// Called when the GameObject with this script attached is started.
    /// Every class that extends EquationPart should call this in its
    /// override for Start() by using base.Start(). You can see an example
    /// of this in the EquationVariable class.
    /// </summary>
    public virtual void Start()
    {
        started = true;
    }

    /// <summary>
    /// Any two EquationParts can be added together using this function.
    /// By default, it creates an AdditionSequence that includes the
    /// source and the parameter, but some subclasses might handle it
    /// differently.
    /// </summary>
    /// <param name="other">The other EquationPart to add to this one.</param>
    /// <returns>An AdditionSequence representing this and the parameter added together.</returns>
    /// <exception cref="ArgumentException">The parameter cannot be null.</exception>
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
            return EquationFactory.MakeNewAdditionSequence(this, other);
        }
    }

    /// <summary>
    /// Any two EquationParts can be subtracted using this function.
    /// By default, it creates an AdditionSequence that includes the
    /// source and a negative version of the parameter (usually a 
    /// MultiplicationSequence of -1 as an EquationNumber and the parameter),
    /// but some subclasses might handle it differently.
    /// </summary>
    /// <param name="other">The other EquationPart to subtract from this one.</param>
    /// <returns>An AdditionSequence representing the parameter subtracted from this.</returns>
    /// <exception cref="ArgumentException">The parameter cannot be null.</exception>
    public virtual EquationPart Subtract(EquationPart other)
    {
        if (other == null)
        {
            throw new ArgumentException("Can't subtract a null EquationPart.");
        }
        else if (other is EquationNumber othernum)
        {
            return Add(EquationFactory.MakeNewEquationNumber(othernum.Number * -1));
        }
        else 
        {
            return Add(EquationFactory.MakeNewMultiplicationSequence(EquationFactory.MakeNewEquationNumber(-1), other));
        }
    }

    /// <summary>
    /// Any two EquationParts can be multiplied using this function.
    /// By default, it creates a MultiplicationSequence that includes the
    /// source and the parameter, but some subclasses might handle it
    /// differently.
    /// </summary>
    /// <param name="other">The other EquationPart to multiply this one by.</param>
    /// <returns>A MultiplicationSequence representing this multiplied by the parameter.</returns>
    /// <exception cref="ArgumentException">The parameter cannot be null.</exception>
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
            return EquationFactory.MakeNewMultiplicationSequence(this, other);
        }
    }

    /// <summary>
    /// Any two EquationParts can be divided using this function.
    /// By default, it creates a Fraction with the source as the numerator (top)
    /// and the parameter as the denominator (bottom,) but some subclasses might
    /// handle it differently.
    /// </summary>
    /// <param name="other">The other EquationPart to divide this one by.</param>
    /// <returns>A Fraction representing this divided by the parameter.</returns>
    /// <exception cref="ArgumentException">The parameter cannot be null.</exception>
    public virtual Fraction Divide(EquationPart other)
    {
        if (other == null)
        {
            throw new ArgumentException("Can't divide a null EquationPart.");
        }
        return EquationFactory.MakeNewFraction(this, other);
    }

    /// <summary>
    /// Checks this EquationPart and all of its children for an EquationVariable
    /// with a given letter. Right now, the only possibilities are 'x' and 'y'.
    /// This isn't case-sensitive.
    /// </summary>
    /// <param name="v">The letter of the EquationVariable to look for.</param>
    /// <returns>True if the variable is found, false if it is not found.</returns>
    public abstract bool ContainsVariable(char v);

    /// <summary>
    /// Checks this EquationPart and all of its children for EquationVariables
    /// with the given letters. Right now, the only possibilities are 'x' and 'y'.
    /// This isn't case-sensitive.
    /// </summary>
    /// <param name="v">The letters of the EquationVariables to look for.</param>
    /// <returns>True if all of the variables are found, false if any of them are not found.</returns>
    public abstract bool ContainsVariables(params char[] v);

    /// <summary>
    /// Checks this EquationPart and all of its children to see which letters
    /// the EquationVariables within have. Right now, the only possibilities are 'x' and 'y'.
    /// </summary>
    /// <returns>An array of all of the variables that were found, in alphabetical order.</returns>
    public abstract char[] GetVariables();

    /// <summary>
    /// Checks to see how much space this EquationPart (and its children) take up.
    /// This is purely mathematical, and doesn't require the EquationPart to have
    /// been rendered first, so you can calculate where to place it ahead of time.
    /// </summary>
    /// <returns>A 2-element array of this EquationPart's width and height.</returns>
    public abstract float[] GetDimensions();

    /// <summary>
    /// Creates all of the EquationDigits and other graphical elements needed to
    /// render this EquationPart and its children, and puts them in the right
    /// places relative to this EquationPart's origin. This should be called when
    /// an ArcheryEquation is modified.
    /// </summary>
    public abstract void UpdateDigits();
}