using System;
using System.Net;

namespace MirDev.ChromaDB.Client.V2
{
    /// <summary>
    /// Represents an exception that occurs when interacting with the ChromaDB API.
    /// </summary>
    public class ChromaApiException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code returned by the server.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the error code returned by the API, if available.
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Initializes a new instance of the ChromaApiException class.
        /// </summary>
        /// <param name="statusCode">The HTTP status code of the error.</param>
        /// <param name="error">The error code from the API response.</param>
        /// <param name="message">The error message.</param>
        public ChromaApiException(HttpStatusCode statusCode, string error, string message)
            : base(message)
        {
            StatusCode = statusCode;
            Error = error;
        }
    }
}