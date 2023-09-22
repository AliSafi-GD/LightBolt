using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float Height;
    [SerializeField] private List<GroundItem> grounds = new List<GroundItem>();
    public Vector3 NextMove;
    public List<MoveTask> Tasks;
    public Quaternion NextRotate;
    private Coroutine MoveCoroutine;
    public GroundItem currentGround;
    public GroundItem forwardGround;
    public GroundItem StartGround;
    public float startRot;

    private void Awake()
    {
        InitPlayerPos();
    }

    void Start()
    {
       
    }

    void InitPlayerPos()
    {
        currentGround = StartGround;
        transform.position = new Vector3(StartGround.position.x,0,StartGround.position.z);
        transform.rotation = Quaternion.AngleAxis(startRot, Vector3.up);
        NextMove = currentGround.position;
        NextRotate = Quaternion.AngleAxis(startRot, Vector3.up);
    }

    void FindForwardGround()
    {
        foreach (var groundItem in grounds)
        {
            if(groundItem.Equals(currentGround))
                continue;
            
            
            var targetPos = groundItem.position;
            targetPos.y = 0;

            var playerPos = transform.position;
            playerPos.y = 0;
            Vector3 targetDir =targetPos - playerPos;
            float angle = Vector3.Angle(targetDir, transform.forward);
//            Debug.Log(groundItem.name+"     "+angle+"       dis : "+Vector3.Distance(targetPos,playerPos));
            if (angle < 10 && Vector3.Distance(targetPos,playerPos)<=1.3f)
            {
                forwardGround = groundItem;
                return;
            }
           
        }

        forwardGround = null;
    }
    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.KeypadEnter))
            MoveCoroutine = StartCoroutine(IStartMove());
      
        FindForwardGround();
        
      
        
        
        
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            Forward();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            CounterClockWiseRotate();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ClockWiseRotate();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    IEnumerator IE_Move()
    {
        while (Vector3.Distance(transform.position,NextMove)>0)
        {
            transform.position = Vector3.MoveTowards(transform.position, NextMove, 5 * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator IE_Rot()
    {
        while (!transform.rotation.eulerAngles.Equals(NextRotate.eulerAngles))
        {
            transform.rotation =Quaternion.Lerp(transform.rotation,NextRotate,10*Time.deltaTime);
            yield return null;
        }
        
    }
    IEnumerator Move(MoveTask task)
    {

        switch (task.Type)
        {
            case MoveType.Forward:
                if (Height == forwardGround.Height)
                {
                    task.Status = TaskStatus.Accept;
                    NextMove += transform.forward;
                    currentGround = forwardGround;
                    yield return StartCoroutine(IE_Move());
                }
                else
                {
                    task.Status = TaskStatus.Reject;
                }
                break;
            case MoveType.ClockWiseRotate:
                task.Status = TaskStatus.Accept;
                NextRotate = Quaternion.AngleAxis(NextRotate.eulerAngles.y + 90,Vector3.up);
                yield return StartCoroutine(IE_Rot());
                break;
            case MoveType.CounterClockWiseRotate:
                task.Status = TaskStatus.Accept;
                NextRotate = Quaternion.AngleAxis(NextRotate.eulerAngles.y - 90,Vector3.up);
                yield return StartCoroutine(IE_Rot());
                break;
            case MoveType.Jump:
                if (Height + 0.5f == forwardGround.Height || Height - 0.5f == forwardGround.Height)
                {
                    task.Status = TaskStatus.Accept;
                    NextMove += transform.forward;
                    NextMove.y =forwardGround.Height;
                    Height = forwardGround.Height;
                    currentGround = forwardGround;
                    yield return StartCoroutine(IE_Move());
                }else
                {
                    task.Status = TaskStatus.Reject;
                }
                break;
            case MoveType.TurnOnLight:
                if (currentGround.GetComponent<GroundLight>())
                {
                    currentGround.GetComponent<GroundLight>().TurnOnLight();
                    task.Status = TaskStatus.Accept;
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    task.Status = TaskStatus.Reject;
                }
                break;
        }
        
    }

    public void Forward()
    {
        if (forwardGround != null)
        {
            Move(new MoveTask(){Type = MoveType.Forward});
        }
      
    }

    void ClockWiseRotate()
    {
        Move(new MoveTask(){Type = MoveType.ClockWiseRotate});
    }

    void CounterClockWiseRotate()
    {

        Move(new MoveTask() { Type = MoveType.CounterClockWiseRotate });

    }

    void Jump()
    { 
        if (forwardGround != null)
        {

            Move(new MoveTask() { Type = MoveType.Jump });

        }
        
    }
    IEnumerator IStartMove()
    {
        foreach (var task in Tasks)
        {
            yield return StartCoroutine(Move(task));
            task.isDone = true;
        }
    }
    public void RunNextMove()
    {
        
    }

    public enum MoveType
    {
        Forward,
        ClockWiseRotate,
        CounterClockWiseRotate,
        Jump,
        TurnOnLight
    }
}
