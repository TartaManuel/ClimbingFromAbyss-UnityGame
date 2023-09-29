using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemScriptLevel3 : MonoBehaviour
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
    void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(lastSelectedObject); // no current selection, go back to last selected
        else
            lastSelectedObject = eventSystem.currentSelectedGameObject;
    }
}
