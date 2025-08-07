using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UIElements;
namespace PG.TemplatesPackageManager
{


    public class GitPackageInstaller : EditorWindow
    {
        private List<GitPackage> packages;
        private double _nextCheckTime;
        private const double k_CheckInterval = 60.0; // проверять раз в минуту

        [MenuItem("Window/PG/Git Package Installer")]
        [MenuItem("Window/Package Management/Git Package Installer")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<GitPackageInstaller>();
            wnd.titleContent = new GUIContent("Git Installer");
            wnd.minSize = new Vector2(480, 500);
        }

        void OnEnable()
        {
            // Инициализируем список пакетов и их начальные состояния
            packages = new List<GitPackage>
        {
            new GitPackage {
                name="Updating the Package Window",
                url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/TemplatesPackageManager"
            },
            new GitPackage {
                name="Menu System",
                url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/Menu",
                dependencies = new List<GitPackage> {
                    new GitPackage {
                        name="PGTween",
                        url="https://github.com/7Lucia7Lokidottir7/PGTween.git"
                    }
                }
            },
            new GitPackage { name="Interact System",      url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/InteractSystem" },
            new GitPackage { name="Quest System",         url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/QuestSystem" },
            new GitPackage { name="Health System",        url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/HealthSystem" },
            new GitPackage { name="Localization System",  url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/LocalizationSystem" },
            new GitPackage { name="Battle System",        url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/BattleSystem" },
            new GitPackage { name="Locomotion System",    url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/LocomotionSystem" },
            new GitPackage { name="VFX Control",          url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/VFXControl" },
        };

            // Начальная проверка статусов
            foreach (var pkg in packages)
            {
                pkg.status = "Checking...";
                pkg.isInstalled = pkg.isInstalling = pkg.hasUpdate = false;
                pkg.currentVersion = pkg.latestVersion = null;
                CheckPackageStatus(pkg, () => CreateGUI());
            }

            // Подписка на обновления интервала
            _nextCheckTime = EditorApplication.timeSinceStartup + k_CheckInterval;
            EditorApplication.update += OnEditorUpdate;

            // Построить UI
            CreateGUI();
        }

        void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        void OnEditorUpdate()
        {
            if (EditorApplication.timeSinceStartup < _nextCheckTime) return;
            _nextCheckTime = EditorApplication.timeSinceStartup + k_CheckInterval;

            foreach (var pkg in packages)
                CheckPackageStatus(pkg, () => CreateGUI());
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            root.Clear();
            root.style.flexDirection = FlexDirection.Column;
            root.style.paddingLeft = 16;
            root.style.paddingRight = 16;
            root.style.paddingTop = 16;
            root.style.paddingBottom = 8;

            // Заголовок
            var title = new Label("Git Package Installer");
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.fontSize = 20;
            title.style.color = new Color(0.92f, 0.92f, 1f, 1f);
            title.style.marginBottom = 4;
            root.Add(title);

            var desc = new Label("Installing git packages and auto-checking for updates:");
            desc.style.fontSize = 12;
            desc.style.marginBottom = 8;
            desc.style.color = new Color(0.75f, 0.75f, 0.9f, 1f);
            root.Add(desc);

            // ScrollView для списка
            var scroll = new ScrollView();
            scroll.style.flexGrow = 1f;
            root.Add(scroll);

            // Цвета
            var boxBg = new Color(0.11f, 0.13f, 0.18f, 1f);
            var colGreen = new Color(0.32f, 0.88f, 0.44f, 1f);
            var colYellow = new Color(0.92f, 0.89f, 0.21f, 1f);
            var colBlue = new Color(0.25f, 0.53f, 0.93f, 1f);
            var colOrange = new Color(1f, 0.6f, 0.12f, 1f);

            // Отрисовка каждой строки
            foreach (var pkg in packages)
            {
                var box = new VisualElement();
                box.style.flexDirection = FlexDirection.Row;
                box.style.alignItems = Align.Center;
                box.style.paddingTop = 4; box.style.paddingBottom = 4;
                box.style.backgroundColor = boxBg;
                box.style.borderBottomWidth = 1;
                box.style.borderBottomColor = new Color(0.2f, 0.2f, 0.2f, 0.34f);
                box.style.marginBottom = 2;

                // Name
                var name = new Label(pkg.name);
                name.style.flexBasis = 0;
                name.style.flexGrow = 2;
                name.style.maxWidth = 250;
                name.style.fontSize = 15;
                name.style.unityFontStyleAndWeight = FontStyle.Bold;
                name.style.color = Color.white;
                name.style.whiteSpace = WhiteSpace.NoWrap;
                name.style.overflow = Overflow.Hidden;
                name.style.textOverflow = TextOverflow.Ellipsis;
                box.Add(name);

                // URL (выделяется и копируется)
                var urlField = new TextField { value = pkg.url };
                urlField.isReadOnly = true;
                urlField.style.flexBasis = 0;
                urlField.style.flexGrow = 4;
                urlField.style.maxWidth = 540;
                urlField.style.fontSize = 10;
                urlField.style.backgroundColor = new Color(0, 0, 0, 0);
                urlField.style.borderBottomWidth = 0;
                urlField.style.borderTopWidth = 0;
                urlField.style.borderLeftWidth = 0;
                urlField.style.borderRightWidth = 0;
                urlField.style.color = colBlue;
                urlField.style.unityTextAlign = TextAnchor.MiddleLeft;
                urlField.style.whiteSpace = WhiteSpace.NoWrap;
                urlField.style.overflow = Overflow.Hidden;
                urlField.style.textOverflow = TextOverflow.Ellipsis;
                box.Add(urlField);

                // Open button
                var openBtn = new Button(() => Application.OpenURL(pkg.url)) { text = "🔗" };
                openBtn.style.width = 24;
                openBtn.style.marginLeft = 2;
                openBtn.tooltip = "Open in browser";
                box.Add(openBtn);

                // Status
                var status = new Label(pkg.status);
                status.style.width = 120;
                status.style.marginLeft = 8;
                status.style.fontSize = 13;
                status.style.color = colYellow;
                box.Add(status);

                // Install / Update button
                var actionBtn = new Button(() => InstallPackageWithDeps(pkg, status))
                {
                    text = pkg.isInstalled
                        ? (pkg.hasUpdate ? "Update" : "Installed")
                        : "Install"
                };
                actionBtn.style.width = 90;
                actionBtn.style.marginLeft = 8;
                actionBtn.style.fontSize = 13;
                actionBtn.style.unityFontStyleAndWeight = FontStyle.Bold;
                actionBtn.style.backgroundColor = colBlue;
                actionBtn.style.color = Color.white;
                actionBtn.SetEnabled(!pkg.isInstalling && (!pkg.isInstalled || pkg.hasUpdate));
                box.Add(actionBtn);

                // Цвет статуса
                if (pkg.isInstalled && !pkg.hasUpdate) status.style.color = colGreen;
                if (pkg.hasUpdate) status.style.color = colOrange;

                scroll.Add(box);
            }
        }

        // Проверяет установлен ли пакет и есть ли новый релиз
        private async void CheckPackageStatus(GitPackage pkg, Action onStatusChanged)
        {
            // 1. Проверяем через UPM
            var listReq = Client.List(true);
            while (!listReq.IsCompleted)
                await Task.Delay(30);

            var upm = listReq.Status == StatusCode.Success
                ? listReq.Result.FirstOrDefault(u => u.packageId.Contains(GetRepoName(pkg.url)))
                : null;

            pkg.isInstalled = upm != null;
            pkg.currentVersion = upm?.version;

            // 2. Получаем последний релиз из GitHub API
            ParseGitHubUrl(pkg.url, out var owner, out var repo);
            pkg.latestVersion = await GetLatestGitHubRelease(owner, repo);

            // 3. Сравниваем
            pkg.hasUpdate = pkg.isInstalled
                && !string.IsNullOrEmpty(pkg.currentVersion)
                && !string.IsNullOrEmpty(pkg.latestVersion)
                && !pkg.currentVersion.Equals(pkg.latestVersion, StringComparison.OrdinalIgnoreCase);

            // 4. Обновляем текст статуса
            if (!pkg.isInstalled)
                pkg.status = string.IsNullOrEmpty(pkg.latestVersion)
                    ? "Not installed"
                    : $"Latest: {pkg.latestVersion}";
            else if (pkg.hasUpdate)
                pkg.status = $"Update: {pkg.currentVersion} → {pkg.latestVersion}";
            else
                pkg.status = $"Installed ({pkg.currentVersion})";

            onStatusChanged?.Invoke();
        }

        static void ParseGitHubUrl(string gitUrl, out string owner, out string repo)
        {
            owner = repo = null;
            var m = Regex.Match(gitUrl, @"github\.com/(?<owner>[^/]+)/(?<repo>[^/\.]+)");
            if (m.Success)
            {
                owner = m.Groups["owner"].Value;
                repo = m.Groups["repo"].Value;
            }
        }

        static async Task<string> GetLatestGitHubRelease(string owner, string repo)
        {
            if (string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(repo))
                return null;

            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("UnityGitInstaller");
                var url = $"https://api.github.com/repos/{owner}/{repo}/releases/latest";
                var json = await client.GetStringAsync(url);
                var m = Regex.Match(json, "\"tag_name\"\\s*:\\s*\"(?<tag>[^\"]+)\"");
                return m.Success ? m.Groups["tag"].Value : null;
            }
            catch
            {
                return null;
            }
        }

        static string GetRepoName(string url)
        {
            if (string.IsNullOrEmpty(url)) return "";
            var m = Regex.Match(url, @"github\.com/([^/]+)/([^/?\.]+)");
            return m.Success ? m.Groups[2].Value.ToLower() : "";
        }

        // Установка с зависимостями
        void InstallPackageWithDeps(GitPackage pkg, Label statusLabel)
        {
            if (pkg.isInstalling || (pkg.isInstalled && !pkg.hasUpdate))
                return;

            pkg.isInstalling = true;
            pkg.status = "Installing...";
            CreateGUI();

            // Сначала зависимости
            if (pkg.dependencies != null && pkg.dependencies.Count > 0)
                InstallDependencies(pkg, 0, () => InstallSingle(pkg));
            else
                InstallSingle(pkg);
        }

        void InstallDependencies(GitPackage pkg, int idx, Action onDone)
        {
            if (idx >= pkg.dependencies.Count)
                onDone();
            else
                InstallSingle(pkg.dependencies[idx], () => InstallDependencies(pkg, idx + 1, onDone));
        }

        void InstallSingle(GitPackage pkg, Action onComplete = null)
        {
            pkg.request = Client.Add(pkg.url);
            EditorApplication.update += Progress;
            void Progress()
            {
                if (!pkg.request.IsCompleted) return;
                EditorApplication.update -= Progress;
                pkg.isInstalling = false;

                if (pkg.request.Status == StatusCode.Success)
                {
                    pkg.isInstalled = true;
                    pkg.status = "Installed";
                }
                else
                {
                    pkg.isInstalled = false;
                    pkg.status = "Error";
                }

                CreateGUI();
                onComplete?.Invoke();
            }
        }
    }
}