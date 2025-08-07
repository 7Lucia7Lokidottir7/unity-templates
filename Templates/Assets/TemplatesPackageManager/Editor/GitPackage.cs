using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
namespace PG.TemplatesPackageManager
{


    public class GitPackage
    {
        // Display name
        public string name;
        // Git URL
        public string url;
        // UPM package name из package.json ("name": "com.pg.xxx")
        public string packageId;
        public List<GitPackage> dependencies = new List<GitPackage>();
        public string status = "Waiting";
        public bool isInstalling = false;
        public bool isInstalled = false;
        public AddRequest request;
    }
}