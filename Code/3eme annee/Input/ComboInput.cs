using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new ComboInput", menuName = "Combo System/ComboInput", order = 51)]
public class ComboInput : ScriptableObject
{
    public List<string> inputs;
    public UnityEvent action;
}
