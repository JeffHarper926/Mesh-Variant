using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshVariant : MonoBehaviour
{
    public bool includeDefault = true; 
    public MeshFilter meshFilter;
    public Texture2D texture; 

    public List<VariantUV> variants = new List<VariantUV>();

    [System.Serializable]
    public class VariantUV
    {
        public List<UVCoord> coords = new List<UVCoord>();

        [System.Serializable]
        public class UVCoord
        {
            public string name; 
            public Vector2Int currentUV;
            public Vector2Int newUV;
        }
    }

    private void Awake()
    {
        if(!meshFilter)
            meshFilter = GetComponent<MeshFilter>();

        int rand = Random.Range(0, variants.Count + 1);

        if (rand == 0 && includeDefault)
            return;

        rand = Random.Range(0, variants.Count);

        VariantUV alter = variants[rand];

        Mesh mesh = meshFilter.sharedMesh;

        List<Vector2> uv = new List<Vector2>(mesh.uv);

        for (int i = 0; i < mesh.uv.Length; i++)
        {
            for (int j = 0; j < alter.coords.Count; j++)
            {
                float xFloor = (float)alter.coords[j].currentUV.x / texture.width;
                float xCeil = (float)(alter.coords[j].currentUV.x + 1) / texture.width;
                float yFloor = (float)alter.coords[j].currentUV.y / texture.height;
                float yCeil = (float)(alter.coords[j].currentUV.y + 1) / texture.height;
                if (mesh.uv[i].x > xFloor && mesh.uv[i].x < xCeil && mesh.uv[i].y > yFloor && mesh.uv[i].y < yCeil)
                    uv[i] = new Vector2(((float)alter.coords[j].newUV.x + 0.5f) / texture.width, ((float)alter.coords[j].newUV.y + 0.5f) / texture.height);
            }
        }

        Mesh newMesh = Instantiate(mesh);
        newMesh.SetUVs(0, uv);

        meshFilter.mesh = newMesh;
    }

    public void AddDefault()
    {
        if(!meshFilter)
            meshFilter = GetComponent<MeshFilter>();

        List<Vector2> uvList = new List<Vector2>();
        Mesh mesh = meshFilter.sharedMesh;
        List<Vector2> uv = new List<Vector2>(mesh.uv);

        for (int i = 0; i < mesh.uv.Length; i++)
        {
            if (!uvList.Contains(mesh.uv[i]))
                uvList.Add(mesh.uv[i]);
        }

        VariantUV variant = new VariantUV();

        for (int i = 0; i < uvList.Count; i++)
        {
            VariantUV.UVCoord coord = new VariantUV.UVCoord();
            coord.currentUV = new Vector2Int((int)(uvList[i].x * 8), (int)(uvList[i].y * 8));
            coord.newUV = new Vector2Int((int)(uvList[i].x * 8), (int)(uvList[i].y * 8));
            variant.coords.Add(coord);
        }

        variants.Add(variant);
    }
}