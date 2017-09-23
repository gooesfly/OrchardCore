using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OrchardCore.Tests.Apis
{
    /// <summary>
    /// The http request extensions.
    /// </summary>
    public static class HttpRequestExtensions
    {
        private readonly static JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// The patch as json async.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="formatter">
        /// The formatter.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            JsonSerializerSettings settings = null)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(value, settings ?? JsonSettings),
                Encoding.UTF8,
                "application/json");

            return await client.PatchAsync(requestUri, content).ConfigureAwait(false);
        }

        /// <summary>
        /// The patch async.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task<HttpResponseMessage> PatchAsync(
            this HttpClient client,
            string requestUri,
            HttpContent content)
        {
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod("PATCH"),
                RequestUri = new Uri(client.BaseAddress + requestUri),
                Content = content
            };
            client.DefaultRequestHeaders.ExpectContinue = false;
            return await client.SendAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// The put as json async.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="formatter">
        /// The formatter.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task<HttpResponseMessage> PutAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            JsonSerializerSettings settings = null)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(value, settings ?? JsonSettings),
                Encoding.UTF8,
                "application/json");

            return await client.PutAsync(requestUri, content).ConfigureAwait(false);
        }

        /// <summary>
        /// PostAsJsonAsync
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="requestUri">
        /// The request uri.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="formatter">
        /// The formatter.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value,
            JsonSerializerSettings settings = null)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(value, settings ?? JsonSettings),
                Encoding.UTF8,
                "application/json");

            client.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client.PostAsync(requestUri, content);
        }

        public static Task<HttpResponseMessage> PostJsonAsync(
            this HttpClient client,
            string requestUri,
            string json)
        {
            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            client.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client.PostAsync(requestUri, content);
        }

        public static Task<HttpResponseMessage> PostJsonApiAsync(
            this HttpClient client,
            string requestUri,
            string json)
        {
            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/vnd.api+json");

            client.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/vnd.api+json"));

            client.DefaultRequestHeaders
                .Add("Content-Type", "application/vnd.api+json");

            return client.PostAsync(requestUri, content);
        }
    }
}