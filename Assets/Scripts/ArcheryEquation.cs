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
            slope = new EquationNumber(slopeNumerator);
        }
        else
        {
            slope = new Fraction(slopeNumerator, slopeDenominator);
        }
        if (interceptDenominator == 1)
        {
            intercept = new EquationNumber(interceptNumerator);
        }
        else
        {
            intercept = new Fraction(interceptNumerator, interceptDenominator);
        }
        equalSign = EquationDigitFactory.makeNewEquationDigit(GetComponent<Transform>());
        equalSign.Digit = '=';
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
