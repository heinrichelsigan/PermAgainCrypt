/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */

package eu.cqrxs.cipherpipe.util;




/**
 * Util provides only static fields
 */
public class Constants {

	//region c constants´
	public final static int BACKLOG = 8;
	public final static int CHAT_PORT = 7777;
	public final static int MAX_KEY_LEN = 1024;
	public final static int MAX_PIPE_LEN = 8;
	public final static int MAX_SERVER_SOCKET_ADDRESSES = 16;
	public final static int CLOSING_TIMEOUT = 6000;
	public final static int MIN_SOCKET_BYTE_BUFFEER = 65536;       // 64 KB Buffer
	public final static int SOCKET_BYTE_BUFFEER = 1048576;         //  1 MB Buffer
	public final static int MAX_BYTE_BUFFEER = 4194240;            //  4 MB Buffer
	public final static int MAX_SOCKET_BYTE_BUFFEER = 33554432;    //  32 MB Buffer  2^25
	public final static int BGWORKWE_BUSYWAITING_SLEEP = 360000;
	public final static boolean CQR_ENCRYPT = true;
	public final static boolean ZEN_MATRIX_SYMMETRIC = false;
    public final static boolean DEBUG = true;

	public final static char ANNOUNCE = ':';
	public final static char DATE_DELIM = '-';
	public final static char WHITE_SPACE = ' ';
	public final static char UNDER_SCORE = '_';

	public final static String APP_NAME = "Area23.At";
	public final static String APP_DIR = "net";
	public final static String APP_ERROR = "AppError";
	public final static String VERSION = "v2.25.411";
	public final static String VALKEY_CACHE_HOST = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com";
	public final static int VALKEY_CACHE_PORT = 6379;
	public final static String VALKEY_CACHE_HOST_PORT = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
	public final static String VALKEY_CACHE_HOST_PORT_KEY = "ValkeyCacheHostPort";
	public final static String EXTERNAL_CLIENT_IP = "ExternalClientIP";
	public final static String EXTERNAL_CLIENT_IP_V4 = "ExternalClientIPv4";
	public final static String SERVER_IP_V4 = "ServerIPv4";
	public final static String SERVER_IP_V6 = "ServerIPv6";
	public final static String CQR_SERVICE_SOAP = "CqrServiceSoap";
	public final static String CQR_SERVICE_SOAP12 = "CqrServiceSoap12";
	public final static String CQR_SRV_SOAP = "CqrSrvSoap";
	public final static String CQR_SRV_SOAP12 = "CqrSrvSoap12";


	public final static String AREA23_URL = "https://area23.at";
	public final static String APP_PATH = "https://area23.at/net/";
	public final static String RPN_URL = "https://area23.at/net/RpnCalc.aspx";
	public final static String GIT_URL = "https://github.com/heinrichelsigan/area23.at";
	public final static String URL_PIC = "https://area23.at/net/res/img/";
	public final static String URL_PREFIX = "https://area23.at/net/res/";
	public final static String AREA23_S = "https://area23.at/s/";
	public final static String URL_SHORT = "https://area23.at/s/?";
	public final static String AREA23_UTF8_URL = "https://area23.at/u/";

	public final static String AREA23_AT = "area23.at";
	public final static String VIRGINA_AREA23_AT = "virginia.area23.at";
	public final static String PARIS_AREA23_AT = "paris.area23.at";
	public final static String PARISIENNE_AREA23_AT = "parisienne.area23.at";
	public final static String CQRXS_EU = "cqrxs.eu";
	public final static String IPV4_CQRXS_EU = "ipv4.cqrxs.eu";
	public final static String IPV6_CQRXS_EU = "ipv6.cqrxs.eu";

	public final static String SPAIN_CQRXS_EU = "cqrxs.eu";
	public final static String ES_CQRXS_EU = "es.cqrxs.eu";
	public final static String MADRID_CQRXS_EU = "madrid.cqrxs.eu";
	public final static String BARCELONA_CQRXS_EU = "barcelona.cqrxs.eu";

	public final static String IT_CQRXS_EU = "it.cqrxs.eu";
	public final static String MILAN_CQRXS_EU = "milan.cqrxs.eu";
	public final static String SICILIENNE_CQRXS_EU = "sicilienne.cqrxs.eu";


	public final static String FR_CQRXS_EU = "fr.cqrxs.eu";
	public final static String PARIS_CQRXS_EU = "paris.cqrxs.eu";
	public final static String PARISIENNSE_CQRXS_EU = "parisienne.cqrxs.eu";

	public final static String DE_CQRXS_EU = "de.cqrxs.eu";
	public final static String FRANKFURT_CQRXS_EU = "frankfurt.cqrxs.eu";
	public final static String BERLINERIN_CQRXS_EU = "berlinerin.cqrxs.eu";

	public final static String SE_CQRXS_EU = "se.cqrxs.eu";
	public final static String STOCKHOLM_CQRXS_EU = "stockholm.cqrxs.eu";

	public final static String IE_CQRXS_EU = "ie.cqrxs.eu";
	public final static String DUBLIN_CQRXS_EU = "dublin.cqrxs.eu";
	public final static String GALWAY_CQRXS_EU = "galway.cqrxs.eu";

	public final static String UK_CQRXS_EU = "uk.cqrxs.eu";
	public final static String LONDON_CQRXS_EU = "london.cqrxs.eu";
	public final static String EDINBURGH_CQRXS_EU = "edinburgh.cqrxs.eu";

	public final static String CH_CQRXS_EU = "ch.cqrxs.eu";
	public final static String ZURICH_CQRXS_EU = "zurich.cqrxs.eu";
	public final static String BERNERIN_CQRXS_EU = "bernerin.cqrxs.eu";


	public final static String ALL_KEYS = "AllKeys";
	public final static String CHATROOMS = "ChatRooms";
	public final static String CQRXS_URL = "https://cqrxs.eu/";
	public final static String CQRXS_HELP_URL = "https://cqrxs.eu/help/";
	public final static String DECRYPTED_TEXT_AREA = "<textarea cols = \"48\" rows=\"10\" name=\"TextBoxDecrypted\" id=\"TextBoxDecrypted\" title=\"TextBox Current Message\" ValidateRequestMode=\"Enabled\" style=\"width:480px;\" >";
	public final static String DECRYPTED_TEXT_BOX = "TextBoxDecrypted";
	public final static String DECRYPTED_TEXT_AREA_END = "</textarea>";
	public final static String CQRXS_TEST_FORM = "CqrXsTestForm";
	public final static String FISH_ON_AES_ENGINE = "FishOnAesEngine";
	public final static String CQRXS_DELETE_DATA_ON_CLOSE = "CqrXsDeleteDataOnClose";
	public final static String PERSIST_MSG_IN = "PersistMsgIn";
	public final static String PERSIST_MSG_IN_APPLICATION_STATE = "ApplicationState";
	public final static String PERSIST_MSG_IN_AMAZON_ELASTIC_CACHE = "AmazonElasticCache";
	public final static String PERSIST_MSG_IN_FILE_SYSTEM = "FileSystem";

	public final static String ACK = "Ack";
	public final static String NACK = "Nack";
	public final static String ENTER_SECRET_KEY = "[enter secret key here]";
	public final static String ENTER_IP_CONTACT = "[Enter IPv4/IPv6 or select Contact]";
	public final static String ENTER_IP = "[Enter peer IPv4/IPv6]";
	public final static String ENTER_CONTACT = "[Select Contact]";

	public final static String ACCEPT_LANGUAGE = "Accept-Language";
	public final static String AES_ENVIROMENT_KEY = "APP_ENCRYPTION_SECRET_KEY";
	public final static String AUTHOR = "Heinrich Elsigan";
	public final static String AUTHOR_EMAIL = "heinrich.elsigan@area23.at";
	public final static String AUTHOR_IV = "6865696e726963682e656c736967616e406172656132332e6174";
	public final static String AREA23_EMAIL = "zen@area23.at";
	public final static String AUTHOR_SIGNATURE = "-- \nHeinrich G.Elsigan\nTheresianumgasse 6/28, A-1040 Vienna\n phone: +43 650 752 79 28 \nmobile: +43 670 406 89 83 \nemails: heinrich.elsigan @gmail.com\n        heinrich.elsigan@live.at\n        sites: area23.at cqrxs.eu\nweblog: blog.area23.at\n   wko: https://firmen.wko.at/DetailsKontakt.aspx?FirmaID=19800fbd-84a2-456d-890e-eb1fa213100f";

	public final static String APP_CONCURRENT_DICT = "APP_CONCURRENT_DICT";
	public final static String APP_FIRST_REG = "APP_FIRST_REG";
	public final static String APP_TRANSPARENT_BADGE = "APP_TRANSPARENT_BADGE";
	public final static String APP_SERVER_KEY = "APP_SERVER_KEY";
	public final static String APP_INPUT_DIALOG = "APP_INPUT_DIALOG";
	public final static String APP_MY_CONTACT = "APP_MY_CONTACT";

	public final static String APP_DIR_PATH_WIN = "AppDirPathWin";
	public final static String BASE_APP_PATH_WIN = "BaseAppPathWin";
	public final static String APP_DIR_PATH_UNIX = "AppDirPathUnix";
	public final static String BASE_APP_PATH_UNIX = "BaseAppPathUnix";

	public final static String BIN_DIR = "bin";
	public final static String CALC_DIR = "Calc";
	public final static String CSS_DIR = "css";
	public final static String CRYPT_DIR = "Crypt";
	public final static String ENCODE_DIR = "Crypt";
	public final static String GAMES_DIR = "Gamez";
	public final static String IMG_DIR = "img";
	public final static String IMG_FOLDER = "Image";
	public final static String JS_DIR = "js";
	public final static String JSON_DIR = "json";
	public final static String LOG_DIR = "log";
	public final static String LOG_EXT = ".log";
	public final static String LOG_EXCEPTION_STATIC = "LogExceptionStatic";
	public final static String OUT_DIR = "out";
	public final static String QR_DIR = "Qr";
	public final static String RES_DIR = "res";
	public final static String RES_FOLDER = "res";
	public final static String TEXT_DIR = "text";
	public final static String TMP_DIR = "tmp";
	public final static String UNIX_DIR = "Unix";
	public final static String UTF8_DIR = "Utf8";
	public final static String UU_DIR = "uu";

	public final static String OBJ_DIR = "obj";
	public final static String RELEASE_DIR = "Release";
	public final static String DEBUG_DIR = "Debug";
	public final static String NET9_WINDOWS7 = "net9.0-windows7.0";
	public final static String NET9_WINDOWS8 = "net9.0-windows8.0";
	public final static String NET9_WINDOWS10 = "net9.0-windows10";
	public final static String NET9_WINDOWS11 = "net9.0-windows11";
	public final static String WIN_X86 = "win-x86";
	public final static String WIN_X64 = "win-x86";
	public final static String MIME_EXT = ".mime";
	public final static String BASE64_EXT = ".base64";
	public final static String ATTACH_FILES_DIR = "AttachFiles";
	public final static String UPSAVED_FILE = "SavedFile";

	public final static String UTF8_JSON = "utf8symol.json";
	public final static String JSON_SAVE_FILE = "urlshort.json";
	public final static String JSON_APPDICT_FILE = "appdict.json";
	public final static String JSON_CONTACTS = "contacts";
	public final static String JSON_CONTACTS_FILE = "contacts.json";
	public final static String JSON_SETTINGS_FILE = "settings.json";
	public final static String CQR_CHAT_FILE = "cqr{0}chat.json";
	public final static String PREVIOUS_EXCEPTION = "previous_exception";
	public final static String LAST_EXCEPTION = "last_exception";
	public final static String COOL_CRYPT_SPLIT = "[;:→\t]";

	public final static String UNKNOWN = "UnKnown";
	public final static String DEFAULT_MIMETYPE = "application/octet-stream";
	public final static String RPN_STACK = "rpnStack";
	public final static String CHANGE_CLICK_EVENTCNT = "change_Click_EventCnt";
	public final static String BC_START_MSG = "bc 1.07.1\r\nCopyright 1991-1994, 1997, 1998, 2000, 2004, 2006, 2008, 2012-2017 Free Software Foundation, Inc.\r\nThis is free software with ABSOLUTELY NO WARRANTY.\r\nFor details type `warranty'.\r\n";

	public final static String BACK_COLOR = "BackColor";
	public final static String QR_COLOR = "QrColor";
	public final static String BACK_COLOR_STRING = "BackColorString";
	public final static String QR_COLOR_STRING = "QrColorString";
	public final static String IMAGE_UPLOAD_CLICK = "click_here_to_upload";
	public final static String IMAGE_UPLOAD_EXTENSION = ".png";

	public final static String ROACH_DESKTOP_WINDOW = "Roach.Desktop.Window";
	public final static String MUTEX_REGOPS = "Mutex.Registry.Operations";

	public final static String EXE_COMMAND_CMD = "cmd";
	public final static String EXE_POWER_SHELL = "powershell";

	public final static String EXE_WIN_INIT = "wininit";
	public final static String EXE_SERVICES = "services";
	public final static String EXE_SVC_HOST = "svchost";
	public final static String EXE_TASK_HOST = "taskhostw";
	public final static String EXE_DLL_HOST = "dllhost";
	public final static String EXE_SCHEDULER = "scheduler";
	public final static String EXE_VM_COMPUTE = "vmcompute";
	public final static String EXE_WIN_DEFENDER = "MsMpEng";
	public final static String EXE_LASS = "lsass";                     // local Security Authority Subsystem Service. 
	public final static String EXE_CSRSS = "csrss";                    // hosts the server side of the Win32 subsystem

	public final static String EXE_WIN_LOGON = "winlogon";             // windows logon handler for current logon
	public final static String EXE_DESKTOP_WINDOW_MANAGER = "dwm";     // window manager for current logon

	public final static String STRING_EMPTY = "";
	public final static String STRING_NULL = null;
	public final static String SNULL = "(null)";
	//endregion application constants

	public final static String RSA_PUB = 
		"-----BEGIN PUBLIC KEY-----" +
		"MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEA468PZ0zl0lXQXX6vkpeM" +
		"ciGeffjHa1Uv+YSxGKxkn+0km7HZ8EwFU5ia01Jkk+VevPCQQiTusY3Renfau4pE" +
		"cgvGHEqgUG3XHPFmtlEJh6Cz9DcLajKC4a281UAEq/D108CSDHkNbxp2xpZTqJ+l" +
		"0aNjY+UUv5IFm5wfoPsJ0QghQ1Z3XsOcKgf0ztUZ1IpbmnfSkQO21EjUUeGqhHiv" +
		"nfri3/c7nx/adUismR5gzR8yxgU3OyJIDAr9JLzKCbaoWwokfID+oX3tibHjCKEo" +
		"6lnzfO3LpGCb11Dhg77+nKi4GcHF7GZBdjhnVfFo/313Qcewu4kVK8rKJ2K3NIl4" +
		"j85V6oaPPRzw+iR1zfr6J4mGMnAmIY0C3EBYjVpuhTZS06kRsSFOlYmwxeg8Ig16" +
		"GXVCC9UONsRIY7nABLnZ3NQREpqHzX7iQVL0gXFidz0sDcJmxxFM56Oa64+Hbihj" +
		"PLZZAas9p5Uie5W7k2wsxTNwI6tRZPIKUZ59czbnLFoocWERh2/D5K0z4TUhUen5" +
		"6x0m8uvhqfQ1hRt9aoqCMvTCDNB384MTAh2bYDQpOnx81i/Jgr6HVTGajScd/KqW" +
		"HQQzvEE8gcOOxbyZ2p34QyKyyei8tKLRu0AUwJaGc/NErkKzHIIIziMJVx5LfxWU" +
		"8zrWQz53qDfl3xmZWZJDcfkCAwEAAQ==" +
		"-----END PUBLIC KEY-----";

        public final static String RSA_PRV = 
			"-----BEGIN PRIVATE KEY-----" + 
			"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" +
			"2sGnnAaPJgE8Iy2z08jS3rF9npK27A==" +
			"-----END PRIVATE KEY-----";

	
}

