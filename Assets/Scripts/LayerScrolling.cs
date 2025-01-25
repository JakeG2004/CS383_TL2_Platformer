using UnityEngine;

public class LayerScrolling2D : MonoBehaviour
{
    
    Material mat;
    Vector3 CameraPosition;
    [Range(0f, 0.5f)]
    public float speed = 0.2f;
    public Transform cameraTransform; //reference to the camera

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        if (cameraTransform == null){ //default initializer
            cameraTransform = Camera.main.transform;
        }
        CameraPosition = cameraTransform.position;
    }

    void Update()
    {
        float deltaX = cameraTransform.position.x - CameraPosition.x;
        //scroll background based on camera movement
        mat.mainTextureOffset += new Vector2(deltaX * speed, 0);
        //update camera position
        CameraPosition = cameraTransform.position;
    }
}