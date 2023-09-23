using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoHelper : MonoBehaviour
{
    public static MonoHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("__Mono Helper__", typeof(MonoHelper));
                _instance = obj.GetComponent<MonoHelper>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }
    private static MonoHelper _instance;
    public static void Move(Transform transform,Vector3 NextMove,Action Complete)
    {
        Instance.StartCoroutine(Instance.IE_Move(transform, NextMove,Complete));
    }
    IEnumerator IE_Move(Transform transform,Vector3 NextMove,Action Complete)
    {
        while (Vector3.Distance(transform.position,NextMove)>0)
        {
            transform.position = Vector3.MoveTowards(transform.position, NextMove, 5 * Time.deltaTime);
            yield return null;
        }
        Complete?.Invoke();
    }
    
    public static void Rotate(Transform transform,Quaternion NextAngle,Action Complete)
    {
        Instance.StartCoroutine(Instance.IE_Rotate(transform, NextAngle,Complete));
    }
    
    IEnumerator IE_Rotate(Transform transform,Quaternion NextAngle,Action Complete)
    {
        while (!transform.rotation.eulerAngles.Equals(NextAngle.eulerAngles))
        {
            transform.rotation =Quaternion.Lerp(transform.rotation,NextAngle,10*Time.deltaTime);
            yield return null;
        }
        Complete?.Invoke();
    }


    public static void RunCoroutine(IEnumerator enumerator)
    {
        Instance.StartCoroutine(enumerator);
    }
}
