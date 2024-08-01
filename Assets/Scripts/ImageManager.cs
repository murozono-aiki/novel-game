using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    [SerializeField] GameManagerScript gameManagerScript;

    [SerializeField] Image backgroundImage;
    [SerializeField] Image eventCGImage;
    
    [SerializeField] SpriteDictionary[] backgroundSprites;
    [SerializeField] SpriteDictionary[] eventCGSprites;
    [SerializeField] Sprite blankSprite;

    void Start()
    {
        eventCGImage.enabled = false;
    }
    
    public void PutImage(string imageType, string imageName)
    {
        if (imageType == "background")
        {
            backgroundImage.sprite = GetSpriteFromImageName(backgroundSprites, imageName);
        }
        else if (imageType == "event")
        {
            eventCGImage.sprite = GetSpriteFromImageName(eventCGSprites, imageName);
            eventCGImage.enabled = true;
        }
    }

    public void RemoveImage(string imageType)
    {
        if (imageType == "background")
        {
            backgroundImage.sprite = blankSprite;
        }
        else if (imageType == "event")
        {
            eventCGImage.sprite = blankSprite;
            eventCGImage.enabled = false;
        }
    }

    Sprite GetSpriteFromImageName(SpriteDictionary[] sprites, string imageName)
    {
        foreach (SpriteDictionary spriteDictionary in sprites)
        {
            if (spriteDictionary.name == imageName)
            {
                return spriteDictionary.sprite;
            }
        }
        return blankSprite;
    }

    [Serializable]
    class SpriteDictionary
    {
        public string name;
        public Sprite sprite;
    }
}
