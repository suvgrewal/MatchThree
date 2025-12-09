using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;

    [SerializeField]
    private int _borderSize;

    [SerializeField]
    private GameObject _tilePrefab;
    [SerializeField]
    private GameObject[] _gamePiecePrefabs;

    private Tile[,] _allTiles;
    private GamePiece[,] _allGamePieces;

    private Tile _clickedTile;
    private Tile _targetTile;

    public float swapTime = 0.5f;

    void Start()
    {
        _allTiles = new Tile[_width, _height];
        _allGamePieces = new GamePiece[_width, _height];

        SetUpTiles();

        SetUpCamera();

        FillRandom();
    }

    void SetUpTiles()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                GameObject tile = Instantiate(_tilePrefab, new Vector3(i, j, 0), Quaternion.identity) as GameObject;

                tile.name = "Tile (" + i + ", " + j + ")";

                _allTiles[i, j] = tile.GetComponent<Tile>();

                tile.transform.parent = transform;

                _allTiles[i, j].Init(i, j, this);
            }
        }
    }

    void SetUpCamera()
    {
        Camera.main.transform.position = new Vector3((float) (_width - 1) / 2f, (float) (_height - 1) / 2f, -10f);

        float aspectRatio = (float) Screen.width / Screen.height;

        float verticalOrthographicSize = (float) _height / 2f + (float) _borderSize;

        float horizontalOrthographicSize = ((float) _width / 2f + (float) _borderSize)  / aspectRatio;

        Camera.main.orthographicSize = (verticalOrthographicSize > horizontalOrthographicSize) ? verticalOrthographicSize : horizontalOrthographicSize;
    }

    GameObject GetRandomGamePiece()
    {
        int randomIdx = Random.Range(0, _gamePiecePrefabs.Length);

        if (_gamePiecePrefabs[randomIdx] == null)
        {
            Debug.LogWarning("BOARD: " + randomIdx + "does not contain a valid GamePiece prefab!");
        }

        return _gamePiecePrefabs[randomIdx];
    }

    public void PlaceGamePiece(GamePiece gamePiece, int x, int y)
    {
        if (gamePiece == null)
        {
            Debug.LogWarning("BOARD: Invalid GamePiece!");

            return;
        }


        gamePiece.transform.position = new Vector3(x, y, 0);
        gamePiece.transform.rotation = Quaternion.identity;

        if (IsWithinBounds(x, y))
        {
            _allGamePieces[x, y] = gamePiece;
        }
        
        gamePiece.SetCoord(x, y);
    }

    void PlaceRandomGamePiece(int xIdx, int yIdx)
    {
        GameObject randomPiece = Instantiate(GetRandomGamePiece(), Vector3.zero, Quaternion.identity) as GameObject;

        if (randomPiece != null)
        {
            randomPiece.GetComponent<GamePiece>().Init(this);

            PlaceGamePiece(randomPiece.GetComponent<GamePiece>(), xIdx, yIdx);
        }
    }

    bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < _width && y >= 0 && y < _height);
    }

    void FillRandom()
    {
        for (int i = 0; i < _width;  i++)
        {
            for (int j = 0; j < _height; j++)
            {
                PlaceRandomGamePiece(i, j);
            }
        }
    }

    public void ClickTile(Tile tile)
    {
        if (_clickedTile == null)
        {
            _clickedTile = tile;
        }
    }

    public void DragToTile(Tile tile)
    {
        if (_clickedTile != null)
        {
            _targetTile = tile;
        }
    }

    public bool IsNextTo(Tile startTile, Tile endTile)
    {
        int dx = Mathf.Abs(startTile.xIndex - endTile.xIndex);
        int dy = Mathf.Abs(startTile.yIndex - endTile.yIndex);

        return (dx + dy) == 1;
    }

    public void ReleaseTile()
    {
        if (_clickedTile != null && _targetTile != null)
        {
            SwitchTiles(_clickedTile, _targetTile);
        }

        _clickedTile = null;
        _targetTile = null;
    }

    void SwitchTiles(Tile clickedTile, Tile targetTile)
    {
        if (!IsNextTo(clickedTile, targetTile))
        {
            Debug.Log("Clicked tile is not next to target tile");
            return;
        }

        GamePiece clickedPiece = _allGamePieces[clickedTile.xIndex, clickedTile.yIndex];
        GamePiece targetPiece = _allGamePieces[targetTile.xIndex, targetTile.yIndex];

        _allGamePieces[clickedTile.xIndex, clickedTile.yIndex] = targetPiece;
        _allGamePieces[targetTile.xIndex, targetTile.yIndex] = clickedPiece;

        clickedPiece.Move(targetTile.xIndex, targetTile.yIndex, swapTime);
        targetPiece.Move(clickedTile.xIndex, clickedTile.yIndex, swapTime);
        
        clickedPiece.SetCoord(targetTile.xIndex, targetTile.yIndex);
        targetPiece.SetCoord(clickedTile.xIndex, clickedTile.yIndex);
    }
}
