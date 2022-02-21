using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyButton : MonoBehaviour , IPointerDownHandler,IPointerUpHandler 
{
    [HideInInspector]
    public bool Pressed;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    public void OnPointerDown(PointerEventData eventData) //basıldıgı anda zıplama olayları
    {
        Pressed = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }

}
