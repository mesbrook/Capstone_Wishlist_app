using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.ServiceModel.Channels;

namespace RetailService.Amazon {
    internal class AmazonMessageHeader : MessageHeader {
        private const string AmazonSecurityNamespace = "http://security.amazonaws.com/doc/2007-01-01/";
        private string _name;

        public AmazonMessageHeader(string name) {
            _name = name;
        }
        public override string Name { get { return _name; } }
        public override string Namespace { get { return AmazonSecurityNamespace; } }
        public string Value { get; set; }

        protected override void OnWriteHeaderContents(System.Xml.XmlDictionaryWriter writer, MessageVersion messageVersion) {
            writer.WriteString(Value);
        }
    }

    public class AmazonMessageSigner : IClientMessageInspector {
        private const string AmazonSecurityNamespace = "http://security.amazonaws.com/doc/2007-01-01/";
        private string _secretKey;
        private string _accessKey;

        public AmazonMessageSigner(string secretKey, string accessKey) {
            _secretKey = secretKey;
            _accessKey = accessKey;
        }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState) {}

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel) {
            var action = request.Headers.Action;
            var actionName = action.Substring(action.LastIndexOf('/') + 1);
            var timeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            var signature = actionName + timeStamp;
            var signatureBytes = Encoding.UTF8.GetBytes(signature);
            var hasher = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
            var hashedSignature = hasher.ComputeHash(signatureBytes);
            var encodedSignature = Convert.ToBase64String(hashedSignature);

            request.Headers.Add(new AmazonMessageHeader("AWSAccessKeyId") {
                Value = _accessKey
            });
            request.Headers.Add(new AmazonMessageHeader("Timestamp") {
                Value = timeStamp
            });
            request.Headers.Add(new AmazonMessageHeader("Signature") {
                Value = encodedSignature
            });

            return null;
        }
    }
}
