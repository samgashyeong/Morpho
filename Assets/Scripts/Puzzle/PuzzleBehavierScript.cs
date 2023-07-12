using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PuzzleBehavierScript : MonoBehaviour
{
    public Camera camera;
    public GameObject parent;
    public GameObject clickElement;
    public Text dragNumText;
    public GameObject successText;
    public Vector2 defaultPosition;
    private Boolean isMove = false;
    private Boolean isComplete = false;
    private RaycastHit hit;

    public int dragNum = 100;

    public GameObject element2x1;
    public GameObject element3x1;
    public GameObject element1x2;
    public GameObject element1x3;
    public GameObject element99;


    public int[,] puzzle = new int[6, 6]
    { { 3, 1, 1, 4, 0, 0 },
      { 3, 0, 3, 4, 0, 0 },
      { 9, 9, 3, 4, 3, 3 },
      { 0, 2, 2, 2, 3, 3 },
      { 0, 0, 0, 0, 3, 3 },
      { 0, 1, 1, 0, 3, 3 }, };


    void Start()
    {
        SetPuzzle();
    }

    private void SetPuzzle()
    {
        successText.SetActive(false);
        for (int i = 0; i < 6; i++)
        {
            int num = 0;
            int previousElement = -1;
            for (int j = 0; j < 6; j++)
            {
                if (puzzle[i, j] == 1 || puzzle[i, j] == 2 || puzzle[i, j] == 9 || puzzle[i, j] == previousElement)
                {
                    previousElement = puzzle[i, j];
                    num++;
                    if (num == 2 && previousElement == 1)
                    {
                        Debug.Log("2x1배치" + i + " " + j);
                        InstantiateElement(new Vector2(j - 4, returnPositionY(i)), element2x1);
                        num = 0;
                    }
                    else if(num == 2 && previousElement == 9)
                    {
                        Debug.Log("메인 블럭배치" + i + " " + j);
                        InstantiateElement(new Vector2(j - 4, returnPositionY(i)), element99);
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
            return j - (j + (j - 2));
        }
        else
        {
            return j - (j - 2);
        }
    }

    private int returnArrayPostionY(float j)
    {
        if (j < 1)
        {
            Debug.Log((int)Math.Ceiling((-1 * j) + 2));
            return (int)Math.Ceiling((-1 * j) + 2);
        }
        else if (j == 1){
            return 1;
        }
        else if (j == 2)
        {
            return 0;
        }
        else
        {
            return -1;
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
        clickElement = null;
        if (isComplete)
        {
            Debug.Log("Success");
            successText.SetActive(true);
        }
    }

    private IEnumerator movePuzzleElement(bool isMaxX, bool isPositiveNum)
    {
        float elapsedTime = 0.0f;
        float lerpTime = 0.1f;
        var destX = -1;
        var x = clickElement.GetComponent<BoxCollider>().transform.parent.transform.position.x;
        var y = clickElement.GetComponent<BoxCollider>().transform.parent.transform.position.y;
        Vector2 velo = new Vector2(0, clickElement.transform.position.y);
        if (isMaxX && !isMove)
        {
            if (isPositiveNum)
            {
                if(clickElement.gameObject.name == "99")
                {
                    destX = 3;
                    Debug.Log("위치 x y " + x + ' ' + y);
                    Debug.Log("배열 위치 x y" + (x + 3) + ' ' + returnArrayPostionY(y));
                    int arrX = (int)(x + 4);
                    int arrY = returnArrayPostionY(y);
                    puzzle[arrY, arrX] = 0; puzzle[arrY, arrX - 1] = 0;
                    if (arrX == 5) { puzzle[arrY, arrX] = 9; puzzle[arrY, arrX - 1] = 9; }
                    else
                    {
                        for (int i = arrX + 1; i < 6; i++)
                        {
                            if (puzzle[arrY, i] != 0)
                            {
                                destX = i - 5;
                                puzzle[arrY, i - 1] = 9; puzzle[arrY, i - 2] = 9;
                                break;
                            }
                            else if (i == 5)
                            {
                                puzzle[arrY, i] = 9; puzzle[arrY, i - 1] = 9;
                            }
                        }
                    }

                    if (destX == 3)
                    {
                        isComplete = true;
                    }

                    puzzleDebug();
                    Debug.Log("destX =  " + destX);
                }
                if (clickElement.gameObject.name == "2x1" )
                {
                    destX = 1;
                    Debug.Log("위치 x y " + x + ' ' + y);
                    Debug.Log("배열 위치 x y" + (x + 3) + ' ' + returnArrayPostionY(y));
                    int arrX = (int)(x + 4);
                    int arrY = returnArrayPostionY(y);
                    puzzle[arrY, arrX] = 0; puzzle[arrY, arrX - 1] = 0;
                    if(arrX == 5) { puzzle[arrY, arrX] = 1; puzzle[arrY, arrX - 1] = 1; }
                    else
                    {
                        for (int i = arrX + 1; i < 6; i++)
                        {
                            if (puzzle[arrY, i] != 0)
                            {
                                destX = i - 5;
                                puzzle[arrY, i - 1] = 1; puzzle[arrY, i - 2] = 1;
                                break;
                            }
                            else if (i == 5)
                            {
                                puzzle[arrY, i] = 1; puzzle[arrY, i - 1] = 1;
                            }
                        }
                    }
                    
                    puzzleDebug();
                    Debug.Log("destX =  " + destX);

                }
                else if(clickElement.gameObject.name == "3x1")
                {
                    destX = 0;
                    Debug.Log("위치 x y " + x + ' ' + y);
                    Debug.Log("배열 위치 x y" + (x + 3) + ' ' + returnArrayPostionY(y));
                    int arrX = (int)(x + 5);
                    int arrY = returnArrayPostionY(y);
                    Debug.Log("arrY = " + arrY);
                    puzzle[arrY, arrX] = 0; puzzle[arrY, arrX - 1] = 0; puzzle[arrY, arrX - 2] = 0;
                    if(arrX == 5) { puzzle[arrY, arrX] = 2; puzzle[arrY, arrX - 1] = 2; puzzle[arrY, arrX - 2] = 2; }
                    else
                    {
                        for (int i = arrX + 1; i < 6; i++)
                        {
                            if (puzzle[arrY, i] != 0)
                            {
                                destX = i - 6;
                                Debug.Log("3x1 퍼즐이동");
                                puzzle[arrY, i - 1] = 2; puzzle[arrY, i - 2] = 2; puzzle[arrY, i - 3] = 2;
                                break;
                            }
                            else if (i == 5)
                            {
                                puzzle[arrY, i] = 2; puzzle[arrY, i - 1] = 2; puzzle[arrY, i - 2] = 2;
                            }
                        }
                    }
                    
                    puzzleDebug();
                    Debug.Log("destX =  " + destX);
                }
            }
            else
            {
                destX = -3;
                if(clickElement.gameObject.name == "99")
                {
                    Debug.Log("위치 x y " + x + ' ' + y);
                    Debug.Log("배열 위치 x y" + (x) + ' ' + returnArrayPostionY(y));
                    int arrX = (int)Math.Ceiling(x + 3);
                    int arrY = returnArrayPostionY(y);
                    puzzle[arrY, arrX] = 0; puzzle[arrY, arrX + 1] = 0;
                    if (arrX == 0) { puzzle[arrY, arrX] = 9; puzzle[arrY, arrX + 1] = 9; }
                    else
                    {
                        for (int i = arrX - 1; i >= 0; i--)
                        {
                            if (puzzle[arrY, i] != 0)
                            {
                                Debug.Log("i =  " + i);
                                destX = i - 2;
                                puzzle[arrY, i + 1] = 9; puzzle[arrY, i + 2] = 9;
                                break;
                            }
                            else if (i == 0 || i == -1)
                            {
                                puzzle[arrY, i] = 9; puzzle[arrY, i + 1] = 9;
                            }
                        }
                    }
                    puzzleDebug();
                    Debug.Log("destX =  " + destX);
                }
                else if (clickElement.gameObject.name == "2x1")
                {
                    Debug.Log("위치 x y " + x + ' ' + y);
                    Debug.Log("배열 위치 x y" + (x) + ' ' + returnArrayPostionY(y));
                    int arrX = (int)Math.Ceiling(x + 3);
                    int arrY = returnArrayPostionY(y);
                    puzzle[arrY, arrX] = 0; puzzle[arrY, arrX+1] = 0;
                    if (arrX == 0) { puzzle[arrY, arrX] = 1; puzzle[arrY, arrX + 1] = 1; }
                    else
                    {
                        for (int i = arrX - 1; i >= 0; i--)
                        {
                            if (puzzle[arrY, i] != 0)
                            {
                                Debug.Log("i =  " + i);
                                destX = i - 2;
                                puzzle[arrY, i + 1] = 1; puzzle[arrY, i + 2] = 1;
                                break;
                            }
                            else if (i == 0 || i == -1)
                            {
                                puzzle[arrY, i] = 1; puzzle[arrY, i + 1] = 1;
                            }
                        }
                    }

                    puzzleDebug();
                    Debug.Log("destX =  " + destX);

                }
                else if (clickElement.gameObject.name == "3x1")
                {
                    destX = -3;
                    Debug.Log("위치 x y " + x + ' ' + y);
                    Debug.Log("배열 위치 x y" + (x+3) + ' ' + returnArrayPostionY(y));
                    int arrX = (int)Math.Ceiling(x + 3);
                    int arrY = returnArrayPostionY(y);
                    puzzle[arrY, arrX] = 0; puzzle[arrY, arrX + 1] = 0; puzzle[arrY, arrX + 2] = 0;
                    if(arrX == 0) { puzzle[arrY, arrX] = 2; puzzle[arrY, arrX+1] = 2; puzzle[arrY, arrX+2] = 2; }
                    else
                    {
                        for (int i = arrX - 1; i >= 0; i--)
                        {
                            if (puzzle[arrY, i] != 0)
                            {
                                Debug.Log("i =  " + i);
                                destX = i - 2;
                                puzzle[arrY, i + 1] = 2; puzzle[arrY, i + 2] = 2; puzzle[arrY, i + 3] = 2;
                                break;
                            }
                            else if (i == 0)
                            {
                                puzzle[arrY, i] = 2; puzzle[arrY, i + 1] = 2; puzzle[arrY, i + 2] = 2;
                            }
                        }
                    }
                    
                    puzzleDebug();
                    Debug.Log("destX =  " + destX);
                }
            }
            while (elapsedTime < lerpTime)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
                isMove = true;
                clickElement.transform.parent.transform.position = Vector3.Lerp(clickElement.transform.parent.position, new Vector2(destX, clickElement.transform.parent.position.y), elapsedTime / lerpTime);
                
            }
            dragNumText.text = (--dragNum).ToString();
        }

        //Y로 이동할때
        else if (!isMaxX && !isMove)
        {
            int destY = -3;
            if (isPositiveNum)
            {
                if(clickElement.gameObject.name == "1x2")
                {
                    destY = 1;
                    Debug.Log("위치 x y " + x + ' ' + y);
                    Debug.Log("배열 위치 x y" + (x+3) + ' ' + returnArrayPostionY(y));
                    int arrX = (int)Math.Ceiling(x + 3);
                    int arrY = returnArrayPostionY(y)-1;
                    puzzle[arrY, arrX] = 0; puzzle[arrY+1, arrX] = 0;
                    if(arrY == 0) { puzzle[arrY, arrX] = 3; puzzle[arrY+1, arrX] = 3; }
                    for (int i = arrY - 1; i >= 0; i--)
                    {
                        if (puzzle[i, arrX] != 0)
                        {
                            Debug.Log("i =  " + i);
                            destY = (int)returnPositionY(i) - 2;
                            puzzle[i+1, arrX] = 3; puzzle[i+2, arrX] = 3;
                            break;
                        }
                        else if (i == 0)
                        {
                            puzzle[i, arrX] = 3; puzzle[i+1, arrX] = 3;
                        }
                    }

                    puzzleDebug();
                    Debug.Log("destY123 =  " + destY);
                }
                else
                {
                    destY = 0;
                    Debug.Log("위치 x y " + x + ' ' + y);
                    Debug.Log("배열 위치 x y" + (x + 3) + ' ' + returnArrayPostionY(y));
                    int arrX = (int)Math.Ceiling(x+3);
                    Debug.Log("arrX = " + arrX);
                    int arrY = returnArrayPostionY(y) - 2;
                    puzzle[arrY, arrX] = 0; puzzle[arrY + 1, arrX] = 0; puzzle[arrY + 2, arrX] = 0;
                    if (arrY == 0) { puzzle[arrY, arrX] = 4; puzzle[arrY + 1, arrX] = 4; puzzle[arrY + 2, arrX] = 4; }
                    else
                    {
                        for (int i = arrY - 1; i >= 0; i--)
                        {
                            if (puzzle[i, arrX] != 0)
                            {
                                Debug.Log("arrx" + puzzle[i, arrX] + ' ' + i + ' ' + arrX);
                                
                                destY = (int)returnPositionY(i)-3;
                                puzzle[i + 1, arrX] = 4; puzzle[i + 2, arrX] = 4; puzzle[i + 3, arrX] = 4;
                                break;
                            }
                            else if (i == 0)
                            {
                                puzzle[i, arrX] = 4; puzzle[i + 1, arrX] = 4; puzzle[i + 2, arrX] = 4;
                            }
                        }

                    }
                    puzzleDebug();
                    Debug.Log("destX =  " + destX);
                }
            }
            else
            {
                if (clickElement.gameObject.name == "1x2")
                {
                    Debug.Log("위치 x y " + x + ' ' + y);
                    Debug.Log("배열 위치 x y" + (x + 3) + ' ' + returnArrayPostionY(y));
                    int arrX = (int)Math.Ceiling(x + 3);
                    int arrY = returnArrayPostionY(y);
                    puzzle[arrY, arrX] = 0; puzzle[arrY - 1, arrX] = 0;
                    if (arrY == 0) { puzzle[arrY, arrX] = 3; puzzle[arrY - 1, arrX] = 3; }
                    for (int i = arrY + 1; i < 6; i++)
                    {
                        if (puzzle[i, arrX] != 0)
                        {
                            Debug.Log("i =  " + i);
                            destY = (int)returnPositionY(i)+1;
                            puzzle[i - 1, arrX] = 3; puzzle[i - 2, arrX] = 3;
                            break;
                        }
                        else if (i == 5)
                        {
                            puzzle[i, arrX] = 3; puzzle[i - 1, arrX] = 3;
                        }
                    }

                    puzzleDebug();
                    Debug.Log("destY =  " + destY);
                }
                else
                {
                    Debug.Log("위치 x y " + x + ' ' + y);
                    Debug.Log("배열 위치 x y" + (x + 3) + ' ' + returnArrayPostionY(y));
                    int arrX = (int)Math.Ceiling(x + 3);
                    int arrY = returnArrayPostionY(y);
                    puzzle[arrY, arrX] = 0; puzzle[arrY - 1, arrX] = 0; puzzle[arrY - 2, arrX] = 0;
                    if (arrY == 0) { puzzle[arrY, arrX] = 4; puzzle[arrY - 1, arrX] = 4; puzzle[arrY - 2, arrX] = 4; }
                    for (int i = arrY + 1; i < 6; i++)
                    {
                        if (puzzle[i, arrX] != 0)
                        {
                            Debug.Log("i =  " + i);
                            destY = (int)returnPositionY(i) + 1;
                            puzzle[i - 1, arrX] = 4; puzzle[i - 2, arrX] = 4; puzzle[i - 3, arrX] = 4;
                            break;
                        }
                        else if (i == 5)
                        {
                            puzzle[i, arrX] = 4; puzzle[i - 1, arrX] = 4; puzzle[i - 2, arrX] = 4;
                        }
                    }

                    puzzleDebug();
                    Debug.Log("destY =  " + destY);
                }
            }
            while (elapsedTime < lerpTime)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
                isMove = true;
                clickElement.transform.parent.transform.position = Vector3.Lerp(clickElement.transform.parent.position, new Vector2(clickElement.transform.parent.position.x, destY), elapsedTime / lerpTime);
            }
            dragNumText.text = (--dragNum).ToString();
        }
    }

    private void puzzleDebug()
    {
        for(int i = 0; i < 6; ++i)
        {
            Debug.Log(puzzle[i, 0] + ", " + puzzle[i, 1] + ", " + puzzle[i, 2] + ", " + puzzle[i, 3] + ", " + puzzle[i, 4] + ", " + puzzle[i, 5] + ", " + "\n");
            
        }
    }

    private void InstantiateElement(Vector2 position, GameObject element)
    {
        var puzzleElement = Instantiate(element, parent.transform);
        puzzleElement.transform.position = position;
    }


}
