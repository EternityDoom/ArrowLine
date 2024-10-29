using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class ArcheryEquation : MonoBehaviour
{
    [SerializeField] double slopeNumerator = 0;
    [SerializeField] double slopeDenominator = 1;
    [SerializeField] double interceptNumerator = 0;
    [SerializeField] double interceptDenominator = 1;
    [SerializeField] bool simplifiedView;
    private EquationPart left, right, slope, intercept;
    private EquationDigit equalSign;

    // Start is called before the first frame update
    void Start()
    {
        if (slopeDenominator == 0)
        {
            throw new ArgumentNullException("ArcheryEquation can't have 0 as the slope denominator.");
        }
        if (interceptDenominator == 0)
        {
            throw new ArgumentNullException("ArcheryEquation can't have 0 as the slope denominator.");
        }
        if (slopeDenominator == 1)
        {
            slope = EquationFactory.MakeNewEquationNumber(transform, slopeNumerator);
        }
        else
        {
            slope = EquationFactory.MakeNewFraction(
                transform, 
                EquationFactory.MakeNewEquationNumber(transform, slopeNumerator), 
                EquationFactory.MakeNewEquationNumber(transform, slopeDenominator)
                );
        }
        if (interceptDenominator == 1)
        {
            intercept = EquationFactory.MakeNewEquationNumber(transform, interceptNumerator);
        }
        else
        {
            intercept = EquationFactory.MakeNewFraction(
                transform,
                EquationFactory.MakeNewEquationNumber(transform, interceptNumerator),
                EquationFactory.MakeNewEquationNumber(transform, interceptDenominator)
                );
        }
        equalSign = EquationFactory.MakeNewEquationDigit(GetComponent<Transform>(), '=');
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDigits()
    {
        left.UpdateDigits();
        right.UpdateDigits();
    }
}
