using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    Board _board;

    void Start()
    {
        
    }
    
    public void Init(int x, int y, Board board)
    {
        xIndex = x;
        yIndex = y;

        //_board = GameObject.Find("Board").GetComponent<Board>();
        _board = board;
    }
}
