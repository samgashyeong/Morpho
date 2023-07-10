using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleBehavierScript : MonoBehaviour
{
    public Camera camera;
    public GameObject parent;
    public GameObject clickElement;
    public Vector2 defaultPosition;
    private Boolean isMove = false;
    private RaycastHit hit;


    public GameObject element2x1;
    public GameObject element3x1;
    public GameObject element1x2;
    public GameObject element1x3;


    private int[,] puzzle = new int[6, 6]
    { { 0, 0, 0, 3, 0, 0 },
      { 2, 2, 2, 3, 4, 0 },
      { 4, 1, 1, 0, 4, 3 },
      { 4, 0, 0, 0, 4, 3 },
      { 4, 0, 0, 0, 1, 1 },
      { 0, 0, 1, 1, 0, 0 }, };

    public PuzzleBehavierScript()
    {
    }

    void Start()
    {
        SetPuzzle();
    }

    private void SetPuzzle()
    {
        for (int i = 0; i < 6; i++)
        {
            int num = 0;
            int previousElement = -1;
            for (int j = 0; j < 6; j++)
            {
                if (puzzle[i, j] == 1 || puzzle[i, j] == 2 || puzzle[i, j] == previousElement)
                {
                    previousElement = puzzle[i, j];
                    num++;
                    if (num == 2 && previousElement == 1)
                    {
                        Debug.Log("2x1배치" + i + " " + j);
                        InstantiateElement(new Vector2(j - 4, returnPositionY(i)), element2x1);
                        num = 0;
                    }
                    else if (num == 3 && previousElement == 2)
                    {
                        Debug.Log("3x1배치" + i + " " + j);
                        InstantiateElement(new Vector2(j - 5, returnPositionY(i)), element3x1);
                        num = 0;
                    }
                }
            }
        }

        for (int i = 0; i < 6; i++)
        {
            int num = 0;
            int previousElement = -1;
            for (int j = 0; j < 6; j++)
            {
                if (puzzle[j, i] == 3 || puzzle[j, i] == 4 || puzzle[j, i] == previousElement)
                {
                    previousElement = puzzle[j, i];
                    num++;
                    if (num == 2 && previousElement == 3)
                    {
                        Debug.Log("1x2배치" + j + " " + i);
                        InstantiateElement(new Vector2(i - 3, returnPositionY(j)), element1x2);
                        num = 0;
                    }
                    else if (num == 3 && previousElement == 4)
                    {
                        Debug.Log("3x1배치" + i + " " + j);
                        InstantiateElement(new Vector2(i - 3, returnPositionY(j)), element1x3);
                        num = 0;
                    }
                }
            }
        }
    }

    private float returnPositionY(int j)
    {
        if (j > 0)
        {
            return j - (j + (j-2));
        }
        else
        {
            return j - (j - 2);
        }
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
            Debug.Log("실행됨");
            if (nowPosition.x - defaultPosition.x > 1)
            {
                Debug.Log(nowPosition.x);
                Debug.Log(defaultPosition.x);
                Debug.Log(defaultPosition.y);
                yield return StartCoroutine(movePuzzleElement(true, true));
            }
            else if(nowPosition.x - defaultPosition.x < -1)
            {
                yield return StartCoroutine(movePuzzleElement(true, false));
            }
        }
        else
        {
            if (nowPosition.y - defaultPosition.y > 1)
            {
                yield return StartCoroutine(movePuzzleElement(false, true));
            }
            else if (nowPosition.y - defaultPosition.y < -1)
            {
                yield return StartCoroutine(movePuzzleElement(false, false));
            }
        }
        isMove = false;
    }

    private IEnumerator movePuzzleElement(bool isMaxX, bool isPositiveNum)
    {
        float elapsedTime = 0.0f;
        float lerpTime = 0.2f;
        Vector2 velo = new Vector2(0, clickElement.transform.position.y);
        if(isMaxX && isPositiveNum && !isMove)
        {
            while (elapsedTime < lerpTime)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
                isMove=true;
                clickElement.transform.position = Vector3.Lerp(clickElement.transform.position, new Vector2(5, clickElement.transform.position.y), elapsedTime/lerpTime);
            }
        }
        else if(isMaxX && !isPositiveNum && !isMove)
        {

        }
        else if(!isMaxX && isPositiveNum && !isMove)
        {

        }
        else
        {

        }
    }

    private void InstantiateElement(Vector2 position, GameObject element)
    {
        var puzzleElement = Instantiate(element, parent.transform);
        puzzleElement.transform.position = position;
    }


}
