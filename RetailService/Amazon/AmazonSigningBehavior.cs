using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace RetailService.Amazon {
    class AmazonSigningBehavior : IEndpointBehavior {
        private string _accessKey;
        private string _secretKey;

        public AmazonSigningBehavior(string accessKey, string secretKey) {
            _accessKey = accessKey;
            _secretKey = secretKey;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) {}
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) {}
        public void Validate(ServiceEndpoint endpoint) {}

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime) {
            clientRuntime.ClientMessageInspectors.Add(new AmazonMessageSigner(_secretKey, _accessKey));
        }
    }
}
