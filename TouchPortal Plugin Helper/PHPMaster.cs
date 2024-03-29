﻿using System;
using System.Net;
using System.Text;

namespace TouchPortal_Plugin_Helper
{
    public abstract class PHPMaster
    {
        public static string GetPost(string Url, params string[] postdata)
        {
            string result = string.Empty;
            string data = string.Empty;

            ASCIIEncoding ascii = new ASCIIEncoding();

            if (postdata.Length % 2 != 0)
            {
                return string.Empty;
            }

            for (int i = 0; i < postdata.Length; i += 2)
            {
                data += string.Format("&{0}={1}", postdata[i], postdata[i + 1]);
            }

            data = data.Remove(0, 1);

            byte[] bytesarr = ascii.GetBytes(data);
            try
            {
                WebRequest request = WebRequest.Create(Url);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytesarr.Length;

                System.IO.Stream streamwriter = request.GetRequestStream();
                streamwriter.Write(bytesarr, 0, bytesarr.Length);
                streamwriter.Close();

                WebResponse response = request.GetResponse();
                streamwriter = response.GetResponseStream();

                System.IO.StreamReader streamread = new System.IO.StreamReader(streamwriter);
                result = streamread.ReadToEnd();
                streamread.Close();
            }
            catch (Exception)
            { }
            return result;
        }
    }
}
