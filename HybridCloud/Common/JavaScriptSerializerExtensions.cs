﻿// ---------------------------------------------------------
// Copyright (c) Schakra Inc. All rights reserved.
// ---------------------------------------------------------

using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.Common
{
    public static class JavaScriptSerializerExtensions
    {
        private static object syncLock = new object();
        private static volatile ScriptingJsonSerializationSection scriptJsonSerializationSection;
        public static ScriptingJsonSerializationSection GetScriptingJsonSerializationSection()
        {
            if (scriptJsonSerializationSection == null)
            {
                lock (syncLock)
                {
                    if (scriptJsonSerializationSection == null)
                    {
                        System.Configuration.Configuration webConfiguration = WebConfigurationManager.OpenWebConfiguration("/");

                        // Get the object related to the <system.web.extensions> section
                        scriptJsonSerializationSection = (ScriptingJsonSerializationSection)webConfiguration.GetSection("system.web.extensions/scripting/webServices/jsonSerialization");
                    }
                }
            }

            return scriptJsonSerializationSection;
        }

        public static JavaScriptSerializer CreateJavaScriptSerializer()
        {
            // Create Serializer and initialize it with the right settings
            ScriptingJsonSerializationSection configSection = GetScriptingJsonSerializationSection();
            JavaScriptSerializer javascriptSerializer = new JavaScriptSerializer();
            javascriptSerializer.MaxJsonLength = configSection.MaxJsonLength;
            javascriptSerializer.RecursionLimit = configSection.RecursionLimit;

            return javascriptSerializer;
        }
    }
}
