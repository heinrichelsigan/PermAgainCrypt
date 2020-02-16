# Crypt namespace

## Cipher namespace

contains various cipher implementations 

## EnDeCoding namespace

contains various encoding and decoding implementations

## Hash namespace

contains various hashing implementations

## 

## CryptSettings.cs

basic singleton for persiting runtime settings

## CException.cs

an application specific exception, which also provides has a linked list to all former exceptions

## Msg

### CFile
basic filesystem file

### CContact
basic contact 

### CImage
basic image 

### CMsg 
basic content message

### CChatRoom
Chatroom abstraction

### CType
<pre>	
	[Serializable]
    public enum CType
    {
        None	= 0,
        Json 	= 1,
        Xml 	= 2,
        Mime 	= 3,
        Raw 	= 4
    }
</pre>

### IMsgAble
basic interface for messages
<pre>
	public interface IMsgAble
    {
        CType MsgType { get; }        
        string Message { get; }
        string SerializedMsg { get; }

        string Hash { get; }
        string Md5Hash { get; }

        string ToJson();
        T FromJson<T>(string jsonText);
        string ToXml();
        T FromXml<T>(string xmlText);
    }
</pre>
	