﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Hoshi.Core
{
    public static class Helpers
    {
        /* UI toolkit */
        const string _ussCommonTextPrimary = "common__text-primary";

        public static Vector3 GetRandomPointInBounds(Bounds bounds)
        {
            return new(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }

        public static Vector3 GetRandomPointOnNavmesh(Vector3 origin, float distance)
        {
            for (int i = 0; i < 5; i++)
            {
                float angle = Random.value * (2f * Mathf.PI);
                Vector3 direction = new(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                Vector3 position = direction * distance;

                //  Vector3 randomDirection = Random.insideUnitCircle * distance;
                position += origin;
                if (NavMesh.SamplePosition(position, out NavMeshHit hit, distance, NavMesh.AllAreas))
                    return hit.position;
            }

            return Vector3.zero;
        }

        public static Vector3 GetHalfwayPointOnNavmesh(Vector3 origin, Vector3 destination)
        {
            Vector3 halfwayPoint = Vector3.Lerp(origin, destination, 0.5f);
            if (NavMesh.SamplePosition(halfwayPoint, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                return hit.position;

            return Vector3.zero;
        }


        public static Gradient GetFakeGradient(Color color)
        {
            Gradient gradient = new();
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[2];
            colorKey[0].color = color;
            colorKey[0].time = 0.5f;
            colorKey[1].color = color;
            colorKey[1].time = 1f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.5f;
            alphaKey[1].time = 1f;

            gradient.SetKeys(colorKey, alphaKey);

            return gradient;
        }

        public static string ParseScriptableObjectName(string text)
        {
            text = text.Replace("(Clone)", "");
            // https://stackoverflow.com/questions/3216085/split-a-pascalcase-string-into-separate-words
            Regex r = new(
                @"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])"
            );

            text = r.Replace(text, " "); // pascal case
            text = Regex.Replace(text, @"\s+", " "); // whitespace clean-up
            return text;
        }

        public static int GetRandomNumber(int digits)
        {
            int min = (int)Mathf.Pow(10, digits - 1);
            int max = (int)Mathf.Pow(10, digits) - 1;
            return Random.Range(min, max);
        }

        public static string ToRoman(int number)
        {
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            return "Error";
        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static Vector3 GetPositionOnCircle(Vector3 center, float radius,
            int currentIndex, int totalCount, float startAngle = 0)
        {
            float theta = startAngle + currentIndex * 2 * Mathf.PI / totalCount;
            float x = Mathf.Cos(theta) * radius + center.x;
            float y = 1f;
            float z = Mathf.Sin(theta) * radius + center.z;

            return new(x, y, z);
        }

        public static Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius)
        {
            var randomDirection = (Random.insideUnitCircle * origin).normalized;

            var randomDistance = Random.Range(minRadius, maxRadius);

            var point = origin + randomDirection * randomDistance;

            return point;
        }


        //https://www.youtube.com/watch?v=q7BL-lboRXo&list=PLFt_AvWsXl0ctd4dgE1F8g3uec4zKNRV0&index=10
        public static List<T> ShuffleList<T>(List<T> list, int seed)
        {
            System.Random prng = new(seed);

            for (int i = 0; i < list.Count - 1; i++)
            {
                int randomIndex = prng.Next(i, list.Count);
                (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
            }

            return list;
        }

        public static void ShuffleList<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }

        public static Color HexToColor(string hex)
        {
            // I want to support #aaa, #AAA, aaa, AAA
            if (hex[0].ToString() != "#")
                hex = "#" + hex;

            Color color;
            if (ColorUtility.TryParseHtmlString(hex, out color))
                return color;

            Debug.LogError($"Couldn't parse color: {hex}");
            return Color.black;
        }

        static List<ArcMovementElement> _arcMovementElements = new();

        public static void SetUpHelpers(VisualElement root)
        {
            Debug.Log($"Setting up helpers {root}");
            _arcMovementElements = new();
            for (int i = 0; i < 50; i++)
            {
                ArcMovementElement el = new(null, Vector3.zero, Vector3.zero);
                el.AddToClassList(_ussCommonTextPrimary);
                _arcMovementElements.Add(el);
                root.Add(el);
            }
        }

        public static void DisplayTextOnElement(VisualElement root, VisualElement element, string text, Color color)
        {
            Label l = new(text);
            l.style.color = color;

            Vector3 start = new(element.worldBound.xMin, element.worldBound.yMin, 0);
            Vector3 end = new(element.worldBound.xMin + Random.Range(-100, 100),
                element.worldBound.yMin - 100, 0);

            ArcMovementElement arcMovementElement = _arcMovementElements.FirstOrDefault(x => !x.IsMoving);
            if (arcMovementElement == null) return;
            arcMovementElement.BringToFront();
            arcMovementElement.InitializeMovement(l, start, end);
            arcMovementElement.OnArcMovementFinished += ()
                => DOTween.To(x => arcMovementElement.style.opacity = x, 1, 0, 1)
                    .SetUpdate(true);
        }
    }
}