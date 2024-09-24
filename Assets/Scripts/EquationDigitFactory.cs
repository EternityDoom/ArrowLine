using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationDigitFactory : MonoBehaviour
{
    private static GameObject digitsource;
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

    public static GameObject makeNewDigitObject(GameObject parent)
    {
        if (digitsource == null) 
        {
            throw new System.ArgumentException("The EquationDigitFactory is missing its template digit.");
        } else if (parent == null)
        {
            throw new System.ArgumentException("Can't make a new digit with a null parent.");
        }
        else
        {
            return Instantiate(digitsource, parent.GetComponent<Transform>());
        }
    }

    public static GameObject makeNewDigitObject(Transform parent)
    {
        if (digitsource == null)
        {
            throw new System.ArgumentException("The EquationDigitFactory is missing its template digit.");
        }
        else if (parent == null)
        {
            throw new System.ArgumentException("Can't make a new digit with a null parent.");
        }
        else
        {
            return Instantiate(digitsource, parent);
        }
    }

    public static EquationDigit makeNewEquationDigit(GameObject parent)
    {
        if (digitsource == null)
        {
            throw new System.ArgumentException("The EquationDigitFactory is missing its template digit.");
        }
        else if (parent == null)
        {
            throw new System.ArgumentException("Can't make a new digit with a null parent.");
        }
        else
        {
            return Instantiate(digitsource, parent.GetComponent<Transform>()).GetComponent<EquationDigit>();
        }
    }

    public static EquationDigit makeNewEquationDigit(Transform parent)
    {
        if (digitsource == null)
        {
            throw new System.ArgumentException("The EquationDigitFactory is missing its template digit.");
        }
        else if (parent == null)
        {
            throw new System.ArgumentException("Can't make a new digit with a null parent.");
        }
        else
        {
            return Instantiate(digitsource, parent).GetComponent<EquationDigit>();
        }
    }
}
