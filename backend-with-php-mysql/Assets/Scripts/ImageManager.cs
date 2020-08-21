using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
    public static ImageManager instance;
    private string _basePath;
    private JObject _versionJson;
    private string _versionJsonPath;

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

        _versionJson = new JObject();
        _versionJsonPath = Path.Combine(_basePath, "VersionJson");

        // Load Json
        if(File.Exists(_versionJsonPath))
        {
            string jsonString = File.ReadAllText(_versionJsonPath);
            _versionJson = JsonConvert.DeserializeObject<JObject>(jsonString); 
        }
    }

    private bool ImageExists(string fileName)
    {
        string path = Path.Combine(_basePath, fileName);
        return File.Exists(path);
    }

    public void SaveImage(string fileName, byte[] bytes, int imgVer)
    {
        string path = Path.Combine(_basePath, fileName);
        File.WriteAllBytes(path, bytes);
        UpdateVersionJson(fileName, imgVer);
    }

    public byte[] LoadImage(string fileName, int imgVer)
    {
        string path = Path.Combine(_basePath, fileName);
        byte[] bytes = new byte[0];

        // 이미지 버전이 최신이 아니면 empty array를 리턴
        if (IsImageUpToDate(fileName, imgVer) == false)
            return bytes;

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

    void UpdateVersionJson(string name, int ver)
    {
        _versionJson[name] = ver;
    }

    bool IsImageUpToDate(string name, int ver)
    {
        if(_versionJson[name] != null)
            return (int)_versionJson[name] >= ver;

        return false;
    }

    public void SaveVersionJson()
    {
        File.WriteAllText(_versionJsonPath, _versionJson.ToString());
    }
}
