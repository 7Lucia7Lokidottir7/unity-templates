using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG
{
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
                if (property.managedReferenceValue != null)
                {
                    // Отрисовываем все вложенные поля объекта
                    SerializedProperty propClone = property.Copy();
                    IEnumerator<SerializedProperty> enumerator = (IEnumerator<SerializedProperty>)propClone.GetEnumerator();

                    if (enumerator.MoveNext())
                    {
                        // Проходим по всем дочерним свойствам
                        foreach (SerializedProperty child in propClone)
                        {
                            PropertyField field = new PropertyField(child);
                            field.Bind(property.serializedObject);
                            propertiesContainer.Add(field);
                        }
                    }
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
}