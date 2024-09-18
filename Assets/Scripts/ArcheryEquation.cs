using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class ArcheryEquation : MonoBehaviour
{
    [SerializeField] int slope;
    [SerializeField] int intercept;
    [SerializeField] GameObject digit;
    private EquationPart left, right;
    private GameObject equalSign;

    // Start is called before the first frame update
    void Start()
    {
        equalSign = Instantiate(digit, GetComponent<Transform>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
