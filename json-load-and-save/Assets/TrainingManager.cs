using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Exercise
{
    public string name;
    public int count;
}

[System.Serializable]
public class Record
{
    public string date;
    public int trainingTime;
    public int totalCalories;
    public string trainingName;
    public List<Exercise> exercises = new List<Exercise>();
}

[System.Serializable]
public class TrainingResult
{
    public string id;
    public string name;
    public List<Record> records = new List<Record>();
}

public class TrainingManager : MonoBehaviour
{
    public TrainingResult trainingResult;

    [ContextMenu("Save To Json")]
    void SaveDataToJson()
    {
        string userName = trainingResult.id;
        string jsonData = JsonUtility.ToJson(trainingResult, true);
        string path = Path.Combine(Application.dataPath, $"{userName}.json");
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("Load From Json Data")]
    void LoadDataFromJson()
    {
        string path = Path.Combine(Application.dataPath, "user1.json");
        string jsonData = File.ReadAllText(path);
        trainingResult = JsonUtility.FromJson<TrainingResult>(jsonData);

        Debug.Log($"id : {trainingResult.id}");
        Debug.Log($"name : {trainingResult.name}");

        string selectedDate = "2020-08-07"; // 가져올 날짜

        var sums = trainingResult.records
            .GroupBy(x => x.date)
            .Select(g => new
            {
                Date = g.Key,
                Time = g.Sum(x => x.trainingTime),
                Calories = g.Sum(x => x.totalCalories)
            })
            .Where(c => c.Date == selectedDate);

        foreach (var sum in sums)
            Debug.Log(sum);

        var trainings = trainingResult.records
            .Select(x => x)
            .Where(c => c.date == selectedDate);

        foreach (var training in trainings)
        {
            Debug.Log($"training : {training.trainingName}");
            Debug.Log(string.Join("/", training.exercises.Select(x => $"{x.name} {x.count}회")));
        }
    }

    private void Start()
    {
        LoadDataFromJson();
    }
}
