using System.Linq;
using System.Transactions;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

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

    public GameObject pen;
    private PenMovement penMovement;

    private InputAction mousePressed;
    private InputAction mousePosition;

    private bool textureReset = true;
    private Color[] pixelColours;

    [SerializeField]
    [Range(0, 1000)]
    private int brushWidth;
    private int savedBrushWidth;

    [SerializeField]
    private float paperDistance;

    private Ray prevRay;
    private SpellManager spellManager;

    private bool createdSpells;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        managerScript = manager.GetComponent<Manager>();

        mousePressed = InputSystem.actions.FindAction("Draw");
        mousePosition = InputSystem.actions.FindAction("MousePosition");

        //Paper
        Texture2D currentTexture = paperObject.GetComponent<Renderer>().material.GetTexture("_MainTex") as Texture2D;
        originalTexture = new Texture2D(currentTexture.width, currentTexture.height, TextureFormat.ARGB32, currentTexture.mipmapCount, false);
        Graphics.CopyTexture(currentTexture, originalTexture);

        savedBrushWidth = brushWidth;
        pixelColours = Enumerable.Repeat(Color.black, savedBrushWidth*savedBrushWidth).ToArray();
        prevRay = new Ray();

        //Pen
        penMovement = pen.GetComponent<PenMovement>();

        //Spells
        spellManager = GetComponent<SpellManager>();
        createdSpells = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //UpdatePaperPos(paperDistance);

        if (brushWidth != savedBrushWidth)
        {
            savedBrushWidth = brushWidth;
            pixelColours = UpdateBrush(brushWidth, Color.black);
        }

        if (!managerScript.castMode)
        {
            // Reset texture on closing paper
            if (!textureReset)
            {
                ResetPaperTexture();
                textureReset = true;
            }
            
            paperObject.SetActive(false);
            penMovement.followCursor = false;
            createdSpells = false;
            managerScript.triggerSpellCheck();
        }
        else
        {
            paperObject.SetActive(true);
            penMovement.followCursor = true;

            if (!createdSpells)
            {
                spellManager.createSpells();
                createdSpells = true;
            }

            DrawOnPaper();
        }

        
    }

    void DrawOnPaper()
    {
        textureReset = false;

        Physics.SyncTransforms();

        Vector2 mousePos = mousePosition.ReadValue<Vector2>();
        Vector3 mouseWorldPos = playerCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, paperDistance));

        Ray newRay = new Ray(playerCam.transform.position, mouseWorldPos - playerCam.transform.position);
        RaycastHit hit;

        Ray ray = newRay;
        if (!Physics.Raycast(ray, out hit, maxDistance: 1000f, layerMask: paperLayerMask))
        {
            Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);
            prevRay = newRay;
            return;
        }

        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.blue);

        if (hit.transform != paperObject.transform)
        {
            prevRay = newRay;
            return;
        }

        penMovement.targetMousePoint = hit.point;
        prevRay = newRay;

        if (mousePressed.IsPressed())
        {
            penMovement.mouseDown = true;

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
        } else
        {
            penMovement.mouseDown = false;
        }
    }

    private Color[] UpdateBrush(int newBrushWidth, Color color)
    {
        Color[] brush = Enumerable.Repeat(color, newBrushWidth*newBrushWidth).ToArray();
        return brush;
    }

    private void UpdatePaperPos(float distance)
    {
        Vector3 currentPos = paperObject.transform.localPosition;
        currentPos.z = distance;
        paperObject.transform.localPosition = currentPos;
    }

    private void ResetPaperTexture()
    {
        Texture2D destinationTexture = paperObject.GetComponent<Renderer>().material.GetTexture("_MainTex") as Texture2D;
        Graphics.CopyTexture(originalTexture, destinationTexture);
        paperObject.GetComponent<Renderer>().material.mainTexture = destinationTexture;
        destinationTexture.Apply();
    }
}
