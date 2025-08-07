using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.TemplatesPackageManager
{

    public partial class GitPackageInstaller : EditorWindow
    {

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
            

            // для каждого сразу проверяем, установлен ли он
            foreach (var pkg in packages)
            {
                pkg.status = "Checking…";
                pkg.isInstalled = false;
                pkg.isInstalling = false;
                CheckInstalledStatus(pkg, () => CreateGUI());
            }

            CreateGUI();
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

            // заголовок
            var title = new Label("Git Package Installer");
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.fontSize = 20;
            title.style.marginBottom = 4;
            root.Add(title);

            var desc = new Label("Install or update git-based packages:");
            desc.style.fontSize = 12;
            desc.style.marginBottom = 8;
            root.Add(desc);

            // scroll
            var scroll = new ScrollView { style = { flexGrow = 1f } };
            root.Add(scroll);

            // цвета
            var bg = new Color(0.11f, 0.13f, 0.18f, 1f);
            var green = new Color(0.32f, 0.88f, 0.44f, 1f);
            var blue = new Color(0.25f, 0.53f, 0.93f, 1f);
            var gray = new Color(0.75f, 0.75f, 0.9f, 1f);

            foreach (var pkg in packages)
            {
                var box = new VisualElement();
                box.style.flexDirection = FlexDirection.Row;
                box.style.alignItems = Align.Center;
                box.style.paddingTop = 4;
                box.style.paddingBottom = 4;
                box.style.backgroundColor = bg;
                box.style.borderBottomWidth = 1;
                box.style.borderBottomColor = new Color(0.2f, 0.2f, 0.2f, 0.34f);
                box.style.marginBottom = 2;

                // имя
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

                // ссылка
                var url = new TextField { value = pkg.url };
                url.isReadOnly = true;
                url.style.flexBasis = 0;
                url.style.flexGrow = 4;
                url.style.maxWidth = 540;
                url.style.fontSize = 10;
                url.style.backgroundColor = new Color(0, 0, 0, 0);
                url.style.borderBottomWidth = 0;
                url.style.borderTopWidth = 0;
                url.style.borderLeftWidth = 0;
                url.style.borderRightWidth = 0;
                url.style.color = blue;
                url.style.unityTextAlign = TextAnchor.MiddleLeft;
                url.style.whiteSpace = WhiteSpace.NoWrap;
                url.style.overflow = Overflow.Hidden;
                url.style.textOverflow = TextOverflow.Ellipsis;
                box.Add(url);

                // открыть в браузере
                var open = new Button(() => Application.OpenURL(pkg.url)) { text = "🔗" };
                open.style.width = 24;
                open.tooltip = "Open in browser";
                open.style.marginLeft = 2;
                box.Add(open);

                // статус
                var status = new Label(pkg.status);
                status.style.width = 100;
                status.style.marginLeft = 8;
                status.style.fontSize = 13;
                status.style.color = pkg.isInstalled ? green : gray;
                box.Add(status);

                // кнопка
                var btn = new Button(() => InstallPackageWithDeps(pkg))
                {
                    text = pkg.isInstalled ? "Update" : "Install"
                };
                btn.style.width = 90;
                btn.style.marginLeft = 8;
                btn.style.fontSize = 13;
                btn.style.unityFontStyleAndWeight = FontStyle.Bold;
                btn.style.backgroundColor = blue;
                btn.style.color = Color.white;
                btn.SetEnabled(!pkg.isInstalling);
                box.Add(btn);

                scroll.Add(box);
            }
        }

        private async void CheckInstalledStatus(GitPackage pkg, Action onChanged)
        {
            var req = Client.List(true);
            while (!req.IsCompleted)
                await Task.Delay(30);

            if (req.Status == StatusCode.Success &&
                req.Result.Any(u => u.packageId.Contains(GetRepoName(pkg.url))))
            {
                pkg.isInstalled = true;
                pkg.status = "Installed";
            }
            else
            {
                pkg.isInstalled = false;
                pkg.status = "Not installed";
            }

            onChanged?.Invoke();
        }

        static string GetRepoName(string url)
        {
            if (string.IsNullOrEmpty(url)) return "";
            var parts = url.Split('/');
            return parts.Length >= 2
                ? parts[parts.Length - 1]
                      .Split(new[] { '.', '?' }, StringSplitOptions.RemoveEmptyEntries)[0]
                      .ToLower()
                : url.ToLower();
        }

        void InstallPackageWithDeps(GitPackage pkg)
        {
            if (pkg.isInstalling) return;

            pkg.isInstalling = true;
            pkg.status = "Installing…";
            CreateGUI();

            if (pkg.dependencies.Count > 0)
                InstallDependencies(pkg, 0, () => InstallSingle(pkg));
            else
                InstallSingle(pkg);
        }

        void InstallDependencies(GitPackage pkg, int idx, Action onDone)
        {
            if (idx >= pkg.dependencies.Count)
            {
                onDone();
                return;
            }
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
                    pkg.status = "Installed";
                else
                    pkg.status = "Error";

                pkg.isInstalled = (pkg.request.Status == StatusCode.Success);
                CreateGUI();
                onComplete?.Invoke();
            }
        }
    }
}
