using UnityEngine;
using UnityEditor;

namespace Nxr.Internal
{

    public class NxrApkEncryptionEditor : EditorWindow
    { 
        bool EnableEncryption = false;
        bool IsHasChecked;
        // <!-- 当前APK为加密版本，需要进行验证设备唯一标识 -->
        //<meta-data android:value="1" android:name="NIBIRU_ENCRYPTION_MODE"/>
        bool CheckEncryption()
        {
            string data = NxrPluginEditor.Read("AndroidManifest.xml");
            string[] lines = data.Split('\n');
            for (int i = 0, l = lines.Length; i < l; i++)
            {
                string lineContent = lines[i];
                if (lineContent.Contains("NIBIRU_ENCRYPTION_MODE"))
                {
                    return lineContent.Contains("1");
                }
            }
            return false;
        }

        private void OnGUI()
        {
            if (!IsHasChecked)
            {
                IsHasChecked = true;
                EnableEncryption = CheckEncryption();
            }
            GUILayout.Space(20);
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.normal.textColor = new Color(220 / 255.0f, 20 / 255.0f, 60 / 255.0f, 1.0f);
            labelStyle.fontSize = 13;
            GUILayout.Label("  APK 加密: \n\n  步骤1: 使用HoloeverEncrypt工具生成加密密钥文件名为 apkpass.txt.\n  步骤2: 检查 apkpass.txt 是否在此路径下 Assets\\Plugins\\Android\\assets.",
                labelStyle);
            GUILayout.Space(20);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("是否开启加密:");
            GUIStyle style = new GUIStyle();
            style.normal.textColor = new Color(0, 122f / 255f, 204f / 255f);
 
            GUILayout.Space(20);

            EnableEncryption = GUILayout.Toggle(EnableEncryption, "启用 APK 加密");
            GUILayout.Space(20);
            bool ApkPassExist = NxrPluginEditor.IsFileExists("assets/apkpass.txt");
            if (!ApkPassExist)
            {
                GUILayout.Label("  [Warning] apkpass.txt 不存在. [Warning] ", labelStyle);
                GUILayout.Space(20);
            }

            if (GUILayout.Button("Confirm", GUILayout.Width(100), GUILayout.Height(30)))
            {
                {
                    string data = NxrPluginEditor.Read("AndroidManifest.xml");
                    string[] lines = data.Split('\n');
                    string newdata = "";
                    for (int i = 0, l = lines.Length; i < l; i++)
                    {
                        string lineContent = lines[i];
                        if (lineContent.Contains("NIBIRU_ENCRYPTION_MODE"))
                        {
                            lineContent = "    <meta-data android:value=\"" + (EnableEncryption ? 1 : 0) + "\" android:name=\"NIBIRU_ENCRYPTION_MODE\"/>";
                        }
                        newdata = newdata + lineContent + "\n";
                    }

                    NxrPluginEditor.Write("AndroidManifest.xml", newdata);
                }

                Close();
            }
        }
    }
}