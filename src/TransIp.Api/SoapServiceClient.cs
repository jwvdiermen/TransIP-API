using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceClient.Web;

namespace TransIp.Api
{
	/// <summary>
	/// Extends the <see cref="Soap12ServiceClient"/> class to support cookies.
	/// </summary>
	internal class SoapServiceClient : Soap12ServiceClient
	{
		/// <summary>
		/// Gets the cookie container.
		/// </summary>
		public CookieContainer Cookies { get; private set; }

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="uri">The location of the service.</param>
		internal SoapServiceClient(string uri)
			: base(uri)
		{
			Cookies = new CookieContainer();
		}

		private ServiceEndpoint SyncReply
		{
			get
			{
				var contract = new ContractDescription("ServiceStack.ServiceClient.Web.ISyncReply", "http://services.servicestack.net/");
				var addr = new EndpointAddress(Uri);
				var endpoint = new ServiceEndpoint(contract, Binding, addr);
				return endpoint;
			}
		}

		private Message SendInternal(Message request)
		{
			using (var client = new GenericProxy<ISyncReply>(SyncReply))
			{
				client.ChannelFactory.Endpoint.Behaviors.Add(new CookieEndpointBehavior(Cookies));

				return client.Proxy.Send(request);
			}
		}

		/// <summary>
		/// Sends the given message.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="args">The arguments.</param>
		public void SendWithCookies(string action, params object[] args)
		{
			var request = Message.CreateMessage(MessageVersion, action, args);
			SendInternal(request);
		}

		/// <summary>
		/// Sends the given message and returns the response.
		/// </summary>
		/// <typeparam name="T">The response message type.</typeparam>
		/// <param name="action">The action.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>The response message.</returns>
		public T SendWithCookies<T>(string action, params object[] args)
		{
			var request = Message.CreateMessage(MessageVersion, action, args);

			try
			{
				var responseMsg = SendInternal(request);
				var response = responseMsg.GetBody<T>();
				var responseStatus = GetResponseStatus(response);
				if (responseStatus != null && !string.IsNullOrEmpty(responseStatus.ErrorCode))
				{
					throw new WebServiceException(responseStatus.Message, null)
					{
						StatusCode = 500,
						ResponseDto = response,
						StatusDescription = responseStatus.Message,
					};
				}
				return response;
			}
			catch (WebServiceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				var webEx = ex as WebException ?? ex.InnerException as WebException;
				if (webEx == null)
				{
					throw new WebServiceException(ex.Message, ex)
					{
						StatusCode = 500,
					};
				}

				var httpEx = webEx.Response as HttpWebResponse;
				throw new WebServiceException(webEx.Message, webEx)
				{
					StatusCode = httpEx != null ? (int)httpEx.StatusCode : 500
				};
			}
		}
	}
}
