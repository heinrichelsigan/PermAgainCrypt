package eu.cqrxs.util;

import java.lang.*;
import java.lang.Throwable;


/**
 * NotImplementedError 
 */
public class NotImplementedError extends java.lang.UnsupportedOperationException {
		
		
	public NotImplementedError(String msg) {
		throw new java.lang.UnsupportedOperationException(msg);
	}
		
		
	public NotImplementedError(String msg, Throwable cause) {
		throw new java.lang.UnsupportedOperationException(msg, cause);
	}
	
}
