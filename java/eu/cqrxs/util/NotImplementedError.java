package eu.cqrxs.util;

import java.io.Serializable;
import java.lang.*;
import java.lang.UnsupportedOperationException;
import java.lang.Throwable;
import eu.cqrxs.util.*;


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
