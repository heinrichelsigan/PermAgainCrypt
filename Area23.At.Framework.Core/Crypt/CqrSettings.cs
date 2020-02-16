using Area23.At.Framework.Core.Crypt.Msg;
using Area23.At.Framework.Core.Util;
using Newtonsoft.Json;

namespace Area23.At.Framework.Core.Crypt
{

    public class CryptSettings
    {
        // TODO: replace it in C# 9.0 to private static readonly lock _lock
        protected static readonly object _lock = true;

        protected static readonly Lazy<CryptSettings> _instance =
            new Lazy<CryptSettings>(() => new CryptSettings());

        #region properties

        public static CryptSettings Instance { get => _instance.Value; }

        public SerType Serializet { get; set; }

        public DateTime TimeStamp { get; set; }

        public DateTime SaveStamp { get; set; }


        public List<string> MyIPs { get; set; }

        public List<string> SecretKeys { get; set; }

        #endregion properties

        #region ctor CryptSettings() CryptSettings(DateTime timeStamp) => Load()

        /// <summary>
        /// CryptSettings constructor maybe needed public for NewTonSoftJson serializing object
        /// </summary>
        public CryptSettings()
        {
            TimeStamp = DateTime.Now;
            Serializet = SerType.Json;
            MyIPs = new List<string>();           
            SecretKeys = new List<string>();
        }


        /// <summary>
        /// ctor with inital timestamp
        /// </summary>
        /// <param name="timeStamp"></param>
        public CryptSettings(DateTime timeStamp) : this()
        {
            TimeStamp = timeStamp;
            Load();
        }

        #endregion ctor CryptSettings() CryptSettings(DateTime timeStamp) => Load()

        #region static members Load() Save(Settings? settings)


        /// <summary>
        /// loads json serialized Settings data string from 
        /// <see cref="LibPaths.AppDirPath"/> + <see cref="Constants.JSON_SAVE_FILE"/>
        /// and deserialize it to singleton instance <see cref="CryptSettings"/> of <seealso cref="Lazy{Settings}"/>
        /// </summary>
        /// <param name="jsonFileName">fileName of serialized json</param>
        /// <returns>singelton <see cref="CryptSettings.Instance"/></returns>
        public CryptSettings Load(string jsonFileName = null)
        {
            string settingsSerializeString = string.Empty;
            Serializet = (Serializet != SerType.None) ? Serializet : SerType.Json;
            CryptSettings settings = null;
            jsonFileName = jsonFileName ?? LibPaths.SystemDirPath + Constants.JSON_SETTINGS_FILE;
            try
            {
                if (File.Exists(jsonFileName))
                {
                    settingsSerializeString = File.ReadAllText(jsonFileName);
                    settings = (CryptSettings)Serializet.DeCerialize<CryptSettings>(settingsSerializeString);
                }
            }
            catch (Exception ex)
            {
                CException.SetLastException(ex);
            }

            if (settings != null)
            {
                _instance.Value.MyIPs = settings.MyIPs;
                _instance.Value.TimeStamp = settings.TimeStamp;
                _instance.Value.SaveStamp = settings.SaveStamp;
            }

            return _instance.Value;
        }


        /// <summary>
        /// json serializes <see cref="CryptSettings"/> and 
        /// saves json serialized data string to 
        /// <see cref="LibPaths.AppDirPath"/> + <see cref="Constants.JSON_SAVE_FILE"/>
        /// </summary>
        /// <param name="CryptSettings">settings to save</param>
        /// <param name="jsonFileName">filename, where writing serialized json</param>
        /// <returns>true on successfully save</returns>
        public static bool Save(CryptSettings settings = null, string jsonFileName = null)
        {
            settings = settings ?? CryptSettings.Instance;
            jsonFileName = jsonFileName ?? LibPaths.SystemDirPath + Constants.JSON_SETTINGS_FILE;
            try
            {
                settings.SaveStamp = DateTime.Now;
                string saveString = JsonConvert.SerializeObject(settings);
                File.WriteAllText(jsonFileName, saveString);
            }
            catch (Exception ex)
            {
                CException.SetLastException(ex);
                return false;
            }

            return true;
        }

        #endregion static members Load() Save(CryptSettings? settings)
        
    }

    
}
