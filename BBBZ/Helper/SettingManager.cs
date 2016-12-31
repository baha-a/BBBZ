using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

public class SettingManager
{
    static string settingfile;

    static int? _startupMenu;
    public static int? StartupMenuItem
    {
        get { return _startupMenu; }
        set
        {
            _startupMenu = value;
            WriteNewSetting();
        }
    }


    static int? _guestGroupId;
    public static int? GuestGroupId
    {
        get { return _guestGroupId; }
        set
        {
            _guestGroupId = value;
            WriteNewSetting();
        }
    }

    static int? _newUserGroupId;
    public static int? NewUserGroupId
    {
        get { return _newUserGroupId; }
        set
        {
            _newUserGroupId = value;
            WriteNewSetting();
        }
    }

    static void WriteNewSetting()
    {
        CheckSettingFile(settingfile);

        File.WriteAllLines(settingfile, new string[] 
        {
            "newUserGroup=" + _newUserGroupId,
            "guestGroup=" + _guestGroupId,
            "startupMenu=" + _startupMenu
        });
    }

    static SettingManager()
    {
        settingfile = HostingEnvironment.MapPath("\\setting.txt");

        CheckSettingFile(settingfile);

        string[] setting = File.ReadAllLines(settingfile);
        int t;
        foreach (var i in setting)
            if (i.StartsWith("newUserGroup") && int.TryParse(i.Substring(i.IndexOf("=") + 1), out t))
                _newUserGroupId = t;
            else if (i.StartsWith("guestGroup") && int.TryParse(i.Substring(i.IndexOf("=") + 1), out t))
                _guestGroupId = t;
            else if (i.StartsWith("startupMenu") && int.TryParse(i.Substring(i.IndexOf("=") + 1), out t))
                _startupMenu = t;
    }

    private static void CheckSettingFile(string file)
    {
        if (File.Exists(file) == false)
            File.CreateText(file);
    }

    public SettingManager() { }
}