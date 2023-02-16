using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TestingSet
{
    public int id;
    public string title;
    public List<TestingStage> stages;
}

[Serializable]
public class TestingStage
{
    public int id;
    public int testingSetId;
    public int testingId;
    public string title;
    public int sortOrder;
}

[Serializable]
public class Testing
{
    public int id;
    public string title;
    public int type; // 1 | 2 | 3 - common | pano | vr
    public bool isShuffleQuestions;
    public int time;
    public List<TestingQuestion> questions;
    public int resultTime;
}

[Serializable]
public class TestingQuestion
{
	public int id;
    public int testingId;
    public string title;
    public int type; // 1 | 2 | 3 - single | mult | free
    public int pano;
    public int vrExperience;
    public List<TestingAnswer> answers;
    public bool isActive;
    public TestingQuestionResult result;
}

[Serializable]
public class TestingAnswer
{
    public int id;
    public string title;
    public bool isValid;
}

[Serializable]
public class TestingQuestionResult
{
    public List<int> chooseResult;
    public string freeResult;
    public int vrResult;

    public TestingQuestionResult()
    {
        chooseResult = null;
        freeResult = null;
    }
}

[Serializable]
public class TestingVrHelmet
{
    public List<TestingVrHelmetQuestion> questions;
    public int restTime;
    public int allTime;

    public TestingVrHelmet()
    {
        questions = new List<TestingVrHelmetQuestion>();
    }
}

[Serializable]
public class TestingVrHelmetQuestion
{
    public int id;
    public string title;
}

[Serializable]
public class User
{
    public int id;
    public int userCategoryId;
    public string firstName;
    public string middleName;
    public string lastName;
    public DateTime createdAt;
    public UserCategory category;
    //public Guid? resultUid;
}

[Serializable]
public class UserCategory
{
    public int id;
    public string title;
    public int testingSetId;
    public FileModel file;
    //public TestingSet Set { get; set; }
}

[Serializable]
public class JsonContainer<T>
{
    public T data;
    public JsonContainer(T obj)
    {
        data = obj;
    }
}

[Serializable]
public class CompetitionResult
{
    public string uid;
    public User user;
    public List<Testing> testingsObj;
    public CompetitionResult(Guid uid, User user, List<Testing> testingsObj)
    {
        this.uid = uid.ToString();
        this.user = user;
        this.testingsObj = testingsObj;
    }
}

[Serializable]
public class LocalPack
{
    public List<UserCategory> userCategories;
    public List<TestingSet> sets;
    public List<Testing> testings;
}

[Serializable]
public class FileModel
{
    public int id;
    public string path;
    public string name;
}

public static class Extensions
{
    public static T Copy<T>(this T obj)
    {
        var json = JsonUtility.ToJson(obj);
        return JsonUtility.FromJson<T>(json);
    }
}