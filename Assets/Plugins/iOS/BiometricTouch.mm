
#import <UIKit/UIKit.h>
#import <LocalAuthentication/LocalAuthentication.h>

#if UNITY_VERSION <= 434
#import "iPhone_View.h"

#endif

@interface BiometricTouch

- (BOOL)canEvaluatePolicy:(LAPolicy)policy error:(NSError * __autoreleasing *)error;

@end

extern "C" 
{    
    void TouchID() 
    {
        LAContext *myContext = [[LAContext alloc] init];
        NSError *authError = nil;
        NSString *myLocalizedReasonString = @"Authenticate using your finger";
        NSMutableString *outputCode = [NSMutableString stringWithString:@"5"];
        if ([myContext canEvaluatePolicy:LAPolicyDeviceOwnerAuthenticationWithBiometrics error:&authError])
        {    
            [myContext evaluatePolicy:LAPolicyDeviceOwnerAuthenticationWithBiometrics localizedReason:myLocalizedReasonString reply:^(BOOL succes, NSError *error) 
                {
                    if (succes) 
                    {
                        [outputCode setString:@"0"];
                        NSLog(@"User is authenticated successfully");
                    } 
                    else 
                    {
                        switch (error.code) {
                            case LAErrorAuthenticationFailed:
                                [outputCode setString:@"1"];
                                NSLog(@"Authentication Failed");
                                break;
                                
                            case LAErrorUserCancel:
                                [outputCode setString:@"2"];
                                NSLog(@"User pressed Cancel button");
                                break;
                                
                            case LAErrorUserFallback:
                                [outputCode setString:@"3"];
                                NSLog(@"User pressed \"Enter Password\"");
                                break;
                                
                            default:
                                [outputCode setString:@"4"];
                                NSLog(@"Touch ID is not configured");
                                break;
                        }
                    }
                    const char *outCodeCStr = [outputCode UTF8String];
                    UnitySendMessage("BiometricTouchObject(Clone)", "OnTouchAuthResponse", outCodeCStr);
                }
            ];
        }
        else
        {
            UnitySendMessage("BiometricTouchObject(Clone)", "OnTouchAuthResponse", "5");
            NSLog(@"Can not evaluate Touch ID");
        }
    }
}


