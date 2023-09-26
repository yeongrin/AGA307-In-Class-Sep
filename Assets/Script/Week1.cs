using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Week1 : MonoBehaviour
{
    public GameObject player;
    public int numberOne = 9;
    public int numberTwo = 7;
    public int newNumber;
    public Color newColor;


    // Start is called before the first frame update
    void Start()
    {
        player.transform.position = new Vector3(numberTwo, numberOne, numberTwo);
        player.GetComponent<Renderer>().material.color = newColor;

        //AddNumbers(); 
        //AddNumbers(numberOne, numberTwo);
        //AddNumbers(6, 8);
        //newNumber = AddNumbers2(7, 7);
    }

    int AddNumbers2(int _one, int _two)
    {
        return _one + _two;
    }

    void AddNumbers(int _one, int _two)
    {
        newNumber = _one + _two;
        print(newNumber);
    }

    // Update is called once per frame
    void AddNumbers()
    {
        newNumber = numberOne + numberTwo;
        print(newNumber);
    }
}
