#if UNITY_IOS
#ifdef __OBJC__
#import <TargetConditionals.h>
#if TARGET_OS_IOS
#import <Foundation/Foundation.h>
#import <CoreNFC/CoreNFC.h>
#endif
#endif

@interface NFCPlugin : NSObject <NFCNDEFReaderSessionDelegate>
//start scanning
+ (void)startScanning;
//start writing
+ (void)startWriting:(NSString *)data;
//stop scanning
+ (void)stopScanning;
@end
#endif