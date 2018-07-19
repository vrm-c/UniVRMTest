using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif
using UnityEngine;


namespace VRM
{
    public static class VRMExportUnityPackage
    {
        static string GetDesktop()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/VRM";
        }

        const string DATE_FORMAT = "yyyyMMdd";
        const string PREFIX = "UniVRM";

        static string System(string dir, string fileName, string args)
        {
            // Start the child process.
            var p = new System.Diagnostics.Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = fileName;
            p.StartInfo.Arguments = args;
            p.StartInfo.WorkingDirectory = dir;
            if (!p.Start())
            {
                return "ERROR";
            }
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            string err = p.StandardError.ReadToEnd();
            p.WaitForExit();

            if (string.IsNullOrEmpty(output))
            {
                return err;
            }
            return output;
        }

        //const string GIT_PATH = "C:\\Program Files\\Git\\mingw64\\bin\\git.exe";
        const string GIT_PATH = "C:\\Program Files\\Git\\bin\\git.exe";

        static string GetGitHash(string path)
        {
            return System(path, "git.exe", "rev-parse HEAD").Trim();
        }

#if false
        [MenuItem("VRM/git")]
        static void X()
        {
            var path = Application.dataPath;
            Debug.LogFormat("{0} => '{1}'", path, GetGitHash(path));
        }
#endif

        static string GetPath(string folder, string prefix)
        {
            //var date = DateTime.Today.ToString(DATE_FORMAT);

            var path = string.Format("{0}/{1}-{2}_{3}.unitypackage",
                folder,
                prefix,
                VRMVersion.VERSION,
                GetGitHash(Application.dataPath + "/VRM").Substring(0, 4)
                ).Replace("\\", "/");

            return path;
        }

        static IEnumerable<string> EnumerateFiles(string path)
        {
            if (Path.GetFileName(path).StartsWith(".git"))
            {
                yield break;
            }

            if (Directory.Exists(path))
            {
                foreach (var child in Directory.GetFileSystemEntries(path))
                {
                    foreach (var x in EnumerateFiles(child))
                    {
                        yield return x;
                    }
                }
            }
            else
            {
                if (Path.GetExtension(path).ToLower() != ".meta")
                {
                    yield return path.Replace("\\", "/");
                }
            }
        }

        public static bool Build(string[] levels)
        {
            var buildPath = Path.GetFullPath(Application.dataPath + "/../build/build.exe");
            Debug.LogFormat("{0}", buildPath);
            var build = BuildPipeline.BuildPlayer(levels,
                buildPath,
                BuildTarget.StandaloneWindows,
                BuildOptions.None
                );
#if UNITY_2018_1_OR_NEWER
            var iSuccess = build.summary.result != BuildResult.Succeeded;
#else
            var iSuccess = !string.IsNullOrEmpty(build);
#endif
            return iSuccess;
        }

        public static bool BuildTestScene()
        {
            var levels = new string[] { "Assets/VRM.Samples/Scenes/VRMRuntimeLoaderSample.unity" };
            return Build(levels);
        }

#if VRM_DEVELOP
        [MenuItem(VRMVersion.VRM_VERSION + "/Export unitypackage")]
#endif
        public static void CreateUnityPackageWithBuild()
        {
            var folder = GetDesktop();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            CreateUnityPackage(folder, true);
        }

        public static void CreateUnityPackage()
        {
            CreateUnityPackage(Path.GetFullPath(Path.Combine(Application.dataPath, "..")), false);
        }

        public static void CreateUnityPackage(string folder, bool build)
        {
            if (build)
            {
                // まずビルドする
                var iSuccess = BuildTestScene();
            }

            var path = GetPath(folder, PREFIX);
            if (File.Exists(path))
            {
                Debug.LogErrorFormat("{0} is already exists", path);
                return;
            }

            // 本体
            AssetDatabase.ExportPackage(EnumerateFiles("Assets/VRM").ToArray()
                , path,
                ExportPackageOptions.Default);

            // サンプル
            AssetDatabase.ExportPackage(EnumerateFiles("Assets/VRM.Samples").Concat(EnumerateFiles("Assets/StreamingAssets")).ToArray()
                , GetPath(folder, PREFIX + "-RuntimeLoaderSample"),
                ExportPackageOptions.Default);

            Debug.LogFormat("exported: {0}", path);
        }
    }
}
