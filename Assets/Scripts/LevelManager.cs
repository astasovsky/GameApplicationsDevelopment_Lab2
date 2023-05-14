using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level texture")] [SerializeField]
    private Texture2D levelTexture;

    [Header("Titles Prefabs")] [SerializeField]
    private GameObject prefabWallTile;

    [SerializeField] private GameObject prefabRoadTile;

    private readonly Color _colorWall = Color.white;
    private readonly Color _colorRoad = Color.black;
    private float _unitPerPixel;

    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        _unitPerPixel = prefabWallTile.transform.lossyScale.x;
        float halfUnitePerPixel = _unitPerPixel / 2;
        float wight = levelTexture.width;
        float height = levelTexture.height;
        Vector3 offset = (new Vector3(wight / 2f, 0f, height / 2f) * _unitPerPixel) - new
            Vector3(halfUnitePerPixel, 0f, halfUnitePerPixel);
        for (int x = 0; x < wight; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixelColor = levelTexture.GetPixel(x, y);
                Vector3 spawnPos = (new Vector3(x, 0f, y) * _unitPerPixel) - offset;
                if (pixelColor == _colorWall)
                    Spawn(prefabWallTile, spawnPos);
                else if (pixelColor == _colorRoad)
                    Spawn(prefabRoadTile, spawnPos);
            }
        }
    }

    private void Spawn(GameObject prefabTile, Vector3 position)
    {
        position.y = prefabTile.transform.position.y;
        GameObject obj = Instantiate(prefabTile, position, Quaternion.identity, transform);
    }
}