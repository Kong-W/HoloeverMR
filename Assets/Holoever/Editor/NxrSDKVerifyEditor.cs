﻿using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

namespace Nxr.Internal
{
    public class NxrSDKVerifyEditor : EditorWindow
    { 
        string GetSDKKey()
        {
            string data = NxrPluginEditor.Read("AndroidManifest.xml");
            string[] lines = data.Split('\n');
            for (int i = 0, l = lines.Length; i < l; i++)
            {
                string lineContent = lines[i];
                if (lineContent.Contains("NIBIRU_SDK_KEY"))
                {
                    MatchCollection mc = Regex.Matches(lineContent, "(?<=\").*?(?=\")");
                    return mc[0].Value;
                }
            }
            return null;
        }

        //输入文字的内容
        private string inputText = null;
        bool IsFirstTrigger = false;
        private void OnGUI()
        {
            if (!IsFirstTrigger)
            {
                IsFirstTrigger = true;
                inputText = GetSDKKey();
            }

            GUILayout.Space(20);
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.normal.textColor = new Color(220 / 255.0f, 20 / 255.0f, 60 / 255.0f, 1.0f);
            labelStyle.fontSize = 13;
            GUILayout.Label("  SDK 验证: \n\n  步骤1: 将包名提交中科永光.\n" +
                "  步骤2: 将获取后的 NibiruSDKKey.bin 复制覆盖在此路径下 Assets\\Plugins\\Android\\assets.\n" +
                "  步骤3: 将获取到的KEY的值,输入并提交.",
                labelStyle);
            GUILayout.Space(20);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("在这里输入KEY:");
            GUIStyle style = new GUIStyle();
            style.normal.textColor = new Color(0, 122f / 255f, 204f / 255f);
 
            GUILayout.Space(20);

            inputText = TextField(inputText, null);// GUILayout.TextField(inputText);

            GUILayout.Space(20);
            bool SDKKeyExist = NxrPluginEditor.IsFileExists("assets/NibiruSDKKey.bin");
            if (!SDKKeyExist)
            {
                GUILayout.Label("  [Warning] NibiruSDKKey.bin 不存在. [Warning] ", labelStyle);
                GUILayout.Space(20);
            }

            if (inputText != null && GUILayout.Button("Confirm", GUILayout.Width(100), GUILayout.Height(30)))
            {
                {
                    string data = NxrPluginEditor.Read("AndroidManifest.xml");
                    string[] lines = data.Split('\n');
                    string newdata = "";
                    for (int i = 0, l = lines.Length; i < l; i++)
                    {
                        string lineContent = lines[i];
                        if (lineContent.Contains("NIBIRU_SDK_KEY"))
                        {
                            lineContent = "    <meta-data android:value=\"" + inputText + "\" android:name=\"NIBIRU_SDK_KEY\"/>";
                        }
                        newdata = newdata + lineContent + "\n";
                    }

                    NxrPluginEditor.Write("AndroidManifest.xml", newdata);
                }

                Close();
            }
        }



        public static string HandleCopyPaste(int controlID)
        {
            if (controlID == GUIUtility.keyboardControl)
            {
                if (Event.current.type == UnityEngine.EventType.KeyUp && (Event.current.modifiers == EventModifiers.Control || Event.current.modifiers == EventModifiers.Command))
                {
                    if (Event.current.keyCode == KeyCode.C)
                    {
                        Event.current.Use();
                        TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                        editor.Copy();
                    }
                    else if (Event.current.keyCode == KeyCode.V)
                    {
                        Event.current.Use();
                        TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                        editor.text = "";
                        editor.Paste();
#if UNITY_5_3_OR_NEWER || UNITY_5_3
                        return editor.text; //以及更高的unity版本中editor.content.text已经被废弃，需使用editor.text代替
#else
                    return editor.content.text;
#endif
                    }
                    else if (Event.current.keyCode == KeyCode.A)
                    {
                        Event.current.Use();
                        TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                        editor.SelectAll();
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// TextField复制粘贴的实现
        /// </summary>
        public static string TextField(string value, params GUILayoutOption[] options)
        {
            int textFieldID = GUIUtility.GetControlID("TextField".GetHashCode(), FocusType.Keyboard) + 1;
            if (textFieldID == 0)
                return value;

            //处理复制粘贴的操作
            value = HandleCopyPaste(textFieldID) ?? value;

            return GUILayout.TextField(value, options);
        }

    }
}