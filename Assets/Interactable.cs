using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private GameObject modelBody;
    [SerializeField]
    private GameObject mainObject;
    [SerializeField]
    private string offClickAnimation = "";

    private bool Disabled = false;

    // Update is called once per frame
    void Update() {
        if (Disabled) return;
        // Play animation when mouse off.
        if (Input.GetMouseButtonUp(0)) {
            RaycastHit hit;
            GameObject target = GetClickedObject(out hit);
            if (target != null && target == modelBody) {
                Debug.Log("Clicked on the modelBody!");
                mainObject.GetComponent<Animator>().Play(offClickAnimation);
            } else {
                Debug.Log("Clicked on something else!");
            }
        }
    }

    public void Disable() {
        Disabled = true;
    }

    public void Enabled() {
        Disabled = false;
    }

    private GameObject GetClickedObject(out RaycastHit hit) {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit)) {
            if (hit.collider != null)
                target = hit.collider.gameObject;
        }
        return target;
    }

    private bool isPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
