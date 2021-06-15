using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class GitHookInit 
{
   
    static string  pattern = @"##version=([0-9\.]+)#";
    [InitializeOnLoadMethod]
    static void BuildHookEnv()
    {
        var dataPath = Application.dataPath;
        var gitHookPath = dataPath.Remove(dataPath.Length - 6);
        gitHookPath += ".git/hooks/";
        // 创建error记录文件
        
        try
        {
            var errorFilePath = gitHookPath + "error.txt";
            File.Delete(errorFilePath);
            if (!File.Exists(errorFilePath))
            {
                var stream = File.CreateText(gitHookPath + "error.txt");
                Debug.Log("注入error缓存文件");
                //stream.Write("Assets/3rd/TextMesh Pro/Load/Fonts & Materials/fallback SDF.asset#@#不允许提交该文件,如果必须提交请ding fengsiyuan\n");
                stream.Flush();
                stream.Dispose();
            }

            // 检查是否有pre-push hook文件
            var pushFilePath = gitHookPath + "pre-push";
            var template = ReadHookTemplate("pre-push");
            if (!File.Exists(pushFilePath))
            {
                InjectTemplate(template, pushFilePath);
            }
            else
            {
                var version = Regex.Match(template, pattern).Groups[1].ToString();
                StreamReader reader = new StreamReader(pushFilePath);
                var code = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
                if (Regex.IsMatch(code, pattern))
                {
                    var match = Regex.Match(code, pattern);

                    if (match.Groups[1].ToString() != version)
                    {
                        InjectTemplate(template, pushFilePath);
                    }
                }
                else
                {
                    InjectTemplate(template, pushFilePath);
                }

            }
        }
        catch (System. Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    static void InjectTemplate(string template, string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
   
        var stream = File.CreateText(path);
        stream.Write(template);
        stream.Flush();
        stream.Dispose();
        Debug.LogFormat("注入拦截文件=>{0}", path);
    }

    [MenuItem("GitTool/检查升级GITHOOK版本")]
    static void ReinitEnv()
    {
        BuildHookEnv();
    }

    static string ReadHookTemplate(string name)
    {
        StreamReader reader = new StreamReader(Application.dataPath + "/Editor/GitHook/"+ name);
        var content = reader.ReadToEnd();
        reader.Dispose();
        return content;
    }
}
