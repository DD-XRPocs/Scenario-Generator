using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEvent : MonoBehaviour
{
    
    public delegate void NextStepEvent();
    public static event NextStepEvent onNextStep;

    public static void TriggerNextStep()
    {
        if (onNextStep != null)
        {
            onNextStep();
        }
    }
}
