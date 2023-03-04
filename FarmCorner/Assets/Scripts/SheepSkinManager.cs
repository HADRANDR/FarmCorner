using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSkinManager : MonoBehaviour
{
    float horizontalF = 0.0005f;
    float verticalF = 0.00025f;
    public static int skin = 0;
    public static Vector3 vectorHorizontalPhase, vectorVerticalPhase;
    float phaseValue;
    public static bool phasecontrol = true;
    float time;
    void Awake()
    {
        time = 0;
    }
    // Update is called once per frame
    void FixedUpdate()  
    {
        if (gameObject.transform.localScale.x<=1.4f && gameObject.transform.localScale.y <= 1.4f && gameObject.transform.localScale.z <= 1.4f && gameObject.transform.localPosition.y <=0.2f)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + horizontalF, gameObject.transform.localScale.y + horizontalF, gameObject.transform.localScale.z + horizontalF);
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + verticalF, gameObject.transform.localPosition.z);           
            if (gameObject.transform.localScale.x < 1.2f && gameObject.transform.localScale.y < 1.2f && gameObject.transform.localScale.z < 1.2f && gameObject.transform.localPosition.y < 0.1f)
            {
                gameObject.tag = "Skin1";
            }
            else if (gameObject.transform.localScale.x > 1.2f && gameObject.transform.localScale.y > 1.2f && gameObject.transform.localScale.z > 1.2f && gameObject.transform.localPosition.y > 0.1f && gameObject.transform.localScale.x < 1.4f && gameObject.transform.localScale.y < 1.4f && gameObject.transform.localScale.z < 1.4f && gameObject.transform.localPosition.y < 0.2f)
            {
                gameObject.tag = "Skin2";
            }
            else if (gameObject.transform.localScale.x >= 1.4f && gameObject.transform.localScale.y >= 1.4f && gameObject.transform.localScale.z >= 1.4f && gameObject.transform.localPosition.y >= 0.2f)
            {
                gameObject.tag = "Skin3";
            }
        }
    }
    private void GrowObject()
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + horizontalF, gameObject.transform.localScale.y + horizontalF, gameObject.transform.localScale.z + horizontalF);
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + verticalF, gameObject.transform.localPosition.z);
    }

}
