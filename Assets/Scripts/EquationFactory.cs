using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
        if (equation != null) equationsource = equation;
        if (variable != null) variablesource = variable;
        if (number != null) numbersource = number;
        if (addition != null) additionsource = addition;
        if (multiplication != null) multiplicationsource = multiplication;
        if (fraction != null) fractionsource = fraction;
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

    public static EquationDigit MakeNewEquationDigit(char digit)
    {
        if (digitsource == null)
        {
            throw new System.ArgumentException("The EquationFactory is missing its template digit.");
        }
        else
        {
            var ed = Instantiate(digitsource).GetComponent<EquationDigit>();
            ed.Digit = digit;
            return ed;
        }
    }

    public static EquationVariable MakeNewEquationVariable(char vchar)
    {
        if (variablesource == null)
        {
            throw new System.ArgumentException("The EquationFactory is missing its template variable.");
        }
        else
        {
            var ev = Instantiate(variablesource).GetComponent<EquationVariable>();
            ev.VariableChar = vchar;
            return ev;
        }
    }

    public static EquationNumber MakeNewEquationNumber(double number)
    {
        if (numbersource == null)
        {
            throw new System.ArgumentException("The EquationFactory is missing its template number.");
        }
        else
        {
            var en = Instantiate(numbersource).GetComponent<EquationNumber>();
            en.Number = number;
            return en;
        }
    }

    public static AdditionSequence MakeNewAdditionSequence(params EquationPart[] parts)
    {
        if (additionsource == null)
        {
            throw new System.ArgumentException("The EquationFactory is missing its template addition sequence.");
        }
        else if (parts == null || parts.Length == 0)
        {
            throw new System.ArgumentException("Can't make a new addition sequence with no parts.");
        }
        else
        {
            AdditionSequence seq = Instantiate(additionsource).GetComponent<AdditionSequence>();
            foreach (var part in parts)
            {
                if (part == null)
                {
                    throw new System.ArgumentException("Can't make a new AdditionSequence with null parts.");
                }
                else 
                { 
                    seq.Sequence.Add(part);
                    part.transform.SetParent(seq.transform, false);
                }
            }
            return seq;
        }
    }

    public static MultiplicationSequence MakeNewMultiplicationSequence(params EquationPart[] parts)
    {
        if (multiplicationsource == null)
        {
            throw new System.ArgumentException("The EquationFactory is missing its template multiplication sequence.");
        }
        else if (parts == null || parts.Length == 0)
        {
            throw new System.ArgumentException("Can't make a new multiplication sequence with no parts.");
        }
        else
        {
            MultiplicationSequence seq = Instantiate(multiplicationsource).GetComponent<MultiplicationSequence>();
            foreach (var part in parts)
            {
                if (part == null)
                {
                    throw new System.ArgumentException("Can't make a new MultiplicationSequence with null parts.");
                }
                else
                {
                    seq.Sequence.Add(part);
                    part.transform.SetParent(seq.transform, false);
                }
            }
            return seq;
        }
    }

    public static Fraction MakeNewFraction(EquationPart numerator, EquationPart denominator)
    {
        if (fractionsource == null)
        {
            throw new System.ArgumentException("The EquationFactory is missing its template fraction.");
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
            Fraction fraction = Instantiate(fractionsource).GetComponent<Fraction>();
            fraction.Numerator = numerator;
            numerator.transform.SetParent(fraction.transform, false);
            fraction.Denominator = denominator;
            denominator.transform.SetParent(fraction.transform, false);
            return fraction;
        }
    }
}
