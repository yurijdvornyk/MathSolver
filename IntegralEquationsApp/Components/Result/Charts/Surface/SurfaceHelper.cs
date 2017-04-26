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
            StringBuilder result = new StringBuilder("<!DOCTYPE HTML><html>");
            result.Append("<head>");
            appendHeader(result);
            appendData(result, points);
            result.Append("</head>");
            appendBody(result);
            result.Append("</html>");
            return result.ToString();
        }

        private static void appendBody(StringBuilder result)
        {
            result.Append("<body onload=\"drawVisualization();\"><div id =\"graph\"></div></body>");
        }

        private static StringBuilder appendHeader(StringBuilder result)
        {
            return result.Append("<style>body{font:10pt arial;}</style>" +
                            "<script type=\"text/javascript\">" + getVisJs() + "</script>");
        }

        private static StringBuilder appendData(StringBuilder result, List<Chart3dPoint> points)
        {
            result.Append("<script type=\"text / javascript\">function drawVisualization(){var data = new vis.DataSet();");
            points.ForEach(point => result.Append("data.add({x:" + point.X + ",y: " + point.Y + ",z:" + point.Z + "})"));
            result.Append("var options={width:'600px',height:'600px',style:'surface',showPerspective:true,showGrid:true,showShadow:false,keepAspectRatio: true,verticalRatio: 0.5};");
            result.Append("var container=document.getElementById('graph');graph3d=new vis.Graph3d(container, data, options);}");
            result.Append("</script>");
            return result;
        }

        private static string getVisJs()
        {
            
            return File.ReadAllText(appDir + @"\Components\Result\Charts\Surface\jquery-3.2.1.min.js");
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
