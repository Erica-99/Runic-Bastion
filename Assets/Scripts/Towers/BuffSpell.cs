using UnityEngine;
using RunicBastion.Utils;

namespace RunicBastion.Towers
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(MeshRenderer))]
    public class BuffSpell : MonoBehaviour
    {
        public float pulseSpeed;
        public float lingerTime;

        private MeshFilter mf;
        private MeshCollider mc;
        private MeshRenderer mr;

        private float maxRadius;
        private float currentRadius = 1f;

        private bool begin = false;
        private bool beginLinger = false;

        private float lingerTimer = 0f;

        private float initialAlpha;

        Mesh currentRingMesh;
        Mesh currentSphereMesh;

        public void Release(float SetMaxRadius)
        {
            maxRadius = SetMaxRadius;
            begin = true;
        }

        void Start()
        {
            currentRingMesh = CustomMeshGenerator.GenerateRingMesh(120, currentRadius, 1f, 0.1f);
            currentSphereMesh = CustomMeshGenerator.GenerateSphereMesh(8, currentRadius);

            mf = GetComponent<MeshFilter>();
            mc = GetComponent<MeshCollider>();

            mf.mesh = currentRingMesh;
            mc.sharedMesh = null;
            mc.sharedMesh = currentSphereMesh;

            mr = GetComponent<MeshRenderer>();
            initialAlpha = mr.material.color.a;
        }

        void Update()
        {
            if (!begin)
            {
                return;
            }

            UpdateMesh();
            ApplyMesh();

            if (beginLinger)
            {
                Linger();
            }
        }

        void UpdateMesh()
        {
            //Code to expand the ring and sphere

            currentRadius += pulseSpeed * Time.deltaTime * (1 - (0.95f*currentRadius / maxRadius));

            currentRingMesh = CustomMeshGenerator.GenerateRingMesh(120, currentRadius, 1f, 0.1f);
            currentSphereMesh = CustomMeshGenerator.GenerateSphereMesh(8, currentRadius);

            if (!(currentRadius + 2f < maxRadius))
            {
                beginLinger = true;
            }
        }

        void ApplyMesh()
        {
            mf.mesh = currentRingMesh;
            mc.sharedMesh = null;
            mc.sharedMesh = currentSphereMesh;
        }

        void Linger()
        {
            if (lingerTimer > lingerTime)
            {
                Destroy(gameObject);
            }
            else
            {
                lingerTimer += Time.deltaTime;

                Color currentColor = mr.material.color;
                currentColor.a = Mathf.Lerp(initialAlpha, 0f, lingerTimer / lingerTime);
                mr.material.color = currentColor;
            }
        }
    }
}