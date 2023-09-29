using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleScreenEventSystemScript : MonoBehaviour
{
    //partea de EventSystem din obiectul EventSystem din Unity
    public EventSystem eventSystem;
    //ultimul obiect pe care l-am avut selectat, butonul
    private GameObject lastSelectedObject;

    // Start is called before the first frame update
    void Start()
    {
        //daca cumva nu am pus event system din unity, se pune aici
        if (eventSystem == null)
            eventSystem = gameObject.GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //aici prima data cand o sa intre, o sa seteze lastSelected ca ultimul element selectat, ultimul buton care o fost
        //selectat, si dupa oricand nu avem niciun buton selectat se reselecteaza
        if (eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(lastSelectedObject);
        else
            lastSelectedObject = eventSystem.currentSelectedGameObject;
    }
}
