using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour
{
    
    void SelectionSort(List<int> list)
    {
        int min;
        int temp;

        for(int i = 0; i < list.Count; i++)
        {
            min = i;
            for(int j = i+1; j < list.Count; j++)
            {
                if (list[j] < list[min])
                {
                    min = j;
                }
            }

            if(min != 1)
            {
                temp = list[i];
                list[i] = list[min];
                list[min] = temp;
                PrintArray(list);
            }
        }
    }

    void PrintArray(List<int> list)
    {
        string str = "";
        for(int i = 0; i < list.Count; i++)
        {
            str = str + list[i] + ", ";
        }

        print(str);
    }

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(1, 99);
        List<int> list = new List<int>(20);
        
        for(int i = 0;  i < list.Count; i++)
        {
            list.Add(rand);
        }

        PrintArray(list);
        SelectionSort(list);
    }

    //list[i].transform.localPosition =
    //                new Vector3(list[min].transform.localPosition.x, tempPosition.y, tempPosition.z);

    //list[min].transform.localPosition =
    //                new Vector3(tempPosition.x, list[min].transform.localPosition.y, list[min].transform.localPosition.z);

    // Update is called once per frame
    void Update()
    {
        
    }
}
