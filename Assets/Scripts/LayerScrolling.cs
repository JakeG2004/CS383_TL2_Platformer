using UnityEngine;

public class LayerScrolling2D : MonoBehaviour
{
    [TextArea]
    public string Infinite_Scrolling_Setup = 
            "1. Apply this script to a background/foreground object\n" +
            "2. Duplicate that object 2 times\n" +
            "3. Set them as the main object's children\n" +
            "4. Move them to the right and left of the parent\n" +
            "5. Delete this script from the children";


    private float startPos, length;
    public GameObject cam;
    [Range(0f,1f)]
    public float speed; //speed bg moves relative to the camera

    void Start()
    {
        startPos = transform.position.x;
        //get length of screen for infinite scrolling
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float distance = cam.transform.position.x * speed;
        float movement = cam.transform.position.x * (1- speed);

        transform.position = new Vector3(startPos+distance,transform.position.y,transform.position.z);

        //move the background to the start of the screen upon reaching its bounds
        //must have the bg be 3x its length set in scene to work
        if(movement > startPos +length){
            startPos+=length;
        }else if (movement < startPos -length){
            startPos -= length;
        }
    }
}