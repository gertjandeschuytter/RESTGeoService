using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeoService_RESTLayer.LogHandler {
    public class LogRequestAndResponseHandler {

        public void LogRequestOrResponse(string reqOrResp)
        {
            string filePath = @"C:\Users\Gertj\Desktop\apiLoggings\log.txt";
            StreamWriter writer = null;
            try
            {
                using (writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(reqOrResp);
                }
            } finally
            {
                if(writer != null)
                {
                    writer.Dispose();
                }
            }
        }
    }
}