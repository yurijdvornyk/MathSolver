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
            StringBuilder result = new StringBuilder(); // "<!DOCTYPE HTML><html>"
            result.AppendLine("<!DOCTYPE HTML><html><head><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/> <style>body {font: 10pt arial;}</style><script type=\"text/javascript\" src=\"" + getVisJs() + "\"></script></head>");
            appendBody(result, points);
            result.AppendLine("</script></body></html>");
            //result.AppendLine("<head>");
            //appendHeader(result);
            //appendData(result, points);
            //result.AppendLine("</head>");
            //appendBody(result);
            //result.AppendLine("</html>");
            return result.ToString();
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

        //private static void appendBody(StringBuilder result)
        //{
        //    result.AppendLine("<body onload=\"drawVisualization();\"><div id =\"graph\"></div></body>");
        //}

        //private static StringBuilder appendHeader(StringBuilder result)
        //{
        //    return result.AppendLine("<style>body{font:10pt arial;}</style>" +
        //                    "<script type=\"text/javascript\">" + getVisJs() + "</script>");
        //}

        //private static StringBuilder appendData(StringBuilder result, List<Chart3dPoint> points)
        //{
        //    result.Append("<script type=\"text/javascript\">function drawVisualization(){var data = new vis.DataSet();");
        //    points.ForEach(point => result.Append("data.add({x:" + point.X + ",y:" + point.Y + ",z:" + point.Z + "})"));
        //    result.Append("var options={width:'600px',height:'600px',style:'surface',showPerspective:true,showGrid:true,showShadow:false,keepAspectRatio: true,verticalRatio: 0.5};");
        //    result.Append("var container=document.getElementById('graph');graph3d=new vis.Graph3d(container, data, options);}");
        //    result.Append("</script>");
        //    return result;
        //}

        private static string getVisJs()
        {
            
            return (appDir + @"\Components\Result\Charts\Surface\vis.min.js");
        }

        private static string getVisCss()
        {
            return File.ReadAllText(appDir + @"\Components\Result\Charts\Surface\vis.min.css");
        }

        private static string getJquery()
        {
            return File.ReadAllText(appDir + @"\Components\Result\Charts\Surface\jquery-3.2.1.min.js");
        }
    }
}
