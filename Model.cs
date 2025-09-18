using System.Collections.Generic;
using UnityEngine;

public class Model
{
List<Vector3> vertices;
private List<Vector3Int> faces;
List<Vector2> texture_Coordinates;
public Model()
{
  defineVertices();
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
  coords.Add(new Vector2Int( 0, 0));
}
void defineFaces() {
  // COUNTER-CLOCKWISE FRONT FACE
  faces = new List<Vector3Int>();
  faces.Add(new Vector3Int(1, 5, 6)); // triangle 1
  faces.Add(new Vector3Int(1, 0, 2)); // triangle 2
 faces.Add(new Vector3Int(1, 6, 0)); // triangle 9
  faces.Add(new Vector3Int(2, 0, 8)); // triangle 3
  faces.Add(new Vector3Int(2, 8, 3)); // triangle 4
  faces.Add(new Vector3Int(8, 7, 3)); // triangle 5
  faces.Add(new Vector3Int(7, 4, 3)); // triangle 6
  faces.Add(new Vector3Int(6, 4, 7)); // triangle 7
  faces.Add(new Vector3Int(6, 5, 4)); // triangle 8
  
  // COUNTER-CLOCKWISE BACKFACE
  /*
  faces.Add(new Vector3Int(10, 15, 14)); // triangle 10
  faces.Add(new Vector3Int(10, 11, 9)); // triangle 11
  faces.Add(new Vector3Int(9, 11, 17)); // triangle 12
  faces.Add(new Vector3Int(17, 11, 16)); // triangle 13
  faces.Add(new Vector3Int(11, 12, 16)); // triangle 14
  faces.Add(new Vector3Int(16, 12, 13)); // triangle 15
  faces.Add(new Vector3Int(15, 16, 13)); // triangle 16
  faces.Add(new Vector3Int(15, 13, 14)); // triangle 17
  */
  
  // CLOCKWISE BACKFACE
  faces.Add(new Vector3Int(10, 14, 15)); // triangle 10
  faces.Add(new Vector3Int(10, 9, 11)); // triangle 11
  faces.Add(new Vector3Int(9, 17, 11)); // triangle 12
  faces.Add(new Vector3Int(17, 16, 11)); // triangle 13
  faces.Add(new Vector3Int(11, 16, 12)); // triangle 14
  faces.Add(new Vector3Int(16, 13, 12)); // triangle 15
  faces.Add(new Vector3Int(15, 13, 16)); // triangle 16
  faces.Add(new Vector3Int(14, 13, 15)); // triangle 17
  faces.Add(new Vector3Int(10, 15, 9)); // triangle 18
  
  // SIDE FACES
  // BOTTOM
  faces.Add(new Vector3Int(1, 2, 10)); // triangle 19
  faces.Add(new Vector3Int(1, 10, 11)); // triangle 20
  // FRONT (FLAT PART)
  faces.Add(new Vector3Int(2, 3, 10)); // triangle 21
  faces.Add(new Vector3Int(10, 3, 14)); // triangle 22
  // FRONT (CURVED PART)
  faces.Add(new Vector3Int(3, 4, 14)); // triangle 23
  faces.Add(new Vector3Int(14, 4, 13)); // triangle 24
  // ROOF
  faces.Add(new Vector3Int(13, 4, 12)); // triangle 25
  faces.Add(new Vector3Int(12, 4, 5)); // triangle 26
  // BACK
  faces.Add(new Vector3Int(11, 12, 1)); // triangle 27
  faces.Add(new Vector3Int(1, 12, 5)); // triangle 28
}

public GameObject CreateUnityGameObject()
{
  Mesh mesh = new Mesh();
  GameObject newGO = new GameObject();
     
  MeshFilter mesh_filter = newGO.AddComponent<MeshFilter>();
  MeshRenderer mesh_renderer = newGO.AddComponent<MeshRenderer>();

  List<Vector3> coords = new List<Vector3>();
  List<int> dummy_indices = new List<int>();
  /*List<Vector2> text_coords = new List<Vector2>();
  List<Vector3> normalz = new List<Vector3>();*/

  for (int i = 0; i < faces.Count; i++)
  {
    //Vector3 normal_for_face = normals[i];

    //normal_for_face = new Vector3(normal_for_face.x, normal_for_face.y, -normal_for_face.z);

    coords.Add(vertices[faces[i].x]); dummy_indices.Add(i * 3); //text_coords.Add(texture_coordinates[texture_index_list[i].x]); normalz.Add(normal_for_face);

    coords.Add(vertices[faces[i].y]); dummy_indices.Add(i * 3 + 2); //text_coords.Add(texture_coordinates[texture_index_list[i].y]); normalz.Add(normal_for_face);

    coords.Add(vertices[faces[i].z]); dummy_indices.Add(i * 3 + 1); //text_coords.Add(texture_coordinates[texture_index_list[i].z]); normalz.Add(normal_for_face);
  }

  mesh.vertices = coords.ToArray();
  mesh.triangles = dummy_indices.ToArray();
  /*mesh.uv = text_coords.ToArray();
  mesh.normals = normalz.ToArray();*/
  mesh_filter.mesh = mesh;

  return newGO;
}

}
