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

    private void OnMouseDown()
    {
        if (_board == null)
        {
            Debug.Log("Board does not exist");
            return;
        }

        _board.ClickTile(this);
    }

    private void OnMouseEnter()
    {
        if (_board == null)
        {
            Debug.Log("Board does not exist");
            return;
        }

        _board.DragToTile(this);
    }

    private void OnMouseUp()
    {
        if (_board == null)
        {
            Debug.Log("Board does not exist");
            return;
        }

        _board.ReleaseTile();
    }
}
