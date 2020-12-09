using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SortScript : MonoBehaviour
{
    //Attributes
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

    //Initialization of Sort Algorithms
    public void StartSelectionSort()
    {
        //Generate a Random List
        Initializerandom();
        //Call Object Move to Camera
        MoveCamera();
        StartCoroutine(SelectionSort(Cubes));
    }

    public void StartBubbleSort()
    {
        //Generate a Random List
        Initializerandom();
        //Call Object Move to Camera
        MoveCamera();
        StartCoroutine(BubbleSort(Cubes));

    }

    public void StartShellSort()
    {
        //Generate a Random List
        Initializerandom();
        //Call Object Move to Camera
        MoveCamera();
        StartCoroutine(ShellSort(Cubes));
    }


    public void StartInsertionSort()
    {
        //Generate a Random List
        Initializerandom();
        //Call Object Move to Camera
        MoveCamera();
        StartCoroutine(InsertionSort(Cubes)); 
    }

    public IEnumerator StartQuickSort()
    {
        //Generate a Random List
        Initializerandom();
        //Call Object Move to Camera
        MoveCamera();
        yield return QuickSort(Cubes, 0, Cubes.Length - 1);
        yield return ColorCube(Cubes);
    }

    public IEnumerator StartHeapSort()
    {
        //Generate a Random List
        Initializerandom();
        //Call Object Move to Camera
        MoveCamera();
        yield return HeapSort(Cubes);
        
    }

    //Move Camera to Object
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

    //Get max val from the list
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
    
    //Generate a Randomized List
    void Initializerandom()
    {
        Cubes = new GameObject[NumberOfCubes];

        for (int i = 0; i < Cubes.Length; i++)
        {
            int rand = UnityEngine.Random.Range(1, CubeMaxHeight + 1);

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

    //Insertion Sort -
    //The Insertion sort algorithm views the data in two halves.
    //The left half of sorted elements and the right half of elements to be sorted.
    //In each iteration, one element from the right half is taken and added to the left half so that the left half is still sorted.
    //Insertion sort is of order O(n2)
    //Insertion sort takes an element from the list and places it in the correct location in the list.
    //This process is repeated until there are no more unsorted items in the list.
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
                    yield return new WaitForSeconds(1);

                }
                
                list[j] = list[j - 1];

                j--;
            }
            list[j] = temp;
        }
        StartCoroutine(ColorCube(list));
    }

    //Shell Sort -
    //Donald Shell published the first version of this sort, hence this is known as Shell sort.
    //This sorting is a generalization of insertion sort that allows the exchange of items that are far apart
    //It starts by comparing elements that are far apart and gradually reduces the gap between elements being compared.
    //The running time of Shell sort varies depending on the gap sequence it uses to sort the elements.
    IEnumerator ShellSort(GameObject[] list)
    {
        int lastIndex = list.Length - 1;
        int gap = list.Length / 2;

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
                        yield return Swap(list, i, i + gap);
                        exchanges = true;
                    }
                }
            } while (exchanges);
            gap = gap / 2;
        } while (gap > 0);
        StartCoroutine(ColorCube(list));
    }

    //Selection Sort -
    //Selection sort is an algorithm of sorting an array where it loop from the start of the loop, and check through other elements to find the minimum value.
    //After the end of the first iteration, the minimum value is swapped with the current element.The iteration then continues from the 2nd element and so on.
    IEnumerator SelectionSort(GameObject[] list)
    {
        int min;

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
                yield return Swap(list, i, min);
            }
            yield return LeanTween.color(list[i], Color.green, 1f);
        }
    }

    //Bubble Sort -
    //Bubble sort changes the potion of numbers or changing an unordered sequence into an ordered sequence.
    //Bubble sort follows a simple logic.It compares adjacent elements in a loop and swaps them if they are not in order.
    //Bubble sort is named this way because, in this sorting method, the smaller elements gradually bubble up to the top of the list.
    //Bubble sort has worst-case and average complexity both О(n2), where n is the number of items being sorted.
    IEnumerator BubbleSort(GameObject[] list)
    {
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
                    yield return Swap(list, i, i + 1);
                    swapMode = true;
                }   
            }
            numOfComparisions--;         
        }
        yield return ColorCube(list);
    }


    //Quick Sort -
    //Quick sort is a divide and conquer algorithm.Here Quick sort first divides a large array into two smaller sub-array: the low elements and the high elements.Quicksort can then recursively sort the sub-arrays
    IEnumerator Partition(GameObject[] list, int left, int right)
    {
        GameObject pivot = list[right];

        int i = left - 1;

        for (int j = left; j <= right - 1; j++)
        {
            if (list[j].transform.position.y <= pivot.transform.position.y)
            {
                i++;
                yield return Swap(list, i, j);

            }
        }
        yield return Swap(list, i + 1, right);
        yield return QuickSortCallback(list, left, right, i + 1);
    }

    IEnumerator QuickSortCallback(GameObject[] list, int left, int right, int pivot)
    {
        yield return QuickSort(list, left, pivot - 1);
        yield return QuickSort(list, pivot + 1, right);
    }

    IEnumerator QuickSort(GameObject[] list, int left, int right)
    {
        if (left < right)
        {
            yield return Partition(list, left, right);
        }
    }


    //Heap Sort -
    //it divides its input into a sorted and an unsorted region, and it iteratively shrinks the unsorted region by extracting the largest element and moving that to the sorted region
    //It first removes the topmost item(the largest) and replace it with the rightmost leaf.
    //The topmost item is stored in an array and Re-establish the heap.this is done until there are no more items left in the heap.
    IEnumerator HeapSort(GameObject[] list)
    {
        var length = list.Length;
        for (int i = length/2 - 1; i >= 0; i--)
        {
            yield return Adjust(list, length, i);
        }
        for(int i = length - 1; i >= 0; i--)
        {
            yield return Swap(list, 0, i);
            yield return Adjust(list, i, 0);
        }
        yield return ColorCube(Cubes);
    }

    IEnumerator Adjust(GameObject[] list, int length, int i)
    {
        int largest = i;
        int left = 2 * i + 1;
        int right = 2 * i + 2;
        if(left < length && list[left].transform.position.y > list[largest].transform.position.y)
        {
            largest = left;
        }
        if(right < length && list[right].transform.position.y > list[largest].transform.position.y)
        {
            largest = right;
        }
        if(largest != i)
        {
            yield return Swap(list, i, largest);
            yield return Adjust(list, length, largest);
        }
    }


  //Swaps 2 Values from the Given List
  IEnumerator Swap(GameObject[] list, int i, int j)
    {
        GameObject temp;
        Vector3 tempPosition;

        temp = list[i];
        list[i] = list[j];
        list[j] = temp;

        LeanTween.color(list[i], Color.red, 1f);
        LeanTween.color(list[j], Color.red, 1f);
        yield return new WaitForSeconds(1f);
        LeanTween.color(list[i], Color.white, 1f);
        LeanTween.color(list[j], Color.white, 1f);

        tempPosition = list[i].transform.localPosition;


        if (!(list[i] == list[j]))
        {
            LeanTween.moveLocalX((list[i]), list[j].transform.localPosition.x, 1);

            LeanTween.moveLocalZ(list[i], -3, .5f).setLoopPingPong(1);

            LeanTween.moveLocalX((list[j]), tempPosition.x, 1);

            LeanTween.moveLocalZ(list[j], 3, .5f).setLoopPingPong(1);
            yield return new WaitForSeconds(1f);
        }
            
    }

    //Colors Values from the List
    IEnumerator ColorCube(GameObject[] list)
    {
        
        for (int i = 0; i < list.Length; i++)
        {
            yield return new WaitForSeconds(1);

            LeanTween.moveLocalZ(list[i], -1, 0.5f).setLoopPingPong(1);

            LeanTween.color(list[i], Color.green, 0.5f);
        }
    }
    
}














































//Disregard


////IEnumerator Partition(GameObject[] list, int left, int right, Action<int> onComplete)
////{
////    GameObject pivot = list[right];

////    //GameObject temp;

////    int i = left - 1;

////    for (int j = left; j <= right - 1; j++)
////    {
////        if (list[j].transform.position.y <= pivot.transform.position.y)
////        {
////            i++;
////            yield return Swap(list, i, j);

////        }
////    }
////    yield return Swap(list, i + 1, right);
////    onComplete.Invoke(i + 1);
////}

////IEnumerator Swap(GameObject[] list, int i, int j)
////{
////    GameObject temp;
////    Vector3 tempPosition;

////    temp = list[i];
////    list[i] = list[j];
////    list[j] = temp;

////    LeanTween.color(list[i], Color.red, 1f);
////    LeanTween.color(list[j], Color.red, 1f);
////    yield return new WaitForSeconds(1.5f);
////    LeanTween.color(list[i], Color.white, 1f);
////    LeanTween.color(list[j], Color.white, 1f);

////    tempPosition = list[i].transform.localPosition;

////    LeanTween.moveLocalX((list[i]), list[j].transform.localPosition.x, 1);

////    LeanTween.moveLocalZ(list[i], -3, .5f).setLoopPingPong(1);

////    LeanTween.moveLocalX((list[j]), tempPosition.x, 1);

////    LeanTween.moveLocalZ(list[j], 3, .5f).setLoopPingPong(1);
////    yield return new WaitForSeconds(1.5f);
////}




////void QuickSort(GameObject[] list, int left, int right)
////{
////    if (left < right)
////    {


////        Action<int> callback = pivot =>
////        {
////            QuickSort(list, left, pivot - 1);
////            QuickSort(list, pivot + 1, right);
////        };

////        StartCoroutine(Partition(list, left, right, callback));
////    }
////}








//IEnumerator InsertionSort(GameObject[] list)
//{
//    int j = 0;
//    GameObject temp;
//    Vector3 tempPosition;

//    for (int i = 1; i < list.Length; i++)
//    {
//        j = i;
//        temp = list[i];
//        while (j > 0 && list[j - 1].transform.localScale.y > temp.transform.localScale.y)
//        {
//            tempPosition = temp.transform.localPosition;

//            if (list[j - 1] != temp)
//            {
//                LeanTween.color(temp, Color.red, 1f);
//                LeanTween.color(list[j - 1], Color.red, 1f);
//                yield return new WaitForSeconds(1);
//                LeanTween.color(temp, Color.white, 1f);
//                LeanTween.color(list[j - 1], Color.white, 1f);

//                LeanTween.moveLocalX((temp), list[j - 1].transform.localPosition.x, 0.5f);

//                LeanTween.moveLocalZ(temp, -3, .5f).setLoopPingPong(1);

//                LeanTween.moveLocalX((list[j - 1]), tempPosition.x, 0.5f);

//                LeanTween.moveLocalZ(list[j - 1], 3, .5f).setLoopPingPong(1);

//            }
//            yield return new WaitForSeconds(1);
//            list[j] = list[j - 1];

//            j--;
//        }
//        list[j] = temp;
//    }
//    StartCoroutine(ColorCube(list));
//}

//IEnumerator ShellSort(GameObject[] list)
//{
//    int lastIndex = list.Length - 1;
//    int gap = list.Length / 2;
//    GameObject temp;
//    Vector3 tempPosition;
//    bool exchanges;
//    do
//    {
//        do
//        {
//            exchanges = false;
//            for (int i = 0; i <= lastIndex - gap; i++)
//            {
//                if (list[i].transform.localScale.y
//                > list[i + gap].transform.localScale.y)
//                {
//                    temp = list[i];
//                    list[i] = list[i + gap];
//                    list[i + gap] = temp;

//                    LeanTween.color(list[i], Color.red, 1f);
//                    LeanTween.color(list[i + gap], Color.red, 1f);
//                    yield return new WaitForSeconds(2);
//                    LeanTween.color(list[i], Color.white, 1f);
//                    LeanTween.color(list[i + gap], Color.white, 1f);

//                    tempPosition = list[i].transform.localPosition;

//                    if (!(list[i] == list[i + gap]))
//                    {
//                        LeanTween.moveLocalX((list[i]), list[i + gap].transform.localPosition.x, 1);

//                        LeanTween.moveLocalZ(list[i], -3, .5f).setLoopPingPong(1);

//                        LeanTween.moveLocalX((list[i + gap]), tempPosition.x, 1);

//                        LeanTween.moveLocalZ(list[i + gap], 3, .5f).setLoopPingPong(1);
//                    }
//                    exchanges = true;
//                }
//            }
//        } while (exchanges);
//        gap = gap / 2;
//    } while (gap > 0);
//    StartCoroutine(ColorCube(list));
//}

//IEnumerator SelectionSort(GameObject[] list)
//{
//    int min;
//    GameObject temp;

//    Vector3 tempPosition;

//    for (int i = 0; i < list.Length; i++)
//    {
//        min = i;
//        for (int j = i + 1; j < list.Length; j++)
//        {
//            if (list[j].transform.localScale.y
//                < list[min].transform.localScale.y)
//            {
//                min = j;
//            }
//        }

//        if (min != 1)
//        {
//            temp = list[i];
//            list[i] = list[min];
//            list[min] = temp;

//            LeanTween.color(list[i], Color.red, 1f);
//            LeanTween.color(list[min], Color.red, 1f);
//            yield return new WaitForSeconds(2);
//            LeanTween.color(list[i], Color.white, 1f);
//            LeanTween.color(list[min], Color.white, 1f);

//            tempPosition = list[i].transform.localPosition;

//            if (!(list[i] == list[min]))
//            {
//                LeanTween.moveLocalX((list[i]), list[min].transform.localPosition.x, 1);

//                LeanTween.moveLocalZ(list[i], -3, .5f).setLoopPingPong(1);

//                LeanTween.moveLocalX((list[min]), tempPosition.x, 1);

//                LeanTween.moveLocalZ(list[min], 3, .5f).setLoopPingPong(1);
//            }

//        }

//        LeanTween.color(list[i], Color.green, 1f);
//    }
//}

//IEnumerator BubbleSort(GameObject[] list)
//{
//    Vector3 tempPosition;
//    GameObject temp = null;
//    bool swapMode = true;
//    int numOfComparisions = list.Length - 1;
//    while (swapMode)
//    {
//        swapMode = false;
//        for (int i = 0; i < numOfComparisions; i++)
//        {
//            if (list[i].transform.localScale.y
//                > list[i + 1].transform.localScale.y)
//            {
//                temp = list[i];
//                list[i] = list[i + 1];
//                list[i + 1] = temp;

//                LeanTween.color(list[i], Color.red, 1f);
//                LeanTween.color(list[i + 1], Color.red, 1f);
//                yield return new WaitForSeconds(1.5f);
//                LeanTween.color(list[i], Color.white, 1f);
//                LeanTween.color(list[i + 1], Color.white, 1f);

//                tempPosition = list[i].transform.localPosition;

//                if (!(list[i] == list[i + 1]))
//                {
//                    LeanTween.moveLocalX((list[i]), list[i + 1].transform.localPosition.x, 1);

//                    LeanTween.moveLocalZ(list[i], -3, .5f).setLoopPingPong(1);

//                    LeanTween.moveLocalX((list[i + 1]), tempPosition.x, 1);

//                    LeanTween.moveLocalZ(list[i + 1], 3, .5f).setLoopPingPong(1);
//                }
//                swapMode = true;
//            }
//        }
//        numOfComparisions--;
//    }
//    StartCoroutine(ColorCube(list));
//}
