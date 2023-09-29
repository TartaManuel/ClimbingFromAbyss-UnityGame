using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemLevel2 : MonoBehaviour
{
    public EventSystem eventSystem;
    private GameObject lastSelectedObject;


    // Start is called before the first frame update
    void Start()
    {
        if (eventSystem == null)
            eventSystem = gameObject.GetComponent<EventSystem>();
    }

    // Update is called once per frame
    //aici ma asigur ca tot timpul este selectat un buton, chiar daca jucatorul da click in afara, pe restul paginii
    void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(lastSelectedObject);
        else
            lastSelectedObject = eventSystem.currentSelectedGameObject;

    }
}
