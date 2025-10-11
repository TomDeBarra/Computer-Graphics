using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

public class GraphicsPipeline : MonoBehaviour
{
    StreamWriter writer;
    // Start is called before the first frame update
    void Start()
    {
        writer = new StreamWriter("Data.txt",false);

        Model myModel = new Model();

        List<Vector3> verts3 = myModel.vertices;
        List<Vector4> verts = convertToHomg(verts3);
        writeVectorsToFile(verts, " Vertices of my letter ", " ---------- ");


        // old: Vector3 axis = (new Vector3(-2,1,1)).normalized;
        // old: Quaternion.AngleAxis(-25, axis)
        // new
       Vector3 axis = (new Vector3(20,1,1)).normalized;
        Matrix4x4 rotationMatrix =
            Matrix4x4.TRS(Vector3.zero,
                        Quaternion.AngleAxis(35, axis),
                        Vector3.one);

        writeMatrixToFile(rotationMatrix, "Rotation Matrix", "  ---------  ");

        List<Vector4> imageAfterRotation =
            applyTransformation(verts, rotationMatrix);

        writeVectorsToFile(imageAfterRotation, " Verts after Rotation ", " -----------------");
        
        Matrix4x4 scaleMatrix =
            Matrix4x4.TRS(Vector3.zero,
            Quaternion.identity,
            new Vector3(20, 2, 1));
        // old: new Vector3(2, 2, 1)

        writeMatrixToFile(scaleMatrix, " Scale Matrix ", " ----------------  ");

        List<Vector4> imageAfterScale =
            applyTransformation(imageAfterRotation, scaleMatrix);

        writeVectorsToFile(imageAfterScale, " After Scale (and Rotation)", " ---------------");
  
        // old: new Vector3()
        Matrix4x4 translationMatrix =
            Matrix4x4.TRS(new Vector3(0, 3, 3),
                Quaternion.identity,
                Vector3.one);
        
        writeMatrixToFile(translationMatrix, " Translation Matrix ", " ---------------- ");

        List<Vector4> imageAfterTranslation =
            applyTransformation(imageAfterScale, translationMatrix);

        writeVectorsToFile(imageAfterTranslation, " After Translation ", " ---------------");
        
        // old: Matrix4x4 viewingMatrix = Matrix4x4.LookAt(new Vector3(), new Vector3(), new Vector3());
        Matrix4x4 viewingMatrix = Matrix4x4.LookAt(
            new Vector3(22, 5, 52),   // camera position
            new Vector3(2, 2, 2),     // look at
            new Vector3(3, 3, 20));   // up

        writeMatrixToFile(viewingMatrix, " Viewing Matrix ", " ---------------- ");
        
        List<Vector4> imageAfterViewing =
            applyTransformation(imageAfterTranslation, viewingMatrix);

        writeVectorsToFile(imageAfterViewing, " Image after Viewing Matrix ", " ---------------");
        
        // ----- Projection by Hand (Division) -----
        List<Vector4> projectionByHand = new List<Vector4>();

        foreach (Vector4 v in imageAfterViewing)
        {
            if (Mathf.Abs(v.z) > 1e-6f)
            {
                float xProj = v.x / v.z;
                float yProj = v.y / v.z;
                float zProj = v.z;
                projectionByHand.Add(new Vector4(xProj, yProj, zProj, 1));
            }
            else
            {
                projectionByHand.Add(v);
            }
        }

        // Write results to file
        writeVectorsToFile(projectionByHand, " Projection by Hand (Division) ", " --------------- ");
        
        // ----- Final Image (After Manual Projection) -----
        writeVectorsToFile(projectionByHand, " Final Image (Manual Projection) ", " --------------- ");

        Matrix4x4 projection = Matrix4x4.Perspective(90, 1, 1, 1000);
        writeMatrixToFile(projection, " Projection Matrix ", " ---------------- ");
        
        List<Vector4> finalImage =
            applyTransformation(imageAfterViewing, projection);

        writeVectorsToFile(finalImage, " Final Image (After Projection) ", " ---------------");
       
        // Single Matrix of Transformations 
        Matrix4x4 singleMatrix = translationMatrix * scaleMatrix * rotationMatrix;
        
        writeMatrixToFile(singleMatrix, " Single Matrix of Transformations ", " ---------------- ");
        
        List<Vector4> imageAfterSingleMatrix =
            applyTransformation(verts, singleMatrix);
        
        writeVectorsToFile(imageAfterSingleMatrix, " Image after Single Matrix of Transformations ", " --------------- ");
        
        // Single Matrix for Everything 
        Matrix4x4 singleMatrixEverything =
            projection * viewingMatrix * translationMatrix * scaleMatrix * rotationMatrix;


        writeMatrixToFile(singleMatrixEverything, " Single Matrix for Everything ", " ---------------- ");


        List<Vector4> finalImageEverything =
            applyTransformation(verts, singleMatrixEverything);
        
        writeVectorsToFile(finalImageEverything, " Final Image (Single Matrix for Everything) ", " --------------- ");

        writer.Close();
        
    }

    private void writeMatrixToFile(Matrix4x4 matrix, string before, string after)
    {
        writer.WriteLine(before);
        
        for (int i = 0; i<4; i++)
        {
            Vector4 v = matrix.GetRow(i);
            writer.WriteLine(" ( " + v.x + " , " + v.y + " , " + v.z + " , " + v.w + " ) ");
        }
        writer.WriteLine(after);
    }

    private void writeVectorsToFile(List<Vector4> verts, string before, string after)
    { writer.WriteLine(before);

      foreach (Vector4 v in verts)
        {
            writer.WriteLine(" ( " + v.x + " , " + v.y + " , " + v.z + " , " + v.w + " ) ");
        }
      writer.WriteLine(after);
    }

    private List<Vector4> convertToHomg(List<Vector3> vertices)
    {
        List<Vector4> output = new List<Vector4>();

        foreach (Vector3 v in vertices)
        {
            output.Add(new Vector4(v.x, v.y, v.z, 1.0f));

        }
        return output;

    }

    private List<Vector4> applyTransformation
        (List<Vector4> verts, Matrix4x4 tranformMatrix)
    {
        List<Vector4> output    = new List<Vector4>();
        foreach (Vector4 v in verts) 
        { output.Add(tranformMatrix * v); }

        return output;
    }

    private void displayMatrix(Matrix4x4 rotationMatrix)
    {
        for (int i = 0; i < 4; i++)
            { print(rotationMatrix.GetRow( i)); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}