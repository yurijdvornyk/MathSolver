using ProblemSdk.Result;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;

namespace IntegralEquationsApp.Components.Result.Charts.Surface
{
    class SurfaceHelper
    {
        private static readonly string appDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;

        public static string GetPageContent(List<Chart3dPoint> points)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("<!DOCTYPE HTML><html><head><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/> <style>body {font: 10pt arial;}</style><script type=\"text/javascript\">" + getVisJs() + "</script></head>");
            appendBody(result, points);
            result.AppendLine("</script></body></html>");
            return result.ToString();
        }

        public static void SavePageToFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        private static void appendBody(StringBuilder result, List<Chart3dPoint> points)
        {
            result.AppendLine("<body><div id=\"visualization\"></div>");
            result.AppendLine("<script type=\"text/javascript\">var data = new vis.DataSet();");
            points.ForEach(point => result.AppendLine("data.add({x:" + point.X + ",y:" + point.Y + ",z:" + point.Z + "});"));
            result.AppendLine("var options = {width: window.innerWidth + 'px', height: window.innerHeight + 'px', style: 'surface', showPerspective: true, showGrid: true, showShadow: false, keepAspectRatio: true, verticalRatio: 0.5};");
            result.AppendLine("var container = document.getElementById('visualization');");
            result.AppendLine("var graph3d = new vis.Graph3d(container, data, options);");
            result.AppendLine("function resize(height){var container=document.getElementById('visualization');container.style.height=height+'px';graph3d.redraw();}");
        }

        private static string getVisJs()
        {
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vis.min.js"));
        }
    }
}
