using UnityEngine;

[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class BuffSpell : MonoBehaviour
{
    public float pulseSpeed;
    public float slowPoint;
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
    private float currentAlpha;

    Mesh currentMesh;

    public void Release(float SetMaxRadius)
    {
        maxRadius = SetMaxRadius;
        begin = true;
    }

    void Start()
    {
        currentMesh = RingMeshGenerator.GenerateMesh(120, currentRadius, 1f, 0.1f);

        mf = GetComponent<MeshFilter>();
        mc = GetComponent<MeshCollider>();

        mf.mesh = currentMesh;
        mc.sharedMesh = null;
        mc.sharedMesh = currentMesh;

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
        //Code to expand the ring

        currentRadius += (pulseSpeed * Time.deltaTime) * (1-(currentRadius/maxRadius));

        currentMesh = RingMeshGenerator.GenerateMesh(120, currentRadius, 1f, 0.1f);

        if (!(currentRadius*1.8f < maxRadius))
        {
            beginLinger = true;
        }
    }

    void ApplyMesh()
    {
        mf.mesh = currentMesh;
        mc.sharedMesh = null;
        mc.sharedMesh = currentMesh;
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
