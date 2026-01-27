/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */

package eu.cqrxs.util;

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

	public final static char ANNOUNCE = ':';
	public final static char DATE_DELIM = '-';
	public final static char WHITE_SPACE = ' ';
	public final static char UNDER_SCORE = '_';

	public final static String APP_NAME = "eu.cqrxs";
	public final static String APP_DIR = "net";
	public final static String APP_ERROR = "AppError";
	public final static String VERSION = "v2.26.128";
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


	public static boolean DirCreate = false;
	public static boolean NOLog = false;
	public static boolean DEBUG = true;

	/**
	 * To calculate the octal value of the given decimal number
	 */
	public static String decimalToOctal(int deciNum) {
		// Initially declaring and initializing the
		// octal number with zero
		int octalNum = 0, countval = 1;
		int dNo = deciNum;

		// Condition check
		while (deciNum != 0) {

			// Decimals remainder is calculated
			int remainder = deciNum % 8;

			// Storing the octalvalue
			octalNum += remainder * countval;

			// Storing exponential value
			countval = countval * 10;
			deciNum /= 8;
		}

		// Print and display the octal number
		return String.valueOf(octalNum);
	}

}

