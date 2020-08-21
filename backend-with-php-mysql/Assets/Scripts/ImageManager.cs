using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
    public static ImageManager instance;
    private string _basePath;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        _basePath = Path.Combine(Application.persistentDataPath, "icons");
        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);

    }

    private bool ImageExists(string fileName)
    {
        string path = Path.Combine(_basePath, fileName);
        return File.Exists(path);
    }

    public void SaveImage(string fileName, byte[] bytes)
    {
        string path = Path.Combine(_basePath, fileName);
        File.WriteAllBytes(path, bytes);
    }

    public byte[] LoadImage(string fileName)
    {
        byte[] bytes = new byte[0];
        string path = Path.Combine(_basePath, fileName);

        if (ImageExists(path))
            bytes = File.ReadAllBytes(path);

        return bytes;
    }

    // convert bytes into sprite
    public Sprite BytesToSprite(byte[] bytes)
    {
        // create Textrue2D
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        // create sprite (to be placed in UI)
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector3(0.5f, 0.5f));

        return sprite;
    }
}
