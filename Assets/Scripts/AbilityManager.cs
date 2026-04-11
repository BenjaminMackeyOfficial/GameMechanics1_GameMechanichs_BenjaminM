using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class AbilityManager : MonoBehaviour
{
    
    PlayerController controller;
    [SerializeField] Ability[] AbilityPrefabs;

    private List<Ability> abilities;
    private Ability activeAbility;

    [SerializeField] InputActionAsset inputActions;

    private InputAction next;
    private InputAction prev;

    private InputAction shoot;
    private InputAction aim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        controller = GetComponent<PlayerController>();
        prev = inputActions.FindAction("Player/Previous");
        next = inputActions.FindAction("Player/Next");
        shoot = inputActions.FindAction("Player/Attack");
        aim = inputActions.FindAction("Player/AimIn");

        prev.performed += SwitchAbilityDown;
        next.performed += SwitchAbilityUp;
        shoot.performed += Shoot;

        aim.performed += Aim;
        aim.canceled += Aim;

        abilities = new List<Ability>();
        foreach (Ability item in AbilityPrefabs)
        {
            Ability newItm = Instantiate(item);
            newItm.Initialize(gameObject);
            abilities.Add(newItm);
        }
        activeAbility = abilities[0];
    }

    private void SwitchAbilityUp(InputAction.CallbackContext context)
    {
        int sendPos = 0;
        int currentPos = abilities.IndexOf(activeAbility);
        if (currentPos == abilities.Count - 1) sendPos = 0;
        else sendPos = currentPos + 1;

        ChangeAbility(sendPos);
    }
    private void SwitchAbilityDown(InputAction.CallbackContext context)
    {
        int sendPos = 0;
        int currentPos = abilities.IndexOf(activeAbility);
        if (currentPos == 0) sendPos = abilities.Count - 1;
        else sendPos = currentPos - 1;

        ChangeAbility(sendPos);
    }

    private void ChangeAbility(int changeTo)
    {
        if (_aiming) return;
        activeAbility = abilities[changeTo];
        UI.Instance.ChangeText("Current Ability: " + activeAbility.name.Split("(")[0]);
    }


    //visuals
    private float fov;
    private float zoomFov;
    private float prog;


    private bool _animating;
    
    //
    // Update is called once per frame
    private void Shoot(InputAction.CallbackContext context)
    {
        activeAbility.Excecute();
    }
    private bool _aiming;
    private void Aim(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            controller.ZoomOut();
            activeAbility.Abort();
            _aiming = false;
        }
        else
        {
            controller.ZoomIn();
            _aiming = true;
            activeAbility.Aim();
        }
    }
    private void Start()
    {
        UI.Instance.ChangeText("Current Ability: " + activeAbility.name.Split("(")[0]);
    }
    void Update()
    {
        
        activeAbility.Update();
    }
}
