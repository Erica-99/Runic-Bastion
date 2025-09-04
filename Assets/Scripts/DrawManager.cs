using System.Linq;
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrawManager : MonoBehaviour
{
    [SerializeField]
    private GameObject paperObject;
    private Texture2D originalTexture;

    [SerializeField]
    private GameObject manager;
    private Manager managerScript;

    [SerializeField]
    private Camera playerCam;
    [SerializeField]
    private LayerMask paperLayerMask;

    private InputAction mousePressed;
    private InputAction mousePosition;

    private bool textureReset = true;
    private Color[] pixelColours;

    [SerializeField]
    [Range(0, 1000)]
    public int brushWidth;
    private int savedBrushWidth;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        managerScript = manager.GetComponent<Manager>();

        mousePressed = InputSystem.actions.FindAction("Draw");
        mousePosition = InputSystem.actions.FindAction("MousePosition");
        Texture2D currentTexture = paperObject.GetComponent<Renderer>().material.GetTexture("_MainTex") as Texture2D;
        originalTexture = new Texture2D(currentTexture.width, currentTexture.height, TextureFormat.ARGB32, currentTexture.mipmapCount, false);
        Graphics.CopyTexture(currentTexture, originalTexture);

        savedBrushWidth = brushWidth;
        pixelColours = Enumerable.Repeat(Color.black, savedBrushWidth*savedBrushWidth).ToArray();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (brushWidth != savedBrushWidth)
        {
            savedBrushWidth = brushWidth;
            pixelColours = UpdateBrush(brushWidth, Color.black);
        }

        if (!managerScript.castMode)
        {
            if (!textureReset)
            {
                Texture2D destinationTexture = paperObject.GetComponent<Renderer>().material.GetTexture("_MainTex") as Texture2D;
                Graphics.CopyTexture(originalTexture, destinationTexture);
                paperObject.GetComponent<Renderer>().material.mainTexture = destinationTexture;
                destinationTexture.Apply();
                textureReset = true;
            }
            
            paperObject.SetActive(false);
            return;
        }

        paperObject.SetActive(true);

        DrawOnPaper();
    }

    void DrawOnPaper()
    {
        textureReset = false;

        if (mousePressed.IsPressed())
        {
            Ray ray = playerCam.ScreenPointToRay(mousePosition.ReadValue<Vector2>());
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, maxDistance:1000f, layerMask:paperLayerMask))
            {
                return;
            }

            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.blue);

            if (hit.transform != paperObject.transform)
            {
                return;
            }

            Renderer rend = hit.transform.GetComponent<Renderer>();
            MeshCollider meshCollider = hit.collider as MeshCollider;

            if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                return;

            Texture2D tex = rend.material.mainTexture as Texture2D;
            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= tex.width;
            pixelUV.y *= tex.height;

            tex.SetPixels((int)pixelUV.x, (int)pixelUV.y, savedBrushWidth, savedBrushWidth, pixelColours);
            tex.Apply();
        }
    }

    private Color[] UpdateBrush(int newBrushWidth, Color color)
    {
        Color[] brush = Enumerable.Repeat(color, newBrushWidth*newBrushWidth).ToArray();
        return brush;
    }
}
