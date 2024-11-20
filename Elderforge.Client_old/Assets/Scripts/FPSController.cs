using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerGameObject;

    [SerializeField]
    private float speed = 0.1f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        playerGameObject.transform.position += playerGameObject.transform.forward * Time.deltaTime * speed;

        // Move the player forward
        //if (Input.GetKey(KeyCode.W))
        //{
        //    playerGameObject.transform.position += playerGameObject.transform.forward * Time.deltaTime * 5;
        //}

        //// Move the player backward
        //if (Input.GetKey(KeyCode.S))
        //{
        //    playerGameObject.transform.position -= playerGameObject.transform.forward * Time.deltaTime * 5;
        //}

        //// Move the player to the right
        //if (Input.GetKey(KeyCode.D))
        //{
        //    playerGameObject.transform.position += playerGameObject.transform.right * Time.deltaTime * 5;
        //}

        //// Move the player to the left
        //if (Input.GetKey(KeyCode.A))
        //{
        //    playerGameObject.transform.position -= playerGameObject.transform.right * Time.deltaTime * 5;
        //}

        //// Rotate with mouse

        //float mouseX = Input.GetAxis("Mouse X");
        //float mouseY = Input.GetAxis("Mouse Y");

        //playerGameObject.transform.Rotate(Vector3.up, mouseX * 5);
        //playerGameObject.transform.Rotate(Vector3.right, mouseY * 5);


    }
}
