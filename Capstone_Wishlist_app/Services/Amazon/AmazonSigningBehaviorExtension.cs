using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Capstone_Wishlist_app.Services.Amazon {
    public class AmazonSigningBehaviorExtension : BehaviorExtensionElement {
        public override Type BehaviorType {
            get { return typeof(AmazonSigningBehavior); }
        }

        [ConfigurationProperty("accessKey")]
        public string AccessKey {
            get { return (string)base["accessKey"]; }
            set { base["accessKey"] = value; }
        }

        [ConfigurationProperty("secretKey")]
        public string SecretKey {
            get { return (string) base["secretKey"]; }
            set { base["secretKey"] = value; }
        }

        protected override object CreateBehavior() {
            return new AmazonSigningBehavior(AccessKey, SecretKey);
        }
    }
}
