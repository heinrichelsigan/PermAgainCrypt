/*
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 *
 * Coded 2021-2027 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
 */
 
package eu.cqrxs.cipherpipe.zip;

import java.io.*;
import java.nio.charset.StandardCharsets;
import java.util.zip.*;
import java.util.zip.*;
// import org.apache.commons.io.IOUtils;

public class GZ  {

    // const int BUFSZE = 1024;
    GZIPInputStream in = null;
    OutputStream out = null;


    /**
     * GZip directly
     * @param bytes byte[] to zip
     * @return gzipped byte[]
     */
    public static byte[] gzip(final byte[] bytes) throws IOException {
        if (bytes == null || bytes.length == 0) {
            return new byte[0];
        }
        final ByteArrayOutputStream out = new ByteArrayOutputStream();
        try (final OutputStream gzip = new GZIPOutputStream(out)) {
            gzip.write(bytes);
        }
        return out.toByteArray();
    }

    /**
     * gzips gzip's a string
     * @param str String to zip
     * @return zipped String as byte[]
     */
    public static byte[] gzips(final String str) {
        if ((str == null) || (str.length() == 0)) {
            throw new IllegalArgumentException("Cannot zip null or empty string");
        }

        try (ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream()) {

            try (GZIPOutputStream gzipOutputStream = new GZIPOutputStream(byteArrayOutputStream)) {
                gzipOutputStream.write(str.getBytes(StandardCharsets.UTF_8));
            }

            return byteArrayOutputStream.toByteArray();

        } catch (IOException e) {
            throw new RuntimeException("Failed to zip content", e);
        }
    }
	

    /**
     * gunzip gunzips a byte array (same as gzip -d )
     * @param gzBytes gzipped byte[]
     * @return unzipped plain byte[]
     */
	public static byte[] gunzip(final byte[] bytes) throws IOException {
		if (bytes == null || bytes.length == 0) {
			throw new IllegalArgumentException("Cannot unzip null or empty byte array");
		}
		try (final GZIPInputStream gunzipStream = new GZIPInputStream(new ByteArrayInputStream(bytes))) {
			final ByteArrayOutputStream byteArrayOutStream = new ByteArrayOutputStream();
			final byte[] data = new byte[16384];
			int nRead;
			while ((nRead = gunzipStream.read(data)) != -1) {
				byteArrayOutStream.write(data, 0, nRead);
			}
			return byteArrayOutStream.toByteArray();
		}
	}

    /**
     * gunzips a gzipped byte[] to a plain text String
     * @param compressed gzipped byte[]
     * @return plain text String
     */
    public static String gunzips(final byte[] compressed) {
        if ((compressed == null) || (compressed.length == 0)) {
            throw new IllegalArgumentException("Cannot unzip null or empty bytes");
        }
        if (!isZipped(compressed)) {
            return new String(compressed);
        }

        try (ByteArrayInputStream byteArrayInputStream = new ByteArrayInputStream(compressed)) {
            try (GZIPInputStream gzipInputStream = new GZIPInputStream(byteArrayInputStream)) {
                try (InputStreamReader inputStreamReader = new InputStreamReader(gzipInputStream, StandardCharsets.UTF_8)) {
                    try (BufferedReader bufferedReader = new BufferedReader(inputStreamReader)) {
                        StringBuilder output = new StringBuilder();
                        String line;
                        while ((line = bufferedReader.readLine()) != null) {
                            output.append(line);
                        }
                        return output.toString();
                    }
                }
            }
        } catch (IOException e) {
            throw new RuntimeException("Failed to unzip content", e);
        }
    }


    public static boolean isZipped(final byte[] compressed) {
        return (compressed[0] == (byte) (GZIPInputStream.GZIP_MAGIC))
                && (compressed[1] == (byte) (GZIPInputStream.GZIP_MAGIC >> 8));
    }

}
