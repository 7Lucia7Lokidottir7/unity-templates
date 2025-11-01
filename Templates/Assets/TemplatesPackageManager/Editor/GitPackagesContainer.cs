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
                    name      = "Menu",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/Menu",
                    packageId = "com.pg.menu",
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
                new GitPackage {
                    name      = "Hunger System",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/HungerSystem",
                    packageId = "com.pg.hunger-system"
                },
                new GitPackage {
                    name      = "Inventory System",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/InventorySystem",
                    packageId = "com.pg.inventory-system"
                },
                new GitPackage {
                    name      = "Loot System",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/LootSystem",
                    packageId = "com.pg.loot-system",
                    dependencies = new List<GitPackage>
                    {
                        new GitPackage
                        {
                            name      = "Inventory System",
                            url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/InventorySystem",
                            packageId = "com.pg.inventory-system"
                        },
                        new GitPackage {
                            name      = "Interact System",
                            url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/InteractSystem",
                            packageId = "com.pg.interact-system"
                        }
                    }
                },
                new GitPackage {
                    name      = "Shoot System",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/ShootSystem",
                    packageId = "com.pg.shoot-system"
                },
                new GitPackage {
                    name      = "PG Dialogue Graph",
                    url       = "https://github.com/7Lucia7Lokidottir7/Dialogue-Graph.git?path=/Dialogue%20Graph/Assets/Dialogue%20Graph",
                    packageId = "com.pg.dialogue-graph"
                },
                new GitPackage {
                    name      = "PG Story Graph",
                    url       = "https://github.com/7Lucia7Lokidottir7/Story-Graph.git?path=/Story%20Graph/Assets/PG/com.pg.story-graph",
                    packageId = "com.pg.story-graph"
                },
                new GitPackage {
                    name      = "PGTween",
                    url       = "https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/PGTween",
                    packageId = "com.pg.pgtween"
                },
                new GitPackage {
                    name      = "PG Hierarchy Folder Creator",
                    url       = "https://github.com/7Lucia7Lokidottir7/PG-Hierarchy-Folder-Creator.git?path=/Hierarchy%20Folder%20Creator/Assets/PG%20Hierarchy%20Folder%20Creator",
                    packageId = "com.pg.hierarchy-folder-creator"
                }
            };
    }
}
