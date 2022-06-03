using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ResizeSpriteByResolution : MonoBehaviour
{
    [SerializeField]
    private Vector2 screenProportions;

    private void Awake()
    {
        ResizeSpriteToScreen(gameObject, Camera.main, screenProportions.x, screenProportions.y);
    }

    private void ResizeSpriteToScreen(GameObject theSprite, Camera theCamera, float fitToScreenWidth, float fitToScreenHeight)
    {        
        // Rescale sprite so it'll cover all of the camera view
        SpriteRenderer sr = theSprite.GetComponent<SpriteRenderer>();

        theSprite.transform.localScale = new Vector3(1,1,1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;
        
        float worldScreenHeight = (float)(theCamera.orthographicSize * 2.0);
        float worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width);
        
        if (fitToScreenWidth != 0)
        {
            Vector2 sizeX = new Vector2(worldScreenWidth / width / fitToScreenWidth,theSprite.transform.localScale.y);
            theSprite.transform.localScale = sizeX;
        }
        
        if (fitToScreenHeight != 0)
        {
            Vector2 sizeY = new Vector2(theSprite.transform.localScale.x, worldScreenHeight / height / fitToScreenHeight);
            theSprite.transform.localScale = sizeY;
        }
    }
}
