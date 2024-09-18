using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Scripts
{
    public class EquationDigit : MonoBehaviour
    {
        private Transform t;
        public char Digit { 
            get
            {
                return digittext.text[0];
            }
            set
            {
                digittext.text = value.ToString();
            } 
        }
        [SerializeField] TMP_Text digittext;
        public Vector3 Position { get { return t.position; } }
        void Start()
        {
            t = GetComponent<Transform>();
        }
    }
}