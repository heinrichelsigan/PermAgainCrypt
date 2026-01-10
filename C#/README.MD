## C#

PermAgainCrypt cipher pipe csharp project can be launched by Visual Studio 2026 with C# .Net10 Core installed.
Following projects are availible under C# inside solution file EU.CqrXs.sln
- Framework EU\CqrXs\EU.CqrXs.csproj 
- Console Application EU\CqrXs\Console\EU.CqrXs.Console.csproj
- WinForm Gui EU\CqrXs\Gui\EU.CqrXs.Gui.csproj
- Test Project EU\CqrXs\Test\EU.CqrXs.Test.csproj 


## C# file tree

```
.
├── EU
│   ├── CqrXs
│   │   ├── Console
│   │   │   ├── Console_Test.bat
│   │   │   ├── EU.CqrXs.Console.csproj
│   │   │   ├── Program.cs
│   │   │   ├── Properties
│   │   │   │   └── launchSettings.json
│   │   │   ├── README.MD
│   │   │   └── SatIcon.ico
│   │   ├── Crypt
│   │   │   ├── CException.cs
│   │   │   ├── Cipher
│   │   │   │   ├── Asymmetric
│   │   │   │   │   ├── DH.cs
│   │   │   │   │   ├── Dsa.cs
│   │   │   │   │   └── Rsa.cs
│   │   │   │   ├── CipherEnum.cs
│   │   │   │   ├── CipherPipe.cs
│   │   │   │   ├── CryptHelper.cs
│   │   │   │   ├── CryptParams.cs
│   │   │   │   ├── README.MD
│   │   │   │   └── Symmetric
│   │   │   │       ├── AesNet.cs
│   │   │   │       ├── CryptBounceCastle.cs
│   │   │   │       ├── CryptParamsPrefered.cs
│   │   │   │       ├── Des3Net.cs
│   │   │   │       ├── FishRequiresAesEngineException.cs
│   │   │   │       ├── SymmCipherEnum.cs
│   │   │   │       ├── SymmCipherPipe.cs
│   │   │   │       ├── SymmCrypt.cs
│   │   │   │       ├── ZenMatrix2.cs
│   │   │   │       └── ZenMatrix.cs
│   │   │   ├── CqrSettings.cs
│   │   │   ├── EnDeCoding
│   │   │   │   ├── Base16.cs
│   │   │   │   ├── Base32.cs
│   │   │   │   ├── Base64.cs
│   │   │   │   ├── DeEnCoder.cs
│   │   │   │   ├── EncodingType.cs
│   │   │   │   ├── EnDeCodeHelper.cs
│   │   │   │   ├── EnDeCoder.cs
│   │   │   │   ├── Hex16.cs
│   │   │   │   ├── Hex32.cs
│   │   │   │   ├── IDecodable.cs
│   │   │   │   ├── RawString.cs
│   │   │   │   ├── README.MD
│   │   │   │   ├── Uu.cs
│   │   │   │   └── Xx.cs
│   │   │   ├── Hash
│   │   │   │   ├── Ascon256.cs
│   │   │   │   ├── BCrypt.cs
│   │   │   │   ├── Blake2xs.cs
│   │   │   │   ├── CShake.cs
│   │   │   │   ├── Dstu7564.cs
│   │   │   │   ├── KeyHash.cs
│   │   │   │   ├── MD5Sum.cs
│   │   │   │   ├── OpenBSDCrypt.cs
│   │   │   │   ├── RipeMD256.cs
│   │   │   │   ├── SCrypt.cs
│   │   │   │   ├── Sha1.cs
│   │   │   │   ├── Sha256Sum.cs
│   │   │   │   ├── Sha384.cs
│   │   │   │   ├── Sha512Sum.cs
│   │   │   │   ├── TupleHash.cs
│   │   │   │   ├── Whirlpool.cs
│   │   │   │   └── Xoodyak.cs
│   │   │   ├── Msg
│   │   │   │   ├── CContact.cs
│   │   │   │   ├── CFile.cs
│   │   │   │   ├── CImage.cs
│   │   │   │   ├── CMsg.cs
│   │   │   │   ├── CryptMsg.cs
│   │   │   │   ├── IMsgAble.cs
│   │   │   │   └── SerType.cs
│   │   │   └── README.MD
│   │   ├── Gui
│   │   │   ├── Controls
│   │   │   │   ├── GroupBoxFiles.cs
│   │   │   │   ├── GroupBoxFiles.Designer.cs
│   │   │   │   ├── GroupBoxFiles.resx
│   │   │   │   └── RadioButtonList.cs
│   │   │   ├── EU.CqrXs.Gui.csproj
│   │   │   ├── Forms
│   │   │   │   ├── AboutDialog.cs
│   │   │   │   ├── AboutDialog.Designer.cs
│   │   │   │   ├── AboutDialog.resx
│   │   │   │   ├── CharHexDecOctBinDialog.cs
│   │   │   │   ├── CharHexDecOctBinDialog.Designer.cs
│   │   │   │   ├── CharHexDecOctBinDialog.resx
│   │   │   │   ├── EncryptFormBase.cs
│   │   │   │   ├── EncryptFormBase.resx
│   │   │   │   ├── EncryptForm.cs
│   │   │   │   ├── EncryptForm.Designer.cs
│   │   │   │   ├── EncryptFormMultiControls.cs
│   │   │   │   ├── EncryptFormMultiControls.Designer.cs
│   │   │   │   ├── EncryptFormMultiControls.resx
│   │   │   │   └── EncryptForm.resx
│   │   │   ├── Helper
│   │   │   │   ├── BitmapPipelineGnerator.cs
│   │   │   │   ├── CPipeExtensionHelper.cs
│   │   │   │   ├── DragNDropState.cs
│   │   │   │   └── ExtensionMethods.cs
│   │   │   ├── Program.cs
│   │   │   ├── Properties
│   │   │   │   ├── launchSettings.json
│   │   │   │   ├── Resources.Designer.cs
│   │   │   │   └── Resources.resx
│   │   │   ├── Resources
│   │   │   │   ├── [images]
│   │   │   │   └── [sounds]
│   │   │   ├── Sound
│   │   │   │   └── IPlayable.cs
│   │   │   └── TransparentFormsIcon.ico
│   │   ├── Properties
│   │   │   ├── Area23Framework.xml
│   │   │   ├── fortune_u8.txt
│   │   │   ├── Resource.Designer.cs
│   │   │   └── Resource.resx
│   │   ├── Resources
│   │   │   └── Blank_640x96.png
│   │   ├── Test
│   │   │   ├── 2025-09-23_Stats.gif
│   │   │   ├── App.config
│   │   │   ├── EU.CqrXs.Test.csproj
│   │   │   ├── MSTestSettings.cs
│   │   │   ├── README.MD
│   │   │   ├── TestCompression.cs
│   │   │   ├── TestEncodings.cs
│   │   │   ├── TestEncryptionHashingAlgo.cs
│   │   │   ├── TestEncryptionIVAlgos.cs
│   │   │   ├── TestEncryptionMaxAlgos.cs
│   │   │   ├── TestEncryptionSingleAlgo.cs
│   │   │   ├── TestEncryptionTwoAlgos.cs
│   │   │   └── TestInitCleanup.cs
│   │   ├── Util
│   │   │   ├── Area23EventArgs.cs
│   │   │   ├── Area23Log.cs
│   │   │   ├── Constants.cs
│   │   │   ├── Extensions.cs
│   │   │   ├── JsonHelper.cs
│   │   │   ├── LibPaths.cs
│   │   │   ├── MimeSignature.cs
│   │   │   ├── MimeType.cs
│   │   │   ├── ProcessCmd.cs
│   │   │   ├── README.MD
│   │   │   ├── RegistryAccessor.cs
│   │   │   ├── ResReader.cs
│   │   │   ├── SLog.cs
│   │   │   └── Utils.cs
│   │   └── Zip
│   │       ├── BZip2.cs
│   │       ├── GZ.cs
│   │       ├── README.MD
│   │       ├── WinZip.cs
│   │       └── ZipType.cs
│   ├── EU.CqrXs.csproj
│   ├── Net
│   │   ├── NameService
│   │   │   └── DnsHelper.cs
│   │   ├── NetworkAddresses.cs
│   │   └── WebHttp
│   │       ├── HttpClientRequest.cs
│   │       ├── TcpClientWebRequest.cs
│   │       └── WebClientRequest.cs
│   ├── Properties
│   │   └── Area23Framework.xml
│   └── README.md
├── EU.CqrXs.sln
└── README.MD

34 directories, 348 files
```
