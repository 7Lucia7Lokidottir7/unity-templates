using System.Collections.Generic;
using UnityEditor;

namespace PG.TemplatesPackageManager
{

    public partial class GitPackageInstaller : EditorWindow
    {
        private List<GitPackage> packages = new List<GitPackage>
            {
                new GitPackage {
                    name      = "Package Window Updater",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/TemplatesPackageManager",
                    packageId = "com.pg.template-package-manager"
                },
                new GitPackage {
                    name      = "Menu System",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/Menu",
                    packageId = "com.pg.menu-system",
                    dependencies = new List<GitPackage>{
                        new GitPackage {
                            name      = "PGTween",
                            url       = "https://github.com/7Lucia7Lokidottir7/PGTween.git",
                            packageId = "com.pg.pgtween"
                        }
                    }
                },
                new GitPackage {
                    name      = "Interact System",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/InteractSystem",
                    packageId = "com.pg.interact-system"
                },
                new GitPackage {
                    name      = "Quest System",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/QuestSystem",
                    packageId = "com.pg.quest-system"
                },
                new GitPackage {
                    name      = "Health System",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/HealthSystem",
                    packageId = "com.pg.health-system"
                },
                new GitPackage {
                    name      = "Localization System",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/LocalizationSystem",
                    packageId = "com.pg.localization-system"
                },
                new GitPackage {
                    name      = "Battle System",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/BattleSystem",
                    packageId = "com.pg.battle-system"
                },
                new GitPackage {
                    name      = "Locomotion System",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/LocomotionSystem",
                    packageId = "com.pg.locomotion-system"
                },
                new GitPackage {
                    name      = "VFX Control",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/VFXControl",
                    packageId = "com.pg.vfx-control"
                },
            };
    }
}