
#import <UIKit/UIKit.h>
#import <LocalAuthentication/LocalAuthentication.h>


#if UNITY_VERSION <= 434
#import "iPhone_View.h"

#endif



@interface BiometricTouch

- (BOOL)canEvaluatePolicy:(LAPolicy)policy error:(NSError * __autoreleasing *)error;

@end

extern "C" {
    
    void TouchID(const char* gameObjectName) 
    {
        NSLog(@"Game object name input is %s", gameObjectName);
        //NSString *goName = [NSString stringWithUTF8String:gameObjectName];
        //NSLog(@"Game object name nsstring is %@", goName);
        
        LAContext *myContext = [[LAContext alloc] init];
        NSError *authError = nil;
        NSString *myLocalizedReasonString = @"Authenticate using your finger";
        if ([myContext canEvaluatePolicy:LAPolicyDeviceOwnerAuthenticationWithBiometrics error:&authError])
        {    
            [myContext evaluatePolicy:LAPolicyDeviceOwnerAuthenticationWithBiometrics
                      localizedReason:myLocalizedReasonString
                                reply:^(BOOL succes, NSError *error) 
                                {
                                    
                                    if (succes) 
                                    {
                                        UnitySendMessage("BiometricTouchObject(Clone)", "OnTouchAuthResponse", "0");
                                        NSLog(@"User is authenticated successfully");
                                    } 
                                    else 
                                    {
                                        
                                        switch (error.code) {
                                            case LAErrorAuthenticationFailed:
                                                UnitySendMessage("BiometricTouchObject(Clone)", "OnTouchAuthResponse", "1");
                                                NSLog(@"Authentication Failed");
                                                break;
                                                
                                            case LAErrorUserCancel:
                                                UnitySendMessage("BiometricTouchObject(Clone)", "OnTouchAuthResponse", "2");
                                                NSLog(@"User pressed Cancel button");
                                                break;
                                                
                                            case LAErrorUserFallback:
                                                UnitySendMessage("BiometricTouchObject(Clone)", "OnTouchAuthResponse", "3");
                                                NSLog(@"User pressed \"Enter Password\"");
                                                break;
                                                
                                            default:
                                                NSLog(@"Touch ID is not configured");
                                                UnitySendMessage("BiometricTouchObject(Clone)", "OnTouchAuthResponse", "4");
                                                break;
                                        }
                                    }
                                }];
        }
        else
        {
            UnitySendMessage("BiometricTouchObject(Clone)", "OnTouchAuthResponse", "5");
            NSLog(@"Can not evaluate Touch ID");
        }
      
    }
    
    
    
}


