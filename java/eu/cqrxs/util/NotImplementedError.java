/**
 * @author           <a href="mailto:heinrich.elsigan@cqrxs.eu">Heinrich Elsigan</a>
 * @version          V 2.26.428
 * @since            API 27 Oreo 8.1
 *
 * eu.cqrxs.util.NotImplementedError
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */

package eu.cqrxs.util;

import java.lang.*;
import java.lang.Throwable;

/**
 * NotImplementedError throws new java.lang.UnsupportedOperationException(...);
 */
public class NotImplementedError extends java.lang.UnsupportedOperationException {

	/**
	 * Constructor with 1 argument
	 * @param msg Exception description message {@link String}
	 * @throws java.lang.UnsupportedOperationException
	 */
	public NotImplementedError(String msg) {
		throw new java.lang.UnsupportedOperationException(msg);
	}

	/**
	 * Constructor with 2 arguments
	 * @param msg Exception description message {@link String}
	 * @param throwable {@link Throwable} inner exception
	 * @throws java.lang.UnsupportedOperationException
	 */
	public NotImplementedError(String msg, Throwable throwable) {
		throw new java.lang.UnsupportedOperationException(msg, throwable);
	}
	
}
