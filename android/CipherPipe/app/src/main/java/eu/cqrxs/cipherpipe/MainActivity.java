/**
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * Coded 2021-2025 by
 * <a href="mailto:he@area23.at">Heinrich.Elsigan</a><a href="https://heinrichelsigan.area23.at">heinrichelsigan.area23.at</a>
.*/

package eu.cqrxs.cipherpipe;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.Toast;


import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import java.util.SortedMap;

import eu.cqrxs.crypt.cipher.CipherEnum;
import eu.cqrxs.crypt.cipher.CipherPipe;
import eu.cqrxs.crypt.cipher.SecureCipherPipe;
import eu.cqrxs.crypt.cipher.CryptParams;
import eu.cqrxs.crypt.cipher.CryptHelper;
import eu.cqrxs.crypt.cipher.CipherMode2;
import eu.cqrxs.crypt.cipher.CryptBounceCastle;
import eu.cqrxs.crypt.cipher.JAes;

import eu.cqrxs.crypt.hash.KeyHash;
import eu.cqrxs.crypt.hash.BCrypt;
import eu.cqrxs.crypt.hash.SCrypt;
import eu.cqrxs.crypt.hash.MD5;
import eu.cqrxs.crypt.hash.OpenBSDCrypt;
import eu.cqrxs.crypt.hash.Sha256;
import eu.cqrxs.crypt.hash.Sha512;

import eu.cqrxs.crypt.encoding.EncodeEnum;
import eu.cqrxs.crypt.encoding.IEncodable;
import eu.cqrxs.crypt.encoding.EnDeCodeHelper;
import eu.cqrxs.crypt.encoding.Hex16Coder;
import eu.cqrxs.crypt.encoding.Hex32Coder;
import eu.cqrxs.crypt.encoding.Hex64Coder;
import eu.cqrxs.crypt.encoding.Base16Coder;
import eu.cqrxs.crypt.encoding.Base64Coder;
import eu.cqrxs.crypt.encoding.UuCoder;
import eu.cqrxs.crypt.encoding.XxEncoder;

import eu.cqrxs.zip.GZ;
import eu.cqrxs.zip.ZipType;


public class MainActivity extends AppCompatActivity {

    Button btnEncrypt, btnDecrypt, btnSetPipe, btnHashPipe, btnRandText, btnReset;
    EditText editEncryptKey, showCipherPipe, editTextSource, showTextDestination, editKeyHash;
    Spinner spinnerHash, spinnerZip, spinnerAlgos, spinnerEncode;
    String[] hashStrings, encodingStrings, zipStrings, algoStrings;
    SortedMap<String, String> sortedAlgoMap, sortedHashMap, sortedEncodingMap;
    ArrayAdapter adapterHash = null, adapterEndoding = null, adapterZip = null, adapterAlgos = null;
    String selectedHash = "", selectEncodeType = "", selectZipType = "", selectCipherAlgo = "", selectedZip = "";
    KeyHash keyHash = KeyHash.Hex;
    EncodeEnum encodeType = EncodeEnum.Base64;
    ZipType zipType = ZipType.None;

    CipherMode2 cmode = CipherMode2.CFB;

    static boolean firstTimeInit = true, firstTimeInitEncodings = true;

    CipherEnum cipher;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);
        try {
            hashStrings = KeyHash.getNames();
            encodingStrings = EncodeEnum.getNames();
            zipStrings = ZipType.getNames();
            algoStrings = CipherEnum.getNames();
        } catch (Exception exi) {
            showTextDestination.setText(exi.toString());
        }
        btnSetPipe = (Button) findViewById(R.id.btnSetPipe);
        btnHashPipe = (Button) findViewById(R.id.btnHashPipe);
        btnEncrypt = (Button) findViewById(R.id.btnEncrypt);
        btnDecrypt = (Button) findViewById(R.id.btnDecrypt);
        btnRandText = (Button) findViewById(R.id.btnRandText);
        btnReset = (Button) findViewById(R.id.btnReset);
        editEncryptKey = (EditText) findViewById(R.id.editEncryptKey);
        editKeyHash = (EditText) findViewById(R.id.editKeyHash);
        showCipherPipe = (EditText) findViewById(R.id.showCipherPipe);
        editTextSource = (EditText) findViewById(R.id.editTextSource);
        showTextDestination = (EditText) findViewById(R.id.showTextDestination);

        spinnerHash = (Spinner) findViewById(R.id.spinnerHash);
        spinnerZip = (Spinner) findViewById(R.id.spinnerZip);
        spinnerAlgos = (Spinner) findViewById(R.id.spinnerAlgos);
        spinnerEncode = (Spinner) findViewById(R.id.spinnerEncode);

        btnSetPipe.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String key = editEncryptKey.getText().toString();
                String hashed = keyHash.hash(key);
                editKeyHash.setText(hashed);

                CipherPipe pipe = new CipherPipe(key, hashed, encodeType, zipType, keyHash, cmode);

                CipherEnum[] cipherEnums = pipe.getInPipe();
                String pipeSting = "";
                for (int ci = 0; ci < cipherEnums.length; ci++)
                    pipeSting = pipeSting + cipherEnums[ci].getName() + ";";
                showCipherPipe.setText(pipeSting);
            }
        });

        btnHashPipe.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String key = editEncryptKey.getText().toString();
                String hashed = keyHash.hash(key);
                editKeyHash.setText(hashed);

                CipherPipe pipe = new CipherPipe(hashed, key, encodeType, zipType, keyHash, cmode);

                CipherEnum[] cipherEnums = pipe.getInPipe();
                String pipeSting = "";
                for (int ci = 0; ci < cipherEnums.length; ci++)
                    pipeSting = pipeSting + cipherEnums[ci].getName() + ";";
                showCipherPipe.setText(pipeSting);
            }
        });

        btnRandText.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String fortune = eu.cqrxs.util.Fortune.getFortune();
                editTextSource.setText(fortune);
            }
        });

        btnReset.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                editEncryptKey.setText("");
                editKeyHash.setText("");
                showCipherPipe.setText("");
                editTextSource.setText("");
                showTextDestination.setText("");
            }
        });

        btnEncrypt.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                ;
                String key = editEncryptKey.getText().toString();
                String hashed = keyHash.hash(key);
                editKeyHash.setText(hashed);

                String cipherPipeString = showCipherPipe.getText().toString();
                CipherEnum[] ciphers = new CipherEnum[0];
                if (cipherPipeString.length() > 0) {
                    ciphers = CipherEnum.parsePipeText(cipherPipeString);
                }
                CipherPipe pipe = new CipherPipe(ciphers, 8, encodeType, zipType, keyHash, cmode);
                String plain = editTextSource.getText().toString();

                String pipeSting = pipe.getPipeString();
                showMsg(String.format("pipe[%s] encrypt with key=%s, hash=%s, \nencode=%s keyHash=%s, zip=%s",
                        pipeSting, key, hashed, encodeType.getName(), keyHash.getName(), zipType.getName()), 4, false);

                String encrypted = encryptString(plain, key, hashed, pipe);
                showTextDestination.setText(encrypted);
            }
        });

        btnDecrypt.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                String key = editEncryptKey.getText().toString();
                String hashed = keyHash.hash(key);
                editKeyHash.setText(hashed);

                String cipherPipeString = showCipherPipe.getText().toString();
                CipherEnum[] ciphers = new CipherEnum[0];
                if (cipherPipeString.length() > 0) {
                    ciphers = CipherEnum.parsePipeText(cipherPipeString);
                }
                CipherPipe pipe = new CipherPipe(ciphers, 8, encodeType, zipType, keyHash, cmode);

                String encrypted = editTextSource.getText().toString();
                CipherEnum[] cipherEnums = pipe.getOutPipe();

                String pipeSting = pipe.getPipeString();
                showMsg(String.format("pipe[%s] decrypt with key=%s, hash=%s, \nencode=%s keyHash=%s, zip=%s",
                        pipeSting, key, hashed, encodeType.getName(), keyHash.getName(), zipType.getName()), 4, true);

                String decrypted = decryptedString(encrypted, key, hashed, pipe);
                showTextDestination.setText(decrypted);
            }
        });

        if (adapterHash == null)
            adapterHash = new ArrayAdapter<>(MainActivity.this, android.R.layout.simple_spinner_dropdown_item, hashStrings);
        adapterHash.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinnerHash.setAdapter(adapterHash);
        spinnerHash.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {

                try {
                    String msg = "adapterViewId: " + parent.getId() + " adapterView: " + parent.getAdapter().toString();
                    Log.d("onItemSelected", msg);
                } catch (Exception exAdapter) {
                }

                selectedHash = parent.getSelectedItem().toString();
                keyHash = KeyHash.getEnum(selectedHash);
                if (editEncryptKey == null)
                    editEncryptKey = (EditText) findViewById(R.id.editEncryptKey);
                String keyValue = "";
                try {
                    keyValue = editEncryptKey.getText().toString();
                } catch (Exception exi) {
                    keyValue = "zen@area23.at";
                }
                String hashed = "";
                try {
                    hashed = keyHash.hash(keyValue);
                    editKeyHash.setText(hashed);
                } catch (Exception exh) {
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                // parent.setSelection(6);
                ;
            }
        });


        if (adapterEndoding == null)
            adapterEndoding = new ArrayAdapter<>(MainActivity.this, android.R.layout.simple_spinner_dropdown_item, encodingStrings);
        adapterEndoding.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinnerEncode.setAdapter(adapterEndoding);
        spinnerEncode.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                try {
                    String msg = "adapterViewId: " + parent.getId() + " adapterView: " + parent.getAdapter().toString();
                    Log.d("onItemSelected", msg);
                } catch (Exception exAdapter) {
                }

                selectEncodeType = parent.getSelectedItem().toString();
                encodeType = EncodeEnum.getEnum(selectEncodeType);
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                ;
            }
        });

        if (firstTimeInitEncodings) {
            spinnerEncode.setSelection(7);
            firstTimeInitEncodings = false;
        }

        if (adapterAlgos == null)
            adapterAlgos = new ArrayAdapter<>(MainActivity.this, android.R.layout.simple_spinner_dropdown_item, algoStrings);
        adapterAlgos.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinnerAlgos.setAdapter(adapterAlgos);
        spinnerAlgos.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                try {
                    String msg = "adapterViewId: " + parent.getId() + " adapterView: " + parent.getAdapter().toString();
                    Log.d("onItemSelected", msg);
                } catch (Exception exAdapter) {
                }
                showCipherPipe = (EditText) findViewById(R.id.showCipherPipe);

                selectCipherAlgo = parent.getSelectedItem().toString();
                cipher = CipherEnum.getEnum(selectCipherAlgo);
                String currentPipe = showCipherPipe.getText().toString();
                if (firstTimeInit) {
                    showCipherPipe.setText("");
                    firstTimeInit = false;
                } else
                    showCipherPipe.setText(currentPipe + cipher.getName() + ";");
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                ;
            }
        });

        if (adapterZip == null)
            adapterZip = new ArrayAdapter<>(MainActivity.this, android.R.layout.simple_spinner_dropdown_item, zipStrings);
        adapterZip.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinnerZip.setAdapter(adapterZip);
        spinnerZip.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                try {
                    String msg = "adapterViewId: " + parent.getId() + " adapterView: " + parent.getAdapter().toString();
                    Log.d("onItemSelected", msg);
                } catch (Exception exAdapter) {
                }

                selectedZip = parent.getSelectedItem().toString();
                zipType = ZipType.getEnum(selectedZip);
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                ;
            }
        });

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
    }


    public String encryptString(String plain, String key, String hashed, CipherPipe pipe) {

        String encrypted = "";
        try {
            encrypted = pipe.encrpytTextGoRounds(plain, key, hashed, encodeType, zipType, keyHash, cmode);
        } catch (Exception exi) {
            showTextDestination.setText(exi.toString());
        }
        return encrypted;
    }

    public String decryptedString(String encrypted, String key, String hashed, CipherPipe pipe) {

        String decrypted = "";
        try {
            decrypted =  pipe.decryptTextRoundsGo(encrypted, key, hashed, encodeType, zipType, keyHash, cmode);
        } catch (Exception exi) {
            showTextDestination.setText(exi.toString());
        }
        return decrypted;
    }


    public void showMsg(String text, int gravity, boolean showLong) {
        Toast toast = Toast.makeText(getBaseContext(), "Info Message", showLong ? Toast.LENGTH_LONG : Toast.LENGTH_SHORT);
        toast.setText(text);
        toast.setGravity(4, 40, 40);
        toast.setMargin(0.8F, 0.4F);
        toast.show();
    }


}
