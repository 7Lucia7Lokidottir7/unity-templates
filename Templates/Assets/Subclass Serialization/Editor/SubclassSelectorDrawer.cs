using PG;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(SubclassSelectorAttribute))]
public class SubclassSelectorDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Контейнер для всего поля
        VisualElement container = new VisualElement();

        if (property.propertyType != SerializedPropertyType.ManagedReference)
        {
            container.Add(new Label("Error: Use [SerializeReference] for SubclassSelector"));
            return container;
        }

        // Получаем базовый тип поля
        Type baseType = GetManagedReferenceFieldType(fieldInfo.FieldType);

        // Находим все типы, которые наследуются от базового
        List<Type> derivedTypes = TypeCache.GetTypesDerivedFrom(baseType)
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .ToList();

        // Формируем список названий для выпадающего меню
        List<string> typeNames = derivedTypes.Select(t => t.Name).ToList();
        typeNames.Insert(0, "None (Null)");

        // Создаем выпадающий список (Dropdown)
        PopupField<string> popup = new PopupField<string>(
            label: property.displayName,
            choices: typeNames,
            defaultIndex: GetCurrentIndex(property, derivedTypes)
        );

        // Контейнер для отрисовки полей выбранного класса
        VisualElement propertiesContainer = new VisualElement();

        // Функция для обновления отображения полей класса
        void UpdatePropertyFields()
        {
            propertiesContainer.Clear();

            // Если ссылка пустая, отрисовывать нечего
            if (property.managedReferenceValue == null) return;

            // Создаем копию, чтобы не "испортить" основной итератор свойства
            SerializedProperty propClone = property.Copy();
            SerializedProperty endProperty = propClone.GetEndProperty();

            // Заходим внутрь объекта (первый NextVisible(true))
            if (propClone.NextVisible(true))
            {
                do
                {
                    // Проверяем, что мы не вышли за пределы текущего ManagedReference
                    if (SerializedProperty.EqualContents(propClone, endProperty))
                        break;

                    // Создаем поле для каждого вложенного свойства
                    PropertyField field = new PropertyField(propClone.Copy());
                    field.Bind(property.serializedObject);
                    propertiesContainer.Add(field);
                }
                // NextVisible(false) гарантирует, что мы идем только по полям текущего уровня,
                // не заходя вглубь их собственных вложенных полей (PropertyField сам разберется с глубиной)
                while (propClone.NextVisible(false));
            }
        }

        // Логика смены типа при выборе в Dropdown
        popup.RegisterValueChangedCallback(evt =>
        {
            int index = popup.index;

            property.serializedObject.Update();

            if (index == 0) // "None"
            {
                property.managedReferenceValue = null;
            }
            else
            {
                Type selectedType = derivedTypes[index - 1];
                property.managedReferenceValue = Activator.CreateInstance(selectedType);
            }

            property.serializedObject.ApplyModifiedProperties();
            UpdatePropertyFields();
        });

        // Начальная отрисовка
        UpdatePropertyFields();

        container.Add(popup);
        container.Add(propertiesContainer);

        return container;
    }

    private Type GetManagedReferenceFieldType(Type type)
    {
        if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)))
            return type.GetGenericArguments()[0];
        return type;
    }

    private int GetCurrentIndex(SerializedProperty property, List<Type> types)
    {
        string fullType = property.managedReferenceFullTypename;
        if (string.IsNullOrEmpty(fullType)) return 0;

        string typeName = fullType.Split(' ').Last();
        int index = types.FindIndex(t => t.FullName == typeName);
        return index + 1;
    }
}