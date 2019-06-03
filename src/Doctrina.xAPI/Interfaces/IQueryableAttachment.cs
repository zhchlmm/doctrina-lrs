namespace Doctrina.xAPI
{
    public interface IAttachmentByHash
    {
        Attachment GetAttachmentByHash(string sha2);
    }
}