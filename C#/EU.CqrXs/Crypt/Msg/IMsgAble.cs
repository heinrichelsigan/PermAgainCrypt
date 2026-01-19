namespace EU.CqrXs.Crypt.Msg
{
    public interface IMsgAble
    {
        SerType Cerializer { get; }        
        string Message { get; }

        string Hash { get; }
        string Md5Hash { get; }

        string Cerialize();
        T? DeCerialize<T>(string jsonText);

    }
}
