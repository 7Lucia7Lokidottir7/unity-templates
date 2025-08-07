using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.TemplatesPackageManager
{
    public class GitPackageInstaller : EditorWindow
    {
        private List<GitPackage> packages;

        [MenuItem("Window/PG/Git Package Installer")]
        [MenuItem("Window/Package Management/Git Package Installer")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<GitPackageInstaller>();
            wnd.titleContent = new GUIContent("Git Installer");
            wnd.minSize = new Vector2(480, 500);
        }

        void CreateGUI()
        {
            // Данные пакетов
            packages = new List<GitPackage>
        {
            new GitPackage {
                name="Updating the Package Window",
                url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/TemplatesPackageManager"
            },
            new GitPackage {
                name="Menu System",
                url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/Menu",
                dependencies = new List<GitPackage>
                {
                    new GitPackage {
                        name="PGTween",
                        url="https://github.com/7Lucia7Lokidottir7/PGTween.git"
                    }
                }
            },
            new GitPackage {
                name="Interact System",
                url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/InteractSystem"
            },
            new GitPackage {
                name="Quest System",
                url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/QuestSystem"
            },
            new GitPackage {
                name="Health System",
                url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/HealthSystem"
            },
            new GitPackage {
                name="Localization System",
                url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/LocalizationSystem"
            },
            new GitPackage {
                name="Battle System",
                url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/BattleSystem"
            },
            new GitPackage {
                name="Locomotion System",
                url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/LocomotionSystem"
            },
            new GitPackage {
                name="VFX Control",
                url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/VFXControl"
            },
        };

            // Корень
            var root = rootVisualElement;
            root.Clear();
            root.style.flexDirection = FlexDirection.Column;
            root.style.paddingLeft = 16;
            root.style.paddingRight = 16;
            root.style.paddingTop = 16;
            root.style.paddingBottom = 6;

            // Заголовок
            var title = new Label("Git Package Installer");
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.fontSize = 20;
            title.style.color = new Color(0.92f, 0.92f, 1, 1);
            title.style.marginBottom = 8;
            root.Add(title);

            var desc = new Label("Installing git packages and auto-checking for updates:");
            desc.style.fontSize = 12;
            desc.style.marginBottom = 8;
            desc.style.color = new Color(0.75f, 0.75f, 0.9f, 1f);
            root.Add(desc);

            // ScrollView
            var scroll = new ScrollView();
            scroll.style.flexGrow = 1f;
            scroll.style.maxHeight = new StyleLength(new Length(100, LengthUnit.Percent));
            root.Add(scroll);

            // --- Стили пакет-боксов ---
            var boxStyle = new StyleColor(new Color(0.11f, 0.13f, 0.18f, 1));
            var green = new StyleColor(new Color(0.32f, 0.88f, 0.44f, 1));
            var yellow = new StyleColor(new Color(0.92f, 0.89f, 0.21f, 1));
            var gray = new StyleColor(new Color(0.7f, 0.7f, 0.7f, 1));
            var blue = new StyleColor(new Color(0.25f, 0.53f, 0.93f, 1));
            var red = new StyleColor(new Color(0.9f, 0.3f, 0.3f, 1));
            var orange = new StyleColor(new Color(1f, 0.6f, 0.12f, 1));

            // --- Отрисовка каждого пакета ---
            foreach (var pkg in packages)
            {
                var box = new VisualElement();
                box.style.flexDirection = FlexDirection.Row;
                box.style.alignItems = Align.Center;
                box.style.paddingTop = 4; box.style.paddingBottom = 4;
                box.style.borderBottomWidth = 1;
                box.style.borderBottomColor = new Color(0.2f, 0.2f, 0.2f, 0.34f);
                box.style.backgroundColor = boxStyle;
                box.style.marginBottom = 2;

                var name = new Label(pkg.name);
                name.style.fontSize = 15;
                name.style.unityFontStyleAndWeight = FontStyle.Bold;
                name.style.color = Color.white;
                name.style.paddingLeft = 5;


                name.style.flexBasis = 0;
                name.style.flexGrow = 2;
                name.style.maxWidth = 250;
                name.style.unityTextAlign = TextAnchor.MiddleLeft;
                name.style.whiteSpace = WhiteSpace.NoWrap;
                name.style.overflow = Overflow.Hidden;
                name.style.textOverflow = TextOverflow.Ellipsis;



                var url = new Label(pkg.url);
                url.style.flexBasis = 0;
                url.style.flexGrow = 4;
                url.style.fontSize = 10;
                url.style.color = blue;
                url.style.marginLeft = 6;
                url.style.maxWidth = 540;
                url.style.unityTextAlign = TextAnchor.MiddleLeft;
                url.style.whiteSpace = WhiteSpace.NoWrap;
                url.style.overflow = Overflow.Hidden;
                url.style.textOverflow = TextOverflow.Ellipsis;
                url.RegisterCallback<MouseDownEvent>(evt => {
                    if (evt.button == 0) Application.OpenURL(pkg.url);
                });
                url.RegisterCallback<MouseEnterEvent>(evt => {
                    url.text = $"<u>{pkg.url}</u>";
                });
                url.RegisterCallback<MouseLeaveEvent>(evt => {
                    url.text = pkg.url;
                });


                var status = new Label(pkg.status);
                status.style.width = 120;
                status.style.marginLeft = 10;
                status.style.fontSize = 13;
                status.style.color = yellow;

                var installBtn = new Button(() => InstallPackageWithDeps(pkg, status, installBtn: null))
                {
                    text = "Install"
                };
                installBtn.style.width = 85;
                installBtn.style.marginLeft = 8;
                installBtn.style.fontSize = 13;
                installBtn.style.unityFontStyleAndWeight = FontStyle.Bold;
                installBtn.style.backgroundColor = blue;
                installBtn.style.color = Color.white;

                // Для обновления UI из async методов
                void Refresh()
                {
                    status.text = pkg.status;
                    if (pkg.isInstalling)
                    {
                        installBtn.SetEnabled(false);
                        installBtn.text = "Installing...";
                    }
                    else if (pkg.isInstalled)
                    {
                        installBtn.SetEnabled(pkg.hasUpdate);
                        installBtn.text = pkg.hasUpdate ? "Update" : "Installed";
                        status.style.color = pkg.hasUpdate ? orange : green;
                        status.text = pkg.hasUpdate ?
                            $"Update: {pkg.currentVersion} → {pkg.latestVersion}" :
                            $"Installed ({pkg.currentVersion ?? "-"})";
                    }
                    else
                    {
                        installBtn.SetEnabled(true);
                        installBtn.text = "Install";
                        status.style.color = yellow;
                        status.text = pkg.status;
                    }
                }

                box.Add(name);
                box.Add(url);
                box.Add(installBtn);
                box.Add(status);
                scroll.Add(box);

                // Инициализация статусов и запуск проверки
                pkg.status = "Checking...";
                pkg.isInstalled = false;
                pkg.isInstalling = false;
                pkg.hasUpdate = false;
                pkg.currentVersion = null;
                pkg.latestVersion = null;
                Refresh();

                // --- Проверка установлен ли пакет и есть ли обновление ---
                CheckPackageStatus(pkg, Refresh);
            }
        }

        // Проверяет: установлен ли пакет и актуальна ли версия (версии берутся из package.json локально и с гита)
        private async void CheckPackageStatus(GitPackage pkg, Action onStatusChanged)
        {
            // 1. Проверяем установлен ли пакет в Unity
            string localVersion = null;
            string installedPackageId = null;
            var listReq = Client.List(true);
            while (!listReq.IsCompleted)
                await Task.Delay(40);

            if (listReq.Status == StatusCode.Success)
            {
                foreach (var up in listReq.Result)
                {
                    // Находим по имени или по урлу гита
                    if (!string.IsNullOrEmpty(pkg.name) && (up.name.Replace(" ", "").ToLower().Contains(pkg.name.Replace(" ", "").ToLower())
                        || (!string.IsNullOrEmpty(pkg.url) && up.packageId.Contains(GetRepoName(pkg.url)))))
                    {
                        pkg.isInstalled = true;
                        localVersion = up.version;
                        installedPackageId = up.packageId;
                    }
                }
            }
            pkg.currentVersion = localVersion;

            // 2. Достаём версию из package.json на GitHub
            string latestVersion = await GetGitPackageJsonVersion(pkg.url);
            pkg.latestVersion = latestVersion;

            // 3. Есть ли обновление
            pkg.hasUpdate = pkg.isInstalled && !string.IsNullOrEmpty(latestVersion) && localVersion != latestVersion;

            pkg.status = pkg.isInstalled
                ? (pkg.hasUpdate ? $"Update: {localVersion} -> {latestVersion}" : $"Installed ({localVersion ?? "-"})")
                : (string.IsNullOrEmpty(latestVersion) ? "Not installed" : $"Latest: {latestVersion}");

            onStatusChanged?.Invoke();
        }

        // Находит ссылку на raw package.json по git url (github)
        static string GetRepoName(string url)
        {
            if (string.IsNullOrEmpty(url)) return "";
            var m = Regex.Match(url, @"github\.com/([^/]+)/([^/?\.]+)");
            return m.Success ? m.Groups[2].Value.ToLower() : "";
        }

        static async Task<string> GetGitPackageJsonVersion(string url)
        {
            try
            {
                var m = Regex.Match(url, @"github\.com/([^/]+)/([^/?\.]+).*?path=([^&]+)");
                if (!m.Success)
                    m = Regex.Match(url, @"github\.com/([^/]+)/([^/?\.]+)");
                if (!m.Success) return null;

                var user = m.Groups[1].Value;
                var repo = m.Groups[2].Value;
                string path = m.Groups.Count > 3 ? m.Groups[3].Value : "";
                path = path.Replace("/Assets/", "/"); // Поддержка структуры repo.git?path=/Templates/Assets/Menu
                path = path.Trim('/');
                var branch = "main"; // В большинстве случаев, либо замени на master или уточни для себя

                string rawUrl = string.IsNullOrEmpty(path)
                    ? $"https://raw.githubusercontent.com/{user}/{repo}/{branch}/package.json"
                    : $"https://raw.githubusercontent.com/{user}/{repo}/{branch}/{path}/package.json";

                using (var client = new HttpClient())
                {
                    var json = await client.GetStringAsync(rawUrl);
                    var verMatch = Regex.Match(json, "\"version\"\\s*:\\s*\"([^\"]+)\"");
                    return verMatch.Success ? verMatch.Groups[1].Value : null;
                }
            }
            catch { return null; }
        }

        // Установка с зависимостями
        void InstallPackageWithDeps(GitPackage pkg, Label statusLabel, Button installBtn)
        {
            if (pkg.isInstalling || pkg.isInstalled && !pkg.hasUpdate)
                return;

            pkg.isInstalling = true;
            pkg.status = "Installing...";
            statusLabel.text = pkg.status;

            void UpdateStatus()
            {
                statusLabel.text = pkg.status;
            }

            // 1. Установить зависимости
            if (pkg.dependencies != null && pkg.dependencies.Count > 0)
            {
                InstallDependencies(pkg, 0, () => InstallSinglePackage(pkg, statusLabel, installBtn, UpdateStatus));
            }
            else
            {
                InstallSinglePackage(pkg, statusLabel, installBtn, UpdateStatus);
            }
        }

        void InstallDependencies(GitPackage pkg, int depIndex, Action onDone)
        {
            if (depIndex >= pkg.dependencies.Count)
            {
                onDone?.Invoke();
                return;
            }
            var dep = pkg.dependencies[depIndex];
            InstallSinglePackage(dep, null, null, () => InstallDependencies(pkg, depIndex + 1, onDone));
        }

        void InstallSinglePackage(GitPackage pkg, Label statusLabel, Button installBtn, Action onDone = null)
        {
            pkg.status = "Installing...";
            pkg.isInstalling = true;
            if (statusLabel != null)
                statusLabel.text = pkg.status;

            pkg.request = Client.Add(pkg.url);
            EditorApplication.update += Progress;

            void Progress()
            {
                if (!pkg.request.IsCompleted) return;
                EditorApplication.update -= Progress;
                pkg.isInstalling = false;
                if (pkg.request.Status == StatusCode.Success)
                {
                    pkg.status = "Installed";
                    pkg.isInstalled = true;
                }
                else
                {
                    pkg.status = "Error: " + pkg.request.Error.message;
                    pkg.isInstalled = false;
                }
                if (statusLabel != null) statusLabel.text = pkg.status;
                CreateGUI(); // Перерисовать окно для актуализации статусов/кнопок
                onDone?.Invoke();
            }
        }
    }
}
