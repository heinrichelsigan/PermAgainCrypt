package eu.cqrxs.cipherpipe;

import android.os.Bundle;
import android.util.Log;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;


import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import java.util.SortedMap;

import eu.cqrxs.cipherpipe.enums.CipherEnum;
import eu.cqrxs.cipherpipe.enums.SymmCipherEnum;
import eu.cqrxs.cipherpipe.enums.EncodingType;
import eu.cqrxs.cipherpipe.enums.ZipType;
import eu.cqrxs.cipherpipe.enums.KeyHash;




public class MainActivity extends AppCompatActivity {

    Button btnEncrypt, btnDecrypt, btnSetPipe;
    EditText editEncryptKey, showCipherPipe, editTextSource, showTextDestination, editKeyHash;
    Spinner spinnerHash, spinnerZip, spinnerAlgos, spinnerEncode;
    String[] hashStrings, encodingStrings, zipStrings, algoStrings;
    SortedMap<String, String> sortedAlgoMap, sortedHashMap, sortedEncodingMap;
    ArrayAdapter adapterHash = null, adapterEndoding = null, adapterZip = null, adapterAlgos = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);
        hashStrings = KeyHash.getNames();
        encodingStrings = EncodingType.getNames();
        zipStrings = ZipType.getNames();
        algoStrings = CipherEnum.getNames();
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

        // spinnerHash.setOnItemSelectedListener((OnItemSelectedListener) this);
        if (adapterHash == null)
            adapterHash = new ArrayAdapter<>(MainActivity.this, android.R.layout.simple_spinner_dropdown_item, hashStrings);
        adapterHash.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinnerHash.setAdapter(adapterHash);

        if (adapterEndoding == null)
            adapterEndoding = new ArrayAdapter<>(MainActivity.this, android.R.layout.simple_spinner_dropdown_item, encodingStrings);
        adapterEndoding.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinnerEncode.setAdapter(adapterEndoding);

        if (adapterAlgos == null)
            adapterAlgos = new ArrayAdapter<>(MainActivity.this, android.R.layout.simple_spinner_dropdown_item, algoStrings);
        adapterAlgos.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinnerAlgos.setAdapter(adapterAlgos);

        if (adapterZip == null)
            adapterZip = new ArrayAdapter<>(MainActivity.this, android.R.layout.simple_spinner_dropdown_item, zipStrings);
        adapterZip.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinnerZip.setAdapter(adapterZip);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
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


    public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
        String msg = "adapterViewId: " + adapterView.getId() + " adapterView: " + adapterView.getAdapter().toString() +
                " view: " + view.getId() +  " name. "  + view.getAccessibilityClassName() +
                " i=" + i + " l=" + l;
        Log.i(TAG, msg);
        String selectedPhone = adapterView.getSelectedItem().toString();
        String finalNumber = "";
        if (sortedTreePhonmeMap.containsKey(selectedPhone)) {
            finalNumber = sortedTreePhonmeMap.getOrDefault(selectedPhone, sortedPhoneMap.firstKey());
        }

        int phoneIdx = selectedPhone.indexOf(" | ");
        if (phoneIdx > 0) {
            finalNumber = selectedPhone.substring(0, phoneIdx);
            txtphoneNo.setText(finalNumber);

        }
    }


 */
}