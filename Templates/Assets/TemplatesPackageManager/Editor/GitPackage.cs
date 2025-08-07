using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
namespace PG.TemplatesPackageManager
{

    public class GitPackage
    {
        public string name;
        public string url;
        public List<GitPackage> dependencies = new List<GitPackage>();
        public string status = "Waiting";
        public AddRequest request;
        public bool isInstalling = false;
        public bool isInstalled = false;
    }
}