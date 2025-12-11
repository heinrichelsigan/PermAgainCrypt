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

import java.io.IOException;
import java.util.SortedMap;

import eu.cqrxs.cipherpipe.crypt.cipher.CipherEnum;
import eu.cqrxs.cipherpipe.crypt.cipher.CipherPipe;
import eu.cqrxs.cipherpipe.enums.*;
import eu.cqrxs.cipherpipe.crypt.hash.*;
import eu.cqrxs.cipherpipe.crypt.encoding.*;




public class MainActivity extends AppCompatActivity {

    Button btnEncrypt, btnDecrypt, btnSetPipe;
    EditText editEncryptKey, showCipherPipe, editTextSource, showTextDestination, editKeyHash;
    Spinner spinnerHash, spinnerZip, spinnerAlgos, spinnerEncode;
    String[] hashStrings, encodingStrings, zipStrings, algoStrings;
    SortedMap<String, String> sortedAlgoMap, sortedHashMap, sortedEncodingMap;
    ArrayAdapter adapterHash = null, adapterEndoding = null, adapterZip = null, adapterAlgos = null;
    String selectedHash = "", selectEncodeType = "", selectZipType = "", selectCipherAlgo = "", selectedZip = "";
    KeyHash keyHash = KeyHash.Hex;
    EncodeEnum encodeType = EncodeEnum.Base64;
    ZipType zipType = ZipType.None;

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
        } catch (Exception exi) {}
        btnSetPipe = (Button) findViewById(R.id.btnSetPipe);
        btnEncrypt = (Button) findViewById(R.id.btnEncrypt);
        btnDecrypt = (Button) findViewById(R.id.btnDecrypt);
        editEncryptKey = (EditText) findViewById(R.id.editEncryptKey);
        showCipherPipe = (EditText) findViewById(R.id.showCipherPipe);
        editTextSource = (EditText) findViewById(R.id.editTextSource);
        showTextDestination = (EditText) findViewById(R.id.showTextDestination);
        editKeyHash = (EditText) findViewById(R.id.editKeyHash);
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

                CipherPipe pipe = new CipherPipe(key, hashed, encodeType, zipType, keyHash);

                CipherEnum[] cipherEnums = pipe.getInPipe();
                String pipeSting = "";
                for (int ci = 0; ci < cipherEnums.length; ci++)
                    pipeSting = pipeSting + cipherEnums[ci].getName() + ";";
                showCipherPipe.setText(pipeSting);
            }
        });

        btnEncrypt.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                editTextSource = (EditText) findViewById(R.id.editTextSource);
                showTextDestination = (EditText) findViewById(R.id.showTextDestination);
                showCipherPipe = (EditText) findViewById(R.id.showCipherPipe);

                String key = editEncryptKey.getText().toString();
                String hashed = keyHash.hash(key);
                editKeyHash.setText(hashed);

                String cipherPipeString = showCipherPipe.getText().toString();
                CipherEnum[] ciphers = new CipherEnum[0];
                if (cipherPipeString.length() > 0) {
                    ciphers = CipherEnum.parsePipeText(cipherPipeString);
                }
                CipherPipe pipe = new CipherPipe(ciphers, 8, encodeType, zipType, keyHash);

                String plain = editTextSource.getText().toString();
                String encrypted = "";
                CipherEnum[] cipherEnums = pipe.getInPipe();
                String pipeSting = "";
                for (int ci = 0; ci < cipherEnums.length; ci++)
                    pipeSting = pipeSting + cipherEnums[ci].getName() + ";";

                showMsg(String.format("PipeString: %s \nEncoding: %s Hashing: %s zipping; %s",
                        pipeSting, encodeType.getName(),  keyHash.getName(), zipType.getName()), 2, false);
                try {
                    showMsg(String.format("pipe.encrypt with key=%s, hash=%s, \nencode=%s keyHash=%s, zip=%s",
                            key, hashed, encodeType.getName(),  keyHash.getName(), zipType.getName()), 4, false);
                    encrypted = pipe.encrpytTextGoRounds(plain, key, hashed, encodeType, zipType, keyHash);

                    showTextDestination.setText(encrypted);
                } catch (Exception ex) {
                    showTextDestination.setText(ex.toString());
                }

            }
        });

        btnDecrypt.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                editTextSource = (EditText) findViewById(R.id.editTextSource);
                showTextDestination = (EditText) findViewById(R.id.showTextDestination);
                editTextSource = (EditText) findViewById(R.id.editTextSource);
                showTextDestination = (EditText) findViewById(R.id.showTextDestination);

                String key = editEncryptKey.getText().toString();
                String hashed = keyHash.hash(key);
                editKeyHash.setText(hashed);


                String cipherPipeString = showCipherPipe.getText().toString();
                CipherEnum[] ciphers = new CipherEnum[0];
                if (cipherPipeString.length() > 0) {
                    ciphers = CipherEnum.parsePipeText(cipherPipeString);
                }
                CipherPipe pipe = new CipherPipe(ciphers, 8, encodeType, zipType, keyHash);

                String plain ="";
                String encrypted = editTextSource.getText().toString();
                CipherEnum[] cipherEnums = pipe.getOutPipe();
                String pipeSting = "";
                for (int ci = 0; ci < cipherEnums.length; ci++)
                    pipeSting = pipeSting + cipherEnums[ci].getName() + ";";
                showMsg(String.format("Out pipe: %s \nEncoding: %s Hashing: %s zipping; %s",
                                pipeSting, encodeType.getName(),  keyHash.getName(), zipType.getName()), 1, true);

                String decrypted = "";
                try {
                    decrypted = pipe.decryptTextRoundsGo(encrypted, key, hashed, encodeType, zipType, keyHash);
                    showTextDestination.setText(decrypted);
                    showMsg(String.format("pipe.decrypt with key=%s, hash=%s, \nencode=%s keyHash=%s, zip=%s",
                            key, hashed, encodeType.getName(),  keyHash.getName(), zipType.getName()), 4, true);
                } catch (Exception ex) {
                    showTextDestination.setText(ex.toString());
                }
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


    public void showMsg(String text, int gravity, boolean showLong) {
        Toast toast = new Toast(getBaseContext());
        toast.setDuration(showLong ? Toast.LENGTH_LONG : Toast.LENGTH_SHORT);
        toast.setText(text);
        toast.setGravity(4, 40, 40);
        toast.setMargin(0.8F, 0.4F);
        toast.show();
    }


/*


    public String[]  getContactList() {
        int phoneCnt = 0;
        sortedPhoneMap = new TreeMap<String, String>();
        sortedTreePhonmeMap = new TreeMap<String, String>();
        List<String> phonebook = new ArrayList<>();

        ContentResolver cr = getContentResolver();
        Cursor cur = null;
        try {
            cur = cr.query(ContactsContract.Contacts.CONTENT_URI, null, null, null, null);
        } catch (Exception exQueryContract1) {
            Log.i(TAG, "getContactList: " + exQueryContract1.toString() );
        }

        if ((cur != null ? cur.getCount() : 0) > 0) {
            while (cur != null && cur.moveToNext()) {
                String contactIdStr = "";
                String contactNameStr = "";
                String contactRawId = "";
                int rawPhoneId = cur.getColumnIndex(ContactsContract.CommonDataKinds.Phone.NAME_RAW_CONTACT_ID);
                if (rawPhoneId > -1) {
                    contactRawId = cur.getString(rawPhoneId);
                }
                int contactId = cur.getColumnIndex(ContactsContract.Contacts._ID);
                if (contactId > -1) {
                    String id = cur.getString(contactId);
                    contactIdStr = cur.getString(contactId);
                }

                int contactNameId = cur.getColumnIndex(ContactsContract.Contacts.DISPLAY_NAME);
                if (contactNameId > -1) {
                    contactNameStr = cur.getString(contactNameId);
                }

                int contactNumberId = cur.getColumnIndex(ContactsContract.Contacts.HAS_PHONE_NUMBER);

                if (cur.getInt(contactNumberId) > 0) {
                    Cursor pCur = cr.query(
                            ContactsContract.CommonDataKinds.Phone.CONTENT_URI,
                            null,
                            ContactsContract.CommonDataKinds.Phone.CONTACT_ID + " = ?",
                            new String[]{contactIdStr}, null);
                    while (pCur.moveToNext()) {
                        int contactPhoneNumberId = pCur.getColumnIndex(
                                ContactsContract.CommonDataKinds.Phone.NUMBER);

                        if (contactPhoneNumberId > -1) {

                            String phoneNo = pCur.getString(contactPhoneNumberId);

                            Log.i(TAG, "rawId: " + contactRawId);
                            Log.i(TAG, "contactId: " + contactIdStr);
                            Log.i(TAG, "Name: " + contactNameStr);
                            Log.i(TAG, "Phone Number: " + phoneNo);

                            if ((phoneNo != null && phoneNo.length() > 5) &&
                                    (phoneNo.startsWith("00") || phoneNo.startsWith("+") || phoneNo.startsWith("0"))) {

                                String phoneNrTrim = phoneNo.replace(" ", "");
                                phoneNrTrim = phoneNrTrim.replace("-", "");
                                phoneNrTrim = phoneNrTrim.replace("/", "");
                                // if (!phonebook.contains(phoneNrTrim)) {

                                if (!sortedTreePhonmeMap.containsKey(phoneNrTrim)) {
                                    sortedTreePhonmeMap.put(phoneNrTrim, contactNameStr+ " | " + phoneNrTrim);
                                    sortedPhoneMap.put(phoneNrTrim + " | " + contactNameStr, contactNameStr+ " | " + phoneNrTrim);
                                    phonebook.add(phoneNrTrim + " | " + contactNameStr);
                                    phoneCnt++;
                                }
                            }
                        }
                    }
                    pCur.close();
                }
            }
        }
        if (cur != null) {
            cur.close();
        }

        sortedTreePhonmeMap = new TreeMap<String, String>(new PhoneComparator(sortedPhoneMap));
        sortedTreePhonmeMap.putAll(sortedPhoneMap);

        // return phonebook.toArray(new String[phoneCnt]);
        return sortedTreePhonmeMap.keySet().toArray(new String[phoneCnt]);
    }

*/


}