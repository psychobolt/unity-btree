using Assets.Editor.BTreeFramework.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Editor.BTreeFramework
{
    public class CodeGenerator
    {
        private const string NEW_BEHAVIOUR_TREE_TITLE = "New Behaviour Tree";
        private const string NEW_BEHAVIOUR_TREE_FILE_NAME = "NewBTreeBehaviour";

        [MenuItem("Assets/Create/Behaviour Tree")]
        public static void GenerateBehaviour()
        {
            string path = selectPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            string filename = Path.GetFileNameWithoutExtension(path);
            BTreeBehaviourCode code = new BTreeBehaviourCode(filename, removeSuffix(Regex.Replace(filename, "(\\B[A-Z])", " $1")));
            try
            {
                File.WriteAllText(Path.Combine(Directory.GetParent(path).FullName, filename + ".cs"), code.TransformText());
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.Log("BTreeFramework [New Behaviour Tree]: Error on creating file: " + e);
            }
        }

        private static string selectPath()
        {
            string filename = NEW_BEHAVIOUR_TREE_FILE_NAME;
            UnityEngine.Object selectedFile = Selection.activeObject;
            string path = selectedFile ? AssetDatabase.GetAssetPath(selectedFile.GetInstanceID()) : null;
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                path = EditorUtility.SaveFilePanelInProject(NEW_BEHAVIOUR_TREE_TITLE, filename + ".cs", "cs", "Please enter a file name: ");
            }
            else
            {
                path = EditorUtility.SaveFilePanel(NEW_BEHAVIOUR_TREE_TITLE, path, filename + ".cs", "cs");
            }
            return path;
        }

        private static string removeSuffix(string behaviourName)
        {
            string name = behaviourName;
            while (true)
            {
                string suffix = null;
                if (name.EndsWith(" Behaviour"))
                {
                    suffix = " Behaviour";
                }
                else if (name.EndsWith(" Behavior"))
                {
                    suffix = " Behavior";
                }
                if (!string.IsNullOrEmpty(suffix))
                {
                    name = name.Substring(0, name.Length - suffix.Length);
                }
                else
                {
                    break;
                }

            }
            if (string.IsNullOrEmpty(name)) {
                return behaviourName;
            }
            return name;
        }
    }
}
