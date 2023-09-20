using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private int Height;
    [SerializeField] private List<GroundItem> grounds = new List<GroundItem>();
    public Vector3 NextMove;
    [SerializeField] private List<MoveType> moves;
    private Quaternion NextRotate;
    private Coroutine MoveCoroutine;
    void Start()
    {
        NextMove = transform.position;
        MoveCoroutine = StartCoroutine(IStartMove());
    }

    bool CanMove(Vector3 nextPos)
    {
        return grounds.Exists(x => Vector3.Distance(nextPos,x.position)<0.8f);
    }

    IEnumerator IStartMove()
    {
        foreach (var move in moves)
        {
            switch (move)
            {
                case MoveType.Forward:
                    NextMove += transform.forward;
                    var canMove = CanMove(NextMove);
                    if(canMove)
                        while (transform.position != NextMove)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, NextMove, 1 * Time.deltaTime);
                            yield return null;
                        }

                    break;
                case MoveType.Jump:
                    
                    break;
                case MoveType.CounterClockWiseRotate:
                    NextRotate = Quaternion.AngleAxis(transform.rotation.y-90, Vector3.up);
                    while (transform.rotation != NextRotate)
                    {
                        transform.rotation =Quaternion.Lerp(transform.rotation,NextRotate,5*Time.deltaTime);
                        yield return null;
                    }
                    
                    break;
                case MoveType.ClockWiseRotate:
                    NextRotate *= Quaternion.AngleAxis(transform.rotation.y + 90,Vector3.up);
                    Debug.Log($"{transform.rotation}     {NextRotate}");
                    while (transform.rotation != NextRotate)
                    {
                        Debug.Log($"{transform.rotation}     {NextRotate}");
                        transform.rotation =Quaternion.Lerp(transform.rotation,NextRotate,5*Time.deltaTime);
                        yield return null;
                    }

                    break;
            }
        }
    }
    public void RunNextMove()
    {
        
    }

    enum MoveType
    {
        Forward,
        ClockWiseRotate,
        CounterClockWiseRotate,
        Jump
    }
}
