using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace WkHtmlToPDFTest
{
    class Program
    {
        static void Main(string[] args)
        {
            HtmlPathToPdf();

            //HtmlUrlToPdf();

            Console.ReadKey();
        }

        static void HtmlPathToPdf()
        {
            var pdfPath = "d:/" + Guid.NewGuid().ToString("N") + ".pdf";

            var htmlPath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "1.html");
            //var htmlPath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "2.html");

            var resPath = HtmlToPdf(htmlPath1, pdfPath);

            Console.WriteLine($"resPath={resPath}");
        }

        static void HtmlUrlToPdf()
        {
            var pdfPath = "d:/" + Guid.NewGuid().ToString("N") + ".pdf";
            var htmlUrl = "http://192.168.0.167:8100/docbook/regulations/505535862431744.html";
            var resUrl = HtmlToPdf(htmlUrl, pdfPath);

            Console.WriteLine($"resUrl={resUrl}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html">本地html路径或url地址</param>
        /// <param name="pdfPath"></param>
        /// <returns></returns>
        static bool HtmlToPdf(string html, string pdfPath)
        {
            //参数 https://wkhtmltopdf.org/usage/wkhtmltopdf.txt

            try
            {
                //ProcessStartInfo
                var process = new Process();
                string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libs", "wkhtmltopdf.exe");
                if (!File.Exists(exePath))
                    return false;

                var Arguments = GetArguments(html, pdfPath);

                process.StartInfo.FileName = exePath;
                process.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
                process.StartInfo.Arguments = Arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();

                process.Close();
                process.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html">本地html路径或url地址</param>
        /// <param name="pdfPath"></param>
        /// <returns></returns>
        static string GetArguments(string html, string pdfPath)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(" --page-height 100 ");
            //sb.Append(" --page-width 100 ");
            sb.Append(" --header-center 页眉 ");
            sb.Append(" --header-line ");//页眉和内容显示横线
            sb.Append(" --footer-center \"页码 [page] / [topage]\" ");
            sb.Append(" --footer-line ");
            sb.Append(" ");
            sb.Append(html);
            sb.Append(" ");
            sb.Append(pdfPath);
            sb.Append(" ");

            return sb.ToString();
        }
    }
}
