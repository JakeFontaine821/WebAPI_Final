using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataClass
{
    public string _id;
    public string username;
    public string password;
    public int savedScore;
}

[Serializable]
public class SaveList
{
    public DataClass[] accountsaves;
}