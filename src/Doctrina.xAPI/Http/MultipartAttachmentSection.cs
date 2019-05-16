using Doctrina.xAPI.Exceptions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Http
{
    public class MultipartAttachmentSection
    {
        private MultipartSection section;
        public string XExperienceApiHash { get; set; }

        public MultipartAttachmentSection(MultipartSection section)
        {
            this.section = section;

            if (section.Headers.TryGetValue("Content-Transfer-Encoding", out StringValues cteValues))
            {
                string value = cteValues;
                if (value != "binary")
                {
                    throw new MultipartSectionException("MUST include a Content-Transfer-Encoding parameter with a value of binary in each part's header after the first (Statements) part.");
                }
            }
            else
            {
                // 
                throw new MultipartSectionException("'Content-Transfer-Encoding' header is missing.");
            }

            if (section.Headers.TryGetValue(Http.Headers.XExperienceApiHash, out StringValues hashValues))
            {
                XExperienceApiHash = hashValues;
            }
            else
            {
                // MUST include a Content-Transfer-Encoding parameter with a value of binary in each part's header after the first (Statements) part.
                throw new MultipartSectionException($"'{Http.Headers.XExperienceApiHash}' is missing.");
            }
        }

        public async Task<byte[]> ReadAsByteArrayAsync()
        {
            var stringPayload = await section.ReadAsStringAsync();
            byte[] payloadBytes = Encoding.UTF8.GetBytes(stringPayload);

            return payloadBytes;
        }

    }
}