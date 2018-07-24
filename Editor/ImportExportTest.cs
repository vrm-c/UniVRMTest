using NUnit.Framework;
using System.IO;
using UniJSON;
using UnityEngine;


namespace VRM
{
    public class VRMTests
    {
        [Test]
        public void ImportExportTest()
        {
            var path = UniGLTF.UnityPath.FromUnityPath("Models/Alicia_vrm-0.40/AliciaSolid_vrm-0.40.vrm");
            var context = new VRMImporterContext(path);
            context.ParseGlb(File.ReadAllBytes(path.FullPath));
            VRMImporter.LoadFromBytes(context);

            using (new ActionDisposer(() => { GameObject.DestroyImmediate(context.Root); }))
            {
                var importJson = JsonParser.Parse(context.Json);
                importJson.SetValue("/extensions/VRM/exporterVersion", VRMVersion.VRM_VERSION);
                importJson.SetValue("/asset/generator", "UniGLTF-1.11");
                importJson.SetValue("/scene", 0);

                var vrm = VRMExporter.Export(context.Root);
                var exportJson = JsonParser.Parse(vrm.ToJson());

                foreach (var kv in importJson.Diff(exportJson))
                {
                    Debug.Log(kv);
                }

                Assert.AreEqual(importJson, exportJson);
            }
        }
    }
}
