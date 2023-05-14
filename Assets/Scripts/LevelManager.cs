using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level texture")] [SerializeField]
    private Texture2D levelTexture;

    [Header("Tiles Prefabs")] [SerializeField]
    private GameObject prefabWallTile;

    [SerializeField] private GameObject prefabRoadTile;

    [HideInInspector] public List<RoadTile> roadTilesList = new();
    [HideInInspector] public RoadTile defaultBallRoadTile;

    private readonly Color _colorWall = Color.white;
    private readonly Color _colorRoad = Color.black;

    private float _unitPerPixel;

    private void Awake()
    {
        Generate();
        defaultBallRoadTile = roadTilesList[0];
    }

    private void Generate()
    {
        _unitPerPixel = prefabWallTile.transform.lossyScale.x;
        float halfUnitPerPixel = _unitPerPixel / 2f;

        float width = levelTexture.width;
        float height = levelTexture.height;

        Vector3 offset = new Vector3(width / 2f, 0f, height / 2f) * _unitPerPixel -
                         new Vector3(halfUnitPerPixel, 0f, halfUnitPerPixel);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //Get pixel color :
                Color pixelColor = levelTexture.GetPixel(x, y);

                Vector3 spawnPos = new Vector3(x, 0f, y) * _unitPerPixel - offset;

                if (pixelColor == _colorWall)
                    Spawn(prefabWallTile, spawnPos);
                else if (pixelColor == _colorRoad)
                    Spawn(prefabRoadTile, spawnPos);
            }
        }
    }

    private void Spawn(GameObject prefabTile, Vector3 position)
    {
        //fix Y position:
        position.y = prefabTile.transform.position.y;

        GameObject obj = Instantiate(prefabTile, position, Quaternion.identity, transform);

        if (prefabTile == prefabRoadTile)
            roadTilesList.Add(obj.GetComponent<RoadTile>());
    }
}