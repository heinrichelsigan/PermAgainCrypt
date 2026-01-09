/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 *
 * Coded 2021-2027 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */

package eu.cqrxs.util;


import java.io.Serializable;
import java.lang.Exception;
import java.lang.RuntimeException;
import java.lang.IllegalStateException;

/**
 * CException extends {@link RuntimeException}
 * is thrown, when state of PermAgainCrypt tool is invalid
 */
public class CException extends RuntimeException implements Serializable  {

	/**
	 * calling ctor in super {@link RuntimeException(String)}
	 * @param sMessage {@link String} for exception message
	 */
	public CException(String sMessage) {
		super(sMessage);
	}

	/**
	 * standard ctor called before {@link #initCause(Throwable throwable)}
	 * @param message {@link String} for exception
	 * @param throwable {@link Throwable} inner exception
	 */
	public CException(String message, Throwable throwable) {
		this(message);
		super.initCause(throwable);
	}

	/**
	 * constructor with (@link IllegalStateException) as {@link Throwable} inner exception
	 * @param column board {@link COLUMN} column indexer
	 * @param row    board {@link Integer} row indexer

	public CException(COLUMN column, int row) {
		this("accessor out of board area at field: " + column.getName() + row + ".",
			((IndexOutOfBoundsException) 
				(new Throwable("out of bounds at colunn=" + column.getValue() + ", row=" + row))));
	}
		 */
		 
}
