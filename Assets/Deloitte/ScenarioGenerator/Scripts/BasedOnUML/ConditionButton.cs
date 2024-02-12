using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionButton : MonoBehaviour
{
    Button btn;

    private void Awake()
    {
        btn = transform.Find("Button").GetComponent<Button>();
    }
    void Start()
    {

         btn.onClick.AddListener(CustomEvent.TriggerNextStep);

    }

}
