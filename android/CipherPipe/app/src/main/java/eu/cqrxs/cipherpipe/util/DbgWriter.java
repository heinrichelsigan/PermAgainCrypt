/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 *
 * Coded 2021-2027 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */

package eu.cqrxs.cipherpipe.util;


import java.io.Serializable;
import java.lang.Exception;
import java.lang.String;
import java.text.SimpleDateFormat;
import java.util.Date;


/**
 * DbgWriter is a simple debug message writer
 */
public class DbgWriter {

	public void msg(String s, int level, boolean ignoreDbg) {
		if (s != null && s.length() > 0 && (Constants.DEBUG || ignoreDbg)) {
            System.out.println(level + ": \t" + s);
        }
    }
	public static void msg(String s, boolean ignoreDbg)  {
		if (s != null && s.length() > 0 && (Constants.DEBUG || ignoreDbg)) {
			SimpleDateFormat formatter = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
			String daTime = formatter.format(new Date());
            System.out.println(daTime + " \t" + s);
        }
	}
	
}