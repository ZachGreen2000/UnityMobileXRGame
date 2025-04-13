package com.DefaultCompany.FinalYear;

import android.content.Intent;
import android.os.Bundle;
import com.unity3d.player.UnityPlayerActivity;
import android.nfc.NfcAdapter;
import android.util.Log;

public class MainActivity extends UnityPlayerActivity {

    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        NFCPlugin.processNFCIntent(intent);
    }

    @Override
    protected void onPause() {
        super.onPause();
        NfcAdapter nfcAdapter = NfcAdapter.getDefaultAdapter(this);
        if (nfcAdapter != null) {
            nfcAdapter.disableForegroundDispatch(this);
            Log.d("NFCPlugin", "Foreground NFC reading disabled");
        }
    }
}