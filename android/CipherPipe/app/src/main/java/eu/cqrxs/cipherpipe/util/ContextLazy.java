/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://area23.at">area23.at</a>
 */
package eu.cqrxs.cipherpipe.util;

// import android.content.Context;
/*
import com.google.gson.annotations.JsonAdapter;
 */
// import org.jetbrains.annotations.Nullable;
// import javax.naming.Context;
// import javax.naming.InitialContext;
import java.io.Serializable;
import java.security.InvalidAlgorithmParameterException;
import java.util.ArrayList;
import eu.cqrxs.cipherpipe.util.Constants;

/**
  * ContextLazy - provides a lazy singelton for application context
  *

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
 */