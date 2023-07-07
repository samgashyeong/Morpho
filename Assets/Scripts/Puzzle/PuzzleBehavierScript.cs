using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleBehavierScript : MonoBehaviour
{
    public Camera camera;
    public GameObject puzzleElement;
    public GameObject parent;
    public GameObject clickElement;
    public Vector2 defaultPosition;
    private Boolean isMove = false;
    private RaycastHit hit;
    void Start()
    {
        var a = Instantiate(puzzleElement, parent.transform);
        a.transform.position = new Vector2(-2, -1.5f);
        Instantiate(puzzleElement, parent.transform);
        Instantiate(puzzleElement, parent.transform);
        Instantiate(puzzleElement, parent.transform);
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y));
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {

                string objectName = hit.collider.gameObject.name;
                clickElement = hit.collider.gameObject;
                clickElement.GetComponent<SpriteRenderer>().color = Color.green;
                defaultPosition = mousePoint;
                Debug.Log(objectName);
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            clickElement.GetComponent<SpriteRenderer>().color = Color.white;
            Vector2 nowPosition = mousePoint;
            StartCoroutine(movePuzzle(nowPosition));
        }
    }

    private IEnumerator movePuzzle(Vector2 nowPosition)
    {
        var x = clickElement.GetComponent<BoxCollider>().size.x;
        var y = clickElement.GetComponent<BoxCollider>().size.y;

        if (x > y)
        {
            Debug.Log("½ÇÇàµÊ");
            if (nowPosition.x - defaultPosition.x > 0)
            {
                Debug.Log(nowPosition.x);
                Debug.Log(defaultPosition.x);
                Debug.Log(defaultPosition.y);
                yield return StartCoroutine(movePuzzleElement(true, true));
            }
            else
            {
                yield return StartCoroutine(movePuzzleElement(true, false));
            }
        }
        else
        {
            if (nowPosition.y - defaultPosition.y > 0)
            {
                yield return StartCoroutine(movePuzzleElement(false, true));
            }
            else
            {
                yield return StartCoroutine(movePuzzleElement(false, false));
            }
        }
        isMove = false;
    }

    private IEnumerator movePuzzleElement(bool isMaxX, bool isPositiveNum)
    {
        Vector2 velo = new Vector2(0, clickElement.transform.position.y);
        if(isMaxX && isPositiveNum)
        {
            Debug.Log("¤©¤§¤¸");
            while (true)
            {
                yield return null;
                clickElement.transform.position = Vector3.Lerp(clickElement.transform.position, new Vector2(5, clickElement.transform.position.y), 10f*Time.deltaTime);
            }
        }
    }



}
