using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Text.RegularExpressions;
using ServiceStack.ServiceClient.Web;
using System.Security.Cryptography;
using Encoder = Microsoft.Security.Application.Encoder;

namespace TransIp.Api
{

	/// <summary>
	/// The base client for TransIP services.
	/// </summary>
	public abstract class ClientBase
	{
		private readonly SoapServiceClient _client;
		private readonly Uri _uri;
		private readonly string _privateKey;

		/// <summary>
		/// Gets the name of the service.
		/// </summary>
		public string ServiceName { get; set; }

		/// <summary>
		/// Gets the login.
		/// </summary>
		public string Login { get; private set; }

		/// <summary>
		/// Gets the mode.
		/// </summary>
		public ClientMode Mode { get; private set; }

		/// <summary>
		/// Creates a new client for communicating with TransIP services.
		/// </summary>
		/// <param name="serviceName">The name of the service.</param>
		/// <param name="uri">The URI of the service.</param>
		/// <param name="login">The login name from TransIP.</param>
		/// <param name="mode">The mode.</param>
		/// <param name="privateKey">The private key.</param>
		protected ClientBase(string serviceName, string uri, string login, ClientMode mode, string privateKey)
		{
			_client = new SoapServiceClient(uri);
			_uri = new Uri(uri);
			_privateKey = privateKey;
			ServiceName = serviceName;
			Login = login;
			Mode = mode;

			// Set the static cookies.
			AddCookie("login", Login);
			AddCookie("mode", Mode.ToString().ToLower());
		}

		/// <summary>
		/// Update the signature cookies for the given
		/// </summary>
		/// <param name="method">The name of the method being called.</param>
		/// <param name="args">The passed arguments.</param>
		protected void SetSignatureCookies(string method, object[] args)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var timestamp = Convert.ToInt64((DateTime.UtcNow - epoch).TotalSeconds);

			var nonce = Guid.NewGuid().ToString("N");

			AddCookie("timestamp", timestamp.ToString());
			AddCookie("nonce", nonce);
			AddCookie("signature", EncryptionHelper.Encode(EncryptionHelper.Sign(_privateKey, args.Concat(new object[]
			{
				new KeyValuePair<string, string>("__method", method),
				new KeyValuePair<string, string>("__service", ServiceName),
				new KeyValuePair<string, string>("__hostname", "api.transip.nl"),
				new KeyValuePair<string, string>("__timestamp", timestamp.ToString()),
				new KeyValuePair<string, string>("__nonce", nonce)
			}).ToArray())));
		}

		private void AddCookie(string name, string value)
		{
			_client.Cookies.Add(new Cookie(name, value, "/", _uri.DnsSafeHost));
		}

		/// <summary>
		/// Sends the given message.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="args">The arguments.</param>
		public void Send(string action, params object[] args)
		{
			SetSignatureCookies(action, args);
			_client.SendWithCookies(action, args);
		}

		/// <summary>
		/// Sends the given message and returns the response.
		/// </summary>
		/// <typeparam name="T">The response type.</typeparam>
		/// <param name="action">The action.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>The response message.</returns>
		public T Send<T>(string action, params object[] args)
		{
			SetSignatureCookies(action, args);
			return _client.SendWithCookies<T>(action, args);
		}
	}

	/// <summary>
	/// The different modes in which the client can operate.
	/// </summary>
	public enum ClientMode
	{
		/// <summary>
		/// Only allow reading.
		/// </summary>
		ReadOnly,

		/// <summary>
		/// Allow both reading and writing.
		/// </summary>
		ReadWrite
	}
}
