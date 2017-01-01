using Assets.Editor.BTreeFramework.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Editor.BTreeFramework
{
    public class CodeGenerator
    {
        [MenuItem("Assets/Create/Behaviour Tree")]
        public static void GenerateBehaviour()
        {
            string browseTitle = "New Behaviour Tree";
            string fileName = "NewBTreeBehaviour";
            UnityEngine.Object selectedFile = Selection.activeObject;
            string path = selectedFile ? AssetDatabase.GetAssetPath(selectedFile.GetInstanceID()) : null;
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                path = EditorUtility.SaveFilePanelInProject(browseTitle, fileName + ".cs", "cs", "Please enter a file name: ");
                if (string.IsNullOrEmpty(path))
                {
                    return;
                }
            } else
            {
                path = EditorUtility.SaveFilePanel(browseTitle, path, fileName + ".cs", "cs");
            }
            fileName = Path.GetFileNameWithoutExtension(path);
            BTreeBehaviourCode code = new BTreeBehaviourCode(fileName);
            string contents = code.TransformText();
            try
            {
                File.WriteAllText(Path.Combine(Directory.GetParent(path).FullName, fileName + ".cs"), contents);
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.Log("BTreeFramework [New Behaviour Tree]: Error on creating file: " + e);
            }
        }
    }
}
