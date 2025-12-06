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

    private Tile[,] _allTiles;

    void Start()
    {
        _allTiles = new Tile[_width, _height];

        SetUpTiles();

        SetUpCamera();
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
}
