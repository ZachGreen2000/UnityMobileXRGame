package com.DefaultCompany.FinalYear;
import android.app.Activity;
import android.app.PendingIntent;
import android.content.Intent;
import android.content.IntentFilter;
import android.nfc.NdefMessage;
import android.nfc.NdefRecord;
import android.nfc.NfcAdapter;
import android.nfc.Tag;
import android.nfc.tech.Ndef;
import android.os.Parcelable;
import android.util.Log;
import com.unity3d.player.UnityPlayer;
import java.util.Locale;
import java.nio.charset.StandardCharsets;
// this class is a plugin for android nfc tag interactions. There is also a edited android manifest file in local folder
// android with api 7 or later should have in-built nfc capabilities
public class NFCPlugin {
    // variables for storing android instance, unity activity, tag and NFC hardware
    private static NFCPlugin instance;
    private Activity activity;
    private NfcAdapter nfcAdapter;
    private Tag currentTag;
    //calls from unity to start reading process
    public void startReading() {
        Activity activity = UnityPlayer.currentActivity;
        NfcAdapter adapter = NfcAdapter.getDefaultAdapter(activity);

        if (adapter != null) {
            Intent intent = new Intent(activity, activity.getClass());
            intent.addFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP);

            PendingIntent pendingIntent = PendingIntent.getActivity(
                activity, 0, intent, PendingIntent.FLAG_MUTABLE // use FLAG_UPDATE_CURRENT for older API
            );

            IntentFilter tagDetected = new IntentFilter(NfcAdapter.ACTION_NDEF_DISCOVERED);
            try {
                tagDetected.addDataType("*/*");
            } catch (IntentFilter.MalformedMimeTypeException e) {
                Log.e("NFC", "Mime type error", e);
            }

            adapter.enableForegroundDispatch(activity, pendingIntent, new IntentFilter[]{ tagDetected }, null);
            Log.d("NFC", "Foreground NFC reading enabled");
        }
    }
    // ensures only one instance of NFCPlugin exsists
    public static NFCPlugin getInstance(Activity act) {
        if (instance == null) {
            instance = new NFCPlugin();
            instance.activity = act;
            instance.nfcAdapter = NfcAdapter.getDefaultAdapter(act); // get NFC hardware
        }
        return instance;
    }
    // for storing current NFC tag
    public void setCurrentTag(Tag tag) {
        this.currentTag = tag;
    }
    // This method writes data to the NFC tag using selected data to pass from Unity
    public void writeToNFC(String data) {
        // check for NFC tag and ready device, messages sent through tags for unity files and functions
        if (currentTag == null) {
            UnityPlayer.UnitySendMessage("NFCManager", "OnNFCError", "No NFC tag detected");
            return;
        }
        Ndef ndef = Ndef.get(currentTag);
        if (ndef == null) {
            UnityPlayer.UnitySendMessage("NFCManager", "OnNFCError", "NFC tag is not writeable");
            return;
        }
        // open connection and write data to NFC tag using an NDEF record containing text data
        try {
            ndef.connect();
            byte[] jsonData = data.getBytes(StandardCharsets.UTF_8); // convert to bytes
            NdefRecord record = NdefRecord.createMime("application/json", jsonData);
            NdefMessage message = new NdefMessage(new NdefRecord[]{record});
            ndef.writeNdefMessage(message);
            ndef.close();
            UnityPlayer.UnitySendMessage("NFCManager", "OnNFCWrite", "Data written");
        } catch (Exception e) { // error checking to stop crashing and log errors
            UnityPlayer.UnitySendMessage("NFCManager", "OnNFCError", "Failed to write");
        }
    }
    // this method handles the logic to proccess the NFC tag data 
    public static void processNFCIntent(Intent intent) {
        // check that intent is the correct for data reading
        if (intent == null || !NfcAdapter.ACTION_NDEF_DISCOVERED.equals(intent.getAction())) {
            Log.d("NFCPlugin", "cannot read or process data");
            return;
        }
        // retrieve messages in NDEF format from NFC intent, extracting the record and convert into a string uising UTF_8
        Parcelable[] rawMsgs = intent.getParcelableArrayExtra(NfcAdapter.EXTRA_NDEF_MESSAGES);
        if (rawMsgs != null && rawMsgs.length > 0) {
            NdefMessage ndefMessage = (NdefMessage) rawMsgs[0];
            NdefRecord record = ndefMessage.getRecords()[0];
            String data = new String(record.getPayload(), StandardCharsets.UTF_8);
            Log.d("NFCPlugin", "Data from NFC" + data);
            UnityPlayer.UnitySendMessage("NFCManager", "OnNFCRead", data); // send the data to Unity
        }else {
            Log.d("NFCPlugin", "cannot read or process data"); 
        }
    }
}