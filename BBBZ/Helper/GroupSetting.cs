using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

public class GroupSetting
{
    static int? _guestGroupId;
    public static int? GuestGroupId
    {
        get { return _guestGroupId; }
        set 
        {
            _guestGroupId = value;
            WriteNewSetting(_newUserGroupId, _guestGroupId); 
        }
    }

    static int? _newUserGroupId;
    public static int? NewUserGroupId
    {
        get { return _newUserGroupId; }
        set 
        {
            _newUserGroupId = value;
            WriteNewSetting(_newUserGroupId, _guestGroupId); 
        }
    }

    static void WriteNewSetting(int? newUserGroup, int? guestGroup)
    {
        if (File.Exists("\\setting.txt") == false)
            File.CreateText("\\setting.txt");

        File.WriteAllLines("\\setting.txt", new string[] { "newUserGroup=" + newUserGroup, "guestGroup=" + guestGroup });
    }

    static GroupSetting()
    {
        if (File.Exists("\\setting.txt") == false)
            File.CreateText("\\setting.txt");
        string[] setting = File.ReadAllLines("\\setting.txt");
        int t;
        foreach (var i in setting)
            if (i.StartsWith("newUserGroup") && int.TryParse(i.Substring(i.IndexOf("=") + 1), out t))
                _newUserGroupId = t;
            else if (i.StartsWith("guestGroup") && int.TryParse(i.Substring(i.IndexOf("=") + 1), out t))
                _guestGroupId = t;
    }

    public GroupSetting() { }
}