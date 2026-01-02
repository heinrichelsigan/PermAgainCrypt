package eu.cqrxs.cipherpipe;

import android.content.Context;

import androidx.test.platform.app.InstrumentationRegistry;
import androidx.test.ext.junit.runners.AndroidJUnit4;

import org.junit.Test;
import org.junit.runner.RunWith;

import static org.junit.Assert.*;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;

import eu.cqrxs.cipherpipe.crypt.cipher.CipherEnum;
import eu.cqrxs.cipherpipe.crypt.cipher.CipherPipe;
import eu.cqrxs.cipherpipe.crypt.encoding.EncodeEnum;
import eu.cqrxs.cipherpipe.crypt.hash.KeyHash;
import eu.cqrxs.cipherpipe.zip.ZipType;

/**
 * TestEncryptionTwoAlgos validate 2 encryptions pipe steps
 * Tests:
 *     useAppContext
 *     testAllEncryptionTwoAlgosString
 * @see <a href="http://d.android.com/tools/testing">Testing documentation</a>
 */
@RunWith(AndroidJUnit4.class)
public class ExampleInstrumentedTest {
    static String Email = eu.cqrxs.cipherpipe.util.Constants.AUTHOR_EMAIL;

    @Test
    public void useAppContext() {
        // Context of the app under test.
        Context appContext = InstrumentationRegistry.getInstrumentation().getTargetContext();
        assertEquals("eu.cqrxs.cipherpipe", appContext.getPackageName());
    }

    @Test
    public void testAllEncryptionTwoAlgosString() {
        String className = "TestEncryptionTwoAlgos";
        String methodBase = "TestAllEncryptionTwoAlgosBytes";
        Email = eu.cqrxs.cipherpipe.util.Constants.AUTHOR_EMAIL;

        LocalDate currentDate = LocalDate.now();
        LocalTime localTime = LocalTime.now();
        System.out.println(String.format("%s \t%s %s.%s() \t[started]",
                    LocalDate.now().toString(), className, methodBase));

        LocalTime startOp = LocalTime.now(), midOp = startOp, endOp = startOp;

        CipherEnum[] cipherEnums = CipherEnum.getCipherEnumArray();
        ZipType[] zTypes = new ZipType[]{ZipType.None, ZipType.Zip, ZipType.GZip, ZipType.BZip2};
        KeyHash kHash = KeyHash.Hex;
        KeyHash[] kHashes = KeyHash.getKeyHashArray();
        ZipType zType = ZipType.None;
        EncodeEnum[] encodingTypes = EncodeEnum.getEncodeEnumArray();
        EncodeEnum encType = EncodeEnum.Base64;
        String plainText = "package eu.cqrxs.cipherpipe;\n\nimport org.junit.Test;\n" +
                "\nimport static org.junit.Assert.*;\n\n/**\n * Example local unit test, " +
                "which will execute on the development machine (host).\n" +
                " * \n @see <a href=\"http://d.android.com/tools/testing\">Testing documentation</a>\n" +
                " \tnpublic class ExampleUnitTest {\n @Test\n npublic void addition_isCorrect() {\n" +
                "        assertEquals(4, 2 + 2);\n" +
                "    }\n" + "}";

        int j = 0;
        for (int i = 0; i < cipherEnums.length; i += 2) {
            CipherEnum cipherType = cipherEnums[i];
            CipherEnum cipherEnum = cipherEnums[((i + 1) % cipherEnums.length)];
            if (cipherType == CipherEnum.Rsa) cipherType = CipherEnum.Des;
            if (cipherEnum == CipherEnum.Rsa) cipherEnum = CipherEnum.BlowFish;

            CipherEnum[] cipherPair = new CipherEnum[]{cipherType, cipherEnum};
            zType = zTypes[j % zTypes.length];
            kHash = kHashes[j % kHashes.length];
            if ((encType = encodingTypes[++j % encodingTypes.length]) == EncodeEnum.None)
                encType = EncodeEnum.Base64;

            CipherPipe pipe = new CipherPipe(cipherPair, 8, encType, zType, kHash);

            try {
                startOp = LocalTime.now();
                java.lang.String encrpyted = pipe.encrpytTextGoRounds(plainText,
                        Email, kHash.hash(Email), encType, zType, kHash);
                assertNotNull(encrpyted);

                midOp = LocalTime.now();
                java.lang.String decrpyted = pipe.decryptTextRoundsGo(encrpyted,
                        Email, kHash.hash(Email), encType, zType, kHash);

                endOp = LocalTime.now();
                // String xxx = (endOp.minus(startOp)).toString();

                assertEquals(plainText, decrpyted);


            } catch (Exception e) {
                (new eu.cqrxs.cipherpipe.util.DbgWriter()).msg(e.toString(), 1,  true);
            }
        }

        return;
    }

}
