using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using Unity.Mathematics;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;


public class GraphicsPipeline : MonoBehaviour
{
    StreamWriter writer;
    public GameObject screenGO;
    Renderer screen;
    Model myModel;

   
    
    // Start is called before the first frame update
    void Start()
    {
        screen = screenGO.GetComponent<Renderer>();
        writer = new StreamWriter("Data.txt",false);

        myModel = new Model();

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
        
        OutCode o1 =  new OutCode();
        OutCode o2 = new OutCode(new Vector2(0, 2));
        OutCode o3 = new OutCode(true,false,false,true);
        
        o1.printFunction();
        o2.printFunction();
        OutCode o4 = o1 + o2;
        o4.printFunction();
        
        OutCode o5 = new OutCode(new Vector2(0, 2));
        OutCode o6 = new OutCode(new Vector2(0, 3));
        
        Vector2 s = new Vector2(-2,0);
        Vector2 e = new Vector2(2,0);
        print(intercept(s, e, 2));

        if (LineClip(ref s, ref e))
        {
            print(s);
            print(e);
        }
        else
        { print("Line rejected"); }
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

    bool LineClip(ref Vector2 start, ref Vector2 end)
    {
        // Test for trivial acceptance
        OutCode startOC = new OutCode(start);
        OutCode endOC = new OutCode(end);
        OutCode inViewPort = new OutCode();

        if ((startOC + endOC) == inViewPort)
        {
            return true;
        }
        
        // Test for trivial rejection

        if ((startOC * endOC) != inViewPort)
        {
            return false;
        }
        
        return false;
    }
    
    Vector2 intercept(Vector2 start, Vector2 end, int edgeIndex)
    {
        if (end.x != start.x)
        {
            float m = (end.y - start.y) / (end.x - start.x);
            switch (edgeIndex)
            {
                case 0: //Top Edge - Y = 1
                    //   x = x1 + (1/m) *(y - y1)
                    if (m != 0)
                    {
                        return new Vector2(start.x + (1 / m) * (1 - start.y), 1);
                    }
                    else
                    {
                        switch (edgeIndex)
                        {
                            case 0: //Top Edge - Y = 1
                                if (start.y == 1)
                                    return new Vector2(1, 1);
                                else
                                {
                                    print("Warning No intercept");
                                    return new Vector2(float.NaN, float.NaN);
                                }
                               
                            case 1: //Bottom Edge - Y = -1
                                if (start.y == -1)
                                    return new Vector2(-1, -1);
                                else
                                {
                                    print("Warning No intercept");
                                    return new Vector2(float.NaN, float.NaN);
                                }
                            case 2: //Left Edge - X = -1
                             return new Vector2(-1, start.y);
                            default: // Right Edge - X = 1 
                                return new Vector2(1, start.y);

                        }
                    }
                    break;
                case 1: //Bottom Edge - Y = -1
                    break;
                case 2: //Left Edge - X = -1
                    
                    return new Vector2(-1, start.y + m * (-1 - start.x));
                default: // Right Edge - X = 1 
                    return new Vector2(1, start.y + m * (1 - start.x));

            }
        }
        else
        {
            switch (edgeIndex)
            {
                case 0: //Top Edge - Y = 1

                    return new Vector2(start.x, 1);
                case 1: //Bottom Edge - Y = -1
                    return new Vector2(start.x, -1);
                case 2: //Left Edge - X = -1
                    if (start.x == -1)
                        return new Vector2(-1, -1);
                    else
                    {
                        print("Warning No intercept");
                        return new Vector2(float.NaN, float.NaN);
                    }
                default: // Right Edge - X = 1 
                    if (start.x == 1)
                        return new Vector2(1, 1);
                    else
                    {
                        print("Warning No intercept");
                        return new Vector2(float.NaN, float.NaN);
                    }

            }
        }
        return Vector2.zero;
    }

    private void drawLine(Vector2Int start, Vector2Int end, Texture2D texture)
    {
        SetPixels(Bresenham(start, end), texture);
    }

    private void SetPixels(List<Vector2Int> vector2Ints, Texture2D fb)
    {
        foreach (Vector2Int p in vector2Ints)
        {
            fb.SetPixel(p.x, p.y, Color.red);
        }
    }

    private List<Vector2Int> pixelize(List<Vector4> projectedVerts, int resX, int resY)
    {
        // first project
        List<Vector2Int> output = new List<Vector2Int> ();
        foreach (Vector4 v in projectedVerts)
        {
            Vector2 ndc = new Vector2(v.x / v.w, v.y / v.w);
            Vector2Int pixel = new Vector2Int((int) ((ndc.x + 1) * 0.5f * (resX - 1)), (int)((ndc.y + 1) * 0.5f * (resY - 1)));
            output.Add(pixel);
        }

        return output;
    }
    
    // Put here so I can use code copied from Rob
    
    private Vector2Int convertToPixel(Vector2 p, int width, int height)
    {
        return new Vector2Int(
            (int)((p.x + 1) * (width - 1) / 2f),
            (int)((p.y + 1) * (height - 1) / 2f)
        );
    }

    private List<Vector2Int> Bresenham(Vector2Int Start, Vector2Int End)
    {
        List<Vector2Int> points = new List<Vector2Int>();

        int dx = End.x - Start.x;

        if (dx < 0) return Bresenham(End, Start);

        int dy = End.y - Start.y;

        if (dy < 0)
            return NegY(Bresenham(NegY(Start), NegY(End)));

        if (dy > dx)
            return SwapXY(Bresenham(SwapXY(Start), SwapXY(End)));

        int neg = 2 * (dy - dx);
        int pos = 2 * dy;
        int p = 2 * dy - dx;

        for (int x = Start.x, y = Start.y; x <= End.x; x++)
        {
            points.Add(new Vector2Int(x, y));
            if (p < 0)
            {
                p += pos;
            }
            else
            {
                y++;
                p += neg;
            }
        }
        return points;
    }


    private List<Vector2Int> SwapXY(List<Vector2Int> l)
    {
        List<Vector2Int> hold = new List<Vector2Int>();
        foreach (Vector2Int v in l)
            hold.Add(SwapXY(v));

        return hold;
    }

    private Vector2Int SwapXY(Vector2Int v)
    {
        return new Vector2Int(v.y, v.x);
    }

    private List<Vector2Int> NegY(List<Vector2Int> l)
    {
        List<Vector2Int> hold = new List<Vector2Int>();
        foreach (Vector2Int v in l)
            hold.Add(NegY(v));
        return hold;
    }

    private Vector2Int NegY(Vector2Int v)
    {
        return new Vector2Int(v.x, -v.y);
    }
    
    void Update()
    {
        List<Vector4> verts4 = convertToHomg(myModel.vertices);
        Matrix4x4 rot = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(Time.time * 10, new Vector3(0, 1, 0)),
            Vector3.one);
        Matrix4x4 worldMatrix = rot * Matrix4x4.TRS(new Vector3(0, 0, 10), Quaternion.identity, Vector3.one);
   
        worldMatrix *= rot;
        Matrix4x4 viewMatrix = Matrix4x4.LookAt(new Vector3(0, 0, 0), new Vector3(0, 0, 10), new Vector3(0, 1, 0));
        Matrix4x4 projectionMat = Matrix4x4.Perspective(90, 1, 1, 1000);
        Matrix4x4 mvp = projectionMat * viewMatrix * worldMatrix;
        
        List<Vector4> viewVerts = applyTransformation(verts4, viewMatrix * worldMatrix); // backface culling
        // get verts before projection ^^^^^^^^
        List<Vector4> projectedVerts = applyTransformation(verts4, mvp);
        
        List<Vector2Int> pixelPoints = pixelize(projectedVerts, 512, 512);

        List<Vector3Int> faces = myModel.faces;
        Texture2D fb = new Texture2D(512, 512);
        foreach (Vector3Int face in faces)
        {   
            // more backface culling
            if (isFaceVisible(face, viewVerts))
                continue; // skip invisible face
            Vector2 p1 = projectedVerts[face.x];
            Vector2 p2 = projectedVerts[face.y];
            Vector2 p3 = projectedVerts[face.z];

         //   if (LineClip(ref p1, ref p2))
         //  {
         //  }
         
            Vector2Int v1 = pixelPoints[face.x];
            Vector2Int v2 = pixelPoints[face.y];
            Vector2Int v3 = pixelPoints[face.z];
            
            drawLine(v1, v2, fb);
            drawLine(v2, v3, fb);
            drawLine(v3, v1, fb);
        }
        
        screen.material.mainTexture = fb;
        fb.Apply();
    }
    
    private void drawLine(Vector2 v1, Vector2 v2, Texture2D texture, Color col)
    {
        Vector2 s = v1;
        Vector2 e = v2;

        if (LineClip(ref s, ref e))
        {
            List<Vector2Int> pts = Bresenham(convertToPixel(s, texture.width, texture.height),
                convertToPixel(e, texture.width, texture.height));
            
            foreach (var p in pts)
                texture.SetPixel(p.x, p.y, col);
        }
    }
    
    private bool isFaceVisible(Vector3Int face, List<Vector4> viewVerts) // derive surface normal for face then check z-value
    {
        // vertex positions in view space before projection
        Vector3 v0 = viewVerts[face.x];
        Vector3 v1 = viewVerts[face.y];
        Vector3 v2 = viewVerts[face.z];

        // two edges of triangle
        Vector3 e1 = v1 - v0;
        Vector3 e2 = v2 - v0;
        
        Vector3 normal = Vector3.Cross(e1, e2); // n = (b - a) × (c - a) formula
        
        return normal.z < 0f;
    }
}