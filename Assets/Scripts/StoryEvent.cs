using UnityEngine;
using System.Collections.Generic;

public enum EventType { Combat, Humanitarian };

public class StoryEvent : ScriptableObject
{
    [System.Serializable]
    public class ResultOptions {
        public enum ResultType { Nothing, Red, Blue };

        public ResultType Type;

        public int ForceRed = 5;

        public int ForceBlue = -5;

        public int LostRed = 100;

        public int LostBlue = 100;
    }

    public string Id;

    public EventType Type;

    public bool RedTeam;

    public bool BlueTeam;

    public string Title;

    public string Description;

    public ResultOptions RedButtonResult;

    public ResultOptions BlueButtonResult;

    public ResultOptions NothingResult;

    public string ResultText;
}