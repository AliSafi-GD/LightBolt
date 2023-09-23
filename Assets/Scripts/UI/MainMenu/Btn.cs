using System;
using UnityEngine;
using UnityEngine.UI;


public class Btn : MonoBehaviour
{
    [SerializeField] private Text txt;
    [SerializeField] private Button btn;
    public void SetData(string txt,Action act)
    {
        this.txt.text = txt;
        btn.onClick.AddListener(()=>act?.Invoke());
    }
}
