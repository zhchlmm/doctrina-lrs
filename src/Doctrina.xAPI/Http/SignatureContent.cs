using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Http
{
    /// <summary>
    /// 
    /// </summary>
    /// https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Data.md#signature-requirements
    public class SignatureContent : HttpContent
    {
        public SignatureContent()
        {
        }

        public SignatureContent(Attachment attachment)
        {

        }

        private void Decode()
        {
            //TODO: Decode the JWS signature, and load the signed serialization of the Statement from the JWS signature payload.
            throw new NotImplementedException();
        }

        private bool Validation(Statement received)
        {
            //TODO: Validate that the original Statement is logically equivalent to the received Statement.
            throw new NotImplementedException();
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            throw new NotImplementedException();
        }

        protected override bool TryComputeLength(out long length)
        {
            throw new NotImplementedException();
        }
    }
}
