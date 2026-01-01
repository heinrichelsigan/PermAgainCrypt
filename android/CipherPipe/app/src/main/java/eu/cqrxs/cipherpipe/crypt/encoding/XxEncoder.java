/**
 * @author           <a href="mailto:heinrich.elsigan@gmail.com">Heinrich Elsigan</a>
 * @version          V 2.25.1224
 * @since            API 34
 *
 * Coded 2021-2033 by <a href="mailto:he@area23.at">Heinrich Elsigan</a>
 * <a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */

/*
package eu.cqrxs.cipherpipe.crypt.encoding;


import com.google.firebase.crashlytics.buildtools.reloc.org.apache.commons.codec.Encoder;

import java.util.Arrays;
import java.util.Map;
import java.nio.charset.Charset;
import java.util.stream.Collectors;
import java.util.stream.IntStream;

public class XxEncoder implements Encoder {
    // @Override
    public Map<Integer, String> getEncodingTable() {
        String encodingMatrixRaw = 
				"+ - 0 1 2 3 4 5 " +
                "6 7 8 9 A B C D " +
                "E F G H I J K L " +
                "M N O P Q R S T " +
                "U V W X Y Z a b " +
                "c d e f g h i j " +
                "k l m n o p q r " +
                "s t u v w x y z";
        var encodingMatrix = Arrays.asList(encodingMatrixRaw.split(" "));
        var n = encodingMatrix.size();
        return IntStream.range(0, n).boxed().collect(Collectors.toMap(i -> i, encodingMatrix::get));
    }

    // @Override
    String getEmptyDataSymbol() {
        return "+";
    }

	// **
	// * Encode - encodes a string to xx string
	// * @param inString string to encode
	// * @return xx string
	// * /
    public static String Encode(String inString) {
        String encoded = (new XxEncoder()).encode(inString);
		return encoded;
    }

	// **
    // * Decode - decodes a xx string to decoded plain String
    // * @param encodedString xx String
    // * @return decoded plain String
    // * /
	public static String Decode(String encodedString) {
		String decoded = (new XxEncoder()).decode(encodedString);
		return decoded;
	}
	
	//*
    // * EncodeBytesToString - converts a binary byte array to xx string
    // * @param inBytes byte array
    // * @return xx string
    // * @exception IllegalArgumentException is thrown when inBytes is null or empty
    // * /
	public static String EncodeBytesToString(byte[] inBytes) {
        if (inBytes == null || inBytes.length < 1)
            throw new IllegalArgumentException("public static string EncodeBytesToString(byte[] inBytes == NULL)");

		String hexString = inBytes.toString();
		String xxString = XxEncoder.Encode(hexString);		

		return xxString;
	}
	
	 // **
     // * DecodeStringToBytes transforms a xx encoded string to binary byte array
     // * @param xxString: a xx string
     // * @return binary byte array
     // * @exception IllegalArgumentException is thrown when hexStr is null or empty
     // * /
	public static byte[] DecodeStringToBytes(String xxString) throws IllegalArgumentException {
        if (xxString == null || xxString.length() == 0)
            throw new IllegalArgumentException("public static byte[] DecodeStringToBytes(string xxString), xxString == NULL || xxString == \"\"");

		String decoded = XxEncoder.Decode(xxString);

		byte[] decodedBytes = decoded.getBytes(Charset.forName("UTF-8"));
		return decodedBytes;
	}

}
*/
