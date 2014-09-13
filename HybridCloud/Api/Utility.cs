//------------------------------------------------------------
// Copyright (c) Schakra Inc.  All rights reserved.
//------------------------------------------------------------

using Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.ApiClient.DataContracts;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.Api
{
    /// <summary>
    /// Generic Utitlities
    /// </summary>
    internal class Utility
    {
        /// <summary>
        /// This is getting used to return error response. This can be used for methods not returning any specific object
        /// </summary>
        /// <param name="request"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="errorResourceCode"></param>
        /// <returns></returns>
        internal static HttpResponseMessage SendResponseException(HttpRequestMessage request, HttpStatusCode statusCode, string message, string errorResourceCode = null)
        {
            return request.CreateResponse<HybridCloudErrorResource>(statusCode, new HybridCloudErrorResource()
            {
                Code = errorResourceCode,
                Message = message
            });  
        }

        /// <summary>
        /// This method is used to throw exceptions
        /// </summary>
        /// <param name="request"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="errorResourceCode"></param>
        /// <returns></returns>
        internal static HttpResponseException ThrowResponseException(HttpRequestMessage request, HttpStatusCode statusCode, string message, string errorResourceCode = null)
        {
            return new HttpResponseException(
             new HttpResponseMessage(statusCode)
             {
                 Content = new ObjectContent<HybridCloudErrorResource>(
                             new HybridCloudErrorResource()
                             {
                                 Code = errorResourceCode,
                                 Message = message,
                             },
                             new XmlMediaTypeFormatter())
             });                   
        }
       
    }
}