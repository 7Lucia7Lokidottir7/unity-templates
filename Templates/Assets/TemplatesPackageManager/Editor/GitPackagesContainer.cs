using System.Collections.Generic;
using UnityEditor;

namespace PG.TemplatesPackageManager
{

    public partial class GitPackageInstaller : EditorWindow
    {
        private List<GitPackage> packages = new List<GitPackage>
            {
                new GitPackage {
                    name = "Updating the Package Window",
                    url  = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/TemplatesPackageManager"
                },
                new GitPackage {
                    name = "Menu System",
                    url  = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/Menu",
                    dependencies = new List<GitPackage> {
                        new GitPackage {
                            name = "PGTween",
                            url  = "https://github.com/7Lucia7Lokidottir7/PGTween.git"
                        }
                    }
                },
                new GitPackage { name="Interact System",     url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/InteractSystem" },
                new GitPackage { name="Quest System",        url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/QuestSystem" },
                new GitPackage { name="Health System",       url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/HealthSystem" },
                new GitPackage { name="Localization System", url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/LocalizationSystem" },
                new GitPackage { name="Battle System",       url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/BattleSystem" },
                new GitPackage { name="Locomotion System",   url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/LocomotionSystem" },
                new GitPackage { name="VFX Control",         url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/VFXControl" },
            };
    }
}