using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace TransIp.Api
{
	internal class CookieEndpointBehavior : IEndpointBehavior
	{
		private readonly CookieContainer _cookieContainer;

		public CookieEndpointBehavior(CookieContainer cookieContainer)
		{
			_cookieContainer = cookieContainer;
		}

		public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			clientRuntime.MessageInspectors.Add(new CookieMessageInspector(_cookieContainer));
		}

		public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
		}

		public void Validate(ServiceEndpoint endpoint)
		{
		}
	}
}
