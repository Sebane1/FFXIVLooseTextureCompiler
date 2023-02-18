// © Anamnesis.
// Licensed under the MIT license.

namespace Anamnesis.Penumbra;

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FFBardMusicPlayer.FFXIV;
using Newtonsoft.Json;

internal static class PenumbraApi
{
	private const string Url = "http://localhost:42069/api";
	private const int TimeoutMs = 500;
	private static bool calledWarningOnce = false;
	public static async Task Post(string route, object content, FFXIVHook hook)
	{
		await PostRequest(route, content, hook);
	}

	public static async Task<T> Post<T>(string route, object content, FFXIVHook hook)
		where T : notnull
	{
		HttpResponseMessage response = await PostRequest(route, content, hook);

		using StreamReader? sr = new StreamReader(await response.Content.ReadAsStreamAsync());
		string json = sr.ReadToEnd();

		return JsonConvert.DeserializeObject<T>(json);
	}

	private static async Task<HttpResponseMessage> PostRequest(string route, object content, FFXIVHook hook)
	{
		if (!route.StartsWith('/'))
			route = '/' + route;

		try
		{
			string json = JsonConvert.SerializeObject(content);

			using HttpClient client = new HttpClient();
			client.Timeout = TimeSpan.FromMilliseconds(TimeoutMs);
			var buffer = Encoding.UTF8.GetBytes(json);
			var byteContent = new ByteArrayContent(buffer);
			byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			using var response = await client.PostAsync(Url + route, byteContent);

			return response;
		}
		catch (Exception ex) {
			if (route.Contains("reload")) {
                hook.SendSyncKey(Keys.Enter);
                Thread.Sleep(500);
                hook.SendString(@"/penumbra reload");
                Thread.Sleep(200);
                hook.SendSyncKey(Keys.Enter);
                Thread.Sleep(2000);
            }
			if (route.Contains("redraw")) {
				hook.SendSyncKey(Keys.Enter);
				Thread.Sleep(500);
				hook.SendString(@"/penumbra redraw self");
				Thread.Sleep(200);
				hook.SendSyncKey(Keys.Enter);
			}
			if (!calledWarningOnce) {
				MessageBox.Show(@"Select ""Enable HTTP API"" inside of penumbra under ""Settings -> Advanced"" for better automatic refresh.");
				calledWarningOnce = true;
			}
            return null;
		}
	}
}
