/**
 * @author           <a href="mailto:heinrich.elsigan@cqrxs.eu">Heinrich Elsigan</a>
 * @version          V 2.26.428
 * @since            API 27 Oreo 8.1
 *
 * eu.cqrxs.util.ContextLazy
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */

package eu.cqrxs.util;

import javax.naming.InitialContext;

/**
  * ContextLazy - provides a lazy singelton for application context
  *
  */
public class ContextLazy {
    private static ContextLazy instance;
    private InitialContext mContext;
	
	public static ContextLazy getInstance(InitialContext context) {
		if (instance == null) {
			// instance = new ContextLazy(context);
			instance = new ContextLazy(context);			
		}
		return instance;
	}

    public static InitialContext getLastContext() {
        return (instance != null) ? instance.mContext : (InitialContext)null;
    }

    private ContextLazy(InitialContext context) {
        mContext = context;
    }

}
