namespace UP07
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ProceduralMesh : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;

        [SerializeField] private int width, height;

        [SerializeField, Range(0F, 1F)] private float rangeScale = 0.1f;
        [SerializeField, Range(0F, 25F)] private float maximumHeight = 10F;

        [SerializeField] private AnimationCurve curve;

        private Mesh mesh;
        private Vector3[] vertices;

        private Camera cam;


        private void Start()
        {
            Generate();
        }


        private void Generate()
        {
            cam = Camera.main;

            meshFilter.mesh = mesh ?? (mesh = new Mesh());

            UpdateVertices();
            UpdateUVs();
            UpdateNormals();
            UpdateTangents();
        }

        private void UpdateVertices()
        {
            if (mesh == null) return;

            vertices = new Vector3[(width + 1) * (height + 1)];
            int[] triangles = new int[width * height * 6];
            for (int y = 0; y <= height; ++y)
            {
                for (int x = 0; x <= width; ++x)
                {
                    int vIdx = y * (width + 1) + x;
                    vertices[vIdx] = new Vector3(x, 0F, y);
                }
            }

            for (int tIdx = 0, y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x, tIdx += 6)
                {
                    int vIdx = y * (width + 1) + x;
                    triangles[tIdx] = vIdx;
                    triangles[tIdx + 2] = triangles[tIdx + 3] = vIdx + 1;
                    triangles[tIdx + 1] = triangles[tIdx + 4] = vIdx + width + 1;
                    triangles[tIdx + 5] = vIdx + width + 2;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
        }

        private void UpdateUVs()
        {
            if (mesh == null) return;

            Vector2[] uvs = new Vector2[(width + 1) * (height + 1)];
            for (int y = 0; y <= height; ++y)
            {
                for (int x = 0; x <= width; ++x)
                {
                    uvs[y * (width + 1) + x] = new Vector2((float)x / width, (float)y / height);
                }
            }

            mesh.uv = uvs;
        }

        private void UpdateNormals()
        {
            if (mesh == null) return;

            mesh.RecalculateNormals();
        }

        private void UpdateTangents()
        {
            if (mesh == null) return;

            mesh.RecalculateTangents();
        }


        private void Update()
        {
            Vector3 camPosition = cam.transform.position;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            float length = -camPosition.y / ray.direction.y;

            Vector3 hitPosition = camPosition + ray.direction * length;

            if (!Input.GetMouseButton(0)) return;

            for (int y = 0; y <= height; ++y)
            {
                for (int x = 0; x <= width; ++x)
                {
                    int vIdx = y * (width + 1) + x;

                    float threshold = new Vector2(x - hitPosition.x, y - hitPosition.z).sqrMagnitude;
                    threshold = curve.Evaluate(threshold * rangeScale);

                    vertices[vIdx].y += Mathf.Lerp(maximumHeight, 0F, threshold) * Time.deltaTime;
                }
            }
            mesh.vertices = vertices;

            UpdateNormals();
            UpdateTangents();
        }
    }
}