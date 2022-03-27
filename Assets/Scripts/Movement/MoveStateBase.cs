using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveStateBase
{
    protected CatMovement m_CatMovement;
    protected string m_Name;

    public MoveStateBase(CatMovement cm, string name)
    {
        m_CatMovement = cm;
        Debug.Assert(name.Length > 0);
        m_Name = name;
    }

    public virtual void Enter() 
    {
        Debug.Log("Start state: " + m_Name);
    }

    public virtual void Exit()
    {
        Debug.Log("Start state: " + m_Name);
    }

    public virtual void Update(float deltaTime)
    {
        Debug.Log("Update state: " + m_Name);
    }


    public virtual void OnMoveActionStarted() { }

    public virtual void OnMoveActionPerformed(Vector2 input) { }

    public virtual void OnMoveActionCanceled() { }

    public virtual void OnJumpActionStarted() { }

    public virtual void OnJumpActionPerformed() { }

    public virtual void OnJumpActionCanceled() { }


}
