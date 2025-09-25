using System.Collections.Generic;
using UnityEngine;

public class Model
{
List<Vector3> vertices;
private List<Vector3Int> faces;
List<Vector2> texture_coordinates;
private List<Vector3Int> texture_index_list;
private List<Vector3> normals;
public Model()
{
  defineVertices();
  defineTextureVertices();
  defineFaces();
  CreateUnityGameObject();
}

void defineVertices()
{
  vertices = new List<Vector3>();
  vertices.Add(new Vector3( -1f,-4f,1f ) );  // 0 x y z
  vertices.Add(new Vector3( -2f,-5f,1f ) );  // 1
  vertices.Add(new Vector3( 1f,-5f,1f ) );  // 2
  vertices.Add(new Vector3( 1f,4f,1f ) );  // 3
  vertices.Add(new Vector3( 0f,5f,1f ) );  // 4
  vertices.Add(new Vector3( -2f,5f,1f ) );  // 5
  vertices.Add(new Vector3( -1f,3f,1f ) );  // 6
  vertices.Add(new Vector3( 0f,3f,1f ) );  // 7
  vertices.Add(new Vector3( 0f,-4f,1f ) );  // 8
  
  vertices.Add(new Vector3( 0f,-4f,-1f ) ); // 9
  vertices.Add(new Vector3( 1f,-5f,-1f ) ); // 10
  vertices.Add(new Vector3( -2f,-5f,-1f ) ); // 11
  vertices.Add(new Vector3( -2f,5f,-1f ) ); // 12
  vertices.Add(new Vector3( 0f,5f,-1f ) ); // 13
  vertices.Add(new Vector3( 1f,4f,-1f ) ); // 14
  vertices.Add(new Vector3( 0f,3f,-1f ) ); // 15
  vertices.Add(new Vector3( -1f,3f,-1f ) ); // 16
  vertices.Add(new Vector3( -1f,-4f,-1f ) );  // 17
  
  
}

void defineTextureVertices()
{
  List<Vector2Int> coords = new List<Vector2Int>();
  
        
  // FRONT FACE
  coords.Add(new Vector2Int(32, 87)); // point 0
  coords.Add(new Vector2Int(20, 100)); // point 1
  coords.Add(new Vector2Int(53, 100)); // point 2
  coords.Add(new Vector2Int(53, 41)); // point 3
  coords.Add(new Vector2Int(42, 28)); // point 4
  coords.Add(new Vector2Int(20, 28)); // point 5
  coords.Add(new Vector2Int(32, 45)); // point 6
  coords.Add(new Vector2Int(40, 45)); // point 7
  coords.Add(new Vector2Int(40, 87)); // point 8
  
  // BACK FACE
  
  coords.Add(new Vector2Int(85, 88)); // point 9
  coords.Add(new Vector2Int(72, 101)); // point 10
  coords.Add(new Vector2Int(105, 101)); // point 11
  coords.Add(new Vector2Int(105, 29)); // point 12
  coords.Add(new Vector2Int(83, 29)); // point 13
  coords.Add(new Vector2Int(72, 42)); // point 14
  coords.Add(new Vector2Int(85, 46)); // point 15
  coords.Add(new Vector2Int(93, 46)); // point 16
  coords.Add(new Vector2Int(93, 88)); // point 17
  
  // BOTTOM
  coords.Add(new Vector2Int(53, 118)); // point 18
  coords.Add(new Vector2Int(53, 106)); // point 19
  coords.Add(new Vector2Int(19, 106)); // point 20
  coords.Add(new Vector2Int(19, 118)); // point 21
  

  // BACK /////////////////////////////////////////////////
  coords.Add(new Vector2Int(18, 100)); // point 22
  coords.Add(new Vector2Int(2, 100)); // point 23
  coords.Add(new Vector2Int(2, 28)); // point 24
  coords.Add(new Vector2Int(18, 28)); // point 25
  
  // TOP
  coords.Add(new Vector2Int(20, 14)); // point 26
  coords.Add(new Vector2Int(20, 26)); // point 27
  coords.Add(new Vector2Int(42, 14)); // point 28
  coords.Add(new Vector2Int(42, 26)); // point 29
  
  // TOP (CURVED)
  coords.Add(new Vector2Int(51, 22)); // point 30
  coords.Add(new Vector2Int(51, 35)); // point 31
  coords.Add(new Vector2Int(70, 22)); // point 32
  coords.Add(new Vector2Int(70, 35)); // point 33
  
  // FRONT
  
  coords.Add(new Vector2Int(55, 40)); // point 34
  coords.Add(new Vector2Int(55, 100)); // point 35
  coords.Add(new Vector2Int(70, 40)); // point 36
  coords.Add(new Vector2Int(70, 100)); // point 37
  
  texture_coordinates = convertToRelative(coords);
  
}

private List<Vector2> convertToRelative(List<Vector2Int> coords)
{
  List<Vector2> text_coord = new List<Vector2>();

  foreach (Vector2Int coord in coords)
  {
    text_coord.Add(new Vector2(coord.x / 128f, 1 - coord.y / 128f));
  }
  return text_coord;
}

void defineFaces() {
  // COUNTER-CLOCKWISE FRONT FACE
  faces = new List<Vector3Int>();
  texture_index_list = new List<Vector3Int>();
  normals =  new List<Vector3>();

        
        faces.Add(new Vector3Int(1, 5, 6)); texture_index_list.Add(new Vector3Int(1,5,6)); normals.Add(new Vector3(0,0,-1));// triangle 1
        faces.Add(new Vector3Int(1, 0, 2)); texture_index_list.Add(new Vector3Int(1, 0, 2)); normals.Add(new Vector3(0,0,-1)); // triangle 2
        faces.Add(new Vector3Int(1, 6, 0)); texture_index_list.Add(new Vector3Int(1, 6, 0)); normals.Add(new Vector3(0,0,-1)); // triangle 9
        faces.Add(new Vector3Int(2, 0, 8)); texture_index_list.Add(new Vector3Int(2, 0, 8)); normals.Add(new Vector3(0,0,-1)); // triangle 3
        faces.Add(new Vector3Int(2, 8, 3)); texture_index_list.Add(new Vector3Int(2, 8, 3)); normals.Add(new Vector3(0,0,-1)); // triangle 4
        faces.Add(new Vector3Int(8, 7, 3)); texture_index_list.Add(new Vector3Int(8, 7, 3)); normals.Add(new Vector3(0,0,-1)); // triangle 5
        faces.Add(new Vector3Int(7, 4, 3)); texture_index_list.Add(new Vector3Int(7, 4, 3)); normals.Add(new Vector3(0,0,-1)); // triangle 6
        faces.Add(new Vector3Int(6, 4, 7)); texture_index_list.Add(new Vector3Int(6, 4, 7)); normals.Add(new Vector3(0,0,-1)); // triangle 7
        faces.Add(new Vector3Int(6, 5, 4)); texture_index_list.Add(new Vector3Int(6, 5, 4)); normals.Add(new Vector3(0,0,-1)); // triangle 8

        // CLOCKWISE BACKFACE
        faces.Add(new Vector3Int(10, 14, 15)); texture_index_list.Add(new Vector3Int(10, 14, 15)); normals.Add(new Vector3(0,0,1)); // triangle 10
        faces.Add(new Vector3Int(10, 9, 11)); texture_index_list.Add(new Vector3Int(10, 9, 11));  normals.Add(new Vector3(0,0,1));  // triangle 11
        faces.Add(new Vector3Int(9, 17, 11)); texture_index_list.Add(new Vector3Int(9, 17, 11));  normals.Add(new Vector3(0,0,1));  // triangle 12
        faces.Add(new Vector3Int(17, 16, 11)); texture_index_list.Add(new Vector3Int(17, 16, 11));  normals.Add(new Vector3(0,0,1));  // triangle 13
        faces.Add(new Vector3Int(11, 16, 12)); texture_index_list.Add(new Vector3Int(11, 16, 12));  normals.Add(new Vector3(0,0,1));  // triangle 14
        faces.Add(new Vector3Int(16, 13, 12)); texture_index_list.Add(new Vector3Int(16, 13, 12));  normals.Add(new Vector3(0,0,1));  // triangle 15
        faces.Add(new Vector3Int(15, 13, 16)); texture_index_list.Add(new Vector3Int(15, 13, 16));  normals.Add(new Vector3(0,0,1));  // triangle 16
        faces.Add(new Vector3Int(14, 13, 15)); texture_index_list.Add(new Vector3Int(14, 13, 15));  normals.Add(new Vector3(0,0,1));  // triangle 17
        faces.Add(new Vector3Int(10, 15, 9)); texture_index_list.Add(new Vector3Int(10, 15, 9));  normals.Add(new Vector3(0,0,1));  // triangle 18

        // SIDE FACES
        // BOTTOM                               
                                            // texture_index_list.Add(new Vector3Int(1, 2, 10));
                                            // texture_index_list.Add(new Vector3Int(1, 10, 11));
        faces.Add(new Vector3Int(1, 2, 10)); texture_index_list.Add(new Vector3Int(21, 18, 19));  normals.Add(new Vector3(0,-1,0));  // triangle 19
        faces.Add(new Vector3Int(1, 10, 11)); texture_index_list.Add(new Vector3Int(21, 19, 20));  normals.Add(new Vector3(0,-1,0));  // triangle 20
        // FRONT (FLAT PART)                
                                            // texture_index_list.Add(new Vector3Int(2, 3, 10));
                                            // texture_index_list.Add(new Vector3Int(10, 3, 14));
        faces.Add(new Vector3Int(2, 3, 10)); texture_index_list.Add(new Vector3Int(35, 34, 37));  normals.Add(new Vector3(1,0,1));  // triangle 21
        faces.Add(new Vector3Int(10, 3, 14)); texture_index_list.Add(new Vector3Int(37, 34, 36));  normals.Add(new Vector3(1,0,1));  // triangle 22
        // FRONT (CURVED PART)
                                             // texture_index_list.Add(new Vector3Int(3, 4, 14));
                                            // texture_index_list.Add(new Vector3Int(14, 4, 13));
        faces.Add(new Vector3Int(3, 4, 14)); texture_index_list.Add(new Vector3Int(31, 30, 33));  normals.Add((new Vector3(1,1,0)).normalized); // triangle 23
        faces.Add(new Vector3Int(14, 4, 13)); texture_index_list.Add(new Vector3Int(33, 30, 32));  normals.Add((new Vector3(1,1,0)).normalized); // triangle 24
        // ROOF
                                            // texture_index_list.Add(new Vector3Int(13, 4, 12));
                                            // texture_index_list.Add(new Vector3Int(12, 4, 5));
        faces.Add(new Vector3Int(13, 4, 12)); texture_index_list.Add(new Vector3Int(27, 26, 29));  normals.Add(new Vector3(0,1,0));// triangle 25
        faces.Add(new Vector3Int(12, 4, 5)); texture_index_list.Add(new Vector3Int(29, 26, 28));  normals.Add(new Vector3(0,1,0));// triangle 26
              
        // BACK                               texture_index_list.Add(new Vector3Int(11, 12, 1));
                                            //texture_index_list.Add(new Vector3Int(1, 12, 5));
        faces.Add(new Vector3Int(11, 12, 1)); texture_index_list.Add(new Vector3Int(23, 24, 22));  normals.Add(new Vector3(-1,0,0));// triangle 27
        faces.Add(new Vector3Int(1, 12, 5)); texture_index_list.Add(new Vector3Int(22, 24, 25));  normals.Add(new Vector3(-1,0,0));// triangle 28

        // INSIDE SIDE FACES
        // INSIDE BOTTOM

        // INSIDE ROOF

        // INSIDE FRONT

        // INSIDE BACK
}

public GameObject CreateUnityGameObject()
{
  Mesh mesh = new Mesh();
  GameObject newGO = new GameObject();
     
  MeshFilter mesh_filter = newGO.AddComponent<MeshFilter>();
  MeshRenderer mesh_renderer = newGO.AddComponent<MeshRenderer>();

  List<Vector3> coords = new List<Vector3>();
  List<int> dummy_indices = new List<int>();
  List<Vector2> text_coords = new List<Vector2>();
  List<Vector3> normalz = new List<Vector3>();

  for (int i = 0; i < faces.Count; i++)
  {
    Vector3 normal_for_face = normals[i];

    normal_for_face = new Vector3(normal_for_face.x, normal_for_face.y, -normal_for_face.z);

    coords.Add(vertices[faces[i].x]); dummy_indices.Add(i * 3); text_coords.Add(texture_coordinates[texture_index_list[i].x]); normalz.Add(normal_for_face);

    coords.Add(vertices[faces[i].y]); dummy_indices.Add(i * 3 + 2); text_coords.Add(texture_coordinates[texture_index_list[i].y]); normalz.Add(normal_for_face);

    coords.Add(vertices[faces[i].z]); dummy_indices.Add(i * 3 + 1); text_coords.Add(texture_coordinates[texture_index_list[i].z]);normalz.Add(normal_for_face);
  }

  mesh.vertices = coords.ToArray();
  mesh.triangles = dummy_indices.ToArray();
  mesh.uv = text_coords.ToArray();
  mesh.normals = normalz.ToArray();
  mesh_filter.mesh = mesh;

  return newGO;
}

}
