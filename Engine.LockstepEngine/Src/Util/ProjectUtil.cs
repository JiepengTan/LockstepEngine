// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lockstep.Logging;

namespace Lockstep.Util {
    public class ProjectUtil {
        public static void Log(object obj){
            Console.WriteLine(obj.ToString());
        }

        public static void Log(string format, params object[] par){
            Console.WriteLine(string.Format(format, par));
        }

        public static void LogError(object obj){
            Console.WriteLine(obj.ToString());
        }

        public static void LogError(string format, params object[] par){
            Console.WriteLine(string.Format(format, par));
        }

        public static void UpdateProjectFile(string projectPath, string fileExt = "*.cs"){
            List<string> paths = new List<string>();
            string projectFilePath = "";
            PathUtil.Walk(projectPath, fileExt, (file) => { paths.Add(file); });
            PathUtil.Walk(projectPath, "*.csproj", (file) => { projectFilePath = file; });
            var dir = Path.GetDirectoryName(projectFilePath).Replace("\\", "/");
            if (!dir.EndsWith("/")) {
                dir = dir + "/";
            }

            var lines = File.ReadAllLines(projectFilePath);
            var finalLines = new List<string>();
            int firstLine = -1;
            var prefixSpaceStr = "\t\t";
            for (int i = 0; i < lines.Length; i++) {
                var isCodeFileDefine = lines[i].Trim().StartsWith("<Compile Include=");
                if (firstLine == -1 && isCodeFileDefine) {
                    prefixSpaceStr = lines[i].Substring(0, lines[i].IndexOf("<Compile Include="));
                    firstLine = i;
                }

                if (!isCodeFileDefine) {
                    finalLines.Add(lines[i]);
                }
            }

            var prefixLen = dir.Length;
            paths.Sort();
            for (int i = 0; i < paths.Count; i++) {
                paths[i] = prefixSpaceStr + "<Compile Include=\"" + paths[i].Replace("\\", "/").Substring(prefixLen) +
                           "\" />";
            }

            StringBuilder codeFileSb = new StringBuilder();
            foreach (var path in paths) {
                codeFileSb.AppendLine(path);
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < firstLine; i++) {
                sb.AppendLine(finalLines[i]);
            }

            if (firstLine == -1) {
                for (int i = 0; i < lines.Length; i++) {
                    if (lines[i].Trim().StartsWith("<ItemGroup>")) {
                        firstLine = i;
                        break;
                    }
                }

                if (firstLine == -1) {
                    Debug.LogError("!!!! file error! should has one <ItemGroup>");
                    return;
                }

                for (int i = 0; i < firstLine; i++) {
                    sb.AppendLine(finalLines[i]);
                }

                sb.AppendLine("\t<ItemGroup>");
                sb.Append(codeFileSb.ToString());
                sb.AppendLine("\t</ItemGroup>");
            }
            else {
                sb.Append(codeFileSb.ToString());
            }

            for (int i = firstLine; i < finalLines.Count; i++) {
                sb.AppendLine(finalLines[i]);
            }

            File.WriteAllText(projectFilePath, sb.ToString());
        }
    }
}