using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;


public class SelectionSortMenu : MonoBehaviour
{
    public SortScript Sort;
    public SortScript ActiveSorted;
    public InputField InputNumberOfCubes;
    public InputField InputMaxCubeSize;
    public Dropdown dropdown;
    public Text text;

    public void ResetSort()
    {
        //yield return new WaitForSeconds(1);
        if (ActiveSorted == null)
        {
            text.text = ("Error! Nothing to Clear!");
        }
        else
        {
            Destroy(ActiveSorted.gameObject);
            dropdown.value = 0;
            InputNumberOfCubes.text = "";
            InputMaxCubeSize.text = "";
            text.text = ("Cleared!");
        }
    }

    public void HandleInputData(int val)
    {
        int maxVal ;
        int.TryParse(InputMaxCubeSize.text, out maxVal);

        if (ActiveSorted == null && val != 0)
        {
            
            if (InputNumberOfCubes.text.Equals("") )
            {
                text.text = ("Input Array Size!");
            }
            else if(InputMaxCubeSize.text.Equals(""))
            {
                text.text = ("Input Max Value Size!");
            }
            else if (InputNumberOfCubes.text.Equals("0"))
            {
                text.text = ("Array Size Cannot be Zero!");
                InputMaxCubeSize.text = "";
                InputNumberOfCubes.text = ""; 
            }
            else if (maxVal < 10)
            {
                text.text = ("Max Value Needs to be More Than 10!");
                InputNumberOfCubes.text = "";
                InputMaxCubeSize.text = "";
            }
            else
            {
                ActiveSorted = Instantiate(Sort);
                ActiveSorted.NumberOfCubes = Convert.ToInt32(InputNumberOfCubes.text);
                ActiveSorted.CubeMaxHeight = Convert.ToInt32(InputMaxCubeSize.text);
                if (val == 1)
                {
                    text.text = ("Selection Sort");
                    ActiveSorted.StartSelectionSort();
                }
                if (val == 2)
                {
                    text.text = ("Bubble Sort");
                    ActiveSorted.StartBubbleSort();
                }
                if(val == 3)
                {
                    text.text = ("Shell Sort");
                    ActiveSorted.StartShellSort();
                }
                if(val == 4)
                {
                    text.text = ("Insertion Sort");
                    ActiveSorted.StartInsertionSort();
                }
            }
            dropdown.value = 0;
        }
        else if (ActiveSorted != null)
        {
            text.text = ("Clear First Instance!");
           
            dropdown.value = 0;
        }
    }

}
