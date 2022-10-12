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

        [Header("Transform")]
        [SerializeField] private Vector3 eulerAngles;
        [SerializeField] private Vector3 localScale = Vector3.one;

        private Mesh mesh;
        private Vector3[] originVertices;
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


            originVertices = new Vector3[(width + 1) * (height + 1)];
            vertices = new Vector3[originVertices.Length];
            int[] triangles = new int[width * height * 6];

            (int w, int h) halfSize = (width / 2, height / 2);

            for (int y = 0; y <= height; ++y)
            {
                for (int x = 0; x <= width; ++x)
                {
                    int vIdx = y * (width + 1) + x;
                    originVertices[vIdx] = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(eulerAngles), localScale) * new Vector3(x - halfSize.w, 0F, y - halfSize.h);
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

            mesh.vertices = originVertices;
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

            var trsMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(eulerAngles), localScale);
            Vector3 upDir = trsMatrix * Vector3.up;

            (int w, int h) halfSize = (width / 2, height / 2);

            for (int y = 0; y <= height; ++y)
            {
                for (int x = 0; x <= width; ++x)
                {
                    int vIdx = y * (width + 1) + x;

                    vertices[vIdx] = trsMatrix * originVertices[vIdx];

                    if (!Input.GetMouseButton(0)) continue;

                    float threshold = new Vector2((x - halfSize.w) - hitPosition.x, (y - halfSize.h) - hitPosition.z).sqrMagnitude;
                    threshold = curve.Evaluate(threshold * rangeScale);
                    originVertices[vIdx].y += Mathf.Lerp(maximumHeight, 0F, threshold) * Time.deltaTime;
                }
            }
            mesh.vertices = vertices;

            UpdateNormals();
            UpdateTangents();
        }
    }
}