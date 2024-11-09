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
    private GameObject slopeText, interceptText;

    // Start is called before the first frame update
    void Start()
    {
        slopeText = transform.Find("SlopeText").gameObject;
        interceptText = transform.Find("InterceptText").gameObject;
        if (slopeDenominator == 0)
        {
            slopeDenominator = 1;
        }
        if (interceptDenominator == 0)
        {
            interceptDenominator = 1;
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
        slope.transform.position = new Vector3(2, 0, 0);
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
        intercept.transform.position = new Vector3(5 + slope.GetDimensions()[0], 0, 0);
        equalSign = EquationFactory.MakeNewEquationDigit(GetComponent<Transform>(), '=');
        equalSign.transform.position = new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDigits()
    {
        if (simplifiedView)
        {
            left.gameObject.SetActive(false);
            right.gameObject.SetActive(false);
            slope.gameObject.SetActive(true);
            intercept.gameObject.SetActive(true);
            slopeText.gameObject.SetActive(true);
            interceptText.gameObject.SetActive(true);
            // When do we call UpdateDigits() for the slope and the intercept?
        }
        else
        {
            slope.gameObject.SetActive(false);
            intercept.gameObject.SetActive(false);
            slopeText.gameObject.SetActive(false);
            interceptText.gameObject.SetActive(false);
            left.gameObject.SetActive(true);
            right.gameObject.SetActive(true);
            left.UpdateDigits();
            right.UpdateDigits();
            // We still need to reposition the right and the equal sign.
        }
    }

    public bool IsInSlopeInterceptForm()
    {
        return EquationPartIsY(left) && (
                (slopeNumerator == 0 && EquationPartIsIntercept(right))
                || (interceptNumerator == 0 && EquationPartMatchesSlopeAndX(right))
                || (
                    right is AdditionSequence addright
                 && addright.Sequence.Count == 2
                 && EquationPartMatchesSlopeAndX(addright.Sequence[0])
                 && EquationPartIsIntercept(addright.Sequence[1])
                )
            );
    }
    private static bool EquationPartIsY(EquationPart part)
    {
        return part is EquationVariable varpart
            && varpart.VariableChar == 'y';
    }
    private static bool EquationPartIsX(EquationPart part)
    {
        return part is EquationVariable varpart
            && varpart.VariableChar == 'x';
    }
    private bool EquationPartIsSlope(EquationPart part)
    {
        return (
                slopeDenominator == 1
             && part is EquationNumber numpart
             && numpart.Number == slopeNumerator
            ) || (
                slopeDenominator != 1
             && part is Fraction fracpart
             && fracpart.Numerator is EquationNumber fractop
             && fractop.Number == slopeNumerator
             && fracpart.Denominator is EquationNumber fracbot
             && fracbot.Number == slopeDenominator
            );
    }
    private bool EquationPartIsIntercept(EquationPart part)
    {
        return (
                interceptDenominator == 1
             && part is EquationNumber numpart
             && numpart.Number == interceptNumerator
            ) || (
                interceptDenominator != 1
             && part is Fraction fracpart
             && fracpart.Numerator is EquationNumber fractop
             && fractop.Number == interceptNumerator
             && fracpart.Denominator is EquationNumber fracbot
             && fracbot.Number == interceptDenominator
            );
    }
    private bool EquationPartIsSlopeTimesX(EquationPart part)
    {
        return  part is MultiplicationSequence seqpart
             && seqpart.IsSimple()
             && seqpart.Sequence.Count == 2
             && EquationPartIsSlope(seqpart.Sequence[0])
             && EquationPartIsX(seqpart.Sequence[1]);
    }
    private bool EquationPartMatchesSlopeAndX(EquationPart part)
    {
        return (slopeNumerator == 1 && slopeDenominator == 1 && EquationPartIsX(right))
            || EquationPartIsSlopeTimesX(right);
    }
    
}
