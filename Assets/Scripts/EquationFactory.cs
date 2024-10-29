using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquationFactory : MonoBehaviour
{
    private static GameObject equationsource;
    private static GameObject variablesource;
    private static GameObject numbersource;
    private static GameObject additionsource;
    private static GameObject multiplicationsource;
    private static GameObject fractionsource;
    private static GameObject digitsource;
    [SerializeField] GameObject equation;
    [SerializeField] GameObject variable;
    [SerializeField] GameObject number;
    [SerializeField] GameObject addition;
    [SerializeField] GameObject multiplication;
    [SerializeField] GameObject fraction;
    [SerializeField] GameObject digit;

    // Start is called before the first frame update
    void Start()
    {
        if (digit != null) digitsource = digit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public static GameObject MakeNewDigitObject(GameObject parent)
    //{
    //    if (digitsource == null) 
    //    {
    //        throw new System.ArgumentException("The EquationFactory is missing its template digit.");
    //    } else if (parent == null)
    //    {
    //        throw new System.ArgumentException("Can't make a new digit with a null parent.");
    //    }
    //    else
    //    {
    //        return Instantiate(digitsource, parent.GetComponent<Transform>());
    //    }
    //}

    //public static GameObject MakeNewDigitObject(Transform parent)
    //{
    //    if (digitsource == null)
    //    {
    //        throw new System.ArgumentException("The EquationFactory is missing its template digit.");
    //    }
    //    else if (parent == null)
    //    {
    //        throw new System.ArgumentException("Can't make a new digit with a null parent.");
    //    }
    //    else
    //    {
    //        return Instantiate(digitsource, parent);
    //    }
    //}

    //public static EquationDigit MakeNewEquationDigit(GameObject parent)
    //{
    //    if (digitsource == null)
    //    {
    //        throw new System.ArgumentException("The EquationFactory is missing its template digit.");
    //    }
    //    else if (parent == null)
    //    {
    //        throw new System.ArgumentException("Can't make a new digit with a null parent.");
    //    }
    //    else
    //    {
    //        return Instantiate(digitsource, parent.GetComponent<Transform>()).GetComponent<EquationDigit>();
    //    }
    //}

    public static EquationDigit MakeNewEquationDigit(Transform parent, char digit)
    {
        if (digitsource == null)
        {
            throw new System.ArgumentException("The EquationFactory is missing its template digit.");
        }
        else if (parent == null)
        {
            throw new System.ArgumentException("Can't make a new digit with a null parent.");
        }
        else
        {
            var ed = Instantiate(digitsource, parent).GetComponent<EquationDigit>();
            ed.Digit = digit;
            return ed;
        }
    }

    public static EquationVariable MakeNewEquationVariable(Transform parent, char vchar)
    {
        if (variablesource == null)
        {
            throw new System.ArgumentException("The EquationFactory is missing its template variable.");
        }
        else if (parent == null)
        {
            throw new System.ArgumentException("Can't make a new variable with a null parent.");
        }
        else
        {
            var ev = Instantiate(variablesource, parent).GetComponent<EquationVariable>();
            ev.VariableChar = vchar;
            return ev;
        }
    }

    public static EquationNumber MakeNewEquationNumber(Transform parent, double number)
    {
        if (numbersource == null)
        {
            throw new System.ArgumentException("The EquationFactory is missing its template number.");
        }
        else if (parent == null)
        {
            throw new System.ArgumentException("Can't make a new number with a null parent.");
        }
        else
        {
            var en = Instantiate(numbersource, parent).GetComponent<EquationNumber>();
            en.Number = number;
            return en;
        }
    }

    public static AdditionSequence MakeNewAdditionSequence(Transform parent, params EquationPart[] parts)
    {
        if (additionsource == null)
        {
            throw new System.ArgumentException("The EquationFactory is missing its template addition sequence.");
        }
        else if (parent == null)
        {
            throw new System.ArgumentException("Can't make a new addition sequence with a null parent.");
        }
        else if (parts == null || parts.Length == 0)
        {
            throw new System.ArgumentException("Can't make a new addition sequence with no parts.");
        }
        else
        {
            AdditionSequence seq = Instantiate(additionsource, parent).GetComponent<AdditionSequence>();
            foreach (var part in parts)
            {
                seq.Add(part);
            }
            return seq;
        }
    }

    public static MultiplicationSequence MakeNewMultiplicationSequence(Transform parent, params EquationPart[] parts)
    {
        if (multiplicationsource == null)
        {
            throw new System.ArgumentException("The EquationFactory is missing its template multiplication sequence.");
        }
        else if (parent == null)
        {
            throw new System.ArgumentException("Can't make a new multiplication sequence with a null parent.");
        }
        else if (parts == null || parts.Length == 0)
        {
            throw new System.ArgumentException("Can't make a new multiplication sequence with no parts.");
        }
        else
        {
            MultiplicationSequence seq = Instantiate(multiplicationsource, parent).GetComponent<MultiplicationSequence>();
            foreach (var part in parts)
            {
                seq.Multiply(part);
            }
            return seq;
        }
    }

    public static Fraction MakeNewFraction(Transform parent, EquationPart numerator, EquationPart denominator)
    {
        if (fractionsource == null)
        {
            throw new System.ArgumentException("The EquationFactory is missing its template fraction.");
        }
        else if (parent == null)
        {
            throw new System.ArgumentException("Can't make a new fraction with a null parent.");
        }
        else if (numerator == null)
        {
            throw new System.ArgumentException("Can't make a new fraction with a null numerator.");
        }
        else if (denominator == null)
        {
            throw new System.ArgumentException("Can't make a new fraction with a null denominator.");
        }
        else
        {
            Fraction fraction = Instantiate(fractionsource, parent).GetComponent<Fraction>();
            EquationPart oldie = fraction.Numerator;
            fraction.Numerator = numerator;
            Destroy(oldie);
            oldie = fraction.Denominator;
            fraction.Denominator = denominator;
            Destroy(oldie);
            return fraction;
        }
    }
}
