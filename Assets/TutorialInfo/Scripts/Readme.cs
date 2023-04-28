using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Readme : ScriptableObject
{
    [FormerlySerializedAs("icon")] public Texture2D _icon;
    [FormerlySerializedAs("title")] public string _title;
    [FormerlySerializedAs("sections")] public Section[] _sections;
    [FormerlySerializedAs("loadedLayout")] public bool _loadedLayout;

    [Serializable]
    public class Section
    {
        [FormerlySerializedAs("heading")] public string _heading;
        [FormerlySerializedAs("text")] public string _text;
        [FormerlySerializedAs("linkText")] public string _linkText;
        [FormerlySerializedAs("url")] public string _url;
    }
}