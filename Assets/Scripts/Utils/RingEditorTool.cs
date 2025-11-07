//using UnityEditor;
using UnityEngine;

namespace RunicBastion.Utils
{
    [RequireComponent(typeof(MeshFilter))]
    public class RingEditorTool : MonoBehaviour
    {
        public int segments = 360;
        public float outerRadius = 1.0f;
        public float thickness = 0.1f;
        public float verticalThickness = 0.1f;

        private Mesh mesh;


        void Start()
        {
            mesh = CustomMeshGenerator.GenerateRingMesh(segments, outerRadius, thickness, verticalThickness);
            SetMesh();
        }

        public void RegenerateMesh()
        {
            mesh = CustomMeshGenerator.GenerateRingMesh(segments, outerRadius, thickness, verticalThickness);
            SetMesh();
        }

        public void SetMesh()
        {
            GetComponent<MeshFilter>().mesh = mesh;
        }
    }

    //[CustomEditor(typeof(RingEditorTool))]
    //public class RingEditorToolEditor : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        DrawDefaultInspector();
    //        RingEditorTool ringTool = (RingEditorTool)target;
    //        if (GUILayout.Button("Regenerate Mesh"))
    //        {
    //            ringTool.RegenerateMesh();
    //        }
    //    }
    //}
}