using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics;
using System.Xml;
public class SourceTreePrefabMergeInit
{
    [InitializeOnLoadMethod]
    static void TryInitSourcePrefabMergeTool()
    {

    }

    

    [MenuItem("GitTool/为SourceTree配置Prefab比对工具")]
    static void Config()
    {

        var appSourceTreeDir = @"C:\Users\" + System.Environment.UserName + @"\AppData\Local\Atlassian";
        var sourceTreeConfigPath = appSourceTreeDir+ @"\SourceTree\hgrc_sourcetree";
        var fileInfo = new FileInfo(sourceTreeConfigPath);
        if (!fileInfo.Exists)
        {
            return;
        }

        string config2 = @"[merge-tools]
sourcetreemerge.executable=#PATH#\Data\Tools\UnityYAMLMerge.exe
sourcetreemerge.args=merge -p $base $other $local $output
sourcetreemerge.premerge=True
sourcetreemerge.checkconflicts=True
sourcetreemerge.binary=True
sourcetreemerge.gui=False";

        var injectCode = config2.Replace("#PATH#", 
            System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase);

        var read = fileInfo.OpenText();
        var content = read.ReadToEnd();
        read.Close();

        string newContent = "";
        var writeKey = true;
        if (Regex.IsMatch(content, @"(\[merge-tools\][^\[]*)"))
        {
            var match = Regex.Match(content, @"(\[merge-tools\][^\[]*)");
            string old = match.Groups[1].Value;

            if (old == injectCode)
            {
                writeKey = false;
            }
            else
            {

                newContent = content.Replace(old, injectCode);
            }

        }
        else
        {
            newContent = content + "\n" + injectCode;
        }

        if (writeKey)
        {
            var write = fileInfo.Open(System.IO.FileMode.Open, System.IO.FileAccess.Write, FileShare.ReadWrite);

            var bytes = Encoding.Default.GetBytes(newContent);
            write.Write(bytes, 0, bytes.Length);

            write.Flush();
            write.Close();
        }


      

        Process[] myproc = Process.GetProcesses();
        foreach (Process item in myproc)
        {
            if (item.ProcessName == "SourceTree")
            {
                item.Kill();
                break;
            }
        }

        var dirInfo = new DirectoryInfo(appSourceTreeDir);

       var dirs = dirInfo.GetDirectories();
        var files = dirInfo.GetFiles("*.config", SearchOption.AllDirectories);

        foreach (var item in files)
        {
            if (Path.GetFileName(item.FullName) == "user.config")
            {
                XmlDocument xml = new XmlDocument();
                var stream = item.Open(FileMode.Open);
                xml.Load(stream);

                var node = xml["configuration"]["userSettings"]["SourceTree.Properties.Settings"];

                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    var n = node.ChildNodes[i].Attributes["name"].Value;
                    if (n == "MergeTool")
                    {
                        var targetNode = node.ChildNodes[i];
                      
                    }
                }

                xml.Save(stream);
              
                stream.Close();

            }
        }
            
 


        // string bbb = myreg.Replace(html, "base64,http://www.66kangba.com\"");

    }
}
