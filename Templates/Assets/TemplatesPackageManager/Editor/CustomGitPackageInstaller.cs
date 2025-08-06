using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UIElements;

public class GitPackage
{
    public string name;
    public string url;
    public List<GitPackage> dependencies = new List<GitPackage>();

    // Для простоты: хранение статуса и запроса
    public string status = "Waiting";
    public AddRequest request;
}

public class GitPackageInstallerUITK : EditorWindow
{
    // Описание пакетов и зависимостей
    List<GitPackage> packages = new List<GitPackage>
    {
        new GitPackage { 
            name="Menu System", 
            url="https://github.com/7Lucia7Lokidottir7/unity-templates.git?path=/Templates/Assets/Menu",
            dependencies = new List<GitPackage>
            {
                new GitPackage
                {
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

    [MenuItem("Tools/Git Package Installer UITK")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<GitPackageInstallerUITK>();
        wnd.titleContent = new GUIContent("Git Installer");
    }

    private void CreateGUI()
    {
        var root = rootVisualElement;
        root.style.paddingLeft = 14;
        root.style.paddingRight = 14;
        root.style.paddingTop = 12;

        var title = new Label("Git Package Installer") { style = { fontSize = 18, unityFontStyleAndWeight = FontStyle.Bold } };
        root.Add(title);
        root.Add(new Label("Быстрая установка пакетов и их зависимостей из GitHub:\n"));

        // Контейнер для пакетов
        var pkgContainer = new VisualElement();

        foreach (var pkg in packages)
        {
            var box = new VisualElement();
            box.style.flexDirection = FlexDirection.Row;
            box.style.marginBottom = 6;
            box.style.alignItems = Align.Center;
            box.style.paddingBottom = 3;
            box.style.paddingTop = 3;
            box.style.borderBottomWidth = 1;
            box.style.borderBottomColor = new Color(0.2f, 0.2f, 0.2f, 0.4f);

            var name = new Label(pkg.name)
            {
                style =
                {
                    width = 160,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    fontSize = 14
                }
            };

            var url = new Label(pkg.url)
            {
                style = {
                    width = 350,
                    fontSize = 6,
                    color = new Color(0.5f,0.7f,1f,1f),
                    unityTextAlign = TextAnchor.MiddleLeft,
                    marginLeft = 6
                }
            };

            var installBtn = new Button(() => InstallPackageWithDeps(pkg, box))
            {
                text = "Install",
                style = {
                    width = 100,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginLeft = 10,
                    backgroundColor = new Color(0.18f, 0.45f, 0.85f, 1f),
                }
            };

            var status = new Label(pkg.status)
            {
                style = {
                    marginLeft = 10,
                    color = new Color(0.9f,0.8f,0.1f,1f),
                    fontSize = 13,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    width = 120
                }
            };

            box.Add(name);
            box.Add(url);
            box.Add(installBtn);
            box.Add(status);

            pkgContainer.Add(box);

            // Для обновления статуса
            pkg.status = "Waiting..";
        }

        root.Add(pkgContainer);

        var help = new Label("• Пакеты с зависимостями можно собирать через List<GitPackage> dependencies\n• Добавь зависимости для пакета в коде — пример ниже");
        help.style.marginTop = 18;
        help.style.fontSize = 11;
        help.style.color = new Color(0.7f, 0.7f, 0.7f, 1f);
        root.Add(help);
    }

    // Запуск установки с зависимостями
    void InstallPackageWithDeps(GitPackage pkg, VisualElement box)
    {
        // Установка зависимостей первой
        if (pkg.dependencies != null && pkg.dependencies.Count > 0)
        {
            InstallDependencies(pkg, 0, () => InstallSinglePackage(pkg, box));
        }
        else
        {
            InstallSinglePackage(pkg, box);
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
        InstallSinglePackage(dep, null, () => InstallDependencies(pkg, depIndex + 1, onDone));
    }

    void InstallSinglePackage(GitPackage pkg, VisualElement box, Action onDone = null)
    {
        pkg.status = "Установка...";
        RefreshUI();

        pkg.request = Client.Add(pkg.url);
        EditorApplication.update += Progress;

        void Progress()
        {
            if (pkg.request.IsCompleted)
            {
                if (pkg.request.Status == StatusCode.Success)
                    pkg.status = "Installed";
                else
                    pkg.status = "Error: " + pkg.request.Error.message;

                pkg.request = null;
                EditorApplication.update -= Progress;
                RefreshUI();
                onDone?.Invoke();
            }
            else
            {
                // Анимация: меняется статус (например, мигающие точки)
                pkg.status = "Installing" + new string('.', (int)(EditorApplication.timeSinceStartup % 4));
                RefreshUI();
            }
        }
    }

    void RefreshUI()
    {
        // Просто заново отрисовываем окно (все статусы обновятся)
        rootVisualElement.Clear();
        CreateGUI();
    }
}
