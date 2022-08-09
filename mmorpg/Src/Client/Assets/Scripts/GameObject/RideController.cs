﻿using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RideController : MonoBehaviour 
    {
    public Transform mountPoint;
    public EntityController rider;
    public Vector3 offset;
    public Animator anim;
    void Start()
    {
        this.anim = this.GetComponent<Animator>();

    }
    void Update()
    {
        if (this.mountPoint==null||this.rider==null) {
            return;
        }
        this.rider.SetRidePosition(this.mountPoint.position+this.mountPoint.TransformDirection(this.offset));

    }
    public void SetRider(EntityController rider) {
        this.rider = rider;
    }
    public void OnEntityEvent(EntityEvent entityEvent,int param) {
        switch (entityEvent) {
            case EntityEvent.Idle:
                anim.SetBool("Move", false);
                anim.SetTrigger("Idle");
                break;
            case EntityEvent.MoveFwd:
                anim.SetBool("Move",true);
                break;
            case EntityEvent.MoveBack:
                anim.SetBool("Move",true);
                break;
            case EntityEvent.Jump:
                anim.SetTrigger("Jump");
                break;
        }
    }

}

