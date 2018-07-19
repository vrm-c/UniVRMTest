using NUnit.Framework;
using System.IO;
using UnityEngine;


namespace VRM
{
    public class VRMTests
    {
        [Test]
        public void ImportExportTest()
        {
            var path = Path.GetFullPath(Application.dataPath + "/../Models/Alicia_vrm-0.40/AliciaSolid_vrm-0.40.vrm");
            var context = new VRMImporterContext(path);
            context.ParseGlb(File.ReadAllBytes(path));
            VRMImporter.LoadFromBytes(context);
            var importJson = context.Json
                .Replace("UniVRM-0.40", "UniVRM-0.41")
                .Replace("UniGLTF-1.10", "UniVRM-0.41")
                .Replace("\"skinRootBone\":-1", "\"skinRootBone\":0")
                .Trim()
                ;

            var vrm = VRMExporter.Export(context.Root);
            var exportJson = vrm.ToJson().Trim();

            Assert.AreEqual(importJson, exportJson);
        }
    }
}
