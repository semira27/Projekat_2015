using System;
using System.Collections.Generic;
using System.Configuration;
using System.EnterpriseServices;
using System.Linq;
using System.Web;


namespace Admin.Configuration
{
    public class Config
    {
        public static string[] ALLOWED_EXTENSIONS = { ".jpg", ".png", ".gif", ".jpeg" };

        public static string[] ALLOWED_EXTENSIONS_DOKUMENT = { ".doc",".docx", ".xls", ".pdf", ".zip", ".rar", ".ppt",".pptx", ".xlsx" };

        private static string _FULL_PATH_IKONICE;
        public static string FULL_PATH_IKONICE
        {
            get
            {
                if (_FULL_PATH_IKONICE == null)
                    _FULL_PATH_IKONICE = "Upload" + "/" + "Ikonice" + "/";
                return _FULL_PATH_IKONICE;
            }
        }

        private static string _FULL_PATH_SLIKE;
        public static string FULL_PATH_SLIKE
        {
            get
            {
                if (_FULL_PATH_SLIKE == null)
                    _FULL_PATH_SLIKE = "Upload" + "/" + "Slike" + "/";
                return _FULL_PATH_SLIKE;
            }
        }


        private static string _FULL_PATH_DOKUMENTI;
        public static string FULL_PATH_DOKUMENTI
        {
            get
            {
                if (_FULL_PATH_DOKUMENTI == null)
                    _FULL_PATH_DOKUMENTI = "Upload" + "/" + "Dokumenti" + "/";
                return _FULL_PATH_DOKUMENTI;
            }
        }

    }
}