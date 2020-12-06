using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SortScript : MonoBehaviour
{
    public int NumberOfCubes = 10;
    public int CubeMaxHeight = 10;
    public GameObject[] Cubes;
    public GameObject parentObject;
    public Renderer m_Renderer;
    GameObject max;
    public new Collider collider;
    private float moveSpeed;
    private float dirX, dirY;
    public float elevation;
    public float cameraDistance = 4.0f;
    public Rigidbody rb;

    // Start is called before the first frame update
    public void StartSelectionSort()
    {
        Initializerandom();
        MoveCamera();
        StartCoroutine(SelectionSort(Cubes));
    }

    public void StartBubbleSort()
    {
        Initializerandom();
        MoveCamera();
        StartCoroutine(BubbleSort(Cubes));

    }

    public void StartShellSort()
    {
        Initializerandom();
        MoveCamera();
        StartCoroutine(ShellSort(Cubes));
    }


    public void StartInsertionSort()
    {
        
        Initializerandom();
        MoveCamera();
        StartCoroutine(InsertionSort(Cubes));

        
    }

    void MoveCamera()
    {
        Camera.main.orthographicSize = 10;
        Vector3 objectSizes = collider.bounds.max - collider.bounds.min;
        float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
        float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * Camera.main.fieldOfView); // Visible height 1 meter in front
        float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
        distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object

        Vector3 groupCenter1 = m_Renderer.bounds.center;

        Vector3 sumVector = new Vector3(0f, 0f, 0f);

        foreach (Transform child in parentObject.transform)
        {
            sumVector += child.position;
        }
        Vector3 groupCenter = sumVector / parentObject.transform.childCount;

        Camera.main.transform.position = new Vector3(groupCenter.x, groupCenter.y - (Max(Cubes).transform.position.y), (groupCenter.z - distance - Cubes.Length - (Max(Cubes).transform.position.y)) * 1.35f);
        Camera.main.orthographicSize = Camera.main.orthographicSize + 10;
    }

    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        moveSpeed = 3f;
        rb.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        dirX = Input.GetAxis("Horizontal") * moveSpeed;
        dirY = Input.GetAxis("Vertical") * moveSpeed;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(dirX, dirY, rb.velocity.z);
    }


    GameObject Max(GameObject[] list)
    {
        if(list.Length > 0)
        {
            max = list[0];
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].transform.position.y > max.transform.position.y)
                {
                    max = list[i];
                }
            }
        }

        return max;
    }
    

    void Initializerandom()
    {
        Cubes = new GameObject[NumberOfCubes];

        for (int i = 0; i < Cubes.Length; i++)
        {
            int rand = Random.Range(1, CubeMaxHeight + 1);

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            cube.transform.localScale = new Vector3(0.9f, rand, 1);

            cube.transform.position = new Vector3(i, rand / 2.0f,  0);

            cube.transform.parent = this.transform;
            Cubes[i] = cube;



            GameObject text = new GameObject();
            text.transform.parent = cube.transform;
            text.name = "Text";

            TextMesh t = text.AddComponent<TextMesh>();
            t.text = rand.ToString();
            t.characterSize = 0.03f;
            t.fontSize = 250;

            
            t.anchor = TextAnchor.MiddleCenter;
            t.alignment = TextAlignment.Center;
            t.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y, cube.transform.position.z);
        }

        transform.position = new Vector3(-NumberOfCubes, -NumberOfCubes, -NumberOfCubes);
    }

    IEnumerator InsertionSort(GameObject[] list)
    {
        int j = 0;
        GameObject temp;
        Vector3 tempPosition;

        for (int i = 1; i < list.Length; i++)
        {
            j = i;
            temp = list[i];
            while(j > 0 && list[j-1].transform.localScale.y > temp.transform.localScale.y)
            {
                tempPosition = temp.transform.localPosition;

                

                if (list[j - 1] != temp)
                {
                    LeanTween.color(temp, Color.red, 1f);
                    LeanTween.color(list[j - 1], Color.red, 1f);
                    yield return new WaitForSeconds(1);
                    LeanTween.color(temp, Color.white, 1f);
                    LeanTween.color(list[j - 1], Color.white, 1f);

                    LeanTween.moveLocalX((temp), list[j - 1].transform.localPosition.x, 0.5f);

                    LeanTween.moveLocalZ(temp, -3, .5f).setLoopPingPong(1);

                    LeanTween.moveLocalX((list[j - 1]), tempPosition.x, 0.5f);

                    LeanTween.moveLocalZ(list[j - 1], 3, .5f).setLoopPingPong(1);
                    
                }
                yield return new WaitForSeconds(1);
                list[j] = list[j - 1];

                j--;
            }
            list[j] = temp;
        }
        StartCoroutine(ColorCube(list));
    }

    IEnumerator ShellSort(GameObject[] list)
    {
        int lastIndex = list.Length - 1;
        int gap = list.Length / 2;
        GameObject temp;
        Vector3 tempPosition;
        bool exchanges;
        do
        {
            do
            {
                exchanges = false;
                for (int i = 0; i <= lastIndex - gap; i++)
                {
                    if (list[i].transform.localScale.y
                    > list[i + gap].transform.localScale.y)
                    {
                        temp = list[i];
                        list[i] = list[i + gap];
                        list[i + gap] = temp;

                        LeanTween.color(list[i], Color.red, 1f);
                        LeanTween.color(list[i + gap], Color.red, 1f);
                        yield return new WaitForSeconds(2);
                        LeanTween.color(list[i], Color.white, 1f);
                        LeanTween.color(list[i + gap], Color.white, 1f);

                        tempPosition = list[i].transform.localPosition;

                        if (!(list[i] == list[i + gap]))
                        {
                            LeanTween.moveLocalX((list[i]), list[i + gap].transform.localPosition.x, 1);

                            LeanTween.moveLocalZ(list[i], -3, .5f).setLoopPingPong(1);

                            LeanTween.moveLocalX((list[i + gap]), tempPosition.x, 1);

                            LeanTween.moveLocalZ(list[i + gap], 3, .5f).setLoopPingPong(1);
                        }
                        exchanges = true;
                    }
                }
            } while (exchanges);
            gap = gap / 2;
        } while (gap > 0);
        StartCoroutine(ColorCube(list));
    }

    IEnumerator SelectionSort(GameObject[] list)
    {
        int min;
        GameObject temp;

        Vector3  tempPosition;

        for (int i = 0; i < list.Length; i++)
        {
            min = i;
            for (int j = i + 1; j < list.Length; j++)
            {
                if (list[j].transform.localScale.y
                    < list[min].transform.localScale.y)
                {
                    min = j;
                }
            }

            if (min != 1)
            {
                temp = list[i];
                list[i] = list[min];
                list[min] = temp;

                LeanTween.color(list[i], Color.red, 1f);
                LeanTween.color(list[min], Color.red, 1f);
                yield return new WaitForSeconds(2);
                LeanTween.color(list[i], Color.white, 1f);
                LeanTween.color(list[min], Color.white, 1f);

                tempPosition = list[i].transform.localPosition;

                if(!(list[i] == list[min]))
                {
                    LeanTween.moveLocalX((list[i]), list[min].transform.localPosition.x, 1);

                    LeanTween.moveLocalZ(list[i], -3, .5f).setLoopPingPong(1);

                    LeanTween.moveLocalX((list[min]), tempPosition.x, 1);

                    LeanTween.moveLocalZ(list[min], 3, .5f).setLoopPingPong(1);
                }

            }

            LeanTween.color(list[i], Color.green, 1f);
        }
    }

    IEnumerator BubbleSort(GameObject[] list)
    {
        Vector3 tempPosition;
        GameObject temp = null;
        bool swapMode = true;
        int numOfComparisions = list.Length - 1;
        while(swapMode)
        {
            swapMode = false;
            for(int i = 0; i < numOfComparisions; i++)
            {
                if(list[i].transform.localScale.y
                    > list[i+1].transform.localScale.y)
                {
                    temp = list[i];
                    list[i] = list[i+1];
                    list[i+1] = temp;

                    LeanTween.color(list[i], Color.red, 1f);
                    LeanTween.color(list[i+1], Color.red, 1f);
                    yield return new WaitForSeconds(1.5f);
                    LeanTween.color(list[i], Color.white, 1f);
                    LeanTween.color(list[i+1], Color.white, 1f);

                    tempPosition = list[i].transform.localPosition;

                    if (!(list[i] == list[i+1]))
                    {
                        LeanTween.moveLocalX((list[i]), list[i+1].transform.localPosition.x, 1);

                        LeanTween.moveLocalZ(list[i], -3, .5f).setLoopPingPong(1);

                        LeanTween.moveLocalX((list[i+1]), tempPosition.x, 1);

                        LeanTween.moveLocalZ(list[i+1], 3, .5f).setLoopPingPong(1);
                    }
                    swapMode = true;
                }   
            }
            numOfComparisions--;         
        }
        StartCoroutine(ColorCube(list));
    }

    IEnumerator ColorCube(GameObject[] list)
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < list.Length; i++)
        {
            
            LeanTween.moveLocalZ(list[i], -1, .5f).setLoopPingPong(1);
            LeanTween.color(list[i], Color.green, 0.5f);
        }
    }
    
}
