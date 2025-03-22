#import "NFCPlugin.h"
#import <CoreNFC/CoreNFC.h>
#import <UIKit/UIKit.h>
// this is the iOS plugin developed for the NFC tag detection that uses sessions to get data and process it.
// this filed is also in relation to a header file in the same local location and a .plist file in the same location
// the plist file is for permissions and should merge on build
@interface NFCPlugin () {
    NFCNDEFReaderSession *nfcSession;
    NFCNDEFTagSession *tagSession;
    NSString *writeData;
}
@end

@implementation NFCPlugin
//start the scanning
+ (void)startScanning {
    dispatch_async(dispatch_get_main_queue(), ^ {
        NFCPlugin *instance = [[NFCPlugin alloc] init];
        instance->nfcSession = [[NFCNDEFReaderSession alloc] initWithDelegate:instance queue:nil invalidateAfterFirstRead:NO];
        [instance->nfcSession beginSession];
    });
}
//stop the scanning
+ (void)stopScanning {
    dispatch_async(dispatch_get_main_queue(), ^ {
        if (nfcSession) {
            [nfcSession invalidateSession];
            nfcSession = nil;
        }
    });
}
// start writting session
+ (void)startWriting:(NSString *)data {
    dispatch_async(dispatch_get_main_queue(), ^ {
        NFCPlugin *instance = [[NFCPlugin alloc] init];
        instance->writeData = data;
        instance->nfcSession = [[NFCNDEFReaderSession alloc] initWithDelegate:instance queue:nil invalidateAfterFirstRead:NO];
        [instance->nfcSession beginSession];
    });
}
// this is called when tag is detected to get data and send to unity
- (void)readerSession:(NFCNDEFReaderSession *)session didDetectNDEFs:(NSArray<NFCNDEFMessage *> *)messages {
    NSMutableString *nfcData = [[NSMutableString alloc] init];
    for (NFCNDEFMessage *message in messages) {
        for (NFCNDEFPayload *payload in message.records) {
            NSString *payloadString = [[NSString alloc] initWithData:payload.payload encoding:NSUTF8StringEncoding];
            [nfcData appendString:payloadString];
        }
    }

    UnitySendMessage("NFCManager", "OnNFCRead", [nfcData UTF8String]);
    [session invalidateSession];
}
//this is called when data is being sent from Unity and written to tag
- (void)readerSession:(NFCNDEFReaderSession *)session didDetectTags:(NSArray<__kindfof id<NFCNDEFTag>> *)tags {
    if (tags.count > 0) {
        id<NFCNDEFTag> tag = tags.firstObject;
        [session connectToTag:tag completionHandler:^(NSError *error) {
            if (error) {
                UnitySendMessage("NFCManager", "OnNFCError", "Failed to connect");
                [session invalidateSession];
                return;
            }
            [tag queryNDEFStatusWithCompletionHandler:^(NFCNDEFStatus status, NSUInteger capacity, NSError *error) {
                if (status == NFCNDEFStatusReadWrite) {
                    NSData *payloadData = [self->writeData dataUsingEncoding:NSUTF8StringEncoding];
                    NCFNDEFPayload *payload = [NCFNDEFPayload ndefPayloadWithFormat:NFCTypeNameFormatNFCWellKnown type:[@"T" dataUsingEncoding:NSUTF8StringEncoding] identifier:[NSData data] payload:payloadData];
                    NFCNDEFMessage *message = [[NFCNDEFMessage alloc] initWithRecords:@[payload]];
                    [tag writeNDEF:message completionHandler:^(NSError *writeError) {
                        if (writeError) {
                            UnitySendMessage("NFCManager", "OnNFCError", "Failed to write");
                        } else {
                            UnitySendMessage("NFCManager", "OnNFCWrite", "Successfully written to tag");
                            [session invalidateSession];
                        }
                    }];
                }
            }];
        }];
    }
}
// error checking
- (void)readerSession:(NFCNDEFReaderSession *)session didInvalidateWithError:(NSError *)error {
    UnitySendMessage("NFCManager", "OnNFCError", "NFC scanning failed");
}
@end