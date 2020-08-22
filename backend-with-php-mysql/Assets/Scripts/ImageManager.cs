using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
    public static ImageManager instance;
    private string _basePath;
    private JObject _versionJson;   // 이미지별 버전정보를 저장할 json
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

        LoadJsonVersion();
    }

    private bool ImageExists(string name)
    {
        string path = Path.Combine(_basePath, name);
        return File.Exists(path);
    }

    public void SaveImage(string name, byte[] bytes, int imgVer)
    {
        string path = Path.Combine(_basePath, name);
        File.WriteAllBytes(path, bytes);
        UpdateJsonVersion(name, imgVer);
    }

    public byte[] LoadImage(string name, int imgVer)
    {
        string path = Path.Combine(_basePath, name);
        byte[] bytes = new byte[0];

        // 이미지 버전이 최신이 아니면 empty array를 리턴(새로 서버에서 받는다)
        if (IsImageUpToDate(name, imgVer) == false)
            return bytes;

        // 이미지가 local에 존재하면 서버에서 받지 않고 그것을 로드한다
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

    void UpdateJsonVersion(string name, int imgVer)
    {
        _versionJson[name] = imgVer;
    }
    
    bool IsImageUpToDate(string name, int ver)
    {
        if(_versionJson[name] != null)
            return (int)_versionJson[name] >= ver;

        return false;
    }

    private void LoadJsonVersion()
    {
        if (File.Exists(_versionJsonPath))
        {
            string jsonString = File.ReadAllText(_versionJsonPath);
            _versionJson = JObject.Parse(jsonString);
        }
    }

    public void SaveVersionJson()
    {
        File.WriteAllText(_versionJsonPath, _versionJson.ToString());
    }
}
